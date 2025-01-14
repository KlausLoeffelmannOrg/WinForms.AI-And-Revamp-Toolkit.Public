using Chatty.ViewModels;
using Chatty.Views;
using CommunityToolkit.WinForms.ComponentModel;
using CommunityToolkit.WinForms.Extensions;

namespace Chatty;

public partial class FrmMain : Form
{
    private static readonly string ApiKeyEnvironmentVarLookup = "AI:OpenAI:ApiKey";
    private readonly IUserSettingsService _settingsService;

    private OptionsViewModel? _options;

    public FrmMain()
    {
        InitializeComponent();
        _settingsService = WinFormsUserSettingsService.CreateAndLoad();

        // Wiring the delegate which provides the Open AI ApiKey when we need it:
        _skCommunicator.ApiKeyGetter =
            () => Environment.GetEnvironmentVariable(ApiKeyEnvironmentVarLookup)
                ?? throw new NullReferenceException("Could not retrieve Open AI ApiKey.");

        // We are wiring up the second on exactly like that.
        _skMetaDataProcessor.ApiKeyGetter = _skCommunicator.ApiKeyGetter;

        // The system-prompt for getting the Meta-Info is kind of an easy one.
        _skMetaDataProcessor.SystemPrompt = "You are an assistant in an LLM Chat-Bot client software. " +
            "You're main task is to provide a good title for an ongoing conversation " +
            "with maximal 40 characters based on the initial discussions." +
            "If the provided conversation string would result in a result too generic or meaningless, " +
            "please return '***TOO GENERIC***'.";

        // Setting up the "personality" ComboBox:
        _tscPersonalities.Items.AddRange([.. s_personalities.Keys]);

        // Handle changing the personalities:
        _tscPersonalities.SelectedIndexChanged += (s, e) =>
        {
            // Lookup the selected personality:
            string selectedPersonality = _tscPersonalities.SelectedItem as string
                ?? throw new NullReferenceException("Personality disorder exception!");

            _skCommunicator.SystemPrompt = s_personalities[selectedPersonality];
        };
    }

    protected override void OnLoad(EventArgs e)
    {
        base.OnLoad(e);

        // Preselect the first personality once the form has completely loaded:
        _tscPersonalities.SelectedIndex = 0;

        Rectangle bounds = _settingsService.GetInstance(
            "bounds",
            this.CenterOnScreen(
                horizontalFillGrade: 80,
                verticalFillGrade: 80));

        Bounds = bounds;

        _options = _settingsService.GetInstance(
            key: "options",
            defaultValue: new OptionsViewModel());
    }

    protected override void OnFormClosed(FormClosedEventArgs e)
    {
        base.OnFormClosed(e);

        _settingsService.SetInstance("bounds", this.GetRestorableBounds());
        _settingsService.SetInstance("options", _options);
        _settingsService.Save();
    }

    private async Task PromptControl_AsyncSendPrompt(object sender, EventArgs e)
    {
        string textToSend = _promptControl.Text;
        _promptControl.Clear();
        
        if (_conversationView.Conversation.Title is null)
        {
            // We use the second component to get a second session,
            // to get a good title from the AI:
            await TryUpdateTitleFromConversationAsync(textToSend);
        }

        _conversationView.AddConversationItem(textToSend, isResponse: false);

        var responses = _conversationView.UpdateCurrentResponseAsync(
            asyncEnumerable: _skCommunicator.RequestPromptResponseStreamAsync(textToSend, true));

        // If you wanted to examine the responses one by one, here's how you could do it:
        await foreach (var response in responses)
        {
            if (response is null)
            {
                continue;
            }
        }

        async Task TryUpdateTitleFromConversationAsync(string conversationFragments)
        {
            var title = await _skMetaDataProcessor.RequestTextPromptResponseAsync(textToSend, false);
            if (title=="***TOO GENERIC***")
            {
                return;
            }

            _conversationView.Conversation.Title = title;
        }
    }

    private async void PromptControl_NewPromptSuggestionRequest(object sender, EventArgs e)
    {
        // string funnyRandomPrompt = FunnyPromptsProvider.GetRandomPrompt();
        // _promptControl.Text = funnyRandomPrompt;

        // We use a "side-affect" of InvokeAsync to schedule this,
        // so that the text is set _before_ we select it.
        await _promptControl.InvokeAsync(() => _promptControl.SelectAll());
    }

    private void BtnClearHistory_Click(object sender, EventArgs e)
    {
        _conversationView.ClearHistory();
        _skCommunicator?.ChatHistory?.Clear();
    }

    private void TsmOptions_Click(object sender, EventArgs e)
    {
        using FrmOptions frmOptions = new(_options!);
        if (frmOptions.ShowDialog() == DialogResult.OK)
        {
            _options = frmOptions.Options;
        }
    }

    private void About_Click(object sender, EventArgs e)
    {
        using FrmAbout about = new();
        about.ShowDialog();
    }

    private static readonly Dictionary<string, string> s_personalities = new()
    {
        {
            "Default",
            "You are a friendly, versatile Assistant, trying to address all requests to the best of your abilities, "
          + "and you are engaged, even if folks just want to chat. But you politely decline requests that are inappropriate or impolite."
        },
        {
            "Funny/Ironic",
            "You are a witty and ironic Assistant, always ready with a playful quip or punchline. Provide helpful answers, "
          + "but pepper them with dry humor or lighthearted irony. Keep responses fun, yet respectful."
        },
        {
            "C#/VB Coding Assistant",
            "You are a specialized Coding Assistant for VB and C#. You handle these topics with expertise and "
            + "politely decline or redirect any non-C#/VB topics. Take into account, that the questions you will be asked, are not necessarily only from the view of the user, "
            + "but also from the view of team members, who are working here at Microsoft. That means, it's not always preferable to"
            + "look for established best practices, but may for ways to come up with new and improved ways for existing"
            + "WinForms features to ultimately establish new best practices."
        },
        {
            "Shakespearean",
            "You are a Shakespearean bard. Speak in the style of Elizabethan English, employing flowery prose and "
          + "theatrical flourishes when you respond, yet strive to remain clear in your meaning."
        },
        {
            "Sarcastic/Grumpy",
            "You are sarcastic and even funny-grumpy. You answer questions directly, but with a hint of annoyance or a sarcastic quip. "
          + "Keep the humor gentle—don’t cross the line into being offensive, but definitely be sassy!"
        },
        {
            "Translate to Klingon",
            "You are a Klingon translator. Try to provide a valid translation into Klingon, "
          + "and add comments on to best pronounce it or that the entered phrased had to be paraphrased."
          + "Please return the actual translated phrase in bold and italic, and additional explanations in a standard font."
        }
    };
}
