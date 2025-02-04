using CommunityToolkit.WinForms.AI.ConverterLogic;
using CommunityToolkit.WinForms.AsyncSupport;
using CommunityToolkit.WinForms.Controls;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using OpenTelemetry;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using System.ComponentModel;
using System.Net.Http.Headers;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Text.Json;

namespace CommunityToolkit.WinForms.AI;

#pragma warning disable SKEXP0010 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
public partial class SemanticKernelComponent : BindableComponent
{
    // We're using the GPT-4o model from OpenAI directory for our Semantic Kernel scenario.
    private const string DefaultModelName = "gpt-4o-2024-11-20";
    private const string ApiEndpoint = "https://api.openai.com/v1/models";

    /// <summary>
    ///  Event triggered to request assistant instructions asynchronously.
    /// </summary>
    /// <remarks>
    ///  <para>
    ///   This event is triggered to request assistant instructions asynchronously.
    ///   It allows subscribers to provide custom instructions for the assistant.
    ///  </para>
    /// </remarks>
    public event AsyncEventHandler<AsyncRequestAssistantInstructionsEventArgs>? AsyncRequestAssistanceInstructions;

    /// <summary>
    ///  Event triggered to request execution settings asynchronously.
    /// </summary>
    /// <remarks>
    ///  <para>
    ///   This event is triggered to request execution settings asynchronously.
    ///   It allows subscribers to provide custom execution settings for the assistant.
    ///  </para>
    /// </remarks>
    public event AsyncEventHandler<AsyncRequestExecutionSettingsEventArgs>? AsyncRequestExecutionSettings;

    /// <summary>
    ///  Event triggered when the next paragraph is received asynchronously.
    /// </summary>
    /// <remarks>
    ///  <para>
    ///   This event is triggered when the next paragraph is received asynchronously.
    ///   It allows subscribers to handle the received paragraph data.
    ///  </para>
    /// </remarks>
    public event AsyncEventHandler<AsyncReceivedNextParagraphEventArgs>? AsyncReceivedNextParagraph;

    /// <summary>
    ///  Event triggered when inline metadata is received asynchronously.
    /// </summary>
    /// <remarks>
    ///  <para>
    ///   This event is triggered when inline metadata is received asynchronously.
    ///   It allows subscribers to handle the received metadata.
    ///  </para>
    /// </remarks>
    public event AsyncEventHandler<AsyncReceivedInlineMetaDataEventArgs>? AsyncReceivedInlineMetaData;

    // The chat history. If you are using ChatGPT for example directly in the WebBrowser,
    // this equals the chat history, so, the things you've said and the responses you've received.
    private ChatHistory? _chatHistory;

    // The kernel for the Semantic Kernel scenario. It bundles the connectors and services.
    private Kernel? _kernel;

    // The parentForm, which we might need to invoke the UI thread.
    private double? _topP;
    private double? _temperature;
    private double? _presencePenalty;
    private double? _frequencyPenalty;
    private JsonSerializerOptions? _jsonSerializerOptions;

    private readonly SynchronizationContext? _syncContext = WindowsFormsSynchronizationContext.Current;
    private string? _systemPrompt;
    private bool _queueSystemPrompt;

    /// <summary>
    ///  Requests a prompt response from the OpenAI API. Make sure, you set at least the <see cref="ApiKeyGetter"/> property,
    ///  and the <see cref="SystemPrompt"/> property, which is the general description, what the Assistant is suppose to do.
    /// </summary>
    /// <param name="valueToProcess">The value as string which the model should process.</param>
    /// <returns>The result from the LLM Model as plain text string or JSon string.</returns>
    /// <exception cref="InvalidOperationException"></exception>
    public async Task<string?> RequestTextPromptResponseAsync(
        string valueToProcess,
        bool keepChatHistory)
    {
        if (string.IsNullOrWhiteSpace(valueToProcess))
            throw new InvalidOperationException("You requested to process a prompt, but did not provide any content to process.");

        (OpenAIChatCompletionService chatService, OpenAIPromptExecutionSettings executionSettings) = await GetOrCreateChatServiceAsync();

        var chatHistory = HandleChatHistory(valueToProcess, keepChatHistory);

        IReadOnlyList<ChatMessageContent> responses = await chatService.GetChatMessageContentsAsync(
            chatHistory,
            kernel: _kernel,
            executionSettings: executionSettings);

        StringBuilder responseStringBuilder = new();

        // We can have several responses, so we'll append them all to the text box.
        // But we need to also add them to the chat history!
        foreach (ChatMessageContent response in responses)
        {
            chatHistory.AddMessage(response.Role, response.ToString());
            responseStringBuilder.Append(response);
        }

        return responseStringBuilder.ToString();
    }

