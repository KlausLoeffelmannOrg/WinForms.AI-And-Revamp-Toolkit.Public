namespace CommunityToolkit.WinForms.FluentUI.Controls;

/// <summary>
/// A panel that captures the underlying content, applies a milk-glass effect,
/// and renders waiting text on top.
/// </summary>
public class ContentFreezePanel : Panel
{
    private Bitmap? _frozenContent;

    /// <summary>
    /// Initializes a new instance of the <see cref="ContentFreezePanel"/> class.
    /// </summary>
    public ContentFreezePanel()
    {
        DoubleBuffered = true;
        Visible = false;
    }

    /// <summary>
    /// Captures the content of the specified control and applies a milk-glass effect.
    /// </summary>
    /// <param name="source">
    /// The control whose content should be captured. Typically your RichTextBox.
    /// </param>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="source"/> is null.</exception>
    public void FreezeContent(Control source)
    {
        if (source is null)
        {
            throw new ArgumentNullException(nameof(source));
        }

        // Capture the source content.
        using var tempBmp = new Bitmap(source.Width, source.Height);
        source.DrawToBitmap(tempBmp, new Rectangle(Point.Empty, source.Size));

        // Create a new bitmap and apply the milk-glass effect.
        var bmp = new Bitmap(source.Width, source.Height);
        using (var g = Graphics.FromImage(bmp))
        {
            g.DrawImage(tempBmp, 0, 0);
            using var overlay = new SolidBrush(Color.FromArgb(100, Color.White));
            g.FillRectangle(overlay, new Rectangle(Point.Empty, bmp.Size));
        }

        // Dispose any previous frozen content.
        _frozenContent?.Dispose();
        _frozenContent = bmp;
        Invalidate();
    }

    /// <summary>
    /// Paints the frozen content and the waiting text.
    /// </summary>
    /// <param name="e">The <see cref="PaintEventArgs"/> instance containing event data.</param>
    protected override void OnPaint(PaintEventArgs e)
    {
        base.OnPaint(e);

        // Draw the frozen (milk-glassed) content if available.
        if (_frozenContent is not null)
        {
            e.Graphics.DrawImage(_frozenContent, 0, 0, Width, Height);
        }

        // Draw waiting text centered.
        if (!string.IsNullOrEmpty(Text))
        {
            SizeF textSize = e.Graphics.MeasureString(Text, Font);
            PointF location = new PointF(
                (Width - textSize.Width) / 2,
                (Height - textSize.Height) / 2);
            using var textBrush = new SolidBrush(ForeColor);
            e.Graphics.DrawString(Text, Font, textBrush, location);
        }
    }

    /// <summary>
    /// Disposes of the resources used by the panel.
    /// </summary>
    /// <param name="disposing">
    /// <see langword="true"/> to release both managed and unmanaged resources;
    /// <see langword="false"/> to release only unmanaged resources.
    /// </param>
    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            _frozenContent?.Dispose();
        }

        base.Dispose(disposing);
    }
}
