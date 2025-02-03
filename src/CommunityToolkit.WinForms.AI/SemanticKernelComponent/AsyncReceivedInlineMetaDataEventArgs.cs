namespace CommunityToolkit.WinForms.AI;

public class AsyncReceivedInlineMetaDataEventArgs(string metaData, int textPosition) : EventArgs
{
    public string MetaData { get; } = metaData;
    public int TextPosition { get; } = textPosition;
}
