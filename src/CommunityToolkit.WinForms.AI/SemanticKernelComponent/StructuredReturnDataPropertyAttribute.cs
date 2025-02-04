namespace CommunityToolkit.WinForms.AI;

[AttributeUsage(AttributeTargets.Property)]
public class StructuredReturnDataPropertyAttribute(string? prompt = null) : Attribute
{
    public string? Prompt { get; } = prompt;
}
