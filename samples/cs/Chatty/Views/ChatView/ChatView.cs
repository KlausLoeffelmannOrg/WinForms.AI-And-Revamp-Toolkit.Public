using Chatty.DataEntities;
using Chatty.DataProcessing;
using CommunityToolkit.WinForms.AI;
using CommunityToolkit.WinForms.AsyncSupport;
using CommunityToolkit.WinForms.Controls.Blazor;
using CommunityToolkit.WinForms.FluentUI.Controls;
using Microsoft.CodeAnalysis;
using Microsoft.SemanticKernel.ChatCompletion;
using System.ComponentModel;
using System.Runtime.ExceptionServices;

namespace Chatty.Views;

public partial class ChatView : UserControl
{
    private static readonly string ApiKeyEnvironmentVarLookup = "AI:OpenAI:ApiKey";

    public const string DefaultCommunicatorModel = "gpt-4o";
    public const string DefaultMetaDataProcessorModel = "gpt-3.5-turbo";

    public event AsyncEventHandler<AsyncNotifyRefreshedMetaDataEventArgs>? AsyncNotifyRefreshedMetaData;
    public event AsyncEventHandler<AsyncListingFileAddedEventArgs>? AsyncListingFileAdded;
    public event AsyncEventHandler<AsyncRequestFileContextEventArgs>? AsyncRequestFileExtractingSettings;
    public event AsyncEventHandler<AsyncRequestFileContextEventArgs>? AsyncNotifySaveChat;

    public event EventHandler? ShowPromptControlChanged;
    public event EventHandler? ShowChatToolStripChanged;
    public event EventHandler? SystemPromptChanged;
    public event EventHandler? ModelNameChanged;
    public event EventHandler<RequestChatViewOptionsEventArgs>? RequestChatViewOptions;

    private ConversationProcessor _conversationProcessor = null!;
    private ChatViewOptions _options = null!;

    public ChatView()
    {
        InitializeComponent();
        ConversationView.ConversationItemAdded += ConversationView_ConversationItemAdded;
        PromptControl.AsyncSendCommand += PromptControl_AsyncSendCommand;

        // Wiring the delegate which provides the Open AI ApiKey when we need it:
        _skCommunicator.ApiKeyGetter =
            () => Environment.GetEnvironmentVariable(ApiKeyEnvironmentVarLookup)
                ?? throw new NullReferenceException("Could not retrieve Open AI ApiKey.");

        // We are wiring up the second one exactly like that.
        _skMetaDataProcessor.ApiKeyGetter = _skCommunicator.ApiKeyGetter;
        _skMetaDataProcessor.ModelName = DefaultMetaDataProcessorModel;
        _skCommunicator.ModelName = DefaultCommunicatorModel;
    }

    protected override void OnLoad(EventArgs e)
    {
        base.OnLoad(e);

        RequestChatViewOptionsEventArgs eArgs = new(null, string.Empty, Guid.Empty);
        OnRequestChatViewOptions(eArgs);
        _options = new ChatViewOptions(eArgs.BasePath, eArgs.LastUsedModel, eArgs.LastUsedConfigurationId);

        _skCommunicator.AsyncReceivedInlineMetaData += ConversationView_ReceivedInlineMetaDataAsync;
        _skCommunicator.AsyncReceivedNextParagraph += SKConversationView_ReceivedNextParagraph;
    }

    [DefaultValue(null)]
    public string? SystemPrompt
    {
        get => _skCommunicator.SystemPrompt;
        set
        {
            if (_skCommunicator.SystemPrompt == value)
            {
                return;
            }

            _skCommunicator.SystemPrompt = value;
            OnSystemPromptChanged(EventArgs.Empty);
        }
    }

    protected virtual void OnSystemPromptChanged(EventArgs e)
        => SystemPromptChanged?.Invoke(this, e);

    [DefaultValue(DefaultCommunicatorModel)]
    public string ModelName
    {
        get => _skCommunicator.ModelName;
        set
        {
            if (_skCommunicator.ModelName == value)
            {
                return;
            }
            _skCommunicator.ModelName = value;
            OnModelNameChanged(EventArgs.Empty);
        }
    }

    protected virtual void OnModelNameChanged(EventArgs e)
        => ModelNameChanged?.Invoke(this, e);

    public ConversationView ConversationView
        => _conversationView;

    public AutoCompleteEditor PromptControl
        => _promptControl;

    public ToolStrip ChatToolStrip
        => _chatToolStrip;

