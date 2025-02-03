using Chatty.ViewModels;
using CommunityToolkit.WinForms.ComponentModel;
using CommunityToolkit.WinForms.Extensions;
using CommunityToolkit.WinForms.Extensions.FormAndControlExtensions;

namespace Chatty.Agents.Personalities;

internal partial class FrmManagePersonalities: Form
{
    private const string Key_Personalities_Bounds = nameof(Key_Personalities_Bounds);

    private readonly PersonalityViewModel _personalityViewModel = null!;
    private readonly IUserSettingsService _settingsService = null!;

    public FrmManagePersonalities()
    {
        InitializeComponent();
        _settingsService = WinFormsUserSettingsService.GetOrThrow();
    }

    public FrmManagePersonalities(PersonalityViewModel personalities) : this()
    {
        _personalityViewModel = personalities;
        InitializeDataGridView();
        PopulateDataGridView();
    }

    protected override void OnLoad(EventArgs e)
    {
        base.OnLoad(e);

        Rectangle bounds = _settingsService.GetInstance(
            Key_Personalities_Bounds,
            this.CenterOnScreen(
                horizontalFillGrade: 80,
                verticalFillGrade: 80));

        Bounds = bounds;
    }

    protected override void OnFormClosing(FormClosingEventArgs e)
    {
        base.OnFormClosing(e);

        _settingsService.SetInstance(Key_Personalities_Bounds, Bounds);
        _settingsService.Save();
    }

    public PersonalityViewModel Personalities => _personalityViewModel;

    private void InitializeDataGridView()
    {
        _dgvPersonalities.ApplyDarkMode();

        // Set column headers style
        var headerStyle = _dgvPersonalities.ColumnHeadersDefaultCellStyle.Clone() as DataGridViewCellStyle;

        if (headerStyle is not null)
        {
            headerStyle.Font = new Font(headerStyle.Font, FontStyle.Bold);
            headerStyle.ForeColor = Color.White;
            headerStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            headerStyle.Font = new Font(headerStyle.Font.FontFamily, headerStyle.Font.Size + 1, FontStyle.Bold);
            headerStyle.Padding = new Padding(4); // Set padding to 4 pixels
            _dgvPersonalities.ColumnHeadersDefaultCellStyle = headerStyle;
        }

        _dgvPersonalities.ColumnHeadersDefaultCellStyle = headerStyle;

        _dgvPersonalities.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        _dgvPersonalities.ReadOnly = true;
        _dgvPersonalities.CellDoubleClick += DataGridView_CellDoubleClick;
        _btnOK.Click += BtnOK_Click;
        _btnCancel.Click += BtnCancel_Click;

        // Set the column header width weight to 30%/70%:
        _dgvPersonalities.Columns[0].FillWeight = 30;
        _dgvPersonalities.Columns[1].FillWeight = 70;

        // Set row height to auto and align content to top
        _dgvPersonalities.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
        _dgvPersonalities.DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopLeft;

        // Make the Rows auto-wrap:
        _dgvPersonalities.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
        _dgvPersonalities.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

        // Prevent column headers from being selected
        _dgvPersonalities.ColumnHeadersDefaultCellStyle.SelectionBackColor = _dgvPersonalities.ColumnHeadersDefaultCellStyle.BackColor;
        _dgvPersonalities.ColumnHeadersDefaultCellStyle.SelectionForeColor = _dgvPersonalities.ColumnHeadersDefaultCellStyle.ForeColor;
    }

    private void PopulateDataGridView()
    {
        foreach (var personality in _personalityViewModel.Personalities)
        {
            _dgvPersonalities.Rows.Add(personality.Name, personality.SystemPrompt, personality.DateCreated, personality.DateLastEdited);
        }
    }

    private void DataGridView_CellDoubleClick(object? sender, DataGridViewCellEventArgs e)
    {
        if (e.RowIndex >= 0)
        {
            _dgvPersonalities.Enabled = false;
            _tlpEditPersonality.Enabled = true;
            var selectedPersonality = _personalityViewModel.Personalities[e.RowIndex];
            _txtPersonalityName.Text = selectedPersonality.Name; // Updated reference
            _txtPersonalityDescription.Text = selectedPersonality.SystemPrompt; // Updated reference
            ApplyPersonalityFileExtractionSetting(selectedPersonality.FileExtractionSettings); // Apply settings to form controls
        }
    }

    private void BtnOK_Click(object? sender, EventArgs e)
    {
        var selectedPersonality = _personalityViewModel.SelectedPersonality;
        if (selectedPersonality != null)
        {
            selectedPersonality.Name = _txtPersonalityName.Text; // Updated reference
            selectedPersonality.SystemPrompt = _txtPersonalityDescription.Text; // Updated reference
            selectedPersonality.FileExtractionSettings = GetPersonalityFileExtractionSetting(); // Read settings from form controls
            _personalityViewModel.IsDirty = true;
        }

        _dgvPersonalities.Enabled = true;
        _tlpEditPersonality.Enabled = false;
    }

    private void BtnCancel_Click(object? sender, EventArgs e)
    {
        _dgvPersonalities.Enabled = true;
        _tlpEditPersonality.Enabled = false;
    }

    private void ApplyPersonalityFileExtractionSetting(PersonalityFileExtractionSetting settings)
    {
        _optExtractCodeAutomatically.Checked = settings.HasFlag(PersonalityFileExtractionSetting.ExtractCodeBlocksAutomatically);
        _chkSaveExtractedFilesAutomatically.Checked = settings.HasFlag(PersonalityFileExtractionSetting.SaveExtractedFilesAutomatically);
        _chkStoreInDedicatedFolders.Checked = settings.HasFlag(PersonalityFileExtractionSetting.StoreInDedicatedFolders);
        _optSelectFolderAtRuntime.Checked = settings.HasFlag(PersonalityFileExtractionSetting.SelectFolderAtRuntime);
        _optDontDoFileExtractions.Checked = settings == PersonalityFileExtractionSetting.None;
    }

    //private PersonalityFileExtractionSetting GetPersonalityFileExtractionSetting()
    //{
    //    PersonalityFileExtractionSetting settings = PersonalityFileExtractionSetting.None;

    //    if (_optExtractCodeAutomatically.Checked)
    //    {
    //        settings |= PersonalityFileExtractionSetting.ExtractCodeBlocksAutomatically;
    //    }
    //    if (_chkSaveExtractedFilesAutomatically.Checked)
    //    {
    //        settings |= PersonalityFileExtractionSetting.SaveExtractedFilesAutomatically;
    //    }
    //    if (_chkStoreInDedicatedFolders.Checked)
    //    {
    //        settings |= PersonalityFileExtractionSetting.StoreInDedicatedFolders;
    //    }
    //    if (_optSelectFolderAtRuntime.Checked)
    //    {
    //        settings |= PersonalityFileExtractionSetting.SelectFolderAtRuntime;
    //    }

    //    return settings;
    //}

    // I asked o3-mini-high to review the above code, which was generated by Copilot-Edits with 4o.
    // This is the feedback I received.
    /// <summary>
    /// Gets the personality file extraction settings based on the current UI selections.
    /// </summary>
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
