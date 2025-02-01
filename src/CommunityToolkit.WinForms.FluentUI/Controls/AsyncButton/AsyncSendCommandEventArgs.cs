namespace CommunityToolkit.WinForms.FluentUI.Controls;

public class AsyncSendCommandEventArgs(string commandText, string? rtfCommandText) : EventArgs
{
    public string CommandText { get; } = commandText;
    public string? RTFCommandText { get; } = rtfCommandText;
}