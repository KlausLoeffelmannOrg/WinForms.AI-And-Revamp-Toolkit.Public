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
        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmManagePersonalities));
        _mainToolStrip = new ToolStrip();
        _tsbNewPersonality = new ToolStripButton();
        _tsbEditPersonality = new ToolStripButton();
        _tsbDeletePersonality = new ToolStripButton();
        toolStripSeparator1 = new ToolStripSeparator();
        _tsbCompactPrompt = new ToolStripButton();
        _dgvPersonalities = new DataGridView();
        dataGridViewTextBoxColumn1 = new DataGridViewTextBoxColumn();
        dataGridViewTextBoxColumn2 = new DataGridViewTextBoxColumn();
        _mainToolStrip.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)_dgvPersonalities).BeginInit();
        SuspendLayout();
        // 
        // _mainToolStrip
        // 
        _mainToolStrip.ImageScalingSize = new Size(48, 48);
        _mainToolStrip.Items.AddRange(new ToolStripItem[] { _tsbNewPersonality, _tsbEditPersonality, _tsbDeletePersonality, toolStripSeparator1, _tsbCompactPrompt });
        _mainToolStrip.Location = new Point(0, 0);
        _mainToolStrip.Name = "_mainToolStrip";
        _mainToolStrip.Padding = new Padding(5);
        _mainToolStrip.Size = new Size(1483, 102);
        _mainToolStrip.TabIndex = 1;
        _mainToolStrip.Text = "_toolStripMain";
        // 
        // _tsbNewPersonality
        // 
        _tsbNewPersonality.Image = (Image)resources.GetObject("_tsbNewPersonality.Image");
        _tsbNewPersonality.ImageTransparentColor = Color.Magenta;
        _tsbNewPersonality.Name = "_tsbNewPersonality";
        _tsbNewPersonality.Padding = new Padding(5);
        _tsbNewPersonality.Size = new Size(62, 87);
        _tsbNewPersonality.Text = "New";
        _tsbNewPersonality.TextImageRelation = TextImageRelation.ImageAboveText;
        // 
        // _tsbEditPersonality
        // 
        _tsbEditPersonality.Image = (Image)resources.GetObject("_tsbEditPersonality.Image");
        _tsbEditPersonality.ImageTransparentColor = Color.Magenta;
        _tsbEditPersonality.Name = "_tsbEditPersonality";
        _tsbEditPersonality.Padding = new Padding(5);
        _tsbEditPersonality.Size = new Size(62, 87);
        _tsbEditPersonality.Text = "Edit";
        _tsbEditPersonality.TextImageRelation = TextImageRelation.ImageAboveText;
        // 
        // _tsbDeletePersonality
        // 
        _tsbDeletePersonality.Image = (Image)resources.GetObject("_tsbDeletePersonality.Image");
        _tsbDeletePersonality.ImageTransparentColor = Color.Magenta;
        _tsbDeletePersonality.Name = "_tsbDeletePersonality";
        _tsbDeletePersonality.Padding = new Padding(5);
        _tsbDeletePersonality.Size = new Size(76, 87);
        _tsbDeletePersonality.Text = "Delete";
        _tsbDeletePersonality.TextImageRelation = TextImageRelation.ImageAboveText;
        // 
        // toolStripSeparator1
        // 
        toolStripSeparator1.Name = "toolStripSeparator1";
        toolStripSeparator1.Size = new Size(6, 92);
        // 
        // _tsbCompactPrompt
        // 
        _tsbCompactPrompt.Image = (Image)resources.GetObject("_tsbCompactPrompt.Image");
        _tsbCompactPrompt.ImageTransparentColor = Color.Magenta;
        _tsbCompactPrompt.Name = "_tsbCompactPrompt";
        _tsbCompactPrompt.Padding = new Padding(5);
        _tsbCompactPrompt.Size = new Size(98, 87);
        _tsbCompactPrompt.Text = "Compact";
        _tsbCompactPrompt.TextImageRelation = TextImageRelation.ImageAboveText;
        // 
        // _dgvPersonalities
        // 
        _dgvPersonalities.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        _dgvPersonalities.Columns.AddRange(new DataGridViewColumn[] { dataGridViewTextBoxColumn1, dataGridViewTextBoxColumn2 });
        _dgvPersonalities.Dock = DockStyle.Fill;
        _dgvPersonalities.Location = new Point(0, 102);
        _dgvPersonalities.Margin = new Padding(4);
        _dgvPersonalities.Name = "_dgvPersonalities";
        _dgvPersonalities.ReadOnly = true;
        _dgvPersonalities.RowHeadersWidth = 82;
        _dgvPersonalities.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        _dgvPersonalities.Size = new Size(1483, 817);
        _dgvPersonalities.TabIndex = 2;
        // 
        // dataGridViewTextBoxColumn1
        // 
        dataGridViewTextBoxColumn1.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        dataGridViewTextBoxColumn1.FillWeight = 30F;
        dataGridViewTextBoxColumn1.HeaderText = "Name";
        dataGridViewTextBoxColumn1.MinimumWidth = 8;
        dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
        dataGridViewTextBoxColumn1.ReadOnly = true;
        // 
        // dataGridViewTextBoxColumn2
        // 
        dataGridViewTextBoxColumn2.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        dataGridViewTextBoxColumn2.FillWeight = 70F;
        dataGridViewTextBoxColumn2.HeaderText = "System Prompt";
        dataGridViewTextBoxColumn2.MinimumWidth = 8;
        dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
        dataGridViewTextBoxColumn2.ReadOnly = true;
        // 
        // FrmManagePersonalities
        // 
        AutoScaleDimensions = new SizeF(12F, 30F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(1483, 919);
        Controls.Add(_dgvPersonalities);
        Controls.Add(_mainToolStrip);
        Font = new Font("Segoe UI", 10.875F, FontStyle.Regular, GraphicsUnit.Point, 0);
        Margin = new Padding(4);
        Name = "FrmManagePersonalities";
        Text = "Manage Personalities";
        _mainToolStrip.ResumeLayout(false);
        _mainToolStrip.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)_dgvPersonalities).EndInit();
        ResumeLayout(false);
        PerformLayout();

        _tsbNewPersonality.Click += TsbNewPersonality_Click;
        _tsbEditPersonality.Click += TsbEditPersonality_Click;
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
    private ToolStrip _mainToolStrip;
    private ToolStripButton _tsbNewPersonality;
    private ToolStripButton _tsbEditPersonality;
    private ToolStripButton _tsbDeletePersonality;
    private ToolStripSeparator toolStripSeparator1;
    private ToolStripButton _tsbCompactPrompt;
    private DataGridView _dgvPersonalities;
    private DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
    private DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
}
