using static CommunityToolkit.WinForms.AI.ModelInfo;

namespace CommunityToolkit.WinForms.AI;
#pragma warning restore SKEXP0010 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.


public class ModelInfo(string Id, string Object, int Created, string OwnedBy, Usage? Usage)
{
    public class Usage(int? MaxTokens)
    {
        public int? MaxTokens { get; } = MaxTokens;
    }

    public string Id { get; } = Id;
    public string Object { get; } = Object;
    public int Created { get; } = Created;
    public string OwnedBy { get; } = OwnedBy;
    public Usage? TokenUsage { get; } = Usage;
}
