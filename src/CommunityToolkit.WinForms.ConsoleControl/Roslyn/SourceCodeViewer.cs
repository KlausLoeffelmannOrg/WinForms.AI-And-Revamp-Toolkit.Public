using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Classification;
using Microsoft.CodeAnalysis.Text;
using System.ComponentModel;
using System.Text;

namespace CommunityToolkit.WinForms.Controls;

public class SourceCodeViewer : RichTextBox
{
    private Document? _codeDocument;
    private Workspace? _workspace;

    private static readonly Color DefaultDarkModeBackColor = Color.FromArgb(30, 30, 30);
    private static readonly Color DefaultDarkModeForeColor = Color.White;

    public SourceCodeViewer()
    {
        // Let's set the font to a monospaced font:
        Font = new("Consolas", 12);

        // Let's disable editing:
        ReadOnly = true;

        if (Application.IsDarkModeEnabled)
        {
            BackColor = DefaultDarkModeBackColor;
            ForeColor = DefaultDarkModeForeColor;
        }
    }

    public Document? Document => _codeDocument;

    private bool ShouldSerializeBackColor() 
        => Application.IsDarkModeEnabled 
            ? BackColor != DefaultDarkModeBackColor 
            : BackColor != DefaultBackColor;

    private bool ShouldSerializeForeColor()
        => Application.IsDarkModeEnabled
            ? ForeColor != DefaultDarkModeForeColor
            : ForeColor != DefaultForeColor;

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public new string Text
    {
        get => base.Text;
        private set => base.Text = value;
    }

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public new string? Rtf
    {
        get => base.Rtf;
        private set => base.Rtf = value;
    }

    public async Task SetSourceCodeAsync(string sourceCode, string filename)
    {
        if (string.IsNullOrEmpty(sourceCode))
        {
            throw new ArgumentException("Source code cannot be null or empty.", nameof(sourceCode));
        }
        if (string.IsNullOrEmpty(filename))
        {
            throw new ArgumentException("Filename cannot be null or empty.", nameof(filename));
        }

        AdhocWorkspace workspace = new();
        _workspace = workspace;

        Project project = workspace.AddProject("TempProject", LanguageNames.CSharp);
        Document document = workspace.AddDocument(project.Id, filename, SourceText.From(sourceCode));
        _codeDocument = document;

        string rtf = await ColorCodeRtf();
        Rtf = rtf;
    }

    private async Task<string> ColorCodeRtf()
    {
        if (_codeDocument == null)
        {
            throw new InvalidOperationException("Code document is not set.");
        }

        SourceText docText = await _codeDocument.GetTextAsync();

        var classifiedSpans = await Classifier
            .GetClassifiedSpansAsync(
                document: _codeDocument,
                textSpan: new TextSpan(0, docText.Length));

        StringBuilder rtfBuilder = new();
        rtfBuilder.Append(@"{\rtf1\ansi");

        foreach (var classifiedSpan in classifiedSpans)
        {
            var spanText = docText.ToString(classifiedSpan.TextSpan);
            var color = GetColorForClassification(classifiedSpan.ClassificationType);

            rtfBuilder.Append($@"\cf{color} {spanText}");
        }

        rtfBuilder.Append('}');
        return rtfBuilder.ToString();

        static int GetColorForClassification(string classificationType)
        {
            return classificationType switch
            {
                ClassificationTypeNames.Keyword => 1,
                ClassificationTypeNames.Identifier => 2,
                ClassificationTypeNames.Comment => 3,
                ClassificationTypeNames.StringLiteral => 4,
                _ => 0,
            };
        }
    }
}
