namespace Chatty.ViewModels;

internal class OptionsViewModel
{
    public bool ArchiveChats { get; set; }

    public string BasePath { get; set; } = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Chatty";
}
