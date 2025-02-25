using CommunityToolkit.WinForms.AI;
using CommunityToolkit.WinForms.AsyncSupport;
using CommunityToolkit.WinForms.ChatUI.AIStructures;
using CommunityToolkit.WinForms.FluentUI.Controls;

using Microsoft.SemanticKernel.ChatCompletion;

using System.ComponentModel;
using System.Runtime.ExceptionServices;

namespace CommunityToolkit.WinForms.ChatUI;

/// <summary>
/// Represents the ChatView control which integrates asynchronous AI chat functionality within a
///  WinForms application.
/// </summary>
/// <remarks>
///  <para>
///   This control handles the processing of user input and AI responses, manages conversation history,
///   and provides a flexible UI to interact with an AI model.
///  </para>
///  <para>
///   It integrates with various components such as the communicator and metadata processor from the 
///   CommunityToolkit to offer a rich chat experience.
///  </para>
/// </remarks>
public partial class ChatView : UserControl
{
    /// <summary>
    ///  Represents the default model identifier for the communicator.
    /// </summary>
    public const string DefaultCommunicatorModel = "gpt-4o";

    /// <summary>
    ///  Represents the default model identifier for the meta data processor.
    /// </summary>
    public const string DefaultMetaDataProcessorModel = "gpt-4o-mini-2024-07-18";

    public event AsyncEventHandler<AsyncNotifyRefreshedMetaDataEventArgs>? AsyncNotifyRefreshedMetaData;

    public event AsyncEventHandler<AsyncCodeBlockInfoProvidedEventArgs>? AsyncCodeBlockInfoProvided;

    public event AsyncEventHandler<AsyncRequestFileContextEventArgs>? AsyncRequestFileExtractingSettings;

    public event AsyncEventHandler<AsyncRequestFileContextEventArgs>? AsyncNotifySaveChat;

    public event EventHandler? ShowPromptControlChanged;

    public event EventHandler? DeveloperPromptChanged;

    public event EventHandler? ModelNameChanged;

    public event EventHandler<RequestChatViewOptionsEventArgs>? RequestChatViewOptions;

    private ConversationProcessor _conversationProcessor = null!;
    private ChatViewOptions _options = null!;
    private bool _requestClearChatHistory;

    public ChatView()
    {
        InitializeComponent();
        ConversationView.ConversationItemAdded += ConversationView_ConversationItemAdded;
        PromptControl.AsyncSendCommand += PromptControl_AsyncSendCommand;

        _skMetaDataProcessor.ModelId = DefaultMetaDataProcessorModel;
        _skCommunicator.ModelId = DefaultCommunicatorModel;

        // We want a toolstrip with 32 Pixels.
        // TODO: We need more configuration options here.
        _decoratorPanel.ProvideToolStripInSize = 32;
        _promptControl.CommandStrip = _decoratorPanel.ToolStrip;
        _promptControl.RequestPromptSendButton = true;
        _promptControl.RequestOpenLibraryButton = true;
    }

    protected override void OnLoad(EventArgs e)
    {
        base.OnLoad(e);

        if (ApiKeyGetter is null)
        {
            throw new InvalidOperationException("ApiKeyGetter is null. Please provide a valid ApiKeyGetter.");
        }

        // Wiring the delegate which provides the Open AI ApiKey when we need it:
        _skCommunicator.ApiKeyGetter = ApiKeyGetter;
        _skMetaDataProcessor.ApiKeyGetter = _skCommunicator.ApiKeyGetter;

        RequestChatViewOptionsEventArgs eArgs = new(null, string.Empty, Guid.Empty);
        OnRequestChatViewOptions(eArgs);
        _options = new ChatViewOptions(eArgs.BasePath, eArgs.LastUsedModel, eArgs.LastUsedConfigurationId);

        _skCommunicator.AsyncReceivedNextParagraph += SKCommunicator_ReceivedNextParagraph;
        _skCommunicator.AsyncCodeBlockInfoProvided += SKCommunicator_AsyncCodeBlockInfoProvided;
    }

    [DefaultValue(null)]
    public string? DeveloperPrompt
    {
        get => _skCommunicator.DeveloperPrompt;
        set
        {
            if (_skCommunicator.DeveloperPrompt == value)
            {
                return;
            }

            _skCommunicator.DeveloperPrompt = value;
            OnDeveloperPromptChanged(EventArgs.Empty);
        }
    }

    protected virtual void OnDeveloperPromptChanged(EventArgs e)
        => DeveloperPromptChanged?.Invoke(this, e);

