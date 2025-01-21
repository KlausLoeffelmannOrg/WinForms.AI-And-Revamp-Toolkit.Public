namespace CommunityToolkit.WinForms.Controls.Blazor;

public class ConversationItemAddedEventArgs(ConversationItem conversationItem) : EventArgs
{
    public ConversationItem ConversationItem { get; } = conversationItem;
}
