using CommunityToolkit.WinForms.AI.ConverterLogic;

namespace CommunityToolkit.WinForms.AI;

public class AsyncReceivedInlineMetaDataEventArgs(ReturnStreamMetaData metaData) : EventArgs
{
    public ReturnStreamMetaData MetaData { get; } = metaData;
}
