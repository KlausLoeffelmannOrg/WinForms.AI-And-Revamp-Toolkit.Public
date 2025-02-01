using CommunityToolkit.WinForms.Controls.Blazor;
using CommunityToolkit.WinForms.FluentUI.Controls;
using System.ComponentModel;

namespace Chatty.Views;

internal partial class ChatView : UserControl
{
    public event EventHandler? RefreshMetaData;
    public event EventHandler? ShowPromptControlChanged;
    public event EventHandler? ShowChatToolStripChanged;

    public ChatView()
    {
        InitializeComponent();
    }

    private void TsbRefreshMetaData_Click(object sender, EventArgs e) 
        => RefreshMetaData?.Invoke(this, EventArgs.Empty);

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
}
