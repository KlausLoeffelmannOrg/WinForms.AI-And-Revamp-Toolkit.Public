using System.Reflection;
using System.Text;

namespace Chatty;

partial class FrmAbout : Form
{
    public FrmAbout()
    {
        InitializeComponent();

        Text = String.Format("About {0}", AssemblyTitle);
        
        (int lines, _lblProductname.Text) = StairFormat(AssemblyProduct);
        _lblProductname.Height = (int)(_lblProductname.Font.Height * 1.2 * lines);

        _lblVersion.Text = String.Format("Version {0}", AssemblyVersion);
        _lblCopyRight.Text = AssemblyCopyright;
        _lblCompany.Text = $"Company: {AssemblyCompany}";
        _lblAuthors.Text = $"Authors: {AssemblyAuthors}";
        _txtDescription.Text = AssemblyDescription;
    }

    private static (int lines, string result) StairFormat(string assemblyProduct)
    {
        string[] words = assemblyProduct.Split(
            separator: [' ', '-'],
            options: StringSplitOptions.RemoveEmptyEntries);

        StringBuilder result = new();
        int indent = 0;

        foreach (string word in words)
        {
            result.AppendLine(
                new string(' ', indent) + word);

            indent += 4;
        }

        return (words.Length, result.ToString());
    }

    public static string AssemblyTitle =>
        Assembly
            .GetExecutingAssembly()
            .GetCustomAttribute<AssemblyTitleAttribute>()?
            .Title ?? Path.GetFileNameWithoutExtension(Assembly.GetExecutingAssembly().Location);

    public static string AssemblyVersion =>
        Assembly.GetExecutingAssembly().GetName().Version?.ToString() ?? string.Empty;

    public static string AssemblyAuthors
    {
        get
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            string? author = assembly
                .GetCustomAttributes<AssemblyMetadataAttribute>()
                .FirstOrDefault(a => a.Key == "Authors")?.Value;

            return author ?? string.Empty;
        }
    }

    public static string AssemblyDescription =>
        Assembly
            .GetExecutingAssembly()
            .GetCustomAttribute<AssemblyDescriptionAttribute>()?
            .Description ?? string.Empty;

    public static string AssemblyProduct =>
        Assembly
            .GetExecutingAssembly()
            .GetCustomAttribute<AssemblyProductAttribute>()?
            .Product ?? string.Empty;

    public static string AssemblyCopyright =>
        Assembly
            .GetExecutingAssembly()
            .GetCustomAttribute<AssemblyCopyrightAttribute>()?
            .Copyright ?? string.Empty;

    public static string AssemblyCompany =>
        Assembly
            .GetExecutingAssembly()
            .GetCustomAttribute<AssemblyCompanyAttribute>()?
            .Company ?? string.Empty;
}