    /// <summary>
    ///  Requests a prompt response from the OpenAI API as an async stream. Make sure, you set at least the <see cref="ApiKeyGetter"/> property,
    ///  and the <see cref="SystemPrompt"/> property, which is the general description, what the Assistant is suppose to do.
    /// </summary>
    /// <param name="valueToProcess">The value as string which the model should process.</param>
    /// <param name="keepChatHistory">Indicates whether to keep the chat history for the session.</param>
    /// <returns>
    ///  Returns an async stream of strings, which are the responses from the LLM model.
    /// </returns>
    /// <exception cref="InvalidOperationException">
    ///  Thrown when the value to process is null or empty.
    /// </exception>
    /// <remarks>
    ///  <para>
    ///   This method sends a prompt to the OpenAI API and returns the responses as an asynchronous stream of strings.
    ///   It ensures that the necessary properties, such as the API key and system prompt, are set before making the request.
    ///  </para>
    ///  <para>
    ///   The method handles the chat history based on the keepChatHistory parameter. If keepChatHistory is false, the chat history is cleared,
    ///   and the system prompt is queued for the next request. If keepChatHistory is true, the chat history is maintained across requests.
    ///  </para>
    ///  <para>
    ///   The responses are processed as an asynchronous stream, allowing the caller to handle each response as it is received.
    ///   The method also raises events for received metadata and paragraphs, enabling subscribers to handle these events accordingly.
    ///  </para>
    /// </remarks>
    public async IAsyncEnumerable<string> RequestPromptResponseStreamAsync(string valueToProcess, bool keepChatHistory)
    {
        if (string.IsNullOrWhiteSpace(valueToProcess))
            throw new InvalidOperationException("You requested to process a prompt, but did not provide any content to process.");

        (OpenAIChatCompletionService chatService, OpenAIPromptExecutionSettings executionSettings) = await GetOrCreateChatServiceAsync();

        var chatHistory = HandleChatHistory(valueToProcess, keepChatHistory);

        IAsyncEnumerable<StreamingChatMessageContent> responses = chatService.GetStreamingChatMessageContentsAsync(
            chatHistory,
            executionSettings: executionSettings,
            kernel: _kernel);

        responses = chatHistory.AddStreamingMessageAsync(
            (IAsyncEnumerable<OpenAIStreamingChatMessageContent>)responses);

        await foreach (var response in ReturnTokenParser.ProcessTokens(
            asyncEnumerable: responses,
            onReceivedMetaDataAction: OnReceivedMetaData,
            onReceivedNextParagraphAction: OnReceivedNextParagraph))
        {
            if (string.IsNullOrEmpty(response))
            {
                continue;
            }

            yield return response;
        }
    }

    protected virtual void OnReceivedMetaData(AsyncReceivedInlineMetaDataEventArgs receivedMetaDataEventArgs)
        => AsyncReceivedInlineMetaData?.Invoke(this, receivedMetaDataEventArgs);

    private ChatHistory HandleChatHistory(string valueToProcess, bool keepChatHistory)
    {
        if (ChatHistory is null)
        {
            throw new InvalidOperationException("You requested to process a prompt, but the ChatHistory is not set.");
        }

        if (!keepChatHistory)
        {
            ChatHistory.Clear();
            _queueSystemPrompt = true;
        }

        if (_queueSystemPrompt)
        {
            ChatHistory.AddMessage(
                AuthorRole.Assistant,
                EffectiveSystemPrompt ?? throw new InvalidOperationException("The effective system prompt is not set."));

            _queueSystemPrompt = false;
        }

        ChatHistory.AddUserMessage(valueToProcess);

        return ChatHistory;
    }

