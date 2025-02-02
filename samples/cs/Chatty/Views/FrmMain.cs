using Chatty.DataEntities;
using Chatty.DataProcessing;
using Chatty.ViewModels;
using Chatty.Views;
using CommunityToolkit.WinForms.AI;
using CommunityToolkit.WinForms.ComponentModel;
using CommunityToolkit.WinForms.Controls.Blazor;
using CommunityToolkit.WinForms.Extensions;
using CommunityToolkit.WinForms.FluentUI.Controls;
using Microsoft.SemanticKernel.ChatCompletion;
using System.Runtime.ExceptionServices;

namespace Chatty;

public partial class FrmMain : Form
{
    private const string Key_MainView_Bounds = nameof(Key_MainView_Bounds);
    private const string Key_Options = nameof(Key_Options);

    private static readonly string ApiKeyEnvironmentVarLookup = "AI:OpenAI:ApiKey";
    private readonly Dictionary<KnownNode, TreeNode> _knownNodes = [];

    private readonly IUserSettingsService _settingsService;

    private OptionsViewModel _options = null!;
    private PersonalityViewModel _personalities = null!;
    private IEnumerable<string>? _openAIModels;
    private TreeNode? _currentNode;
    private ConversationProcessor _conversationProcessor = null!;
    private string? _lastListingTitle;
    private string? _lastListingFilename;
    private readonly CancellationTokenSource _shutDownCancellation = new();

    private readonly ChatView _chatView;

    public FrmMain()
    {
        InitializeComponent();

        CreateKnownTreeViewNodes();

        Application.ThreadException += (s, e) =>
        {
            ReportToStatusBarInfo(
                message: e.Exception.Message,
                toolTipText: e.Exception.Message,
                additionalInfo: e.Exception.StackTrace,
                critical: true);
        };

        _chatView = new();
        _settingsService = WinFormsUserSettingsService.CreateAndLoad();

        // Wiring the delegate which provides the Open AI ApiKey when we need it:
        _skCommunicator.ApiKeyGetter =
            () => Environment.GetEnvironmentVariable(ApiKeyEnvironmentVarLookup)
                ?? throw new NullReferenceException("Could not retrieve Open AI ApiKey.");

        // We are wiring up the second one exactly like that.
        _skMetaDataProcessor.ApiKeyGetter = _skCommunicator.ApiKeyGetter;
        _skMetaDataProcessor.ModelName = "gpt-3.5-turbo";

        // Handle changing the personalities:
        _tscPersonalities.SelectedIndexChanged += (s, e) =>
        {
            // Lookup the selected personality:
            PersonalityItemViewModel selectedPersonality = _tscPersonalities.SelectedItem as PersonalityItemViewModel
                ?? throw new NullReferenceException("Personality disorder exception!");

            _skCommunicator.SystemPrompt = selectedPersonality.SystemPrompt;
        };

        _tscModels.SelectedIndexChanged += (s, e) =>
        {
            _options.LastUsedModel = _tscModels.SelectedItem as string
                ?? throw new NullReferenceException("Models disorder exception!");

            _skCommunicator.ModelName = _options.LastUsedModel;
        };
    }

