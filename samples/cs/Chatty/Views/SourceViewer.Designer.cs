namespace Chatty.Views
{
    partial class SourceViewer
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            _sourceCodeViewer = new CommunityToolkit.WinForms.Controls.SourceCodeViewer();
            _splitterMain = new SplitContainer();
            _mainTab = new CommunityToolkit.WinForms.FluentUI.FluentTabControl();
            ((System.ComponentModel.ISupportInitialize)_splitterMain).BeginInit();
            _splitterMain.Panel1.SuspendLayout();
            _splitterMain.Panel2.SuspendLayout();
            _splitterMain.SuspendLayout();
            SuspendLayout();
            // 
            // _sourceCodeViewer
            // 
            _sourceCodeViewer.Dock = DockStyle.Fill;
            _sourceCodeViewer.Font = new Font("Consolas", 12F);
            _sourceCodeViewer.Location = new Point(10, 10);
            _sourceCodeViewer.Name = "_sourceCodeViewer";
            _sourceCodeViewer.ReadOnly = true;
            _sourceCodeViewer.Size = new Size(926, 480);
            _sourceCodeViewer.TabIndex = 0;
            // 
            // _splitterMain
            // 
            _splitterMain.Dock = DockStyle.Fill;
            _splitterMain.Location = new Point(10, 10);
            _splitterMain.Name = "_splitterMain";
            _splitterMain.Orientation = Orientation.Horizontal;
            // 
            // _splitterMain.Panel1
            // 
            _splitterMain.Panel1.Controls.Add(_sourceCodeViewer);
            _splitterMain.Panel1.Padding = new Padding(10);
            // 
            // _splitterMain.Panel2
            // 
            _splitterMain.Panel2.Controls.Add(_mainTab);
            _splitterMain.Panel2.Padding = new Padding(10);
            _splitterMain.Size = new Size(946, 724);
            _splitterMain.SplitterDistance = 500;
            _splitterMain.TabIndex = 1;
            // 
            // _mainTab
            // 
            _mainTab.Dock = DockStyle.Fill;
            _mainTab.Location = new Point(10, 10);
            _mainTab.Name = "_mainTab";
            _mainTab.Size = new Size(926, 200);
            _mainTab.TabIndex = 0;
            // 
            // SourceViewer
            // 
            AutoScaleDimensions = new SizeF(12F, 30F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(_splitterMain);
            Name = "SourceViewer";
            Padding = new Padding(10);
            Size = new Size(966, 744);
            _splitterMain.Panel1.ResumeLayout(false);
            _splitterMain.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)_splitterMain).EndInit();
            _splitterMain.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private CommunityToolkit.WinForms.Controls.SourceCodeViewer _sourceCodeViewer;
        private SplitContainer _splitterMain;
        private CommunityToolkit.WinForms.FluentUI.FluentTabControl _mainTab;
    }
}
