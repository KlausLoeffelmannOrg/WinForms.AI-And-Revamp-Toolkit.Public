namespace CommunityToolkit.WinForms.Controls.Blazor;

public class ConversationItemAddedEventArgs(ConversationItemViewModel conversationItem) : EventArgs
{
    public ConversationItemViewModel ConversationItem { get; } = conversationItem;
}
