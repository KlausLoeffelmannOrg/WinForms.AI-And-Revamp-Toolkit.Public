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
using System.Text;

namespace CommunityToolkit.WinForms.AI;

#pragma warning disable SKEXP0010 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
public partial class SemanticKernelComponent : BindableComponent
{
    // We're using the GPT-4o model from OpenAI directory for our Semantic Kernel scenario.
    private const string DefaultModelName = "gpt-4o-2024-08-06";

    public event AsyncEventHandler<AsyncRequestAssistantInstructionsEventArgs>? AsyncRequestAssistanceInstructions;
    public event AsyncEventHandler<AsyncRequestExecutionSettingsEventArgs>? AsyncRequestExecutionSettings;

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

        var (chatService, executionSettings) = await GetOrCreateChatServiceAsync();

        if (ChatHistory is null)
            throw new InvalidOperationException("You requested to process a prompt, but the ChatHistory is not set.");

        if (!keepChatHistory)
            ChatHistory.Clear();

        ChatHistory.AddUserMessage(valueToProcess);

        var responses = await chatService.GetChatMessageContentsAsync(
            ChatHistory,
            kernel: _kernel,
            executionSettings: executionSettings);

        StringBuilder responseStringBuilder = new();

        // We can have several responses, so we'll append them all to the text box.
        // But we need to also add them to the chat history!
        foreach (ChatMessageContent response in responses)
        {
            ChatHistory.AddMessage(response.Role, response.ToString());
            responseStringBuilder.Append(response);
        }