    [DefaultValue(true)]
    public bool ShowChatToolStrip
    {
        get => _chatToolStrip.Visible;
        set
        {
            if (_chatToolStrip.Visible == value)
            {
                return;
            }
            _chatToolStrip.Visible = value;
            OnShowChatToolStripChanged(EventArgs.Empty);
        }
    }

    protected virtual void OnShowChatToolStripChanged(EventArgs e)
    {
        // We need to re-layout:
        PerformLayout();
        ShowChatToolStripChanged?.Invoke(this, e);
    }

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

    protected virtual void OnShowPromptControlChanged(EventArgs e)
    {
        // We need to re-layout:
        PerformLayout();
        ShowPromptControlChanged?.Invoke(this, e);
    }

    public void StartNewChat()
    {
        ConversationView.ClearHistory();
        ConversationView.Conversation.Title = Conversation.GetDefaultTitle();
    }

    public void AddChatItem(bool isResponse, string message) 
        => _skCommunicator.AddChatItem(isResponse, message);

    public async Task LoadConversationAsync(string filename)
    {
        var eArgs = new RequestChatViewOptionsEventArgs(_options);
        OnRequestChatViewOptions(eArgs);

        _options.LastUsedModel = eArgs.LastUsedModel;
        _options.LastUsedConfigurationId = eArgs.LastUsedConfigurationId;
        _options.BasePath = eArgs.BasePath
            ?? throw new InvalidOperationException("BasePath is null. Please handle the RequestChatViewOptions to set a valid base path.");

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
        string textToSend = e.CommandText;
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
                asyncEnumerable: _skCommunicator.RequestPromptResponseStreamAsync(textToSend, true));

            await ResponsePumpingAsync(responses);

            ChatMetaData? chatMetaData = await GetChatMetaDataAsync(_skCommunicator.ChatHistory, textToSend);
            await OnAsyncNotifyRefreshMetaDataAsync(new AsyncNotifyRefreshedMetaDataEventArgs(chatMetaData));

            // This updates the meta-data we just got with the current conversation.
            chatMetaData?.Merge(conversation);

            _conversationProcessor ??= new(
                conversation: conversation,
                basePath: _options.BasePath!);

            await _conversationProcessor.SaveConversationAsync();


            PromptControl.Clear();
        }
        catch (Exception ex)
        {
            var dispatchInfo = ExceptionDispatchInfo.Capture(ex);
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
    private async Task<ChatMetaData?> GetChatMetaDataAsync(ChatHistory? chatHistory, string? text = default)
    {
        string chatContent = string.Empty;

        if (chatHistory is not null)
        {
            chatContent = GetJoinedChatHistory(chatHistory);
        }

        chatContent += "\n" + text;

        ChatMetaData? returnValue = await _skMetaDataProcessor.RequestStructuredResponseAsync<ChatMetaData?>(
            chatContent,
            CancellationToken.None);

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
            ChatMetaData? metaData = await GetChatMetaDataAsync(_skCommunicator.ChatHistory);

            if (metaData is null)
            {
                return;
            }

            metaData.Merge(ConversationView.Conversation);

            var eventArgs = new AsyncNotifyRefreshedMetaDataEventArgs(metaData);
            await OnAsyncNotifyRefreshMetaDataAsync(eventArgs);
        }
        catch (Exception ex)
        {
            var eventArgs = new AsyncNotifyRefreshedMetaDataEventArgs(ex);
            await OnAsyncNotifyRefreshMetaDataAsync(eventArgs);
        }
    }

    protected virtual Task OnAsyncNotifyRefreshMetaDataAsync(AsyncNotifyRefreshedMetaDataEventArgs e)
        => AsyncNotifyRefreshedMetaData?.Invoke(this, e) ?? Task.CompletedTask;

    protected virtual Task OnAsyncListingFileAddedAsync(AsyncListingFileAddedEventArgs e)
        => AsyncListingFileAdded?.Invoke(this, e) ?? Task.CompletedTask;

    protected virtual Task OnAsyncRequestFileExtractingSettingsAsync(AsyncRequestFileContextEventArgs e)
        => AsyncRequestFileExtractingSettings?.Invoke(this, e) ?? Task.CompletedTask;

    protected virtual Task OnAsyncNotifySaveChatAsync(AsyncRequestFileContextEventArgs e)
        => AsyncNotifySaveChat?.Invoke(this, e) ?? Task.CompletedTask;

    protected virtual void OnRequestChatViewOptions(RequestChatViewOptionsEventArgs e)
        => RequestChatViewOptions?.Invoke(this, e);
}
