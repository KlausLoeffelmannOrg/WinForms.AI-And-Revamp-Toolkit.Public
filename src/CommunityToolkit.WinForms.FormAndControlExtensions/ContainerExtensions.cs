namespace CommunityToolkit.WinForms.Extensions.FormAndControlExtensions;

public static class ContainerExtensions
{
    /// <summary>
    ///  Suspends the layout logic for the control and all its child controls recursively.
    /// </summary>
    /// <param name="control">The container control whose layout logic to suspend.</param>
    public static void SuspendLayoutRecursively(this ScrollableControl control)
    {
        foreach (Control child in control.Controls)
        {
            if (child is ScrollableControl scrollableControl)
            {
                scrollableControl.SuspendLayoutRecursively();
            }
        }
    }

    /// <summary>
    ///  Resumes usual layout logic for the control and all its child controls recursively.
    /// </summary>
    public static void ResumeLayoutRecursively(this ScrollableControl control)
    {
        foreach (Control child in control.Controls)
        {
            if (child is ScrollableControl scrollableControl)
            {
                scrollableControl.ResumeLayoutRecursively();
            }
        }
    }
}
