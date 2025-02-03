namespace Chatty.Views
{
    partial class RoslynSourceView
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
            _lblListingTitel = new Label();
            ((System.ComponentModel.ISupportInitialize)_splitterMain).BeginInit();
            _splitterMain.Panel1.SuspendLayout();
            _splitterMain.Panel2.SuspendLayout();
            _splitterMain.SuspendLayout();
            SuspendLayout();
            // 
            // _sourceCodeViewer
            // 
            _sourceCodeViewer.Dock = DockStyle.Fill;
            _sourceCodeViewer.Location = new Point(8, 46);
            _sourceCodeViewer.Margin = new Padding(2, 2, 2, 2);
            _sourceCodeViewer.Name = "_sourceCodeViewer";
            _sourceCodeViewer.ReadOnly = true;
            _sourceCodeViewer.Size = new Size(773, 363);
            _sourceCodeViewer.TabIndex = 0;
            // 
            // _splitterMain
            // 
            _splitterMain.Dock = DockStyle.Fill;
            _splitterMain.Location = new Point(8, 8);
            _splitterMain.Margin = new Padding(2, 2, 2, 2);
            _splitterMain.Name = "_splitterMain";
            _splitterMain.Orientation = Orientation.Horizontal;
            // 
            // _splitterMain.Panel1
            // 
            _splitterMain.Panel1.Controls.Add(_sourceCodeViewer);
            _splitterMain.Panel1.Controls.Add(_lblListingTitel);
            _splitterMain.Panel1.Padding = new Padding(8, 8, 8, 8);
            // 
            // _splitterMain.Panel2
            // 
            _splitterMain.Panel2.Controls.Add(_mainTab);
            _splitterMain.Panel2.Padding = new Padding(8, 8, 8, 8);
            _splitterMain.Size = new Size(789, 604);
            _splitterMain.SplitterDistance = 417;
            _splitterMain.SplitterWidth = 3;
            _splitterMain.TabIndex = 1;
            // 
            // _mainTab
            // 
            _mainTab.Dock = DockStyle.Fill;
            _mainTab.Location = new Point(8, 8);
            _mainTab.Margin = new Padding(2, 2, 2, 2);
            _mainTab.Name = "_mainTab";
            _mainTab.Size = new Size(773, 168);
            _mainTab.TabIndex = 0;
            // 
            // _lblListingTitel
            // 
            _lblListingTitel.Dock = DockStyle.Top;
            _lblListingTitel.Font = new Font("Segoe UI Semibold", 11F, FontStyle.Bold, GraphicsUnit.Point, 0);
            _lblListingTitel.Location = new Point(8, 8);
            _lblListingTitel.Name = "_lblListingTitel";
            _lblListingTitel.Padding = new Padding(3);
            _lblListingTitel.Size = new Size(773, 38);
            _lblListingTitel.TabIndex = 1;
            _lblListingTitel.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // SourceViewer
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(_splitterMain);
            Margin = new Padding(2, 2, 2, 2);
            Name = "SourceViewer";
            Padding = new Padding(8, 8, 8, 8);
            Size = new Size(805, 620);
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
        private Label _lblListingTitel;
    }
}
