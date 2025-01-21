namespace CommunityToolkit.WinForms.Controls.Blazor;

public class ConversationTitleChangedEventArgs(string conversationTitle) : EventArgs
{
    public string ConversationTitle { get; } = conversationTitle;
}
