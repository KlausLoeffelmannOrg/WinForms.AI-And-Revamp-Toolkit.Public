using CommunityToolkit.WinForms.Controls.Blazor;
using SemanticKernelDemo.Controls;

namespace Chatty.Views;

internal partial class ChatView : UserControl
{
    public event EventHandler? RefreshMetaData;

    public ChatView()
    {
        InitializeComponent();
    }

    public ConversationView ConversationView
        => _conversationView;

    public AsyncPromptControl PromptControl
        => _promptControl;

    public ToolStrip ChatToolStrip
        => _chatToolStrip;

    private void TsbRefreshMetaData_Click(object sender, EventArgs e)
    {
        RefreshMetaData?.Invoke(this, EventArgs.Empty);
    }
}
