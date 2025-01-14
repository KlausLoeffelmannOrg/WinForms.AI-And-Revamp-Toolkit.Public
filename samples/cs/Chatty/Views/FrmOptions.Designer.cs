namespace Chatty.Views
{
    partial class FrmOptions
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
            _chkArchiveChats = new CheckBox();
            groupBox1 = new GroupBox();
            fluentDecoratorPanel1 = new CommunityToolkit.WinForms.FluentUI.FluentDecoratorPanel();
            _txtAppDataPath = new TextBox();
            _btnPickPath = new Button();
            _btnOK = new Button();
            _btnCancel = new Button();
            groupBox1.SuspendLayout();
            fluentDecoratorPanel1.SuspendLayout();
            SuspendLayout();
            // 
            // _chkArchiveChats
            // 
            _chkArchiveChats.AutoSize = true;
            _chkArchiveChats.Location = new Point(42, 55);
            _chkArchiveChats.Margin = new Padding(4);
            _chkArchiveChats.Name = "_chkArchiveChats";
            _chkArchiveChats.Size = new Size(341, 44);
            _chkArchiveChats.TabIndex = 0;
            _chkArchiveChats.Text = "Archive chats to folder:";
            _chkArchiveChats.UseVisualStyleBackColor = true;
            _chkArchiveChats.CheckedChanged += ChkArchiveChats_CheckedChanged;
            // 
            // groupBox1
            // 
            groupBox1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            groupBox1.Controls.Add(fluentDecoratorPanel1);
            groupBox1.Controls.Add(_chkArchiveChats);
            groupBox1.Location = new Point(13, 33);
            groupBox1.Margin = new Padding(4);
            groupBox1.Name = "groupBox1";
            groupBox1.Padding = new Padding(4);
            groupBox1.Size = new Size(862, 188);
            groupBox1.TabIndex = 1;
            groupBox1.TabStop = false;
            groupBox1.Text = "Archive chats";
            // 
            // fluentDecoratorPanel1
            // 
            fluentDecoratorPanel1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            fluentDecoratorPanel1.BorderThickness = 1;
            fluentDecoratorPanel1.Controls.Add(_txtAppDataPath);
            fluentDecoratorPanel1.Controls.Add(_btnPickPath);
            fluentDecoratorPanel1.Location = new Point(76, 111);
            fluentDecoratorPanel1.Margin = new Padding(4);
            fluentDecoratorPanel1.Name = "fluentDecoratorPanel1";
            fluentDecoratorPanel1.Padding = new Padding(6);
            fluentDecoratorPanel1.Size = new Size(752, 65);
            fluentDecoratorPanel1.TabIndex = 1;
            // 
            // _txtAppDataPath
            // 
            _txtAppDataPath.BorderStyle = BorderStyle.None;
            _txtAppDataPath.Location = new Point(7, 13);
            _txtAppDataPath.Margin = new Padding(4);
            _txtAppDataPath.Name = "_txtAppDataPath";
            _txtAppDataPath.Size = new Size(678, 39);
            _txtAppDataPath.TabIndex = 0;
            // 
            // _btnPickPath
            // 
            _btnPickPath.Font = new Font("Segoe UI", 7.8F, FontStyle.Regular, GraphicsUnit.Point, 0);
            _btnPickPath.Location = new Point(695, 13);
            _btnPickPath.Margin = new Padding(4);
            _btnPickPath.Name = "_btnPickPath";
            _btnPickPath.Size = new Size(50, 39);
            _btnPickPath.TabIndex = 2;
            _btnPickPath.Text = "...";
            _btnPickPath.UseVisualStyleBackColor = true;
            _btnPickPath.Click += BtnPickPath_Click;
            // 
            // _btnOK
            // 
            _btnOK.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            _btnOK.Location = new Point(912, 33);
            _btnOK.Name = "_btnOK";
            _btnOK.Size = new Size(215, 71);
            _btnOK.TabIndex = 2;
            _btnOK.Text = "OK";
            _btnOK.UseVisualStyleBackColor = true;
            // 
            // _btnCancel
            // 
            _btnCancel.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            _btnCancel.Location = new Point(912, 125);
            _btnCancel.Name = "_btnCancel";
            _btnCancel.Size = new Size(215, 71);
            _btnCancel.TabIndex = 3;
            _btnCancel.Text = "Cancel";
            _btnCancel.UseVisualStyleBackColor = true;
            // 
            // FrmOptions
            // 
            AutoScaleDimensions = new SizeF(16F, 40F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1139, 234);
            Controls.Add(_btnCancel);
            Controls.Add(_btnOK);
            Controls.Add(groupBox1);
            Font = new Font("Segoe UI", 10.8F, FontStyle.Regular, GraphicsUnit.Point, 0);
            Margin = new Padding(4);
            Name = "FrmOptions";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Options";
            Load += FrmOptions_Load;
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            fluentDecoratorPanel1.ResumeLayout(false);
            fluentDecoratorPanel1.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private CheckBox _chkArchiveChats;
        private GroupBox groupBox1;
        private CommunityToolkit.WinForms.FluentUI.FluentDecoratorPanel fluentDecoratorPanel1;
        private TextBox _txtAppDataPath;
        private Button _btnPickPath;
        private Button _btnOK;
        private Button _btnCancel;
    }
}