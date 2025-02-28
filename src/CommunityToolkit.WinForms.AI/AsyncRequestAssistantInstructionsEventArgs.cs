using Microsoft.SemanticKernel.Connectors.OpenAI;

namespace CommunityToolkit.WinForms.AI;


/// <summary>
///  Provides data for the async request execution settings event.
/// </summary>
/// <remarks>
///  <para>
///   This class is used to pass information about the <see cref="OpenAIPromptExecutionSettings"/> for the
///   asynchronous request assistance.
///  </para>
/// </remarks>
public class AsyncRequestExecutionSettingsEventArgs : EventArgs
{
    public AsyncRequestExecutionSettingsEventArgs(OpenAIPromptExecutionSettings executionSettings)
     : base()
    {
        ExecutionSettings = executionSettings;
    }

    /// <summary>
    ///  Gets or sets the <see cref="OpenAIPromptExecutionSettings"/>.
    /// </summary>
    public OpenAIPromptExecutionSettings ExecutionSettings { get; set; }
}
