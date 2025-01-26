namespace CommunityToolkit.WinForms.Controls.Blazor;

public class ReceivedNextParagraphEventArgs(string paragraph, int textPosition) : EventArgs
{
    public string Paragraph { get; } = paragraph;
    public int TextPosition { get; } = textPosition;
}

public class ReceivedMetaDataEventArgs(string metaData, int textPosition) : EventArgs
{
    public string MetaData { get; } = metaData;
    public int TextPosition { get; } = textPosition;
}