    private async Task<(OpenAIChatCompletionService chatService, OpenAIPromptExecutionSettings executionSettings)> GetOrCreateChatServiceAsync()
    {
        if (ApiKeyGetter is null)
            throw new InvalidOperationException("You have tried to request a prompt, but did not provide Func delegate to get the api key.");

        AsyncRequestAssistantInstructionsEventArgs eArgs = new(GetAssistantInstructions());
        await OnRequestAssistantInstructionsAsync(eArgs);

        OpenAIPromptExecutionSettings executionSettings = new()
        {
            ModelId = ModelName
        };

        if (!ModelName.StartsWith("o1") && !ModelName.StartsWith("o3"))
        {
            executionSettings = executionSettings
            .WithDefaultModelParameters(
                MaxTokens: MaxTokens,
                temperature: Temperature,
                topP: TopP,
                frequencyPenalty: _frequencyPenalty,
                presencePenalty: _presencePenalty);
        }

        if (!string.IsNullOrEmpty(JsonSchema))
        {
            executionSettings = executionSettings.WithJsonReturnSchema(
                JsonSchema,
                JsonSchemaName,
                JsonSchemaDescription);

            EffectiveSystemPrompt = $"{eArgs.AssistantInstructions}\n\n{SystemPromptSchemaAmendment}" +
                  $"\n\nThe Json Schema is as follows:\n{JsonSchema}";
        }
        else
        {
            string formatRequestString = $"\n\nIMPORTANT: Please make sure that you format your replies consistently "
                + ReturnStringsFormat switch
                {
                    ReturnStringsFormat.Markdown => "Markdown formatted.",
                    ReturnStringsFormat.PlainText => "as plain text without any additional formatting.",
                    ReturnStringsFormat.Html => "as HTML.",
                    ReturnStringsFormat.MicrosoftRichText => "as Microsoft Rich Text Format (RTF).",
                    _ => "Plain text."
                };

            EffectiveSystemPrompt = $"{eArgs.AssistantInstructions}\n" + $"{formatRequestString}";
        }

        AsyncRequestExecutionSettingsEventArgs settingsEventArgs = new(executionSettings);
        await OnRequestExecutionSettingsAsync(settingsEventArgs);

        string apiKey = ApiKeyGetter.Invoke()
            ?? throw new InvalidOperationException("The ApiKeyGetter did not retrieve a working api key.");

        IKernelBuilder kernelBuilder = Kernel
            .CreateBuilder()
            .AddOpenAIChatCompletion(ModelName, apiKey);

        if (LogConsole is not null)
        {
            ILoggerFactory loggerFactory = CreateTelemetryLogger();
            kernelBuilder.Services.AddSingleton(loggerFactory);

            Console.SetOut(LogConsole.ConsoleOut);
            Console.WriteLine("Logging started.");
            Console.WriteLine();
        }

        _kernel = kernelBuilder.Build();
        _chatHistory ??= [];

        OpenAIChatCompletionService chatService = (OpenAIChatCompletionService)_kernel
            .GetRequiredService<IChatCompletionService>();

        return (chatService, settingsEventArgs.ExecutionSettings);
    }

    public async Task<IEnumerable<string>?> QueryOpenAiModelNamesAsync()
    {
        string apiKey = (ApiKeyGetter?.Invoke()) ?? throw new InvalidOperationException("API-Key for Open-AI access could not be retrieved.");

        using HttpClient client = new();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);

        try
        {
            _jsonSerializerOptions ??= new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                WriteIndented = true
            };

            HttpResponseMessage response = await client.GetAsync(ApiEndpoint);
            response.EnsureSuccessStatusCode();

            string responseBody = await response.Content.ReadAsStringAsync();

            OpenAiModelList? models = JsonSerializer.Deserialize<OpenAiModelList>(
                responseBody,
                options: _jsonSerializerOptions);

