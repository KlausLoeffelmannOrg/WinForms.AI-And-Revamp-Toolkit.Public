using CommunityToolkit.WinForms.AI;

namespace Chatty.Agents.ModelExplorer;

[StructuredReturnData(
    Prompt = ClassPrompt,
    ProvideDate = true,
    ProvideTime = true)]
public class AIClassicMercedes
{
    private const string ClassPrompt = """
        You are an assistant for listing interesting Mercedes-Benz cars in certain decades.
        Please provide a list of n cars, the exact number which the user provides, with the 
        data derived from the following schema information.
        """;

    [StructuredReturnDataProperty("* The model name of the Mercedes, like 280E")]
    public string? TypeName { get; set; }

    [StructuredReturnDataProperty("* The year of the car, like 1975")]
    public int Year { get; set; }

    [StructuredReturnDataProperty("* The engine size of the car, like 2.8L")]
    public string? EngineSize { get; set; }

    [StructuredReturnDataProperty("* The number of cylinders, like 6")]
    public int Cylinders { get; set; }

    [StructuredReturnDataProperty("* The horse Power")]
    public int? HorsePower { get; set; }
}
