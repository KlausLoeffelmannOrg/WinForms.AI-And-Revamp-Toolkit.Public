namespace CommunityToolkit.WinForms.AI;

[AttributeUsage(AttributeTargets.Property)]
public class StructuredReturnDataTypeAttribute(Type type) : Attribute
{
    public Type Type { get; } = type;
}