            return models?.Data.Select(item => item.Id);
        }
        catch (Exception ex)
        {
            ExceptionDispatchInfo dispatchInfo = ExceptionDispatchInfo.Capture(ex);

            // Marshal to the UI thread
            _syncContext?.Post(_ =>
                {
                    dispatchInfo.Throw();
                }, null);

            return null;
        }
    }

    public async Task<ModelInfo?> QueryOpenAiModelInfoAsync(string modelName)
    {
        string apiKey = (ApiKeyGetter?.Invoke()) ?? throw new InvalidOperationException("API-Key for Open-AI access could not be retrieved.");

        using HttpClient client = new();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);

        try
        {
            HttpResponseMessage response = await client.GetAsync($"{ApiEndpoint}/{modelName}");
            response.EnsureSuccessStatusCode();

            string content = await response.Content.ReadAsStringAsync();
            ModelInfo? modelInfo = JsonSerializer.Deserialize<ModelInfo>(content);

            return modelInfo;
        }
        catch (Exception ex)
        {
            ExceptionDispatchInfo dispatchInfo = ExceptionDispatchInfo.Capture(ex);

            // Marshal to the UI thread
            _syncContext?.Post(_ =>
                {
                    dispatchInfo.Throw();
                }, null);
        }

        return null;
    }

    public async Task<T> RequestStructuredResponseAsync<T>(string request, CancellationToken token)
    {
        string? jsonSchema = PromptFromTypeProcessor.GetJSonSchema<T>();
        StructuredReturnDataAttribute promptInfo = PromptFromTypeProcessor.GetTypePromptInfo<T>();
        string systemPrompt = promptInfo.Prompt ?? throw new InvalidOperationException("The type did not return a basic (system) prompt.");

        StringBuilder stringBuilder = new();

        if (promptInfo.ProvideDate)
        {
            stringBuilder.AppendLine($"Today is {DateTime.Now:d}.");
        }

        if (promptInfo.ProvideTime)
        {
            stringBuilder.AppendLine($"The current time is {DateTime.Now:T}.");
        }

        if (promptInfo.ProvideTimeZone)
        {
            stringBuilder.AppendLine($"The current time zone is {TimeZoneInfo.Local.DisplayName}.");
        }

        if (promptInfo.LanguageCulture is not null)
        {
            stringBuilder.AppendLine($"The user requested assistance for the following language: {promptInfo.LanguageCulture.DisplayName}.");
        }

        stringBuilder.AppendLine("Please provide the requested information and return Json according to the provided Json Schema.");
        stringBuilder.AppendLine("Please provide the following information:");

        IEnumerable<string> propertiesRequests = PromptFromTypeProcessor.GetTypePropertyPrompts<T>();

        foreach (string propertyRequest in propertiesRequests)
        {
            stringBuilder.AppendLine(propertyRequest);
        }

        stringBuilder.AppendLine();
        stringBuilder.AppendLine("Please include Markdown formatting only for the inner json content " +
            "where it applies, but not to the json envelope itself!");

        stringBuilder.AppendLine("The Data to work with is as follow:");
        stringBuilder.AppendLine();
        stringBuilder.AppendLine(request);

        string prompt = stringBuilder.ToString();

        string? previousJSonSchema = JsonSchema;
        string? previousSystemPrompt = SystemPrompt;

        JsonSchema = jsonSchema;
        SystemPrompt = systemPrompt;

        try
        {
            string? jsonReturnData = await RequestTextPromptResponseAsync(prompt, false);

            if (string.IsNullOrEmpty(jsonReturnData))
            {
                throw new InvalidOperationException("Trying to parse the text input failed for unknown reasons.");
            }

            // Let's see, if we have to eliminate "```..." in the first line and
            // "```" in the last line, as this is markdown formatting.
            if (jsonReturnData.StartsWith("```"))
            {
                // The complete first line has to go:
                int firstLineEnd = jsonReturnData.IndexOf('\n');
                if (firstLineEnd > 0)
                {
                    jsonReturnData = jsonReturnData[firstLineEnd..];
                }
            }

            if (jsonReturnData.EndsWith("```"))
            {
                // The complete last line has to go:
                int lastLineStart = jsonReturnData.LastIndexOf('\n');
                if (lastLineStart > 0)
                {
                    jsonReturnData = jsonReturnData[..lastLineStart];
                }
            }

            try
            {
                var options = new JsonSerializerOptions
                {
                    AllowTrailingCommas = true
                };

                T typedReturnValue = JsonSerializer.Deserialize<T>(jsonReturnData, options)!;

                return typedReturnValue;
            }

            catch (Exception innerException)
            {
                if (innerException is FormatException)
                {
                    throw;
                }

                throw new FormatException("There was a problem constructing the type from the response data.", innerException);
            }
        }
        finally
        {
            JsonSchema = previousJSonSchema;
            SystemPrompt = previousSystemPrompt;
        }
    }

    public class ParentArray<T>
    {
        public T[]? Items { get; set; }
    }

    public async Task<string[]> RequestStructuredStringListResponseAsync(string request, CancellationToken token)
    {
        string prompt = $"{request}\n\nPlease provide the requested information and return Json as an Array of string.";
        string? jsonReturnData = await RequestTextPromptResponseAsync(prompt, false);

        return JsonSerializer.Deserialize<string[]>(jsonReturnData!)!;
    }

    public async Task<T[]> RequestStructuredListResponseAsync<T>(string request, CancellationToken token)
    {
        string? jsonSchema = PromptFromTypeProcessor.GetJSonSchema<ParentArray<T>>();
        StructuredReturnDataAttribute promptInfo = PromptFromTypeProcessor.GetTypePromptInfo<T>();

        string systemPrompt = promptInfo.Prompt ?? throw new InvalidOperationException("The type did not return a basic (system) prompt.");

        StringBuilder stringBuilder = new();

        if (promptInfo.ProvideDate)
        {
            stringBuilder.AppendLine($"Today is {DateTime.Now:d}.");
        }

        if (promptInfo.ProvideTime)
        {
            stringBuilder.AppendLine($"The current time is {DateTime.Now:T}.");
        }

        if (promptInfo.ProvideTimeZone)
        {
            stringBuilder.AppendLine($"The current time zone is {TimeZoneInfo.Local.DisplayName}.");
        }

        if (promptInfo.LanguageCulture is not null)
        {
            stringBuilder.AppendLine($"The user requested assistance for the following language: {promptInfo.LanguageCulture.DisplayName}.");
        }

        stringBuilder.AppendLine("Please provide the requested information and return Json as an Array according to the provided Json Schema.");
        stringBuilder.AppendLine("Please provide the following information:");

        IEnumerable<string> propertiesRequests = PromptFromTypeProcessor.GetTypePropertyPrompts<T>();

        foreach (string propertyRequest in propertiesRequests)
        {
            stringBuilder.AppendLine(propertyRequest);
        }

        stringBuilder.AppendLine();
        stringBuilder.AppendLine("Please include Markdown formatting only for the inner json content " +
            "where it applies, but not to the json envelope itself!");

        stringBuilder.AppendLine("The Data to work with is as follow:");
        stringBuilder.AppendLine();
        stringBuilder.AppendLine(request);

        string prompt = stringBuilder.ToString();

        string? previousJSonSchema = JsonSchema;
        string? previousSystemPrompt = SystemPrompt;

        JsonSchema = jsonSchema;
        SystemPrompt = systemPrompt;

        try
        {
            string? jsonReturnData = await RequestTextPromptResponseAsync(prompt, false);

            if (string.IsNullOrEmpty(jsonReturnData))
            {
                throw new InvalidOperationException("Trying to parse the text input failed for unknown reasons.");
            }

            // Let's see, if we have to eliminate "```..." in the first line and
            // "```" in the last line, as this is markdown formatting.
            if (jsonReturnData.StartsWith("```"))
            {
                // The complete first line has to go:
                int firstLineEnd = jsonReturnData.IndexOf('\n');
                if (firstLineEnd > 0)
                {
                    jsonReturnData = jsonReturnData[firstLineEnd..];
                }
            }

            if (jsonReturnData.EndsWith("```"))
            {
                // The complete last line has to go:
                int lastLineStart = jsonReturnData.LastIndexOf('\n');
                if (lastLineStart > 0)
                {
                    jsonReturnData = jsonReturnData[..lastLineStart];
                }
            }

            try
            {
                T[] typedReturnValue = JsonSerializer.Deserialize<T[]>(jsonReturnData)!;
                return typedReturnValue;
            }

            catch (Exception innerException)
            {
                if (innerException is FormatException)
                {
                    throw;
                }

                throw new FormatException("There was a problem constructing the type from the response data.", innerException);
            }
        }
        finally
        {
            JsonSchema = previousJSonSchema;
            SystemPrompt = previousSystemPrompt;
        }
    }


    private static ILoggerFactory CreateTelemetryLogger()
    {
        ResourceBuilder resourceBuilder = ResourceBuilder
            .CreateDefault()
            .AddService("TelemetryConsoleQuickstart");

        // Enable model diagnostics with sensitive data!!
        // AppContext.SetSwitch("Microsoft.SemanticKernel.Experimental.GenAI.EnableOTelDiagnosticsSensitive", true);

        // Enable model diagnostics without sensitive data.
        AppContext.SetSwitch("Microsoft.SemanticKernel.Experimental.GenAI.EnableOTelDiagnostics", true);

        using TracerProvider traceProvider = Sdk.CreateTracerProviderBuilder()
            .SetResourceBuilder(resourceBuilder)
            .AddSource("Microsoft.SemanticKernel*")
            .AddConsoleExporter()
            .Build();

        using MeterProvider meterProvider = Sdk.CreateMeterProviderBuilder()
            .SetResourceBuilder(resourceBuilder)
            .AddMeter("Microsoft.SemanticKernel*")
            .AddConsoleExporter()
            .Build();

        ILoggerFactory loggerFactory = LoggerFactory.Create(builder =>
        {
            // Add OpenTelemetry as a logging provider
            builder.AddOpenTelemetry(options =>
            {
                options.SetResourceBuilder(resourceBuilder);
                options.AddConsoleExporter();
                options.IncludeFormattedMessage = true;
                options.IncludeScopes = true;
            });

            builder.SetMinimumLevel(LogLevel.Trace);
        });

        return loggerFactory;
    }

    protected virtual string? GetAssistantInstructions()
        => SystemPrompt;

    protected virtual Task OnRequestAssistantInstructionsAsync(AsyncRequestAssistantInstructionsEventArgs eArgs)
        => AsyncRequestAssistanceInstructions?.Invoke(this, eArgs) ?? Task.CompletedTask;

    protected virtual Task OnRequestExecutionSettingsAsync(AsyncRequestExecutionSettingsEventArgs eArgs)
        => AsyncRequestExecutionSettings?.Invoke(this, eArgs) ?? Task.CompletedTask;

    /// <summary>
    /// Raises the <see cref="AsyncReceivedNextParagraph"/> event.
    /// </summary>
    /// <param name="receivedNextParagraphEventArgs">The event data.</param>
    protected virtual void OnReceivedNextParagraph(AsyncReceivedNextParagraphEventArgs receivedNextParagraphEventArgs)
        => AsyncReceivedNextParagraph?.Invoke(this, receivedNextParagraphEventArgs);

    public void AddChatItem(bool isResponse, string message)
    {
        _chatHistory ??= [];

        _chatHistory.AddMessage(
            isResponse ? AuthorRole.Assistant : AuthorRole.User,
            message);
    }

    [Bindable(true)]
    [Browsable(true)]
    [Category("Model Parameter")]
    [Description("Gets or sets the Format in which human readable, non-structured Text is requested to be returned from the Model.")]
    [DefaultValue(ReturnStringsFormat.PlainText)]
    public ReturnStringsFormat ReturnStringsFormat { get; set; } = ReturnStringsFormat.PlainText;

    /// <summary>
    ///  Gets or sets a Func that returns the API key to use for the OpenAI API. 
    ///  Don't ever store the key in the source code! Rather put it for example in a environment variable, or even better,
    ///  get it from a secure storage like Azure Key Vault.
    /// </summary>
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    [Browsable(false)]
    public Func<string>? ApiKeyGetter { get; set; }

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public ConsoleControl? LogConsole { get; set; }

    /// <summary>
    ///  Gets or sets the JSon schema for the return format. In that case, the SystemPrompt will be automatically amended to 
    ///  instruct the model to return the result in JSon. Make sure, though, your system prompt includes the description of 
    ///  the return value schema for the LLM model.
    /// </summary>
    [Bindable(true)]
    [Browsable(true)]
    [DefaultValue(null)]
    [Category("Model Parameter")]
    [Description("Gets or sets JSon schema for the return value. The easiest way to create this is, well, with a LLM!")]
    public string? JsonSchema { get; set; } = null;

    /// <summary>
    ///  Gets or sets the JSon schema name for the return format.
    /// </summary>
    [Bindable(true)]
    [Browsable(true)]
    [DefaultValue(null)]
    [Category("Model Parameter")]
    [Description("Gets or sets the name for the JSon schema. This does currently not influence what the model returns.")]
    public string? JsonSchemaName { get; set; } = null;

    /// <summary>
    ///  Gets or sets the JSon schema name for the return format.
    /// </summary>
    [Bindable(true)]
    [Browsable(true)]
    [DefaultValue(null)]
    [Category("Model Parameter")]
    [Description("Gets or sets the Json schema description. This does currently not influence what the model returns.")]
    public string? JsonSchemaDescription { get; set; } = null;

    [Bindable(true)]
    [Browsable(true)]
    [DefaultValue(4096)]
    [Category("Model Parameter")]
    [Description("Gets or sets the maximum number of tokens to return.")]
    public int MaxTokens { get; set; } = 4096;

    /// <summary>
    ///  Gets or sets the resource string source for the Assistant Instructions.
    /// </summary>
    [DefaultValue(null)]
    public string? ResourceStreamSource { get; set; }

    /// <summary>
    ///  Gets or sets the chat history for the Semantic Kernel scenario.
    /// </summary>
    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public ChatHistory? ChatHistory => _chatHistory;

    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public virtual string? SystemPrompt
    {
        get => _systemPrompt;
        set
        {
            if (value == _systemPrompt)
                return;

            _systemPrompt = value;
            _queueSystemPrompt = true;
        }
    }

    /// <summary>
    ///  The SystemPrompt, as it became constructed based on schema information, original SystemPrompt and SystemPrompt event.
    /// </summary>
    private string? EffectiveSystemPrompt { get; set; }

    /// <summary>
    ///  Gets or sets an amendment to the system prompt in the case, a JSon schema had been provided, so 
    ///  we need to instruct the model to return the result in JSon according to the schema information.
    /// </summary>
    protected virtual string SystemPromptSchemaAmendment { get; set; } =
        // Note: The "IMPORTANT" is actually not necessary anymore with more modern models, like GPT-4o.
        // Also read: https://cookbook.openai.com/examples/structured_outputs_intro
        $"""
        In addition to everything previously said, it is absolutely essential that you return 
        the result in Json according to the enclosed json schema information. 
        
        IMPORTANT: DO NOT add any listing tags or other formatting to the result, just the plain json string!
        """;

    /// <summary>
    ///  Gets or sets the model name to use for chat completion.
    /// </summary>
    [Category("Model Parameter")]
    [DefaultValue(DefaultModelName)]
    [Description("The model name to use for chat completion - for example 'chat-gpt4o'.")]
    public string ModelName { get; set; } = DefaultModelName;

    /// <summary>
    ///  Gets or sets the frequency penalty to apply during chat completion.
    /// </summary>
    /// <remarks>
    ///  The frequency penalty is a value between 0 and 1 that penalizes the model for repeating the same response.
    ///  A higher value will make the model less likely to repeat responses.
    /// </remarks>
    [Category("Model Parameter")]
    [DefaultValue(null)]
    [Description("The frequency penalty is a value between 0 and 1 that penalizes the model for repeating the same response.")]
    public double? FrequencyPenalty
    {
        get => _frequencyPenalty;
        set => _frequencyPenalty = value;
    }

    /// <summary>
    ///  Gets or sets the presence penalty to apply during chat completion.
    /// </summary>
    /// <remarks>
    ///  The presence penalty is a value between 0 and 1 that penalizes the model for generating responses that are too long.
    ///  A higher value will make the model more likely to generate shorter responses.
    /// </remarks>
    [Category("Model Parameter")]
    [DefaultValue(null)]
    [Description("The presence penalty is a value between 0 and 1 that penalizes the model for generating responses that are too long.")]
    public double? PresencePenalty
    {
        get => _presencePenalty;
        set => _presencePenalty = value;
    }

    /// <summary>
    ///  Gets or sets the temperature value for chat completion.
    /// </summary>
    /// <remarks>
    ///  The temperature value controls the randomness of the model's responses.
    ///  A higher value will make the responses more random, while a lower value 
    ///  will make them more deterministic.
    /// </remarks>
    [Category("Model Parameter")]
    [DefaultValue(null)]
    [Description("The temperature value controls the randomness of the model's responses.")]
    public double? Temperature
    {
        get => _temperature;
        set => _temperature = value;
    }

    /// <summary>
    ///  Gets or sets the top-p value for chat completion.
    /// </summary>
    /// <remarks>
    ///  The top-p value is a value between 0 and 1 that controls the diversity 
    ///  of the model's responses.
    ///  A higher value will make the responses more diverse, while a lower value 
    ///  will make them more focused.
    /// </remarks>
    [Category("Model Parameter")]
    [Description("The top-p value is a value between 0 and 1 that controls the diversity of the model's responses.")]
    [DefaultValue(1)]
    public double? TopP
    {
        get => _topP;
        set => _topP = value;
    }
}
