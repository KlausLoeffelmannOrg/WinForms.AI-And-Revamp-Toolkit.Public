using CommunityToolkit.WinForms.AI;
using CommunityToolkit.WinForms.Controls.Blazor;
using CommunityToolkit.WinForms.ConversationView;

namespace Chatty
{
    partial class FrmMain
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmMain));
            _menuStrip = new MenuStrip();
            _tsmFile = new ToolStripMenuItem();
            toolStripMenuItem1 = new ToolStripMenuItem();
            toolStripSeparator3 = new ToolStripSeparator();
            _tsmQuit = new ToolStripMenuItem();
            _tsiEdit = new ToolStripMenuItem();
            _tsiPersonalities = new ToolStripMenuItem();
            chatMetadataToolStripMenuItem = new ToolStripMenuItem();
            toolStripMenuItem5 = new ToolStripMenuItem();
            filterKeywordsToolStripMenuItem = new ToolStripMenuItem();
            filterChatsWithFilesToolStripMenuItem = new ToolStripMenuItem();
            chatSummaryToolStripMenuItem = new ToolStripMenuItem();
            _tsmAgents = new ToolStripMenuItem();
            toolStripMenuItem6 = new ToolStripMenuItem();
            classicApproachToolStripMenuItem = new ToolStripMenuItem();
            soThatWithAIToolStripMenuItem = new ToolStripMenuItem();
            fineButWhatAboutToolStripMenuItem = new ToolStripMenuItem();
            toolStripSeparator5 = new ToolStripSeparator();
            modelOverviewToolStripMenuItem = new ToolStripMenuItem();
            _tsmPromptBuilder = new ToolStripMenuItem();
            _tsmPersonalityBuilder = new ToolStripMenuItem();
            toolStripMenuItem4 = new ToolStripMenuItem();
            toolStripMenuItem3 = new ToolStripSeparator();
            _tsmLanguageConverter = new ToolStripMenuItem();
            _classDocumenter = new ToolStripMenuItem();
            _vsProjectRampUpper = new ToolStripMenuItem();
            toolStripMenuItem2 = new ToolStripSeparator();
            formLocalizerToolStripMenuItem = new ToolStripMenuItem();
            winFormsBestPracticesQuizzerToolStripMenuItem = new ToolStripMenuItem();
            _tsmTools = new ToolStripMenuItem();
            _tsmOptions = new ToolStripMenuItem();
            helpToolStripMenuItem = new ToolStripMenuItem();
            _tsmAbout = new ToolStripMenuItem();
            _statusStrip = new StatusStrip();
            _tslItemDateInfoCaption = new ToolStripStatusLabel();
            _tslItemDateInfo = new ToolStripStatusLabel();
            _tslKeywordsCaption = new ToolStripStatusLabel();
            _tsddKeywords = new ToolStripDropDownButton();
            _tslPersonalityCaption = new ToolStripStatusLabel();
            _tslPersonality = new ToolStripStatusLabel();
            _tslProcessTimesCaption = new ToolStripStatusLabel();
            _tslProcessTimes = new ToolStripStatusLabel();
            _tslStatusCaption = new ToolStripStatusLabel();
            _tslInfo = new ToolStripStatusLabel();
            _tsbInfo = new ToolStripSplitButton();
            _tslClockInfo = new ToolStripStatusLabel();
            _toolStrip = new ToolStrip();
            newToolStripButton = new ToolStripButton();
            toolStripSeparator = new ToolStripSeparator();
            copyToolStripButton = new ToolStripButton();
            toolStripSeparator1 = new ToolStripSeparator();
            toolStripLabel1 = new ToolStripLabel();
            _tscPersonalities = new ToolStripComboBox();
            toolStripSeparator2 = new ToolStripSeparator();
            toolStripSeparator4 = new ToolStripSeparator();
            _tslModels = new ToolStripLabel();
            _tscModels = new ToolStripComboBox();
            _splitMain = new SplitContainer();
            _tlpNavigatorLayout = new TableLayoutPanel();
            _searchBoxDecorator = new CommunityToolkit.WinForms.FluentUI.FluentDecoratorPanel();
            _cmbNavigatorSearch = new ComboBox();
            fluentDecoratorPanel1 = new CommunityToolkit.WinForms.FluentUI.FluentDecoratorPanel();
            _txtSummary = new TextBox();
            _tlpHeader = new TableLayoutPanel();
            _mainTabControl = new CommunityToolkit.WinForms.FluentUI.FluentTabControl();
            _lblConversationTitle = new Label();
            _lblDate = new Label();
            _skCommunicator = new SemanticKernelComponent();
            _skMetaDataProcessor = new SemanticKernelComponent();
            fluentDecoratorPanel2 = new CommunityToolkit.WinForms.FluentUI.FluentDecoratorPanel();
            _trvConversationHistory = new TreeView();
            _menuStrip.SuspendLayout();
            _statusStrip.SuspendLayout();
            _toolStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)_splitMain).BeginInit();
            _splitMain.Panel1.SuspendLayout();
            _splitMain.Panel2.SuspendLayout();
            _splitMain.SuspendLayout();
            _tlpNavigatorLayout.SuspendLayout();
            _searchBoxDecorator.SuspendLayout();
            fluentDecoratorPanel1.SuspendLayout();
            _tlpHeader.SuspendLayout();
            fluentDecoratorPanel2.SuspendLayout();
            SuspendLayout();
            // 
            // _menuStrip
            // 
            _menuStrip.Font = new Font("Segoe UI", 11F, FontStyle.Regular, GraphicsUnit.Point, 0);
            _menuStrip.ImageScalingSize = new Size(24, 24);
            _menuStrip.Items.AddRange(new ToolStripItem[] { _tsmFile, _tsiEdit, toolStripMenuItem5, _tsmAgents, _tsmTools, helpToolStripMenuItem });
            _menuStrip.Location = new Point(0, 0);
            _menuStrip.Name = "_menuStrip";
            _menuStrip.Padding = new Padding(7, 2, 0, 2);
            _menuStrip.Size = new Size(1731, 44);
            _menuStrip.TabIndex = 0;
            _menuStrip.Text = "menuStrip1";
            // 
            // _tsmFile
            // 
            _tsmFile.DropDownItems.AddRange(new ToolStripItem[] { toolStripMenuItem1, toolStripSeparator3, _tsmQuit });
            _tsmFile.Name = "_tsmFile";
            _tsmFile.Size = new Size(72, 40);
            _tsmFile.Text = "&File";
            // 
            // toolStripMenuItem1
            // 
            toolStripMenuItem1.Name = "toolStripMenuItem1";
            toolStripMenuItem1.Size = new Size(299, 44);
            toolStripMenuItem1.Text = "Start new chat";
            toolStripMenuItem1.Click += BtnStartNewChat_Click;
            // 
            // toolStripSeparator3
            // 
            toolStripSeparator3.Name = "toolStripSeparator3";
            toolStripSeparator3.Size = new Size(296, 6);
            // 
            // _tsmQuit
            // 
            _tsmQuit.Name = "_tsmQuit";
            _tsmQuit.Size = new Size(299, 44);
            _tsmQuit.Text = "Quit";
            // 
            // _tsiEdit
            // 
            _tsiEdit.DropDownItems.AddRange(new ToolStripItem[] { _tsiPersonalities, chatMetadataToolStripMenuItem });
            _tsiEdit.Name = "_tsiEdit";
            _tsiEdit.Size = new Size(76, 40);
            _tsiEdit.Text = "&Edit";
            // 
            // _tsiPersonalities
            // 
            _tsiPersonalities.Name = "_tsiPersonalities";
            _tsiPersonalities.Size = new Size(356, 44);
            _tsiPersonalities.Text = "Chat Personalities...";
            _tsiPersonalities.Click += Personalities_Click;
            // 
            // chatMetadataToolStripMenuItem
            // 
            chatMetadataToolStripMenuItem.Name = "chatMetadataToolStripMenuItem";
            chatMetadataToolStripMenuItem.Size = new Size(356, 44);
            chatMetadataToolStripMenuItem.Text = "Chat Metadata...";
            // 
            // toolStripMenuItem5
            // 
            toolStripMenuItem5.DropDownItems.AddRange(new ToolStripItem[] { filterKeywordsToolStripMenuItem, filterChatsWithFilesToolStripMenuItem, chatSummaryToolStripMenuItem });
            toolStripMenuItem5.Name = "toolStripMenuItem5";
            toolStripMenuItem5.Size = new Size(88, 40);
            toolStripMenuItem5.Text = "View";
            // 
            // filterKeywordsToolStripMenuItem
            // 
            filterKeywordsToolStripMenuItem.Name = "filterKeywordsToolStripMenuItem";
            filterKeywordsToolStripMenuItem.Size = new Size(371, 44);
            filterKeywordsToolStripMenuItem.Text = "Filter Keywords";
            // 
            // filterChatsWithFilesToolStripMenuItem
            // 
            filterChatsWithFilesToolStripMenuItem.Name = "filterChatsWithFilesToolStripMenuItem";
            filterChatsWithFilesToolStripMenuItem.Size = new Size(371, 44);
            filterChatsWithFilesToolStripMenuItem.Text = "Filter Chats with files";
            // 
            // chatSummaryToolStripMenuItem
            // 
            chatSummaryToolStripMenuItem.Checked = true;
            chatSummaryToolStripMenuItem.CheckState = CheckState.Checked;
            chatSummaryToolStripMenuItem.Name = "chatSummaryToolStripMenuItem";
            chatSummaryToolStripMenuItem.Size = new Size(371, 44);
            chatSummaryToolStripMenuItem.Text = "Chat Summary";
            // 
            // _tsmAgents
            // 
            _tsmAgents.DropDownItems.AddRange(new ToolStripItem[] { toolStripMenuItem6, toolStripSeparator5, modelOverviewToolStripMenuItem, _tsmPromptBuilder, _tsmPersonalityBuilder, toolStripMenuItem4, toolStripMenuItem3, _tsmLanguageConverter, _classDocumenter, _vsProjectRampUpper, toolStripMenuItem2, formLocalizerToolStripMenuItem, winFormsBestPracticesQuizzerToolStripMenuItem });
            _tsmAgents.Name = "_tsmAgents";
            _tsmAgents.Size = new Size(114, 40);
            _tsmAgents.Text = "Agents";
            // 
            // toolStripMenuItem6
            // 
            toolStripMenuItem6.DropDownItems.AddRange(new ToolStripItem[] { classicApproachToolStripMenuItem, soThatWithAIToolStripMenuItem, fineButWhatAboutToolStripMenuItem });
            toolStripMenuItem6.Name = "toolStripMenuItem6";
            toolStripMenuItem6.Size = new Size(511, 44);
            toolStripMenuItem6.Text = "Classic UIs? Think different!";
            // 
            // classicApproachToolStripMenuItem
            // 
            classicApproachToolStripMenuItem.Name = "classicApproachToolStripMenuItem";
            classicApproachToolStripMenuItem.Size = new Size(390, 44);
            classicApproachToolStripMenuItem.Text = "Classic approach...";
            // 
            // soThatWithAIToolStripMenuItem
            // 
            soThatWithAIToolStripMenuItem.Name = "soThatWithAIToolStripMenuItem";
            soThatWithAIToolStripMenuItem.Size = new Size(390, 44);
            soThatWithAIToolStripMenuItem.Text = "So, that with AI...";
            // 
            // fineButWhatAboutToolStripMenuItem
            // 
            fineButWhatAboutToolStripMenuItem.Name = "fineButWhatAboutToolStripMenuItem";
            fineButWhatAboutToolStripMenuItem.Size = new Size(390, 44);
            fineButWhatAboutToolStripMenuItem.Text = "Fine. But what about...";
            // 
            // toolStripSeparator5
            // 
            toolStripSeparator5.Name = "toolStripSeparator5";
            toolStripSeparator5.Size = new Size(508, 6);
            // 
            // modelOverviewToolStripMenuItem
            // 
            modelOverviewToolStripMenuItem.Name = "modelOverviewToolStripMenuItem";
            modelOverviewToolStripMenuItem.Size = new Size(511, 44);
            modelOverviewToolStripMenuItem.Text = "Model learner...";
            // 
            // _tsmPromptBuilder
            // 
            _tsmPromptBuilder.Name = "_tsmPromptBuilder";
            _tsmPromptBuilder.Size = new Size(511, 44);
            _tsmPromptBuilder.Text = "Prompt builder...";
            // 
            // _tsmPersonalityBuilder
            // 
            _tsmPersonalityBuilder.Name = "_tsmPersonalityBuilder";
            _tsmPersonalityBuilder.Size = new Size(511, 44);
            _tsmPersonalityBuilder.Text = "Personality builder...";
            // 
            // toolStripMenuItem4
            // 
            toolStripMenuItem4.Name = "toolStripMenuItem4";
            toolStripMenuItem4.Size = new Size(511, 44);
            toolStripMenuItem4.Text = "Idea challenger...";
            // 
            // toolStripMenuItem3
            // 
            toolStripMenuItem3.Name = "toolStripMenuItem3";
            toolStripMenuItem3.Size = new Size(508, 6);
            // 
            // _tsmLanguageConverter
            // 
            _tsmLanguageConverter.Name = "_tsmLanguageConverter";
            _tsmLanguageConverter.Size = new Size(511, 44);
            _tsmLanguageConverter.Text = "Language converter...";
            // 
            // _classDocumenter
            // 
            _classDocumenter.Name = "_classDocumenter";
            _classDocumenter.Size = new Size(511, 44);
            _classDocumenter.Text = "Class documenter...";
            // 
            // _vsProjectRampUpper
            // 
            _vsProjectRampUpper.Name = "_vsProjectRampUpper";
            _vsProjectRampUpper.Size = new Size(511, 44);
            _vsProjectRampUpper.Text = "VS Project Ramp-upper...";
            // 
            // toolStripMenuItem2
            // 
            toolStripMenuItem2.Name = "toolStripMenuItem2";
            toolStripMenuItem2.Size = new Size(508, 6);
            // 
            // formLocalizerToolStripMenuItem
            // 
            formLocalizerToolStripMenuItem.Name = "formLocalizerToolStripMenuItem";
            formLocalizerToolStripMenuItem.Size = new Size(511, 44);
            formLocalizerToolStripMenuItem.Text = "Form localizer...";
            // 
            // winFormsBestPracticesQuizzerToolStripMenuItem
            // 
            winFormsBestPracticesQuizzerToolStripMenuItem.Name = "winFormsBestPracticesQuizzerToolStripMenuItem";
            winFormsBestPracticesQuizzerToolStripMenuItem.Size = new Size(511, 44);
            winFormsBestPracticesQuizzerToolStripMenuItem.Text = "WinForms best practize quizzer...";
            // 
            // _tsmTools
            // 
            _tsmTools.DropDownItems.AddRange(new ToolStripItem[] { _tsmOptions });
            _tsmTools.Name = "_tsmTools";
            _tsmTools.Size = new Size(91, 40);
            _tsmTools.Text = "Tools";
            // 
            // _tsmOptions
            // 
            _tsmOptions.Name = "_tsmOptions";
            _tsmOptions.Size = new Size(244, 44);
            _tsmOptions.Text = "Options...";
            _tsmOptions.Click += TsmOptions_Click;
            // 
            // helpToolStripMenuItem
            // 
            helpToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { _tsmAbout });
            helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            helpToolStripMenuItem.Size = new Size(172, 40);
            helpToolStripMenuItem.Text = ">> &Help <<";
            // 
            // _tsmAbout
            // 
            _tsmAbout.Name = "_tsmAbout";
            _tsmAbout.Size = new Size(224, 44);
            _tsmAbout.Text = "About...";
            _tsmAbout.Click += About_Click;
            // 
            // _statusStrip
            // 
            _statusStrip.Font = new Font("Segoe UI", 11F, FontStyle.Regular, GraphicsUnit.Point, 0);
            _statusStrip.ImageScalingSize = new Size(24, 24);
            _statusStrip.Items.AddRange(new ToolStripItem[] { _tslItemDateInfoCaption, _tslItemDateInfo, _tslKeywordsCaption, _tsddKeywords, _tslPersonalityCaption, _tslPersonality, _tslProcessTimesCaption, _tslProcessTimes, _tslStatusCaption, _tslInfo, _tsbInfo, _tslClockInfo });
            _statusStrip.Location = new Point(0, 930);
            _statusStrip.Name = "_statusStrip";
            _statusStrip.Padding = new Padding(1, 0, 17, 0);
            _statusStrip.Size = new Size(1731, 46);
            _statusStrip.TabIndex = 1;
            _statusStrip.Text = "statusStrip1";
            // 
            // _tslItemDateInfoCaption
            // 
            _tslItemDateInfoCaption.Font = new Font("Segoe UI", 11.1428576F, FontStyle.Bold);
            _tslItemDateInfoCaption.Name = "_tslItemDateInfoCaption";
            _tslItemDateInfoCaption.Size = new Size(143, 37);
            _tslItemDateInfoCaption.Text = "Date info:";
            _tslItemDateInfoCaption.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // _tslItemDateInfo
            // 
            _tslItemDateInfo.Name = "_tslItemDateInfo";
            _tslItemDateInfo.Size = new Size(45, 37);
            _tslItemDateInfo.Text = "---";
            // 
            // _tslKeywordsCaption
            // 
            _tslKeywordsCaption.Font = new Font("Segoe UI", 11.1428576F, FontStyle.Bold);
            _tslKeywordsCaption.Name = "_tslKeywordsCaption";
            _tslKeywordsCaption.Size = new Size(151, 37);
            _tslKeywordsCaption.Text = "Keywords:";
            // 
            // _tsddKeywords
            // 
            _tsddKeywords.DisplayStyle = ToolStripItemDisplayStyle.Text;
            _tsddKeywords.Image = (Image)resources.GetObject("_tsddKeywords.Image");
            _tsddKeywords.ImageTransparentColor = Color.Magenta;
            _tsddKeywords.Name = "_tsddKeywords";
            _tsddKeywords.Size = new Size(66, 42);
            _tsddKeywords.Text = "---";
            // 
            // _tslPersonalityCaption
            // 
            _tslPersonalityCaption.Font = new Font("Segoe UI", 11.1428576F, FontStyle.Bold, GraphicsUnit.Point, 0);
            _tslPersonalityCaption.Name = "_tslPersonalityCaption";
            _tslPersonalityCaption.Size = new Size(168, 37);
            _tslPersonalityCaption.Text = "Personality:";
            // 
            // _tslPersonality
            // 
            _tslPersonality.Name = "_tslPersonality";
            _tslPersonality.Size = new Size(59, 37);
            _tslPersonality.Text = "- - -";
            // 
            // _tslProcessTimesCaption
            // 
            _tslProcessTimesCaption.Font = new Font("Segoe UI", 11.1428576F, FontStyle.Bold, GraphicsUnit.Point, 0);
            _tslProcessTimesCaption.Name = "_tslProcessTimesCaption";
            _tslProcessTimesCaption.Size = new Size(263, 37);
            _tslProcessTimesCaption.Text = "Process times (ms):";
            // 
            // _tslProcessTimes
            // 
            _tslProcessTimes.Name = "_tslProcessTimes";
            _tslProcessTimes.Size = new Size(59, 37);
            _tslProcessTimes.Text = "- - -";
            // 
            // _tslStatusCaption
            // 
            _tslStatusCaption.Font = new Font("Segoe UI", 11.1428576F, FontStyle.Bold, GraphicsUnit.Point, 0);
            _tslStatusCaption.Name = "_tslStatusCaption";
            _tslStatusCaption.Size = new Size(110, 37);
            _tslStatusCaption.Text = "Status: ";
            // 
            // _tslInfo
            // 
            _tslInfo.Name = "_tslInfo";
            _tslInfo.Overflow = ToolStripItemOverflow.Never;
            _tslInfo.Size = new Size(504, 37);
            _tslInfo.Spring = true;
            _tslInfo.Text = "#info";
            _tslInfo.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // _tsbInfo
            // 
            _tsbInfo.DisplayStyle = ToolStripItemDisplayStyle.Text;
            _tsbInfo.Image = (Image)resources.GetObject("_tsbInfo.Image");
            _tsbInfo.ImageTransparentColor = Color.Magenta;
            _tsbInfo.Name = "_tsbInfo";
            _tsbInfo.Size = new Size(57, 42);
            _tsbInfo.Text = "...";
            _tsbInfo.ToolTipText = "...";
            // 
            // _tslClockInfo
            // 
            _tslClockInfo.Name = "_tslClockInfo";
            _tslClockInfo.Size = new Size(88, 37);
            _tslClockInfo.Text = "#clock";
            // 
            // _toolStrip
            // 
            _toolStrip.Font = new Font("Segoe UI", 11F, FontStyle.Regular, GraphicsUnit.Point, 0);
            _toolStrip.ImageScalingSize = new Size(32, 32);
            _toolStrip.Items.AddRange(new ToolStripItem[] { newToolStripButton, toolStripSeparator, copyToolStripButton, toolStripSeparator1, toolStripLabel1, _tscPersonalities, toolStripSeparator2, toolStripSeparator4, _tslModels, _tscModels });
            _toolStrip.Location = new Point(0, 44);
            _toolStrip.Name = "_toolStrip";
            _toolStrip.Padding = new Padding(10);
            _toolStrip.Size = new Size(1731, 65);
            _toolStrip.TabIndex = 2;
            _toolStrip.Text = "toolStrip1";
            // 
            // newToolStripButton
            // 
            newToolStripButton.DisplayStyle = ToolStripItemDisplayStyle.Image;
            newToolStripButton.Image = (Image)resources.GetObject("newToolStripButton.Image");
            newToolStripButton.ImageTransparentColor = Color.Magenta;
            newToolStripButton.Margin = new Padding(0, 4, 0, 5);
            newToolStripButton.Name = "newToolStripButton";
            newToolStripButton.Size = new Size(40, 36);
            newToolStripButton.Text = "&New";
            newToolStripButton.Click += BtnStartNewChat_Click;
            // 
            // toolStripSeparator
            // 
            toolStripSeparator.Name = "toolStripSeparator";
            toolStripSeparator.Size = new Size(6, 45);
            // 
            // copyToolStripButton
            // 
            copyToolStripButton.DisplayStyle = ToolStripItemDisplayStyle.Image;
            copyToolStripButton.Image = (Image)resources.GetObject("copyToolStripButton.Image");
            copyToolStripButton.ImageTransparentColor = Color.Magenta;
            copyToolStripButton.Margin = new Padding(4, 0, 5, 0);
            copyToolStripButton.Name = "copyToolStripButton";
            copyToolStripButton.Size = new Size(40, 45);
            copyToolStripButton.Text = "&Copy";
            // 
            // toolStripSeparator1
            // 
            toolStripSeparator1.Name = "toolStripSeparator1";
            toolStripSeparator1.Size = new Size(6, 45);
            // 
            // toolStripLabel1
            // 
            toolStripLabel1.Margin = new Padding(4, 0, 5, 0);
            toolStripLabel1.Name = "toolStripLabel1";
            toolStripLabel1.Size = new Size(146, 45);
            toolStripLabel1.Text = "Personality:";
            // 
            // _tscPersonalities
            // 
            _tscPersonalities.DropDownWidth = 300;
            _tscPersonalities.Font = new Font("Segoe UI", 11F, FontStyle.Regular, GraphicsUnit.Point, 0);
            _tscPersonalities.Margin = new Padding(4, 0, 5, 0);
            _tscPersonalities.Name = "_tscPersonalities";
            _tscPersonalities.Size = new Size(300, 45);
            _tscPersonalities.SelectedIndexChanged += TscPersonalities_SelectedIndexChanged;
            // 
            // toolStripSeparator2
            // 
            toolStripSeparator2.Name = "toolStripSeparator2";
            toolStripSeparator2.Size = new Size(6, 45);
            // 
            // toolStripSeparator4
            // 
            toolStripSeparator4.Name = "toolStripSeparator4";
            toolStripSeparator4.Size = new Size(6, 45);
            // 
            // _tslModels
            // 
            _tslModels.Name = "_tslModels";
            _tslModels.Size = new Size(112, 39);
            _tslModels.Text = "Models: ";
            // 
            // _tscModels
            // 
            _tscModels.Name = "_tscModels";
            _tscModels.Size = new Size(250, 45);
            // 
            // _splitMain
            // 
            _splitMain.Dock = DockStyle.Fill;
            _splitMain.Location = new Point(0, 109);
            _splitMain.Name = "_splitMain";
            // 
            // _splitMain.Panel1
            // 
            _splitMain.Panel1.Controls.Add(_tlpNavigatorLayout);
            _splitMain.Panel1.Padding = new Padding(10);
            // 
            // _splitMain.Panel2
            // 
            _splitMain.Panel2.Controls.Add(_tlpHeader);
            _splitMain.Panel2.Padding = new Padding(10);
            _splitMain.Size = new Size(1731, 821);
            _splitMain.SplitterDistance = 494;
            _splitMain.TabIndex = 3;
            // 
            // _tlpNavigatorLayout
            // 
            _tlpNavigatorLayout.ColumnCount = 1;
            _tlpNavigatorLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            _tlpNavigatorLayout.Controls.Add(_searchBoxDecorator, 0, 0);
            _tlpNavigatorLayout.Controls.Add(fluentDecoratorPanel1, 0, 2);
            _tlpNavigatorLayout.Controls.Add(fluentDecoratorPanel2, 0, 1);
            _tlpNavigatorLayout.Dock = DockStyle.Fill;
            _tlpNavigatorLayout.Location = new Point(10, 10);
            _tlpNavigatorLayout.Name = "_tlpNavigatorLayout";
            _tlpNavigatorLayout.RowCount = 3;
            _tlpNavigatorLayout.RowStyles.Add(new RowStyle());
            _tlpNavigatorLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 70F));
            _tlpNavigatorLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 30F));
            _tlpNavigatorLayout.Size = new Size(474, 801);
            _tlpNavigatorLayout.TabIndex = 1;
            // 
            // _searchBoxDecorator
            // 
            _searchBoxDecorator.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            _searchBoxDecorator.BorderThickness = 1;
            _searchBoxDecorator.Controls.Add(_cmbNavigatorSearch);
            _searchBoxDecorator.Location = new Point(5, 5);
            _searchBoxDecorator.Margin = new Padding(5);
            _searchBoxDecorator.Name = "_searchBoxDecorator";
            _searchBoxDecorator.Padding = new Padding(10);
            _searchBoxDecorator.Size = new Size(464, 52);
            _searchBoxDecorator.TabIndex = 2;
            // 
            // _cmbNavigatorSearch
            // 
            _cmbNavigatorSearch.FormattingEnabled = true;
            _cmbNavigatorSearch.Location = new Point(11, 4);
            _cmbNavigatorSearch.Name = "_cmbNavigatorSearch";
            _cmbNavigatorSearch.Size = new Size(442, 44);
            _cmbNavigatorSearch.TabIndex = 0;
            // 
            // fluentDecoratorPanel1
            // 
            fluentDecoratorPanel1.BorderThickness = 1;
            fluentDecoratorPanel1.Controls.Add(_txtSummary);
            fluentDecoratorPanel1.Dock = DockStyle.Fill;
            fluentDecoratorPanel1.Location = new Point(3, 582);
            fluentDecoratorPanel1.Name = "fluentDecoratorPanel1";
            fluentDecoratorPanel1.Padding = new Padding(10);
            fluentDecoratorPanel1.Size = new Size(468, 216);
            fluentDecoratorPanel1.TabIndex = 3;
            // 
            // _txtSummary
            // 
            _txtSummary.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            _txtSummary.BorderStyle = BorderStyle.None;
            _txtSummary.Location = new Point(11, 12);
            _txtSummary.Multiline = true;
            _txtSummary.Name = "_txtSummary";
            _txtSummary.Size = new Size(446, 192);
            _txtSummary.TabIndex = 2;
            // 
            // _tlpHeader
            // 
            _tlpHeader.ColumnCount = 2;
            _tlpHeader.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 60F));
            _tlpHeader.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40F));
            _tlpHeader.Controls.Add(_mainTabControl, 0, 1);
            _tlpHeader.Controls.Add(_lblConversationTitle, 0, 0);
            _tlpHeader.Controls.Add(_lblDate, 1, 0);
            _tlpHeader.Dock = DockStyle.Fill;
            _tlpHeader.Location = new Point(10, 10);
            _tlpHeader.Name = "_tlpHeader";
            _tlpHeader.RowCount = 2;
            _tlpHeader.RowStyles.Add(new RowStyle());
            _tlpHeader.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            _tlpHeader.Size = new Size(1213, 801);
            _tlpHeader.TabIndex = 0;
            // 
            // _mainTabControl
            // 
            _mainTabControl.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            _tlpHeader.SetColumnSpan(_mainTabControl, 2);
            _mainTabControl.Location = new Point(3, 54);
            _mainTabControl.Name = "_mainTabControl";
            _mainTabControl.Size = new Size(1207, 744);
            _mainTabControl.TabIndex = 0;
            // 
            // _lblConversationTitle
            // 
            _lblConversationTitle.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            _lblConversationTitle.AutoSize = true;
            _lblConversationTitle.Font = new Font("Segoe UI Semibold", 16F, FontStyle.Bold, GraphicsUnit.Point, 0);
            _lblConversationTitle.Location = new Point(3, 0);
            _lblConversationTitle.Name = "_lblConversationTitle";
            _lblConversationTitle.Size = new Size(721, 51);
            _lblConversationTitle.TabIndex = 1;
            _lblConversationTitle.Text = "Conversation Title";
            // 
            // _lblDate
            // 
            _lblDate.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right;
            _lblDate.AutoEllipsis = true;
            _lblDate.AutoSize = true;
            _lblDate.Font = new Font("Segoe UI", 11F, FontStyle.Bold, GraphicsUnit.Point, 0);
            _lblDate.Location = new Point(1066, 0);
            _lblDate.Name = "_lblDate";
            _lblDate.Size = new Size(144, 51);
            _lblDate.TabIndex = 2;
            _lblDate.Text = "#dateTime";
            _lblDate.TextAlign = ContentAlignment.MiddleRight;
            // 
            // _skCommunicator
            // 
            _skCommunicator.TopP = null;
            // 
            // _skMetaDataProcessor
            // 
            _skMetaDataProcessor.TopP = null;
            // 
            // fluentDecoratorPanel2
            // 
            fluentDecoratorPanel2.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            fluentDecoratorPanel2.BorderThickness = 1;
            fluentDecoratorPanel2.Controls.Add(_trvConversationHistory);
            fluentDecoratorPanel2.Location = new Point(3, 65);
            fluentDecoratorPanel2.Name = "fluentDecoratorPanel2";
            fluentDecoratorPanel2.Size = new Size(468, 511);
            fluentDecoratorPanel2.TabIndex = 4;
            // 
            // _trvConversationHistory
            // 
            _trvConversationHistory.BorderStyle = BorderStyle.None;
            _trvConversationHistory.FullRowSelect = true;
            _trvConversationHistory.HideSelection = false;
            _trvConversationHistory.Location = new Point(1, 8);
            _trvConversationHistory.Name = "_trvConversationHistory";
            _trvConversationHistory.ShowNodeToolTips = true;
            _trvConversationHistory.Size = new Size(466, 494);
            _trvConversationHistory.TabIndex = 1;
            // 
            // FrmMain
            // 
            AutoScaleDimensions = new SizeF(14F, 36F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1731, 976);
            Controls.Add(_splitMain);
            Controls.Add(_toolStrip);
            Controls.Add(_statusStrip);
            Controls.Add(_menuStrip);
            Font = new Font("Segoe UI", 11F, FontStyle.Regular, GraphicsUnit.Point, 0);
            MainMenuStrip = _menuStrip;
            Margin = new Padding(4);
            Name = "FrmMain";
            Text = "C.H.A.T.T.Y. - A tip-toe in WinForms with leaping results!";
            _menuStrip.ResumeLayout(false);
            _menuStrip.PerformLayout();
            _statusStrip.ResumeLayout(false);
            _statusStrip.PerformLayout();
            _toolStrip.ResumeLayout(false);
            _toolStrip.PerformLayout();
            _splitMain.Panel1.ResumeLayout(false);
            _splitMain.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)_splitMain).EndInit();
            _splitMain.ResumeLayout(false);
            _tlpNavigatorLayout.ResumeLayout(false);
            _searchBoxDecorator.ResumeLayout(false);
            fluentDecoratorPanel1.ResumeLayout(false);
            fluentDecoratorPanel1.PerformLayout();
            _tlpHeader.ResumeLayout(false);
            _tlpHeader.PerformLayout();
            fluentDecoratorPanel2.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private MenuStrip _menuStrip;
        private ToolStripMenuItem _tsmFile;
        private ToolStripMenuItem _tsmQuit;
        private StatusStrip _statusStrip;
        private ToolStrip _toolStrip;
        private ToolStripButton newToolStripButton;
        private ToolStripSeparator toolStripSeparator;
        private ToolStripButton copyToolStripButton;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripStatusLabel _tslItemDateInfoCaption;
        private SplitContainer _splitMain;
        private ToolStripMenuItem helpToolStripMenuItem;
        private ToolStripMenuItem _tsmAbout;
        private ToolStripComboBox _tscPersonalities;
        private ToolStripLabel toolStripLabel1;
        private ToolStripSeparator toolStripSeparator2;
        private ToolStripMenuItem toolStripMenuItem1;
        private ToolStripSeparator toolStripSeparator3;
        private ToolStripMenuItem _tsmTools;
        private ToolStripMenuItem _tsmOptions;
        private SemanticKernelComponent _skMetaDataProcessor;
        private ToolStripMenuItem _tsiEdit;
        private ToolStripMenuItem _tsiPersonalities;
        private ToolStripMenuItem _tsmAgents;
        private TableLayoutPanel _tlpHeader;
        private CommunityToolkit.WinForms.FluentUI.FluentTabControl _mainTabControl;
        private Label _lblConversationTitle;
        private ToolStripStatusLabel _tslClockInfo;
        private Label _lblDate;
        private ToolStripSeparator toolStripSeparator4;
        private ToolStripLabel _tslModels;
        private ToolStripComboBox _tscModels;
        private ToolStripMenuItem modelOverviewToolStripMenuItem;
        private ToolStripMenuItem _tsmPromptBuilder;
        private ToolStripSeparator toolStripMenuItem3;
        private ToolStripMenuItem _tsmLanguageConverter;
        private ToolStripMenuItem _classDocumenter;
        private ToolStripMenuItem _tsmPersonalityBuilder;
        private ToolStripMenuItem _vsProjectRampUpper;
        private ToolStripMenuItem chatMetadataToolStripMenuItem;
        private ToolStripSeparator toolStripMenuItem2;
        private ToolStripMenuItem formLocalizerToolStripMenuItem;
        private ToolStripStatusLabel _tslItemDateInfo;
        private ToolStripMenuItem toolStripMenuItem4;
        private ToolStripMenuItem winFormsBestPracticesQuizzerToolStripMenuItem;
        private ToolStripStatusLabel _tslKeywordsCaption;
        private ToolStripDropDownButton _tsddKeywords;
        private ToolStripSplitButton _tsbInfo;
        private ToolStripStatusLabel _tslInfo;
        private ToolStripMenuItem toolStripMenuItem5;
        private ToolStripMenuItem filterKeywordsToolStripMenuItem;
        private ToolStripMenuItem filterChatsWithFilesToolStripMenuItem;
        private ToolStripMenuItem chatSummaryToolStripMenuItem;
        public SemanticKernelComponent _skCommunicator;
        private ToolStripStatusLabel _tslPersonalityCaption;
        private ToolStripStatusLabel _tslPersonality;
        private ToolStripStatusLabel _tslStatusCaption;
        private ToolStripMenuItem toolStripMenuItem6;
        private ToolStripMenuItem classicApproachToolStripMenuItem;
        private ToolStripMenuItem soThatWithAIToolStripMenuItem;
        private ToolStripMenuItem fineButWhatAboutToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator5;
        private ToolStripStatusLabel _tslProcessTimesCaption;
        private ToolStripStatusLabel _tslProcessTimes;
        private TableLayoutPanel _tlpNavigatorLayout;
        private CommunityToolkit.WinForms.FluentUI.FluentDecoratorPanel _searchBoxDecorator;
        private ComboBox _cmbNavigatorSearch;
        private CommunityToolkit.WinForms.FluentUI.FluentDecoratorPanel fluentDecoratorPanel1;
        private TextBox _txtSummary;
        private CommunityToolkit.WinForms.FluentUI.FluentDecoratorPanel fluentDecoratorPanel2;
        private TreeView _trvConversationHistory;
    }
}
