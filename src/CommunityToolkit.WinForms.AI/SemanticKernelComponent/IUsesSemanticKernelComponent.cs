namespace CommunityToolkit.WinForms.AI.SemanticKernel;

public interface IUsesSemanticKernelComponent
{
    protected Func<SemanticKernelComponent>? SemanticKernelGetter { get; set; }

    public SemanticKernelComponent SemanticKernel => SemanticKernelGetter?.Invoke() 
        ?? throw new InvalidOperationException("SemanticKernelGetter is not set.");

    public Func<string>? OpenAiModelId
    {
        get => SemanticKernel.ApiKeyGetter;
        set => SemanticKernel.ApiKeyGetter = value;
    }

    public string ModelName
    {
        get => SemanticKernel.ModelId;
        set => SemanticKernel.ModelId = value;
    }

    private void Initialize()
    {
        SemanticKernel.MaxTokens = 100;
        SemanticKernel.Temperature = 0.7;
        SemanticKernel.TopP = 1.0;
        SemanticKernel.FrequencyPenalty = 0.0;
        SemanticKernel.PresencePenalty = 0.0;
        SemanticKernel.JsonSchemaString = string.Empty;
        SemanticKernel.SystemPrompt = string.Empty;
    }

    static protected void ProvidePropertyProcessingDelegates(
        IUsesSemanticKernelComponent instance,
        Func<SemanticKernelComponent> semanticKernelGetter)
    {
        instance.SemanticKernelGetter = semanticKernelGetter;
    }
}
