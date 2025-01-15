namespace Chatty.Views
{
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
            _tlpEditPersonality = new TableLayoutPanel();
            label1 = new Label();
            label2 = new Label();
            _fdpPersonalityName = new CommunityToolkit.WinForms.FluentUI.FluentDecoratorPanel();
            _fdpPersonalityDescription = new CommunityToolkit.WinForms.FluentUI.FluentDecoratorPanel();
            _rtbSystemPrompt = new RichTextBox();
            _btnOK = new Button();
            _btnCancel = new Button();
            _txtPersonalityName = new TextBox();
            ((System.ComponentModel.ISupportInitialize)_splitMain).BeginInit();
            _splitMain.Panel1.SuspendLayout();
            _splitMain.Panel2.SuspendLayout();
            _splitMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)_dgvPersonalities).BeginInit();
            _tlpEditPersonality.SuspendLayout();
            _fdpPersonalityName.SuspendLayout();
            _fdpPersonalityDescription.SuspendLayout();
            SuspendLayout();
            // 
            // _splitMain
            // 
            _splitMain.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            _splitMain.Location = new Point(13, 13);
            _splitMain.Margin = new Padding(4, 4, 4, 4);
            _splitMain.Name = "_splitMain";
            _splitMain.Orientation = Orientation.Horizontal;
            // 
            // _splitMain.Panel1
            // 
            _splitMain.Panel1.Controls.Add(_dgvPersonalities);
            // 
            // _splitMain.Panel2
            // 
            _splitMain.Panel2.Controls.Add(_tlpEditPersonality);
            _splitMain.Size = new Size(1827, 874);
            _splitMain.SplitterDistance = 496;
            _splitMain.SplitterWidth = 5;
            _splitMain.TabIndex = 0;
            // 
            // _dgvPersonalities
            // 
            _dgvPersonalities.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            _dgvPersonalities.Dock = DockStyle.Fill;
            _dgvPersonalities.Location = new Point(0, 0);
            _dgvPersonalities.Margin = new Padding(4, 4, 4, 4);
            _dgvPersonalities.Name = "_dgvPersonalities";
            _dgvPersonalities.RowHeadersWidth = 82;
            _dgvPersonalities.Size = new Size(1827, 496);
            _dgvPersonalities.TabIndex = 0;
            // 
            // _tlpEditPersonality
            // 
            _tlpEditPersonality.ColumnCount = 2;
            _tlpEditPersonality.ColumnStyles.Add(new ColumnStyle());
            _tlpEditPersonality.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            _tlpEditPersonality.Controls.Add(label1, 0, 0);
            _tlpEditPersonality.Controls.Add(label2, 0, 1);
            _tlpEditPersonality.Controls.Add(_fdpPersonalityName, 1, 0);
            _tlpEditPersonality.Controls.Add(_fdpPersonalityDescription, 1, 1);
            _tlpEditPersonality.Dock = DockStyle.Fill;
            _tlpEditPersonality.Location = new Point(0, 0);
            _tlpEditPersonality.Margin = new Padding(4, 4, 4, 4);
            _tlpEditPersonality.Name = "_tlpEditPersonality";
            _tlpEditPersonality.RowCount = 2;
            _tlpEditPersonality.RowStyles.Add(new RowStyle());
            _tlpEditPersonality.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            _tlpEditPersonality.Size = new Size(1827, 373);
            _tlpEditPersonality.TabIndex = 0;
            // 
            // label1
            // 
            label1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            label1.AutoSize = true;
            label1.Location = new Point(3, 0);
            label1.Name = "label1";
            label1.Size = new Size(214, 79);
            label1.TabIndex = 0;
            label1.Text = "Personality:";
            label1.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // label2
            // 
            label2.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            label2.AutoSize = true;
            label2.Location = new Point(3, 99);
            label2.Margin = new Padding(3, 20, 3, 0);
            label2.Name = "label2";
            label2.Size = new Size(214, 274);
            label2.TabIndex = 1;
            label2.Text = "System-Prompt";
            // 
            // _fdpPersonalityName
            // 
            _fdpPersonalityName.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            _fdpPersonalityName.BorderThickness = 1;
            _fdpPersonalityName.Controls.Add(_txtPersonalityName);
            _fdpPersonalityName.Location = new Point(223, 3);
            _fdpPersonalityName.Name = "_fdpPersonalityName";
            _fdpPersonalityName.Padding = new Padding(10);
            _fdpPersonalityName.Size = new Size(1601, 73);
            _fdpPersonalityName.TabIndex = 2;
            // 
            // _fdpPersonalityDescription
            // 
            _fdpPersonalityDescription.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            _fdpPersonalityDescription.BorderThickness = 1;
            _fdpPersonalityDescription.Controls.Add(_rtbSystemPrompt);
            _fdpPersonalityDescription.Location = new Point(223, 82);
            _fdpPersonalityDescription.Name = "_fdpPersonalityDescription";
            _fdpPersonalityDescription.Padding = new Padding(10);
            _fdpPersonalityDescription.Size = new Size(1601, 288);
            _fdpPersonalityDescription.TabIndex = 3;
            // 
            // _rtbSystemPrompt
            // 
            _rtbSystemPrompt.Anchor = AnchorStyles.Top | AnchorStyles.Bottom;
            _rtbSystemPrompt.BorderStyle = BorderStyle.None;
            _rtbSystemPrompt.Location = new Point(11, 23);
            _rtbSystemPrompt.Name = "_rtbSystemPrompt";
            _rtbSystemPrompt.Size = new Size(1579, 242);
            _rtbSystemPrompt.TabIndex = 0;
            _rtbSystemPrompt.Text = "";
            // 
            // _btnOK
            // 
            _btnOK.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            _btnOK.Location = new Point(1382, 918);
            _btnOK.Name = "_btnOK";
            _btnOK.Size = new Size(220, 68);
            _btnOK.TabIndex = 0;
            _btnOK.Text = "OK";
            _btnOK.UseVisualStyleBackColor = true;
            // 
            // _btnCancel
            // 
            _btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            _btnCancel.Location = new Point(1617, 918);
            _btnCancel.Name = "_btnCancel";
            _btnCancel.Size = new Size(220, 68);
            _btnCancel.TabIndex = 1;
            _btnCancel.Text = "Cancel";
            _btnCancel.UseVisualStyleBackColor = true;
            // 
            // _txtPersonalityName
            // 
            _txtPersonalityName.BorderStyle = BorderStyle.None;
            _txtPersonalityName.Location = new Point(11, 17);
            _txtPersonalityName.Name = "_txtPersonalityName";
            _txtPersonalityName.Size = new Size(1579, 39);
            _txtPersonalityName.TabIndex = 0;
            // 
            // FrmManagePersonalities
            // 
            AutoScaleDimensions = new SizeF(16F, 40F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1853, 1007);
            Controls.Add(_btnCancel);
            Controls.Add(_btnOK);
            Controls.Add(_splitMain);
            Font = new Font("Segoe UI", 10.875F, FontStyle.Regular, GraphicsUnit.Point, 0);
            Margin = new Padding(4, 4, 4, 4);
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
            _fdpPersonalityName.PerformLayout();
            _fdpPersonalityDescription.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private SplitContainer _splitMain;
        private DataGridView _dgvPersonalities;
        private TableLayoutPanel _tlpEditPersonality;
        private Label label1;
        private Label label2;
        private CommunityToolkit.WinForms.FluentUI.FluentDecoratorPanel _fdpPersonalityName;
        private CommunityToolkit.WinForms.FluentUI.FluentDecoratorPanel _fdpPersonalityDescription;
        private RichTextBox _rtbSystemPrompt;
        private Button _btnOK;
        private Button _btnCancel;
        private TextBox _txtPersonalityName;
    }
}