namespace Chatty.Agents.Personalities;

partial class FrmManagePersonalities
{
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }
        base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
        _splitMain = new SplitContainer();
        _dgvPersonalities = new DataGridView();
        dataGridViewTextBoxColumn1 = new DataGridViewTextBoxColumn();
        dataGridViewTextBoxColumn2 = new DataGridViewTextBoxColumn();
        _tlpEditPersonality = new TableLayoutPanel();
        _lblPersonality = new Label();
        _lblSystemPrompt = new Label();
        _fdpPersonalityName = new CommunityToolkit.WinForms.FluentUI.FluentDecoratorPanel();
        _txtPersonalityName = new CommunityToolkit.WinForms.FluentUI.Controls.AutoCompleteEditor();
        _fdpPersonalityDescription = new CommunityToolkit.WinForms.FluentUI.FluentDecoratorPanel();
        _txtPersonalityDescription = new CommunityToolkit.WinForms.FluentUI.Controls.AutoCompleteEditor();
        _optDontDoFileExtractions = new RadioButton();
        _pnlSettings1 = new Panel();
        _optExtractCodeAutomatically = new RadioButton();
        _chkSaveExtractedFilesAutomatically = new CheckBox();
        _chkStoreInDedicatedFolders = new CheckBox();
        _pnlSettings2 = new Panel();
        _optSelectFolderAtRuntime = new RadioButton();
        _chkStoreInDedicatedFolders2 = new CheckBox();
        _btnOK = new Button();
        _btnCancel = new Button();
        ((System.ComponentModel.ISupportInitialize)_splitMain).BeginInit();
        _splitMain.Panel1.SuspendLayout();
        _splitMain.Panel2.SuspendLayout();
        _splitMain.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)_dgvPersonalities).BeginInit();
        _tlpEditPersonality.SuspendLayout();
        _fdpPersonalityName.SuspendLayout();
        _fdpPersonalityDescription.SuspendLayout();
        _pnlSettings1.SuspendLayout();
        _pnlSettings2.SuspendLayout();
        SuspendLayout();
        // 
        // _splitMain
        // 
        _splitMain.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        _splitMain.Location = new Point(13, 13);
        _splitMain.Margin = new Padding(4);
        _splitMain.Name = "_splitMain";
        _splitMain.Orientation = Orientation.Horizontal;
        // 
        // _splitMain.Panel1
        // 
        _splitMain.Panel1.Controls.Add(_dgvPersonalities);
        _splitMain.Panel1.Padding = new Padding(10);
        // 
        // _splitMain.Panel2
        // 
        _splitMain.Panel2.Controls.Add(_tlpEditPersonality);
        _splitMain.Panel2.Padding = new Padding(10);
        _splitMain.Size = new Size(1827, 1063);
        _splitMain.SplitterDistance = 410;
        _splitMain.SplitterWidth = 5;
        _splitMain.TabIndex = 0;
        // 
        // _dgvPersonalities
        // 
        _dgvPersonalities.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        _dgvPersonalities.Columns.AddRange(new DataGridViewColumn[] { dataGridViewTextBoxColumn1, dataGridViewTextBoxColumn2 });
        _dgvPersonalities.Dock = DockStyle.Fill;
        _dgvPersonalities.Location = new Point(10, 10);
        _dgvPersonalities.Margin = new Padding(4);
        _dgvPersonalities.Name = "_dgvPersonalities";
        _dgvPersonalities.ReadOnly = true;
        _dgvPersonalities.RowHeadersWidth = 82;
        _dgvPersonalities.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        _dgvPersonalities.Size = new Size(1807, 390);
        _dgvPersonalities.TabIndex = 0;
        _dgvPersonalities.CellDoubleClick += DataGridView_CellDoubleClick;
        // 
        // dataGridViewTextBoxColumn1
        // 
        dataGridViewTextBoxColumn1.HeaderText = "Name";
        dataGridViewTextBoxColumn1.MinimumWidth = 8;
        dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
        dataGridViewTextBoxColumn1.ReadOnly = true;
        dataGridViewTextBoxColumn1.Width = 150;
        // 
        // dataGridViewTextBoxColumn2
        // 
        dataGridViewTextBoxColumn2.HeaderText = "System Prompt";
        dataGridViewTextBoxColumn2.MinimumWidth = 8;
        dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
        dataGridViewTextBoxColumn2.ReadOnly = true;
        dataGridViewTextBoxColumn2.Width = 150;
        // 
        // _tlpEditPersonality
        // 
        _tlpEditPersonality.ColumnCount = 2;
        _tlpEditPersonality.ColumnStyles.Add(new ColumnStyle());
        _tlpEditPersonality.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        _tlpEditPersonality.Controls.Add(_lblPersonality, 0, 0);
        _tlpEditPersonality.Controls.Add(_lblSystemPrompt, 0, 1);
        _tlpEditPersonality.Controls.Add(_fdpPersonalityName, 1, 0);
        _tlpEditPersonality.Controls.Add(_fdpPersonalityDescription, 1, 1);
        _tlpEditPersonality.Controls.Add(_optDontDoFileExtractions, 1, 2);
        _tlpEditPersonality.Controls.Add(_pnlSettings1, 1, 3);
        _tlpEditPersonality.Controls.Add(_pnlSettings2, 1, 4);
        _tlpEditPersonality.Dock = DockStyle.Fill;
        _tlpEditPersonality.Location = new Point(10, 10);
        _tlpEditPersonality.Margin = new Padding(4);
        _tlpEditPersonality.Name = "_tlpEditPersonality";
        _tlpEditPersonality.RowCount = 5;
        _tlpEditPersonality.RowStyles.Add(new RowStyle());
        _tlpEditPersonality.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        _tlpEditPersonality.RowStyles.Add(new RowStyle());
        _tlpEditPersonality.RowStyles.Add(new RowStyle());
        _tlpEditPersonality.RowStyles.Add(new RowStyle());
        _tlpEditPersonality.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
        _tlpEditPersonality.Size = new Size(1807, 628);
        _tlpEditPersonality.TabIndex = 0;
        // 
        // _lblPersonality
        // 
        _lblPersonality.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        _lblPersonality.AutoSize = true;
        _lblPersonality.Location = new Point(3, 0);
        _lblPersonality.Name = "_lblPersonality";
        _lblPersonality.Size = new Size(163, 55);
        _lblPersonality.TabIndex = 0;
        _lblPersonality.Text = "Personality:";
        _lblPersonality.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // _lblSystemPrompt
        // 
        _lblSystemPrompt.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        _lblSystemPrompt.AutoSize = true;
        _lblSystemPrompt.Location = new Point(3, 55);
        _lblSystemPrompt.Name = "_lblSystemPrompt";
        _lblSystemPrompt.Size = new Size(163, 308);
        _lblSystemPrompt.TabIndex = 2;
        _lblSystemPrompt.Text = "System-Prompt";
        // 
        // _fdpPersonalityName
        // 
        _fdpPersonalityName.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        _fdpPersonalityName.BorderThickness = 1;
        _fdpPersonalityName.Controls.Add(_txtPersonalityName);
        _fdpPersonalityName.Location = new Point(172, 3);
        _fdpPersonalityName.Name = "_fdpPersonalityName";
        _fdpPersonalityName.Padding = new Padding(10);
        _fdpPersonalityName.Size = new Size(1632, 49);
        _fdpPersonalityName.TabIndex = 1;
        // 
        // _txtPersonalityName
        // 
        _txtPersonalityName.BorderStyle = BorderStyle.None;
        _txtPersonalityName.Location = new Point(11, 10);
        _txtPersonalityName.Name = "_txtPersonalityName";
        _txtPersonalityName.Size = new Size(1610, 30);
        _txtPersonalityName.TabIndex = 0;
        _txtPersonalityName.Text = "";
        // 
        // _fdpPersonalityDescription
        // 
        _fdpPersonalityDescription.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        _fdpPersonalityDescription.BorderThickness = 1;
        _fdpPersonalityDescription.Controls.Add(_txtPersonalityDescription);
        _fdpPersonalityDescription.Location = new Point(172, 58);
        _fdpPersonalityDescription.Name = "_fdpPersonalityDescription";
        _fdpPersonalityDescription.Padding = new Padding(10);
        _fdpPersonalityDescription.Size = new Size(1632, 302);
        _fdpPersonalityDescription.TabIndex = 3;
        // 
        // _txtPersonalityDescription
        // 
        _txtPersonalityDescription.BorderStyle = BorderStyle.None;
        _txtPersonalityDescription.Dock = DockStyle.Fill;
        _txtPersonalityDescription.Location = new Point(11, 10);
        _txtPersonalityDescription.Name = "_txtPersonalityDescription";
        _txtPersonalityDescription.Size = new Size(1610, 282);
        _txtPersonalityDescription.TabIndex = 0;
        _txtPersonalityDescription.Text = "";
        // 
        // _optDontDoFileExtractions
        // 
        _optDontDoFileExtractions.AutoSize = true;
        _optDontDoFileExtractions.Location = new Point(175, 369);
        _optDontDoFileExtractions.Margin = new Padding(6);
        _optDontDoFileExtractions.Name = "_optDontDoFileExtractions";
        _optDontDoFileExtractions.Size = new Size(267, 34);
        _optDontDoFileExtractions.TabIndex = 4;
        _optDontDoFileExtractions.TabStop = true;
        _optDontDoFileExtractions.Text = "Don't do file extractions";
        _optDontDoFileExtractions.UseVisualStyleBackColor = true;
        // 
        // _pnlSettings1
        // 
        _pnlSettings1.Controls.Add(_optExtractCodeAutomatically);
        _pnlSettings1.Controls.Add(_chkSaveExtractedFilesAutomatically);
        _pnlSettings1.Controls.Add(_chkStoreInDedicatedFolders);
        _pnlSettings1.Location = new Point(172, 412);
        _pnlSettings1.Name = "_pnlSettings1";
        _pnlSettings1.Size = new Size(1632, 122);
        _pnlSettings1.TabIndex = 5;
        // 
        // _optExtractCodeAutomatically
        // 
        _optExtractCodeAutomatically.AutoSize = true;
        _optExtractCodeAutomatically.Location = new Point(3, 3);
        _optExtractCodeAutomatically.Name = "_optExtractCodeAutomatically";
        _optExtractCodeAutomatically.Size = new Size(639, 34);
        _optExtractCodeAutomatically.TabIndex = 0;
        _optExtractCodeAutomatically.TabStop = true;
        _optExtractCodeAutomatically.Text = "Extract and save code in Markdown code blocks automatically.";
        _optExtractCodeAutomatically.UseVisualStyleBackColor = true;
        _optExtractCodeAutomatically.CheckedChanged += RadioButton1_CheckedChanged;
        // 
        // _chkSaveExtractedFilesAutomatically
        // 
        _chkSaveExtractedFilesAutomatically.AutoSize = true;
        _chkSaveExtractedFilesAutomatically.Location = new Point(32, 43);
        _chkSaveExtractedFilesAutomatically.Name = "_chkSaveExtractedFilesAutomatically";
        _chkSaveExtractedFilesAutomatically.Size = new Size(762, 34);
        _chkSaveExtractedFilesAutomatically.TabIndex = 1;
        _chkSaveExtractedFilesAutomatically.Text = "Automatically save the extracted listing/text files to the Conversation folder";
        _chkSaveExtractedFilesAutomatically.UseVisualStyleBackColor = true;
        // 
        // _chkStoreInDedicatedFolders
        // 
        _chkStoreInDedicatedFolders.AutoSize = true;
        _chkStoreInDedicatedFolders.Location = new Point(33, 83);
        _chkStoreInDedicatedFolders.Name = "_chkStoreInDedicatedFolders";
        _chkStoreInDedicatedFolders.Size = new Size(1062, 34);
        _chkStoreInDedicatedFolders.TabIndex = 2;
        _chkStoreInDedicatedFolders.Text = "Store extracted conversation files in dedicated folders derived from the automatic title of the conversation";
        _chkStoreInDedicatedFolders.UseVisualStyleBackColor = true;
        // 
        // _pnlSettings2
        // 
        _pnlSettings2.Controls.Add(_optSelectFolderAtRuntime);
        _pnlSettings2.Controls.Add(_chkStoreInDedicatedFolders2);
        _pnlSettings2.Location = new Point(172, 540);
        _pnlSettings2.Name = "_pnlSettings2";
        _pnlSettings2.Size = new Size(1632, 85);
        _pnlSettings2.TabIndex = 6;
        // 
        // _optSelectFolderAtRuntime
        // 
        _optSelectFolderAtRuntime.AutoSize = true;
        _optSelectFolderAtRuntime.Location = new Point(3, 3);
        _optSelectFolderAtRuntime.Name = "_optSelectFolderAtRuntime";
        _optSelectFolderAtRuntime.Size = new Size(1191, 34);
        _optSelectFolderAtRuntime.TabIndex = 0;
        _optSelectFolderAtRuntime.TabStop = true;
        _optSelectFolderAtRuntime.Text = "Extract and save code, but select a folder for text-file/data extraction, only at the time when the Conversation requires it";
        _optSelectFolderAtRuntime.UseVisualStyleBackColor = true;
        _optSelectFolderAtRuntime.CheckedChanged += RadioButton2_CheckedChanged;
        // 
        // _chkStoreInDedicatedFolders2
        // 
        _chkStoreInDedicatedFolders2.AutoSize = true;
        _chkStoreInDedicatedFolders2.Location = new Point(31, 43);
        _chkStoreInDedicatedFolders2.Name = "_chkStoreInDedicatedFolders2";
        _chkStoreInDedicatedFolders2.Size = new Size(1062, 34);
        _chkStoreInDedicatedFolders2.TabIndex = 1;
        _chkStoreInDedicatedFolders2.Text = "Store extracted conversation files in dedicated folders derived from the automatic title of the conversation";
        _chkStoreInDedicatedFolders2.UseVisualStyleBackColor = true;
        // 
        // _btnOK
        // 
        _btnOK.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
        _btnOK.Location = new Point(1491, 1096);
        _btnOK.Name = "_btnOK";
        _btnOK.Size = new Size(170, 51);
        _btnOK.TabIndex = 1;
        _btnOK.Text = "OK";
        _btnOK.UseVisualStyleBackColor = true;
        _btnOK.Click += BtnOK_Click;
        // 
        // _btnCancel
        // 
        _btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
        _btnCancel.Location = new Point(1667, 1096);
        _btnCancel.Name = "_btnCancel";
        _btnCancel.Size = new Size(170, 51);
        _btnCancel.TabIndex = 2;
        _btnCancel.Text = "Cancel";
        _btnCancel.UseVisualStyleBackColor = true;
        _btnCancel.Click += BtnCancel_Click;
        // 
        // FrmManagePersonalities
        // 
        AutoScaleDimensions = new SizeF(12F, 30F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(1853, 1168);
        Controls.Add(_btnCancel);
        Controls.Add(_btnOK);
        Controls.Add(_splitMain);
        Font = new Font("Segoe UI", 10.875F, FontStyle.Regular, GraphicsUnit.Point, 0);
        Margin = new Padding(4);
        Name = "FrmManagePersonalities";
        Text = "Manage Personalities";
        _splitMain.Panel1.ResumeLayout(false);
        _splitMain.Panel2.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)_splitMain).EndInit();
        _splitMain.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)_dgvPersonalities).EndInit();
        _tlpEditPersonality.ResumeLayout(false);
        _tlpEditPersonality.PerformLayout();
        _fdpPersonalityName.ResumeLayout(false);
        _fdpPersonalityDescription.ResumeLayout(false);
        _pnlSettings1.ResumeLayout(false);
        _pnlSettings1.PerformLayout();
        _pnlSettings2.ResumeLayout(false);
        _pnlSettings2.PerformLayout();
        ResumeLayout(false);

    }

    private void RadioButton1_CheckedChanged(object sender, EventArgs e)
    {
        throw new NotImplementedException();
    }

    private void RadioButton2_CheckedChanged(object sender, EventArgs e)
    {
        throw new NotImplementedException();
    }

    #endregion

    private System.Windows.Forms.SplitContainer _splitMain;
    private System.Windows.Forms.DataGridView _dgvPersonalities;
    private System.Windows.Forms.TableLayoutPanel _tlpEditPersonality;
    private System.Windows.Forms.Label _lblPersonality;
    private System.Windows.Forms.Label _lblSystemPrompt;
    private CommunityToolkit.WinForms.FluentUI.FluentDecoratorPanel _fdpPersonalityName;
    private CommunityToolkit.WinForms.FluentUI.FluentDecoratorPanel _fdpPersonalityDescription;
    private System.Windows.Forms.Button _btnOK;
    private System.Windows.Forms.Button _btnCancel;
    private CommunityToolkit.WinForms.FluentUI.Controls.AutoCompleteEditor _txtPersonalityName;
    private CommunityToolkit.WinForms.FluentUI.Controls.AutoCompleteEditor _txtPersonalityDescription;
    private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
    private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
    private System.Windows.Forms.Panel _pnlSettings1;
    private System.Windows.Forms.RadioButton _optExtractCodeAutomatically;
    private System.Windows.Forms.CheckBox _chkSaveExtractedFilesAutomatically;
    private System.Windows.Forms.CheckBox _chkStoreInDedicatedFolders;
    private System.Windows.Forms.Panel _pnlSettings2;
    private System.Windows.Forms.RadioButton _optSelectFolderAtRuntime;
    private System.Windows.Forms.CheckBox _chkStoreInDedicatedFolders2;
    private System.Windows.Forms.RadioButton _optDontDoFileExtractions; // New option button
}
