namespace CommunityToolkit.WinForms.Controls.Blazor;

public class ReceivedNextParagraphEventArgs(string paragraph) : EventArgs
{
    public string Paragraph { get; } = paragraph;
}
