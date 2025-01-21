using System.Globalization;

namespace CommunityToolkit.WinForms.AI;

[AttributeUsage(AttributeTargets.Class)]
public class StructuredReturnDataAttribute() : Attribute
{
    public string? Prompt { get; set; }
    public bool ProvideTime { get; set; }
    public bool ProvideDate { get; set; }
    public bool ProvideTimeZone { get; set; }
    public CultureInfo? FormatCulture { get; set; }
    public CultureInfo? LanguageCulture { get; set; }
}