    protected async override void OnLoad(EventArgs e)
    {
        base.OnLoad(e);

        _skCommunicator.AsyncReceivedMetaData += AsyncConversationView_ReceivedMetaData;
        _skCommunicator.AsyncReceivedNextParagraph += SKConversationView_ReceivedNextParagraph;
        _chatView.ConversationView.ConversationItemAdded += ConversationView_ConversationItemAdded;
        _chatView.PromptControl.AsyncSendCommand += PromptControl_AsyncSendCommand;
        _chatView.RefreshMetaData += ChatView_RefreshMetaData;

        _mainTabControl.AddTab("Conversation", _chatView);

        _lblConversationTitle.Text = $"New chat";
        _lblDate.Text = $"{DateTime.Now:dddd, MMM dd yyyy - HH:mm}";

        Rectangle bounds = _settingsService.GetInstance(
            Key_MainView_Bounds,
            this.CenterOnScreen(
                horizontalFillGrade: 80,
                verticalFillGrade: 80));

        Bounds = bounds;

        _options = _settingsService.GetInstance(
            key: Key_Options,
            defaultValue: new OptionsViewModel());

        _personalities = PersonalityViewModel
            .GetPersonalitiesOrDefault(_options.BasePath);

        RebuildPersonalitiesDropDown(_options.LastUsedIdPersonality);
        UpdateTreeView();

        await SetupOpenAIModelsAsync();
        await ShowTimeAsync(_shutDownCancellation.Token);

        // The loop which is showing the time is running in the background.
        async Task ShowTimeAsync(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                await Task.Delay(500, token).ConfigureAwait(false);
                await InvokeAsync(() =>
                {
                    _tslClockInfo.Text = $"{DateTime.Now:dddd, MMM dd yyyy - HH:mm:ss}";
                }, token);
            }
        }
    }

    private async Task PromptControl_AsyncSendCommand(object sender, AsyncSendCommandEventArgs e)
    {
        string textToSend = e.CommandText;
        Conversation conversation = _chatView.ConversationView.Conversation;

        conversation.LastKickOffTime = TimeSpan.Zero;
        conversation.DateLastEdited = DateTime.Now;

        if (conversation.ConversationItems.Count == 0)
        {
            conversation.Model = _options.LastUsedModel;
            conversation.IdPersonality = _options.LastUsedIdPersonality;
        }

        try
        {
            // First, we add our original question to the conversation view.
            // This, by the way, will then raise the ConversationItemAdded event,
            // where we then write the conversation to disc and update the tree view.
            _chatView.ConversationView.AddConversationItem(textToSend, isResponse: false);

            // And then, we let the _skCommunicator "pump" it's partial results from the
            // async stream to the conversation view. When the answer is complete, we will be
            // getting a respective event a la WinForms. And that's where we can write the
            // conversation items on disc.
            IAsyncEnumerable<string> responses = _chatView.ConversationView.UpdateCurrentResponseAsync(
                asyncEnumerable: _skCommunicator.RequestPromptResponseStreamAsync(textToSend, true));

            await ResponsePumpingAsync(responses);

            ChatMetaData? chatMetaData = await GetChatMetaDataAsync(_skCommunicator.ChatHistory, textToSend);

            // This updates the meta-data we just got with the current conversation.
            chatMetaData?.Merge(conversation);

            _conversationProcessor ??= new(
                conversation: conversation,
                basePath: _options.BasePath);

            await _conversationProcessor.SaveConversationAsync();

            // Iterate through any additional open tabs and save the respective listing files:
            foreach (Panel tabPage in _mainTabControl.Tabs)
            {
                if (tabPage.Controls[0] is not RoslynSourceView sourceViewer)
                {
                    continue;
                }

                await sourceViewer.SaveFileAsync(_conversationProcessor?.ConversationBaseFolder 
                    ?? throw new NullReferenceException(nameof(_conversationProcessor)));
            }

            await _lblConversationTitle.InvokeAsync(() =>
            {
                _lblConversationTitle.Text = chatMetaData?.ChatTitle;
                _currentNode = UpdateTreeView(_chatView.ConversationView.Conversation.Id);
                UpdateStatusBar(conversation);
            });

            _chatView.PromptControl.Clear();
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

    protected override void OnFormClosing(FormClosingEventArgs e)
    {
        // Every async thing which is still active, please stop what you are doing!
        _shutDownCancellation.Cancel();

        base.OnFormClosing(e);
    }

    protected override void OnFormClosed(FormClosedEventArgs e)
    {
        base.OnFormClosed(e);

        _settingsService.SetInstance(Key_MainView_Bounds, this.GetRestorableBounds());
        _settingsService.SetInstance(Key_Options, _options);
        _settingsService.Save();
    }

    private void ConversationView_ConversationItemAdded(object? sender, ConversationItemAddedEventArgs e)
    {
        // We can save only with 2 items or more, since otherwise the Meta data
        // we retrieve from the meta data skComponent can't provide enough reliable
        // information.
        if (_chatView.ConversationView.Conversation is not Conversation conversation
            || conversation.ConversationItems.Count < 2)
        {
            return;
        }

        e.ConversationItem.FirstResponseDuration = _chatView.ConversationView.Conversation.LastKickOffTime;
        e.ConversationItem.CompleteProcessDuration = DateTime.Now - conversation.DateCreated;
    }

    private Task AsyncConversationView_ReceivedMetaData(object? sender, AsyncReceivedMetaDataEventArgs e)
    {
        if (e.MetaData.Contains(':'))
        {
            _lastListingTitle = e.MetaData;
        }
        else
        {
            _lastListingFilename = e.MetaData;
        }

        if (_conversationProcessor is null)
        {
            return Task.CompletedTask;
        }

        _conversationProcessor.CurrentListingDescription = _lastListingTitle ?? string.Empty;
        _conversationProcessor.CurrentListingFilename = _lastListingFilename ?? string.Empty;

        return Task.CompletedTask;
    }

    private async Task SKConversationView_ReceivedNextParagraph(object? sender, AsyncReceivedNextParagraphEventArgs e)
    {
        string additionalResourceText = """
            By default, no prompt asked for an extended format of the listing headers in Mark Down.
            Listing Extraction is only possible, if the resulting Markdown formatted listing includes
            ```csharp{{filename:myfilename.cs}}{{title:My Title}}.
            
            The resulting Markdown of the prompt did not include this information, which means
            that the prompt did not ask for an extended format of the listing headers in Mark Down.
            
            Rewrite the prompt with regards to this and check, if the result satisfies the requirements.
            """;

        if (_conversationProcessor is null)
        {
            _conversationProcessor = new(
                conversation: _chatView.ConversationView.Conversation,
                _options.BasePath);

            _conversationProcessor.AsyncListingFileAdded += ConversationProcessor_ListingFileAdded;
        }

        await _conversationProcessor.HandleNewParagraphAsync(
            paragraph: e.Paragraph,
            textPosition: e.TextPosition,
            isLastParagraph: e.IsLastParagraph);

        async Task ConversationProcessor_ListingFileAdded(object? sender, AsyncListingFileAddedEventArgs e)
        {
            // If we don't have a filename, it's likely the user picked a profile, which returned
            // a different format. In that case, the prompt is missing the ask to include
            // meta-information in the response like {{filename:myfilename.cs}} or {{title:My Title}}.
            if (string.IsNullOrEmpty(_lastListingFilename))
            {
                // We now report that as a warning to the user:
                ReportToStatusBarInfo(
                    message: "The profile you selected does not support listing extraction.",
                    toolTipText: "The profile you selected does not support the meta-information.",
                    additionalResourceText,
                    critical: true);
            }

            try
            {
                RoslynSourceView sourceViewer = null!;

                await InvokeAsync(() =>
                {
                    // Let's add another Tab to the TabControl:
                    sourceViewer = new();

                    _mainTabControl.AddTab(
                        tabPageTitle: _lastListingFilename ?? throw new NullReferenceException(nameof(_lastListingFilename)),
                        tabContent: sourceViewer);

                    e.ListingFile.FileName = _lastListingFilename;
                    e.ListingFile.ListingTitle = _lastListingTitle;
                });

                await sourceViewer.SetListingFileAsync(
                    listingFile: e.ListingFile);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }

    private async void ChatView_RefreshMetaData(object? sender, EventArgs e)
    {
        try
        {
            // Update the title and summary of the conversation:
            var metaData = await GetChatMetaDataAsync(_skCommunicator.ChatHistory);

            if (metaData is null)
            {
                return;
            }

            metaData.Merge(_chatView.ConversationView.Conversation);

            await InvokeAsync(() =>
            {
                _currentNode = UpdateTreeView(_chatView.ConversationView.Conversation.Id);
            });
        }
        catch (Exception ex)
        {
            await InvokeAsync(() =>
            {
                _tslItemDateInfoCaption.Text = $"Failed to refresh metadata: {ex.Message}";
                _tslItemDateInfoCaption.ToolTipText = ex.StackTrace;
                _tslItemDateInfoCaption.ForeColor = Color.Red;
            });
        }
    }

    private async Task SetupOpenAIModelsAsync()
    {
        _openAIModels = await _skCommunicator.QueryOpenAiModelNamesAsync();

        if (_openAIModels is null)
        {
            return;
        }

        _openAIModels = _openAIModels.Order();

        _tscModels.Items.Clear();
        _tscModels.Items.AddRange([.. _openAIModels]);
        _tscModels.SelectedItem = _options.LastUsedModel;
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

    private void BtnStartNewChat_Click(object sender, EventArgs e)
    {
        _chatView.ConversationView.ClearHistory();
        _chatView.ConversationView.Conversation.Title = Conversation.GetDefaultTitle();
        _skCommunicator?.ChatHistory?.Clear();
    }

    private void Personalities_Click(object sender, EventArgs e)
    {
        // Let's open the personality editor:
        FrmManagePersonalities editor = new(_personalities);

        if (editor.ShowDialog(this) == DialogResult.OK)
        {
            _personalities = editor.Personalities;
            RebuildPersonalitiesDropDown(_options.LastUsedIdPersonality);
        }
    }

    private void RebuildPersonalitiesDropDown(Guid lastUsedIdPersonality)
    {
        // Let's remember the currently selected ID:
        Guid selectedId = _tscPersonalities.SelectedItem is PersonalityItemViewModel selectedPersonality
            ? selectedPersonality.Id
            : lastUsedIdPersonality;

        _tscPersonalities.Items.Clear();
        _tscPersonalities.Items.AddRange([.. _personalities.Personalities]);

        // If the remembered Guid is Empty, let's select the first one:
        if (selectedId == Guid.Empty && _tscPersonalities.Items.Count > 0)
        {
            _tscPersonalities.SelectedIndex = 0;
            return;
        }

        try
        {
            // And select the one we had before:
            _tscPersonalities.SelectedItem = _tscPersonalities.Items
                .OfType<PersonalityItemViewModel>()
                .FirstOrDefault(item => item.Id == selectedId);
        }
        catch (Exception)
        {
            _tscPersonalities.SelectedIndex = 0;
        }
    }

    private void CreateKnownTreeViewNodes()
    {
        foreach (KnownNode knownNode in Enum.GetValues<KnownNode>())
        {
            TreeNode node = _trvConversationHistory.Nodes.Add(knownNode.ToString());
            node.NodeFont = new Font(_trvConversationHistory.Font, FontStyle.Bold);
            _knownNodes[knownNode] = node;
        }
    }

    private void TscPersonalities_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (_tscPersonalities.SelectedItem is not PersonalityItemViewModel selectedPersonality)
        {
            return;
        }

        _options.LastUsedIdPersonality = selectedPersonality.Id;
    }

    private void TrvConversationHistory_AfterSelect(object sender, TreeViewEventArgs e)
    {
        if (e.Node?.Tag is not Conversation conversation)
        {
            return;
        }

        _currentNode = e.Node;

        // We're updating the status bar:
        UpdateStatusBar(conversation);
    }
}
