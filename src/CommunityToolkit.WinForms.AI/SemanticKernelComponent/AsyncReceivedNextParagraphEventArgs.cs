namespace CommunityToolkit.WinForms.AI;

public class AsyncReceivedNextParagraphEventArgs(string paragraph, int textPosition, bool isLastParagraph) : EventArgs
{
    public string Paragraph { get; } = paragraph;
    public int TextPosition { get; } = textPosition;
    public bool IsLastParagraph { get; } = isLastParagraph;
}
