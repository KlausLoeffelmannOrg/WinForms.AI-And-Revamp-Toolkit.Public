using CommunityToolkit.WinForms.Controls.Blazor;
using SemanticKernelDemo.Controls;
using System.Diagnostics.CodeAnalysis;

namespace Chatty.Views;

internal partial class ChatView: UserControl
{
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
}
