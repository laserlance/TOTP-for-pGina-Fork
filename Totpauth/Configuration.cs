using System;
using System.Windows.Forms;

namespace pGina.Plugin.Totpauth
{
    public partial class Configuration : Form
    {
        private readonly Gauth _gauth = new Gauth();

        public Configuration()
        {
            InitializeComponent();
        }

        private void Configuration_Load(object sender, EventArgs e)
        {

            pictureBox1.Load(_gauth.QrCodeUrl);

            this.WindowState = FormWindowState.Minimized;

            this.Show();

            this.WindowState = FormWindowState.Normal;
        }
    }
}