    [DefaultValue(DefaultCommunicatorModel)]
    public string ModelName
    {
        get => _skCommunicator.ModelId;
        set
        {
            if (_skCommunicator.ModelId == value)
            {
                return;
            }
            _skCommunicator.ModelId = value;
            OnModelNameChanged(EventArgs.Empty);
        }
    }

    [Bindable(true)]
    [Browsable(true)]
    [Category("Model Parameter")]
    [Description("Gets or sets the Format in which human readable, non-structured Text is requested to be returned from the Model.")]
    [DefaultValue(ResponseTextFormat.PlainText)]
    public ResponseTextFormat ReturnStringsFormat
    {
        get => _skCommunicator.ResponseTextFormat;
        set => _skCommunicator.ResponseTextFormat = value;
    }

    protected virtual void OnModelNameChanged(EventArgs e)
        => ModelNameChanged?.Invoke(this, e);

    public ConversationView ConversationView
        => _conversationView;

    public AutoCompleteEditor PromptControl
        => _promptControl;

    [DefaultValue(true)]
    public bool ShowPromptControl
    {
        get => _promptControl.Visible;
        set
        {
            if (_promptControl.Visible == value)
            {
                return;
            }

            _promptControl.Visible = value;
            OnShowPromptControlChanged(EventArgs.Empty);
        }
    }

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public Func<string>? ApiKeyGetter { get; set; }

    protected virtual void OnShowPromptControlChanged(EventArgs e)
    {
        // We need to re-layout:
        PerformLayout();
        ShowPromptControlChanged?.Invoke(this, e);
    }

    public void ClearChatHistory()
    {
        _requestClearChatHistory = true;
    }

    public void StartNewChat()
    {
        _requestClearChatHistory = true;
        ConversationView.ClearHistory();
        ConversationView.Conversation.Title = Conversation.GetDefaultTitle();
    }

    public void AddChatItem(bool isResponse, string message)
        => _skCommunicator.AddChatItem(isResponse, message);

    public async Task LoadConversationAsync(string filename)
    {
        RequestChatViewOptionsEventArgs eArgs = new(_options);
        OnRequestChatViewOptions(eArgs);

        ClearChatHistory();

        _options.LastUsedModel = eArgs.LastUsedModel;
        _options.LastUsedConfigurationId = eArgs.LastUsedConfigurationId;
        _options.BasePath = eArgs.BasePath
            ?? throw new InvalidOperationException("BasePath is null. Please handle the RequestChatViewOptions to set a valid base path.");

        _conversationProcessor = await ConversationProcessor.FromFileAsync(
        _options.BasePath, filename);

        ConversationView.Conversation = _conversationProcessor.Conversation;
        foreach (ConversationItem item in ConversationView.Conversation.ConversationItems)
        {
            AddChatItem(
            item.IsResponse,
            item.MarkdownContent!);
        }

        PromptControl.Clear();
    }

    public Task SendRequestAsync(string verbalRequest)
    {
        PromptControl.Text = verbalRequest;
        return PromptControl.SendCommandAsync();
    }

    public Task<IEnumerable<string>?> QueryOpenAiModelNamesAsync()
    => _skCommunicator.QueryOpenAiModelNamesAsync();

    public Task<ModelInfo?> QueryOpenAiModelInfoAsync(string modelName)
        => _skCommunicator.QueryOpenAiModelInfoAsync(modelName);

    private void ConversationView_ConversationItemAdded(object? sender, ConversationItemAddedEventArgs e)
    {
        // We can save only with 2 items or more, since otherwise the Meta data
        // we retrieve from the meta data skComponent can't provide enough reliable
        // information.
        if (ConversationView.Conversation is not Conversation conversation
            || conversation.ConversationItems.Count < 2)
        {
            return;
        }

        // TODO: Calculate last turn-around time and save it in the Conversation itself.
        e.ConversationItem.FirstResponseDuration = ConversationView.Conversation.LastKickOffTime;
        e.ConversationItem.CompleteProcessDuration = DateTime.Now - conversation.DateCreated;
    }

