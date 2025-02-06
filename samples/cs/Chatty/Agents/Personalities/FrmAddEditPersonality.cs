using Chatty.ViewModels;

namespace Chatty.Agents.Personalities;

internal partial class FrmAddEditPersonality : Form
{
    private Personality _personality;

    public FrmAddEditPersonality()
    {
        InitializeComponent();
    }

    protected override void OnLoad(EventArgs e)
    {
        base.OnLoad(e);

        _personality = (Personality?)DataContext
            ?? throw new InvalidOperationException("No data context provided.");

        ApplyEditData();
    }

    protected override void OnDataContextChanged(EventArgs e)
    {
        base.OnDataContextChanged(e);
    }

    private void ApplyEditData()
    {
        _txtPersonalityName.Text = _personality.Name;
        _txtPersonalityDescription.Text = _personality.SystemPrompt;
        ApplyPersonalityFileExtractionSetting(_personality.FileExtractionSettings);
    }

    private Personality GetEditData(Personality? existingData = null)
    {
        existingData ??= new Personality();
        existingData.Name = _txtPersonalityName.Text;
        existingData.SystemPrompt = _txtPersonalityDescription.Text;
        existingData.FileExtractionSettings = GetPersonalityFileExtractionSetting();

        return existingData;
    }

    public override bool ValidateChildren()
    {
        bool returnValue = true;

        if (string.IsNullOrWhiteSpace(_txtPersonalityName.Text))
        {
            _fdpPersonalityName.StartSignalError();
            returnValue = false;
        }

        if (string.IsNullOrWhiteSpace(_txtPersonalityDescription.Text))
        {
            _fdpPersonalityDescription.StartSignalError();
            returnValue = false;
        }

        if (returnValue)
        {
            DataContext = GetEditData(_personality);
        }

        return returnValue;
    }

    private void ApplyPersonalityFileExtractionSetting(PersonalityFileExtractionSetting settings)
    {
        _optExtractCodeAutomatically.Checked = settings.HasFlag(PersonalityFileExtractionSetting.ExtractCodeBlocksAutomatically);
        _chkSaveExtractedFilesAutomatically.Checked = settings.HasFlag(PersonalityFileExtractionSetting.SaveExtractedFilesAutomatically);
        _chkStoreInDedicatedFolders.Checked = settings.HasFlag(PersonalityFileExtractionSetting.StoreInDedicatedFolders);
        _optSelectFolderAtRuntime.Checked = settings.HasFlag(PersonalityFileExtractionSetting.SelectFolderAtRuntime);
        _optDontDoFileExtractions.Checked = settings == PersonalityFileExtractionSetting.None;
    }

    private PersonalityFileExtractionSetting GetPersonalityFileExtractionSetting() =>
        (_optExtractCodeAutomatically.Checked
            ? PersonalityFileExtractionSetting.ExtractCodeBlocksAutomatically
            : PersonalityFileExtractionSetting.None)
        | (_chkSaveExtractedFilesAutomatically.Checked
            ? PersonalityFileExtractionSetting.SaveExtractedFilesAutomatically
            : PersonalityFileExtractionSetting.None)
        | (_chkStoreInDedicatedFolders.Checked
            ? PersonalityFileExtractionSetting.StoreInDedicatedFolders
            : PersonalityFileExtractionSetting.None)
        | (_optSelectFolderAtRuntime.Checked
            ? PersonalityFileExtractionSetting.SelectFolderAtRuntime
            : PersonalityFileExtractionSetting.None);
}
