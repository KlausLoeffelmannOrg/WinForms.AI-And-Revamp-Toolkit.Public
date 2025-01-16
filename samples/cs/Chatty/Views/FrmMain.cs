namespace Chatty
{
    public partial class FrmMain : Form
    {
        public FrmMain()
        {
            InitializeComponent();
        }

        private void About_Click(object sender, EventArgs e)
        {
            using FrmAbout about = new();
            about.ShowDialog();
        }
    }
}
