using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Classification;
using Microsoft.CodeAnalysis.Formatting;
using Microsoft.CodeAnalysis.Text;
using System.ComponentModel;
using System.Text;

namespace CommunityToolkit.WinForms.Controls;

public class SourceCodeViewer : RichTextBox
{
    private Document? _codeDocument;
    private Workspace? _workspace;

    private const string CodeFontName= "Cascadia Code";

    private static readonly Color DefaultDarkModeBackColor = Color.FromArgb(30, 30, 30);
    private static readonly Color DefaultDarkModeForeColor = Color.White;

    public SourceCodeViewer()
    {
        // Let's disable editing:
        ReadOnly = true;

        if (Application.IsDarkModeEnabled)
        {
            BackColor = DefaultDarkModeBackColor;
            ForeColor = DefaultDarkModeForeColor;
        }
    }

    public Document? Document => _codeDocument;

    protected override void OnParentChanged(EventArgs e)
    {
        base.OnParentChanged(e);

        // When we get a new Parent, we set the Font 2 points bigger than the parent's font,
        // and we are also going for a monospaced font:
        if (Parent is not null)
        {
            Font = new Font(CodeFontName, Parent.Font.Size + 2);
        }
    }

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

    // We shadow the Font property, because we don't want to serialize it.
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public new Font Font
    {
        get => base.Font;
        set => base.Font = value;
    }

    public async Task SetSourceCodeAsync(string sourceCode, string? filename)
    {
        if (string.IsNullOrEmpty(sourceCode))
        {
            throw new ArgumentException("Source code cannot be null or empty.", nameof(sourceCode));
        }

        if (string.IsNullOrEmpty(filename))
        {
            filename = "* untitled";
        }

        AdhocWorkspace workspace = new();
        _workspace = workspace;

        Project project = workspace.AddProject("TempProject", LanguageNames.CSharp);
        Document document = workspace.AddDocument(project.Id, filename, SourceText.From(sourceCode));

        // We need to format the document now:
        document = await Formatter.FormatAsync(document);

        _codeDocument = document;

        string rtf = await ColorCodeRtf();
        Rtf = rtf;
    }

    private async Task<string> ColorCodeRtf()
    {
        if (_codeDocument is null)
        {
            throw new InvalidOperationException("Code document is not set.");
        }

        SourceText docText = await _codeDocument.GetTextAsync();
        var classifiedSpans = await Classifier.GetClassifiedSpansAsync(
            _codeDocument,
            new TextSpan(0, docText.Length));

        // We'll define five slots in the color table:
        // Index 1 = Blue, 2 = DarkGreen, 3 = Gray, 4 = Red
        StringBuilder rtfBuilder = new();
        rtfBuilder.Append(@"{\rtf1\ansi");
        rtfBuilder.Append(@"{\colortbl ;\red0\green0\blue255;\red0\green128\blue0;\red128\green128\blue128;\red255\green0\blue0;}");

        // Start writing the content
        foreach (var classifiedSpan in classifiedSpans)
        {
            var spanText = docText.ToString(classifiedSpan.TextSpan);

            // Escape the text for RTF. 
            // This is a simple approach that handles braces and backslashes.
            spanText = EscapeForRtf(spanText);

            var colorIndex = GetColorForClassification(classifiedSpan.ClassificationType);
            rtfBuilder.Append(@"\cf");
            rtfBuilder.Append(colorIndex);
            rtfBuilder.Append(' ');
            rtfBuilder.Append(spanText);
        }

        rtfBuilder.Append("}");
        return rtfBuilder.ToString();

        static int GetColorForClassification(string classificationType)
            => classificationType switch
            {
                ClassificationTypeNames.Keyword => 1,
                ClassificationTypeNames.Identifier => 2,
                ClassificationTypeNames.Comment => 3,
                ClassificationTypeNames.StringLiteral => 4,
                _ => 0, // 0 => default color (black)
            };

        static string EscapeForRtf(string text)
        {
            // RTF requires braces, backslash, and other special chars to be escaped.
            // This is the minimal approach for braces and backslashes:
            StringBuilder sb = new(text.Length);
            foreach (char c in text)
            {
                switch (c)
                {
                    case '\\': sb.Append(@"\\"); break;
                    case '{': sb.Append(@"\{"); break;
                    case '}': sb.Append(@"\}"); break;
                    // You can optionally handle tab/CR/LF or multiple spaces here.
                    default: sb.Append(c); break;
                }
            }

            return sb.ToString();
        }
    }
}

