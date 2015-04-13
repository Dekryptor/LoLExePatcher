using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace LoLExePatcher
{
    public partial class frmAd : Form
    {
        bool adSkiped = false;
        bool tipPopupShown = false;

        public frmAd()
        {
            InitializeComponent();
        }

        private void frmAd_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!adSkiped)
                e.Cancel = true;
            else
                this.Dispose();
        }

        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            if (!tipPopupShown)
            {
                if (e.Url.Host.Replace("https://", "").Replace("http://", "").Replace("/", "").ToLower() == "adf.ly")
                {
                    MessageBox.Show("稍等5秒後，請按右上角黃色按鈕繼續下載");
                    tipPopupShown = true;
                }
            }

            if (e.Url == new Uri("http://blog.nitroxenon.com/lolexepatcher-redirect.html"))
            {
                if (webBrowser1.DocumentText.ToString().Trim() == "Redirecting...")
                {
                    System.Threading.Thread.Sleep(1500);
                    adSkiped = true;
                    this.Dispose();
                }
                else
                {
                    MessageBox.Show("出現未知錯誤，按確定退出程式");
                    Environment.Exit(0);
                }
            }
        }
    }
}
