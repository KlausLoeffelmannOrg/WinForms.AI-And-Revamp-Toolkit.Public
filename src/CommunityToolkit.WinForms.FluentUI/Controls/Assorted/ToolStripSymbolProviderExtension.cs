using System.Drawing.Imaging;
using System.Reflection;

namespace CommunityToolkit.WinForms.FluentUI.Controls;

public static class ToolStripSymbolProviderExtension
{
    private static readonly Dictionary<(string fontName, float fontSize), Font> _fontCache = new();
    private static SolidBrush? s_foreColorBrush;

    /// <summary>
    ///  Gets the cached font based on the provided enum's type name and font size.
    /// </summary>
    /// <param name="symbol">The enum used to determine the font.</param>
    /// <param name="fontSize">The desired font size.</param>
    /// <returns>The cached or newly created Font.</returns>
    private static Font GetFont(Enum symbol, float fontSize)
    {
        string typeName = symbol.GetType().Name;

        // We need to get the Fontname from the SourceFontNameAttribute.
        // If we don't find it, we throw:
        string? fontName = symbol.GetType().GetCustomAttributes<SourceFontNameAttribute>()
            .FirstOrDefault()?.FontName ?? throw new InvalidOperationException(
                $"The enum type '{typeName}' for getting Icons/Images from Symbol Fonts does not have a SourceFontNameAttribute.");

        if (!_fontCache.TryGetValue((fontName, fontSize), out var iconFont))
        {
            iconFont = new Font(fontName, fontSize, GraphicsUnit.Pixel);
            _fontCache[(fontName, fontSize)] = iconFont;
        }

        return iconFont;
    }

    /// <summary>
    ///  Gets the symbol icon based on the provided enum and size.
    /// </summary>
    /// <param name="symbol">The enum used to determine the symbol.</param>
    /// <param name="size">The desired size of the icon.</param>
    /// <returns>The generated Icon.</returns>
    public static Icon GetSymbolIcon(this ToolStrip toolStrip, Enum symbol, int size = 0)
    {
        if (size == 0)
        {
            size = toolStrip.ImageScalingSize.Width;
        }

        // Convert the symbol value into a Unicode string:
        string unicode = char.ConvertFromUtf32(Convert.ToInt32(symbol));
        Font iconFont = GetFont(symbol, size);


        Bitmap bitmap = new(size, size);

        using (Graphics g = Graphics.FromImage(bitmap))
        {
            g.DrawString(unicode, iconFont, Brushes.Black, new PointF(0, 0));
        }

        return Icon.FromHandle(bitmap.GetHicon());
    }

    /// <summary>
    ///  Gets the symbol image based on the provided enum and size.
    /// </summary>
    /// <param name="symbol">The enum used to determine the symbol.</param>
    /// <param name="size">The desired size of the image.</param>
    /// <returns>The generated Image.</returns>
    public static Image GetSymbolImage(
        this ToolStrip toolStrip,
        Enum symbol,
        int size = 0,
        Color? foreColor = null,
        Color? backColor = null)
    {
        ArgumentNullException.ThrowIfNull(toolStrip);

        if (size == 0)
        {
            size = toolStrip.ImageScalingSize.Width;
        }

        foreColor ??= toolStrip.ForeColor;
        backColor ??= toolStrip.BackColor;

        if (s_foreColorBrush is not SolidBrush currentBrush
            || currentBrush.Color != foreColor)
        {
            try
            {
                currentBrush = new SolidBrush(foreColor.Value);
            }
            catch (Exception)
            {
                throw;
            }

            s_foreColorBrush = currentBrush;
        }

        // Convert the symbol value into a Unicode string:
        char unicode = Convert.ToChar(symbol);
        Font iconFont = GetFont(symbol, size - 10);

        Bitmap bitmap;
        bitmap = new(size, size, Graphics.FromHwnd(toolStrip.Handle));

        using (Graphics g = Graphics.FromImage(bitmap))
        {
            g.Clear(backColor.Value);
            g.DrawString(unicode.ToString(), iconFont, currentBrush, new PointF(-2, 0));
        }

        return bitmap;
    }
}
