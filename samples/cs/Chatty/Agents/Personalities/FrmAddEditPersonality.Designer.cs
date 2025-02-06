namespace Chatty.Agents.Personalities
{
    partial class FrmAddEditPersonality
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
            _tlpEditPersonality = new TableLayoutPanel();
            _lblPersonality = new Label();
            _lblSystemPrompt = new Label();
            _fdpPersonalityName = new CommunityToolkit.WinForms.FluentUI.FluentDecoratorPanel();
            _txtPersonalityName = new CommunityToolkit.WinForms.FluentUI.Controls.AutoCompleteEditor();
            _fdpPersonalityDescription = new CommunityToolkit.WinForms.FluentUI.FluentDecoratorPanel();
            _txtPersonalityDescription = new CommunityToolkit.WinForms.FluentUI.Controls.AutoCompleteEditor();
            _groupBoxOptions = new GroupBox();
            _optDontDoFileExtractions = new RadioButton();
            _optExtractCodeAutomatically = new RadioButton();
            _chkSaveExtractedFilesAutomatically = new CheckBox();
            _chkStoreInDedicatedFolders = new CheckBox();
            _optSelectFolderAtRuntime = new RadioButton();
            _chkStoreInDedicatedFolders2 = new CheckBox();
            _btnOK = new Button();
            _btnCancel = new Button();
            _tlpEditPersonality.SuspendLayout();
            _fdpPersonalityName.SuspendLayout();
            _fdpPersonalityDescription.SuspendLayout();
            _groupBoxOptions.SuspendLayout();
            SuspendLayout();
            // 
            // _tlpEditPersonality
            // 
            _tlpEditPersonality.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            _tlpEditPersonality.ColumnCount = 2;
            _tlpEditPersonality.ColumnStyles.Add(new ColumnStyle());
            _tlpEditPersonality.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            _tlpEditPersonality.Controls.Add(_lblPersonality, 0, 0);
            _tlpEditPersonality.Controls.Add(_lblSystemPrompt, 0, 1);
            _tlpEditPersonality.Controls.Add(_fdpPersonalityName, 1, 0);
            _tlpEditPersonality.Controls.Add(_fdpPersonalityDescription, 1, 1);
            _tlpEditPersonality.Controls.Add(_groupBoxOptions, 1, 2);
            _tlpEditPersonality.Location = new Point(19, 30);
            _tlpEditPersonality.Margin = new Padding(6);
            _tlpEditPersonality.Name = "_tlpEditPersonality";
            _tlpEditPersonality.RowCount = 3;
            _tlpEditPersonality.RowStyles.Add(new RowStyle());
            _tlpEditPersonality.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            _tlpEditPersonality.RowStyles.Add(new RowStyle());
            _tlpEditPersonality.Size = new Size(1375, 832);
            _tlpEditPersonality.TabIndex = 1;
            // 
            // _lblPersonality
            // 
            _lblPersonality.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            _lblPersonality.AutoSize = true;
            _lblPersonality.Location = new Point(5, 0);
            _lblPersonality.Margin = new Padding(5, 0, 5, 0);
            _lblPersonality.Name = "_lblPersonality";
            _lblPersonality.Size = new Size(199, 81);
            _lblPersonality.TabIndex = 0;
            _lblPersonality.Text = "Personality:";
            _lblPersonality.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // _lblSystemPrompt
            // 
            _lblSystemPrompt.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            _lblSystemPrompt.AutoSize = true;
            _lblSystemPrompt.Location = new Point(5, 81);
            _lblSystemPrompt.Margin = new Padding(5, 0, 5, 0);
            _lblSystemPrompt.Name = "_lblSystemPrompt";
            _lblSystemPrompt.Size = new Size(199, 335);
            _lblSystemPrompt.TabIndex = 2;
            _lblSystemPrompt.Text = "System-Prompt";
            // 
            // _fdpPersonalityName
            // 
            _fdpPersonalityName.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            _fdpPersonalityName.BorderThickness = 1;
            _fdpPersonalityName.Controls.Add(_txtPersonalityName);
            _fdpPersonalityName.Location = new Point(214, 5);
            _fdpPersonalityName.Margin = new Padding(5);
            _fdpPersonalityName.Name = "_fdpPersonalityName";
            _fdpPersonalityName.Padding = new Padding(14);
            _fdpPersonalityName.Size = new Size(1156, 71);
            _fdpPersonalityName.TabIndex = 1;
            // 
            // _txtPersonalityName
            // 
            _txtPersonalityName.BorderStyle = BorderStyle.None;
            _txtPersonalityName.Location = new Point(15, 17);
            _txtPersonalityName.Margin = new Padding(5);
            _txtPersonalityName.MaxRemainingCharsSuggestionRequestSensitivity = 0;
            _txtPersonalityName.MinCharChangedBeforeNextTextReviewRequest = 60;
            _txtPersonalityName.MinTimeThresholdForNextTextReviewRequest = 4F;
            _txtPersonalityName.Name = "_txtPersonalityName";
            _txtPersonalityName.Size = new Size(1126, 36);
            _txtPersonalityName.TabIndex = 0;
            _txtPersonalityName.Text = "";
            // 
            // _fdpPersonalityDescription
            // 
            _fdpPersonalityDescription.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            _fdpPersonalityDescription.BorderThickness = 1;
            _fdpPersonalityDescription.Controls.Add(_txtPersonalityDescription);
            _fdpPersonalityDescription.Location = new Point(214, 86);
            _fdpPersonalityDescription.Margin = new Padding(5);
            _fdpPersonalityDescription.Name = "_fdpPersonalityDescription";
            _fdpPersonalityDescription.Padding = new Padding(14);
            _fdpPersonalityDescription.Size = new Size(1156, 325);
            _fdpPersonalityDescription.TabIndex = 3;
            // 
            // _txtPersonalityDescription
            // 
            _txtPersonalityDescription.BorderStyle = BorderStyle.None;
            _txtPersonalityDescription.Dock = DockStyle.Fill;
            _txtPersonalityDescription.Location = new Point(15, 14);
            _txtPersonalityDescription.Margin = new Padding(5);
            _txtPersonalityDescription.MaxRemainingCharsSuggestionRequestSensitivity = 0;
            _txtPersonalityDescription.MinCharChangedBeforeNextTextReviewRequest = 60;
            _txtPersonalityDescription.MinTimeThresholdForNextTextReviewRequest = 4F;
            _txtPersonalityDescription.Name = "_txtPersonalityDescription";
            _txtPersonalityDescription.Size = new Size(1126, 297);
            _txtPersonalityDescription.TabIndex = 0;
            _txtPersonalityDescription.Text = "";
            // 
            // _groupBoxOptions
            // 
            _groupBoxOptions.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            _groupBoxOptions.Controls.Add(_optDontDoFileExtractions);
            _groupBoxOptions.Controls.Add(_optExtractCodeAutomatically);
            _groupBoxOptions.Controls.Add(_chkSaveExtractedFilesAutomatically);
            _groupBoxOptions.Controls.Add(_chkStoreInDedicatedFolders);
            _groupBoxOptions.Controls.Add(_optSelectFolderAtRuntime);
            _groupBoxOptions.Controls.Add(_chkStoreInDedicatedFolders2);
            _groupBoxOptions.Location = new Point(214, 421);
            _groupBoxOptions.Margin = new Padding(5);
            _groupBoxOptions.Name = "_groupBoxOptions";
            _groupBoxOptions.Padding = new Padding(5);
            _groupBoxOptions.Size = new Size(1156, 406);
            _groupBoxOptions.TabIndex = 4;
            _groupBoxOptions.TabStop = false;
            _groupBoxOptions.Text = "Options";
            // 
            // _optDontDoFileExtractions
            // 
            _optDontDoFileExtractions.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            _optDontDoFileExtractions.AutoEllipsis = true;
            _optDontDoFileExtractions.Checked = true;
            _optDontDoFileExtractions.Location = new Point(28, 43);
            _optDontDoFileExtractions.Margin = new Padding(8);
            _optDontDoFileExtractions.Name = "_optDontDoFileExtractions";
            _optDontDoFileExtractions.Size = new Size(1097, 49);
            _optDontDoFileExtractions.TabIndex = 0;
            _optDontDoFileExtractions.TabStop = true;
            _optDontDoFileExtractions.Text = "Don't do file extractions";
            _optDontDoFileExtractions.UseVisualStyleBackColor = true;
            // 
            // _optExtractCodeAutomatically
            // 
            _optExtractCodeAutomatically.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            _optExtractCodeAutomatically.AutoEllipsis = true;
            _optExtractCodeAutomatically.Location = new Point(28, 101);
            _optExtractCodeAutomatically.Margin = new Padding(5);
            _optExtractCodeAutomatically.Name = "_optExtractCodeAutomatically";
            _optExtractCodeAutomatically.Size = new Size(1097, 49);
            _optExtractCodeAutomatically.TabIndex = 1;
            _optExtractCodeAutomatically.TabStop = true;
            _optExtractCodeAutomatically.Text = "Extract and save code in Markdown code blocks automatically.";
            _optExtractCodeAutomatically.UseVisualStyleBackColor = true;
            // 
            // _chkSaveExtractedFilesAutomatically
            // 
            _chkSaveExtractedFilesAutomatically.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            _chkSaveExtractedFilesAutomatically.AutoEllipsis = true;
            _chkSaveExtractedFilesAutomatically.Location = new Point(56, 158);
            _chkSaveExtractedFilesAutomatically.Margin = new Padding(5);
            _chkSaveExtractedFilesAutomatically.Name = "_chkSaveExtractedFilesAutomatically";
            _chkSaveExtractedFilesAutomatically.Size = new Size(1069, 49);
            _chkSaveExtractedFilesAutomatically.TabIndex = 2;
            _chkSaveExtractedFilesAutomatically.Text = "Automatically save the extracted listing/text files to the Conversation folder";
            _chkSaveExtractedFilesAutomatically.UseVisualStyleBackColor = true;
            // 
            // _chkStoreInDedicatedFolders
            // 
            _chkStoreInDedicatedFolders.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            _chkStoreInDedicatedFolders.AutoEllipsis = true;
            _chkStoreInDedicatedFolders.Location = new Point(56, 216);
            _chkStoreInDedicatedFolders.Margin = new Padding(5);
            _chkStoreInDedicatedFolders.Name = "_chkStoreInDedicatedFolders";
            _chkStoreInDedicatedFolders.Size = new Size(1069, 49);
            _chkStoreInDedicatedFolders.TabIndex = 3;
            _chkStoreInDedicatedFolders.Text = "Store extracted conversation files in dedicated folders derived from the automatic title of the conversation";
            _chkStoreInDedicatedFolders.UseVisualStyleBackColor = true;
            // 
            // _optSelectFolderAtRuntime
            // 
            _optSelectFolderAtRuntime.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            _optSelectFolderAtRuntime.AutoEllipsis = true;
            _optSelectFolderAtRuntime.Location = new Point(28, 274);
            _optSelectFolderAtRuntime.Margin = new Padding(5);
            _optSelectFolderAtRuntime.Name = "_optSelectFolderAtRuntime";
            _optSelectFolderAtRuntime.Size = new Size(1097, 49);
            _optSelectFolderAtRuntime.TabIndex = 4;
            _optSelectFolderAtRuntime.TabStop = true;
            _optSelectFolderAtRuntime.Text = "Extract and save code, but select a folder for text-file/data extraction, only at the time when the Conversation requires it";
            _optSelectFolderAtRuntime.UseVisualStyleBackColor = true;
            // 
            // _chkStoreInDedicatedFolders2
            // 
            _chkStoreInDedicatedFolders2.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            _chkStoreInDedicatedFolders2.AutoEllipsis = true;
            _chkStoreInDedicatedFolders2.Location = new Point(56, 331);
            _chkStoreInDedicatedFolders2.Margin = new Padding(5);
            _chkStoreInDedicatedFolders2.Name = "_chkStoreInDedicatedFolders2";
            _chkStoreInDedicatedFolders2.Size = new Size(1069, 49);
            _chkStoreInDedicatedFolders2.TabIndex = 5;
            _chkStoreInDedicatedFolders2.Text = "Store extracted conversation files in dedicated folders derived from the automatic title of the conversation";
            _chkStoreInDedicatedFolders2.UseVisualStyleBackColor = true;
            // 
            // _btnOK
            // 
            _btnOK.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            _btnOK.DialogResult = DialogResult.OK;
            _btnOK.Location = new Point(1410, 28);
            _btnOK.Margin = new Padding(5);
            _btnOK.Name = "_btnOK";
            _btnOK.Size = new Size(212, 62);
            _btnOK.TabIndex = 2;
            _btnOK.Text = "OK";
            _btnOK.UseVisualStyleBackColor = true;
            // 
            // _btnCancel
            // 
            _btnCancel.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            _btnCancel.DialogResult = DialogResult.Cancel;
            _btnCancel.Location = new Point(1410, 98);
            _btnCancel.Margin = new Padding(5);
            _btnCancel.Name = "_btnCancel";
            _btnCancel.Size = new Size(212, 62);
            _btnCancel.TabIndex = 3;
            _btnCancel.Text = "Cancel";
            _btnCancel.UseVisualStyleBackColor = true;
            // 
            // FrmAddEditPersonality
            // 
            AcceptButton = _btnOK;
            AutoScaleDimensions = new SizeF(14F, 36F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoValidate = AutoValidate.EnableAllowFocusChange;
            CancelButton = _btnCancel;
            ClientSize = new Size(1640, 884);
            Controls.Add(_btnCancel);
            Controls.Add(_btnOK);
            Controls.Add(_tlpEditPersonality);
            Font = new Font("Segoe UI", 11.1428576F, FontStyle.Regular, GraphicsUnit.Point, 0);
            Margin = new Padding(5);
            Name = "FrmAddEditPersonality";
            Text = "AddEditPersonality";
            _tlpEditPersonality.ResumeLayout(false);
            _tlpEditPersonality.PerformLayout();
            _fdpPersonalityName.ResumeLayout(false);
            _fdpPersonalityDescription.ResumeLayout(false);
            _groupBoxOptions.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private TableLayoutPanel _tlpEditPersonality;
        private Label _lblPersonality;
        private Label _lblSystemPrompt;
        private CommunityToolkit.WinForms.FluentUI.FluentDecoratorPanel _fdpPersonalityName;
        private CommunityToolkit.WinForms.FluentUI.Controls.AutoCompleteEditor _txtPersonalityName;
        private CommunityToolkit.WinForms.FluentUI.FluentDecoratorPanel _fdpPersonalityDescription;
        private CommunityToolkit.WinForms.FluentUI.Controls.AutoCompleteEditor _txtPersonalityDescription;
        private GroupBox _groupBoxOptions;
        private RadioButton _optDontDoFileExtractions;
        private RadioButton _optExtractCodeAutomatically;
        private CheckBox _chkSaveExtractedFilesAutomatically;
        private CheckBox _chkStoreInDedicatedFolders;
        private RadioButton _optSelectFolderAtRuntime;
        private CheckBox _chkStoreInDedicatedFolders2;
        private Button _btnOK;
        private Button _btnCancel;
    }
}