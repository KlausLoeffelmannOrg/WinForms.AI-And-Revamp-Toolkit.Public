using Chatty.ViewModels;
using CommunityToolkit.DesktopGeneric.Mvvm;
using CommunityToolkit.WinForms.ComponentModel;
using CommunityToolkit.WinForms.Extensions;
using CommunityToolkit.WinForms.Extensions.FormAndControlExtensions;
using CommunityToolkit.WinForms.FluentUI.Controls;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace Chatty.Agents.Personalities;

// TODO: Clean-up hacky and embarrassing Form control.
internal partial class FrmManagePersonalities : Form
{
    public event EventHandler? PersonalitiesChanged;

    private const string Key_Personalities_Bounds = nameof(Key_Personalities_Bounds);

    private readonly PersonalityManager _personalityManager = null!;
    private readonly IUserSettingsService _settingsService = null!;

    public PersonalityManager? PersonalitiesManager => _personalityManager;

    public FrmManagePersonalities()
    {
        InitializeComponent();
        _settingsService = WinFormsUserSettingsService.GetOrThrow();

        _tsbNewPersonality.Image = _mainToolStrip.GetSymbolImage(FluentSymbols.CommonToolStripSymbols.New);
        _tsbEditPersonality.Image = _mainToolStrip.GetSymbolImage(FluentSymbols.CommonToolStripSymbols.Edit);
        _tsbDeletePersonality.Image = _mainToolStrip.GetSymbolImage(FluentSymbols.CommonToolStripSymbols.Delete);
        _tsbCompactPrompt.Image = _mainToolStrip.GetSymbolImage(FluentSymbols.AllSymbols.Crop);
    }

    public FrmManagePersonalities(PersonalityManager personalityManager) : this()
    {
        _personalityManager = personalityManager;

        _personalityManager.Personalities.CollectionChanged
            += Personalities_Changed;

        InitializeDataGridView();
        PopulateDataGridView();
    }

    private void Personalities_Changed(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        => PersonalitiesChanged?.Invoke(this, EventArgs.Empty);

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

        _personalityManager.Personalities.CollectionChanged
            -= Personalities_Changed;

        _settingsService.SetInstance(Key_Personalities_Bounds, Bounds);
        _settingsService.Save();
    }

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
        foreach (var personality in _personalityManager.Personalities)
        {
            _dgvPersonalities.Rows.Add(personality.Name, personality.SystemPrompt, personality.DateCreated, personality.DateLastEdited);
        }
    }

    private async void DataGridView_CellDoubleClick(object? sender, DataGridViewCellEventArgs e)
    {
        if (GetCurrentPersonality() is Personality personality)
        {
            Personality? editedPersonality = await EditPersonalityAsync(personality);

            if (editedPersonality is not null)
            {
                _personalityManager.Personalities[_dgvPersonalities.CurrentRow!.Index] = editedPersonality;

                // Update the data in the row:
                _dgvPersonalities.Rows[_dgvPersonalities.CurrentRow.Index].SetValues(
                    editedPersonality.Name,
                    editedPersonality.SystemPrompt,
                    editedPersonality.DateCreated,
                    editedPersonality.DateLastEdited);
            }
        }
    }

    private async void TsbNewPersonality_Click(object? sender, EventArgs e)
    {
        Personality? newPersonality = await AddNewPersonalityAsync();

        if (newPersonality is not null)
        {
            _personalityManager.Personalities.Add(newPersonality);
        }
    }

    private async void TsbEditPersonality_Click(object? sender, EventArgs e)
    {
        if (GetCurrentPersonality() is Personality personality)
        {
            var editedPersonality = await EditPersonalityAsync(personality);
            if (editedPersonality is not null)
            {
                _personalityManager.Personalities[_dgvPersonalities.CurrentRow!.Index] = editedPersonality;
            }
        }
    }

    public async Task<Personality?> AddNewPersonalityAsync()
    {
        Personality personality = new();
        FrmAddEditPersonality addEditForm = new();

        IModalDialogResult<Personality> result 
            = await addEditForm.ShowDialogAsync(personality);

        if (result.DialogCloseReason == DialogCloseReason.OK)
        {
            _personalityManager.Personalities.Add(result.ReturnValue!);
            return result.ReturnValue;
        }

        return null;
    }

    public async Task<Personality?> EditPersonalityAsync(Personality personality)
    {
        Personality editedPersonality = new(personality);
        FrmAddEditPersonality addEditForm = new();

        IModalDialogResult<Personality> result 
            = await addEditForm.ShowDialogAsync(editedPersonality);

        if (result.DialogCloseReason == DialogCloseReason.First)
        {
            return result.ReturnValue;
        }

        return null;
    }

    public Personality? GetCurrentPersonality()
    {
        if (_dgvPersonalities.CurrentRow != null)
        {
            return _personalityManager.Personalities[_dgvPersonalities.CurrentRow.Index];
        }

        return null;
    }
}
