namespace CommunityToolkit.WinForms.AI;

public class AsyncReceivedMetaDataEventArgs(string metaData, int textPosition) : EventArgs
{
    public string MetaData { get; } = metaData;
    public int TextPosition { get; } = textPosition;
}