        return responseStringBuilder.ToString();
    }

    /// <summary>
    ///  Requests a prompt response from the OpenAI API as an async stream. Make sure, you set at least the <see cref="ApiKeyGetter"/> property,
    ///  and the <see cref="SystemPrompt"/> property, which is the general description, what the Assistant is suppose to do.
    /// </summary>
    /// <param name="valueToProcess">The value as string which the model should process.</param>
    /// <returns>
    ///  Returns an async stream of strings, which are the responses from the LLM model.
    /// </returns>
    /// <exception cref="InvalidOperationException"></exception>
    public async IAsyncEnumerable<string> RequestPromptResponseStreamAsync(string valueToProcess, bool keepChatHistory)
    {
        if (string.IsNullOrWhiteSpace(valueToProcess))
            throw new InvalidOperationException("You requested to process a prompt, but did not provide any content to process.");

        var (chatService, executionSettings) = await GetOrCreateChatServiceAsync();

        if (ChatHistory is null)
            throw new InvalidOperationException("You requested to process a prompt, but the ChatHistory is not set.");

        if (!keepChatHistory)
            ChatHistory.Clear();

        ChatHistory.AddUserMessage(valueToProcess);

        var responses = chatService.GetStreamingChatMessageContentsAsync(
            ChatHistory,
            executionSettings: executionSettings,
            kernel: _kernel);

        responses = ChatHistory.AddStreamingMessageAsync(
            (IAsyncEnumerable<OpenAIStreamingChatMessageContent>)responses);

        await foreach (var response in responses)
        {
            if (response.Content is null)
            {
                continue;
            }

            yield return response.Content;
        }
    }

    private async Task<(OpenAIChatCompletionService chatService, OpenAIPromptExecutionSettings executionSettings)> GetOrCreateChatServiceAsync()
    {
        if (ApiKeyGetter is null)
            throw new InvalidOperationException("You have tried to request a prompt, but did not provide Func delegate to get the api key.");

        AsyncRequestAssistantInstructionsEventArgs eArgs = new(GetAssistantInstructions());
        await OnRequestAssistantInstructionsAsync(eArgs);

        EffectiveSystemPrompt = string.IsNullOrWhiteSpace(JsonSchema)
            ? eArgs.AssistantInstructions
            : $"{eArgs.AssistantInstructions}\n\n{SystemPromptSchemaAmendment}" +
              $"\n\nThe Json Schema is as follows:\n{JsonSchema}";

        OpenAIPromptExecutionSettings executionSettings = new()
        {
            ModelId = ModelName
        };

        executionSettings = executionSettings
            .WithSystemPrompt(EffectiveSystemPrompt)
                    .WithDefaultModelParameters(
                        MaxTokens: 8000,
                        temperature: Temperature,
                        topP: TopP,
                        frequencyPenalty: _frequencyPenalty,
                        presencePenalty: _presencePenalty);

        if (!string.IsNullOrEmpty(JsonSchema))
        {
            executionSettings = executionSettings.WithJsonReturnSchema(
                JsonSchema,
                JsonSchemaName,
                JsonSchemaDescription);
        }

        AsyncRequestExecutionSettingsEventArgs settingsEventArgs = new(executionSettings);
        await OnRequestExecutionSettingsAsync(settingsEventArgs);

        string apiKey = ApiKeyGetter.Invoke()
            ?? throw new InvalidOperationException("The ApiKeyGetter did not retrieve a working api key.");

        var kernelBuilder = Kernel
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

        var chatService = (OpenAIChatCompletionService)_kernel.GetRequiredService<IChatCompletionService>();

        return (chatService, settingsEventArgs.ExecutionSettings);
    }

    private ILoggerFactory CreateTelemetryLogger()
    {
        var resourceBuilder = ResourceBuilder
            .CreateDefault()
            .AddService("TelemetryConsoleQuickstart");

        // Enable model diagnostics with sensitive data!!
        // AppContext.SetSwitch("Microsoft.SemanticKernel.Experimental.GenAI.EnableOTelDiagnosticsSensitive", true);

        // Enable model diagnostics without sensitive data.
        AppContext.SetSwitch("Microsoft.SemanticKernel.Experimental.GenAI.EnableOTelDiagnostics", true);

        using var traceProvider = Sdk.CreateTracerProviderBuilder()
            .SetResourceBuilder(resourceBuilder)
            .AddSource("Microsoft.SemanticKernel*")
            .AddConsoleExporter()
            .Build();

        using var meterProvider = Sdk.CreateMeterProviderBuilder()
            .SetResourceBuilder(resourceBuilder)
            .AddMeter("Microsoft.SemanticKernel*")
            .AddConsoleExporter()
            .Build();

        var loggerFactory = LoggerFactory.Create(builder =>
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

    protected virtual string GetAssistantInstructions()
        => SystemPrompt;

    protected virtual Task OnRequestAssistantInstructionsAsync(AsyncRequestAssistantInstructionsEventArgs eArgs)
        => AsyncRequestAssistanceInstructions?.Invoke(this, eArgs)
            ?? Task.CompletedTask;

    protected virtual Task OnRequestExecutionSettingsAsync(AsyncRequestExecutionSettingsEventArgs eArgs)
        => AsyncRequestExecutionSettings?.Invoke(this, eArgs)
            ?? Task.CompletedTask;

    public void AddChatItem(bool isResponse, string message)
    {
        if (_chatHistory is null)
        {
            _chatHistory = [];
            
        }

        _chatHistory.AddMessage(
            isResponse ? AuthorRole.Assistant : AuthorRole.User,
            message);
    }

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
    public virtual string SystemPrompt { get; set; }
        = "You are an Assistant for helping Developers with questions around .NET, C# and Visual Basic.";

    /// <summary>
    ///  The SystemPrompt, as it became constructed based on schema information, original SystemPrompt and SystemPrompt event.
    /// </summary>
    private string? EffectiveSystemPrompt { get; set; }

    /// <summary>
    ///  Gets or sets an amendment to the system prompt in the case, a JSon schema had been provided, so 
    ///  we need to instruct the model to return the result in JSon according to the schema information.
    /// </summary>
    protected virtual string SystemPromptSchemaAmendment { get; set; } =
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
#pragma warning restore SKEXP0010 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
