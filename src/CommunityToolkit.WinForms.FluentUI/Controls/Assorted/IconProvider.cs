namespace CommunityToolkit.WinForms.FluentUI.Controls;

public class IconProvider
{
    public static Icon GetIcon(string iconName)
    {
        // This is a simplified example. In a real implementation, you would map iconName to a Unicode code point.
        string unicode = "\uE700"; // Example Unicode for a specific icon in Segoe MDL2 Assets
        Font iconFont = new Font("Segoe MDL2 Assets", 16);
        Bitmap bitmap = new Bitmap(32, 32);
        using (Graphics g = Graphics.FromImage(bitmap))
        {
            g.DrawString(unicode, iconFont, Brushes.Black, new PointF(0, 0));
        }
        return Icon.FromHandle(bitmap.GetHicon());
    }
}