    private async Task PromptControl_AsyncSendCommand(object sender, AsyncSendCommandEventArgs e)
    {
        string textToSend = e.CommandText ?? string.Empty;
        Conversation conversation = ConversationView.Conversation;

        conversation.LastKickOffTime = TimeSpan.Zero;
        conversation.DateLastEdited = DateTime.Now;

        if (conversation.ConversationItems.Count == 0)
        {
            conversation.Model = _options.LastUsedModel;
            conversation.IdPersonality = _options.LastUsedConfigurationId;
        }

        try
        {
            // First, we add our original question to the conversation view.
            // This, by the way, will then raise the ConversationItemAdded event,
            // where we then write the conversation to disc and update the tree view.
            ConversationView.AddConversationItem(textToSend, isResponse: false);

            // And then, we let the _skCommunicator "pump" it's partial results from the
            // async stream to the conversation view. When the answer is complete, we will be
            // getting a respective event a la WinForms. And that's where we can write the
            // conversation items on disc.
            IAsyncEnumerable<string> responses = ConversationView.UpdateCurrentResponseAsync(
                asyncEnumerable: _skCommunicator.RequestPromptResponseStreamAsync(
                    valueToProcess: textToSend,
                    keepChatHistory: !_requestClearChatHistory));

            _requestClearChatHistory = false;

            await ResponsePumpingAsync(responses);

            ChatInfoAITemplate? chatInfo = await GetChatMetaDataAsync(_skMetaDataProcessor.ChatHistory, textToSend);
            await OnAsyncNotifyRefreshMetaDataAsync(new AsyncNotifyRefreshedMetaDataEventArgs(chatInfo));

            // This updates the meta-data we just got with the current conversation.
            chatInfo?.Merge(conversation);

            _conversationProcessor ??= new(
                conversation: conversation,
                basePath: _options.BasePath!);

            await _conversationProcessor.SaveConversationAsync();


            PromptControl.Clear();
        }
        catch (Exception ex)
        {
            ExceptionDispatchInfo dispatchInfo = ExceptionDispatchInfo.Capture(ex);
            dispatchInfo.Throw();
        }


        // And this is actual pumping, which here does nothing.
        // This could be modified, for example to write logs or construct something.
        static async Task ResponsePumpingAsync(IAsyncEnumerable<string> responses)
        {
            await foreach (string? response in responses)
            {
                if (response is null)
                {
                    continue;
                }
            }
        }
    }

    // That's the task which gets the title from the conversation.
    private async Task<ChatInfoAITemplate?> GetChatMetaDataAsync(ChatHistory? chatHistory, string? text = default)
    {
        string chatContent = string.Empty;

        if (chatHistory is not null)
        {
            chatContent = GetJoinedChatHistory(chatHistory);
        }

        chatContent += "\n" + text;

        ChatInfoAITemplate? returnValue = await _skMetaDataProcessor.RequestStructuredResponseAsync<ChatInfoAITemplate?>(
            chatContent);

        return returnValue;
    }

    private static string GetJoinedChatHistory(ChatHistory? chatHistory) =>
        // Let's build one string from all the text items in the chat history:
        chatHistory is null
            ? string.Empty
            : string.Join("\n", chatHistory.Select(item => item.Content));

    private async Task RefreshMetaData(object? sender, EventArgs e)
    {
        try
        {
            // Update the title and summary of the conversation:
            ChatInfoAITemplate? metaData = await GetChatMetaDataAsync(_skCommunicator.ChatHistory);

            if (metaData is null)
            {
                return;
            }

            metaData.Merge(ConversationView.Conversation);

            AsyncNotifyRefreshedMetaDataEventArgs eventArgs = new(metaData);
            await OnAsyncNotifyRefreshMetaDataAsync(eventArgs);
        }
        catch (Exception ex)
        {
            AsyncNotifyRefreshedMetaDataEventArgs eventArgs = new(ex);
            await OnAsyncNotifyRefreshMetaDataAsync(eventArgs);
        }
    }

    private Task SKCommunicator_AsyncCodeBlockInfoProvided(object sender, AsyncCodeBlockInfoProvidedEventArgs e)
        => OnAsyncCodeBlockInfoProvidedAsync(e);

    protected virtual Task OnAsyncCodeBlockInfoProvidedAsync(AsyncCodeBlockInfoProvidedEventArgs e)
        => AsyncCodeBlockInfoProvided?.Invoke(this, e) ?? Task.CompletedTask;

    protected virtual Task OnAsyncNotifyRefreshMetaDataAsync(AsyncNotifyRefreshedMetaDataEventArgs e)
        => AsyncNotifyRefreshedMetaData?.Invoke(this, e) ?? Task.CompletedTask;

    protected virtual Task OnAsyncRequestFileExtractingSettingsAsync(AsyncRequestFileContextEventArgs e)
        => AsyncRequestFileExtractingSettings?.Invoke(this, e) ?? Task.CompletedTask;

    protected virtual Task OnAsyncNotifySaveChatAsync(AsyncRequestFileContextEventArgs e)
        => AsyncNotifySaveChat?.Invoke(this, e) ?? Task.CompletedTask;

    protected virtual void OnRequestChatViewOptions(RequestChatViewOptionsEventArgs e)
        => RequestChatViewOptions?.Invoke(this, e);
}
