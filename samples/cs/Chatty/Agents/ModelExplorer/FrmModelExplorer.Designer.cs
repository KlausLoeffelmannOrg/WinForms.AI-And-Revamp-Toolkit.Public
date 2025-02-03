namespace Chatty.Agents.ModelExplorer
{
    partial class FrmModelExplorer
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
            chatView1 = new Chatty.Views.ChatView();
            SuspendLayout();
            // 
            // chatView1
            // 
            chatView1.Location = new Point(12, 12);
            chatView1.Name = "chatView1";
            chatView1.Size = new Size(991, 595);
            chatView1.TabIndex = 0;
            // 
            // FrmModelExplorer
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1015, 619);
            Controls.Add(chatView1);
            Name = "FrmModelExplorer";
            Text = "Model Explorer Agent";
            ResumeLayout(false);
        }

        #endregion

        private Views.ChatView chatView1;
    }
}