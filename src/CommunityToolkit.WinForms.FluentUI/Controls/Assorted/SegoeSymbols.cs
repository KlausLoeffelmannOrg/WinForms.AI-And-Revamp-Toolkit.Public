namespace CommunityToolkit.WinForms.FluentUI.Controls;

/// <summary>
/// Provides methods to get symbols, icons, images, and image lists from the Segoe UI Symbol font.
/// </summary>
public static partial class SegoeSymbols
{
    /// <summary>
    /// Gets or sets the cached font used for symbols.
    /// </summary>
    public static (int size, Font font)? CachedSymbolsFont { get; set; }

    /// <summary>
    /// Converts the specified symbol to a string.
    /// </summary>
    /// <param name="symbol">The symbol to convert.</param>
    /// <returns>A string representation of the symbol.</returns>
    public static string GetSymbol(int symbol)
    {
        return char.ConvertFromUtf32((int)symbol);
    }

    /// <summary>
    /// Gets an icon from the specified symbol of the Segoe UI Symbol font.
    /// </summary>
    /// <param name="symbol">The symbol to get the icon for.</param>
    /// <param name="color">The color of the icon.</param>
    /// <param name="size">The size of the icon.</param>
    /// <returns>An <see cref="Icon"/> representing the specified symbol.</returns>
    public static Icon GetIconFromSymbol(int symbol, Color color, int size)
    {
        if (CachedSymbolsFont == null || CachedSymbolsFont.Value.size != size)
        {
            CachedSymbolsFont = (size, new Font("Segoe UI Symbol", size));
        }

        Font iconFont = CachedSymbolsFont.Value.font;
        Bitmap bitmap = new(size, size);

        using (Graphics g = Graphics.FromImage(bitmap))
        {
            g.DrawString(GetSymbol(symbol), iconFont, new SolidBrush(color), new PointF(0, 0));
        }
        return Icon.FromHandle(bitmap.GetHicon());
    }

    /// <summary>
    /// Gets an image from the specified symbol of the Segoe UI Symbol font.
    /// </summary>
    /// <param name="symbol">The symbol to get the image for.</param>
    /// <param name="color">The color of the image.</param>
    /// <param name="size">The size of the image.</param>
    /// <returns>An <see cref="Image"/> representing the specified symbol.</returns>
    public static Image GetImageFromSymbol(int symbol, Color color, int size)
    {
        if (CachedSymbolsFont == null || CachedSymbolsFont.Value.size != size)
        {
            CachedSymbolsFont = (size, new Font("Segoe UI Symbol", size));
        }

        Font iconFont = CachedSymbolsFont.Value.font;
        Bitmap bitmap = new(size, size);

        using (Graphics g = Graphics.FromImage(bitmap))
        {
            g.DrawString(GetSymbol(symbol), iconFont, new SolidBrush(color), new PointF(0, 0));
        }

        return bitmap;
    }

    /// <summary>
    /// Gets an image list from the specified symbols of the Segoe UI Symbol font.
    /// </summary>
    /// <param name="color">The color of the images.</param>
    /// <param name="size">The size of the images.</param>
    /// <param name="symbols">The symbols to get the images for.</param>
    /// <returns>An <see cref="ImageList"/> containing images representing the specified symbols.</returns>
    public static ImageList GetImageListFromSymbols(Color color, int size, params int[] symbols)
    {
        ImageList imageList = new()
        {
            ImageSize = new Size(size, size),
            ColorDepth = ColorDepth.Depth32Bit
        };

        if (CachedSymbolsFont == null || CachedSymbolsFont.Value.size != size)
        {
            CachedSymbolsFont = (size, new Font("Segoe UI Symbol", size));
        }

        Font iconFont = CachedSymbolsFont.Value.font;

        foreach (var symbol in symbols)
        {
            Bitmap bitmap = new(size, size);

            using (Graphics g = Graphics.FromImage(bitmap))
            {
                g.DrawString(GetSymbol(symbol), iconFont, new SolidBrush(color), new PointF(0, 0));
            }

            // Get the Symbols-enum from the int value:
            AllSymbols segoeSymbol = (AllSymbols)symbol;

            imageList.Images.Add(segoeSymbol.ToString(), bitmap);
        }

        return imageList;
    }
}
