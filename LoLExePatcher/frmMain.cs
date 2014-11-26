using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Newtonsoft.Json;
using System.Net;
using System.IO;

namespace LoLExePatcher
{
    public partial class frmMain : Form
    {
        readonly string DownloadPath = Application.StartupPath + "\\files";
        bool downloading = false;
        RootObject versionObj;
        Dictionary<string, string> VersionDict = new Dictionary<string, string>();
        WebClient downloadExe = new WebClient();

        public frmMain()
        {
            InitializeComponent();

            downloadExe.DownloadFileCompleted += new AsyncCompletedEventHandler(downloadExe_DownloadFileCompleted);
            downloadExe.DownloadProgressChanged += new DownloadProgressChangedEventHandler(downloadExe_DownloadProgressChanged);

            /************* Test **************
            WebClient wc = new WebClient();
            RootObject obj = JsonConvert.DeserializeObject<RootObject>(wc.DownloadString("http://nitroxenon.com/LoLExePatcher/versions.json"));
            MessageBox.Show(obj.versions[0].link);
            *********************************/
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            if (!Directory.Exists(DownloadPath))
            {
                Directory.CreateDirectory(DownloadPath);
            }

            //下載版本文件
            versionObj = DownloadVersionList();

            if (versionObj != null)
            {
                foreach (var o in versionObj.versions)
                {
                    if (o.newest)
                    {
                        boxVersion.Items.Add(o.version + " (最新版)");
                    }
                    else
                    {
                        boxVersion.Items.Add(o.version);
                    }

                    VersionDict.Add(o.version, o.link);
                }
            }
        }

        private RootObject DownloadVersionList()
        {
            WebClient wc = new WebClient();
            try
            {
                return JsonConvert.DeserializeObject<RootObject>(wc.DownloadString("http://nitroxenon.com/LoLExePatcher/versions.json"));
            }
            catch { return null; }
        }

        private void btnDownload_Click(object sender, EventArgs e)
        {
            foreach (var o in VersionDict)
            {
                if (o.Key == boxVersion.SelectedItem.ToString().Replace(" (最新版)",""))
                {
#if DEBUG
                    MessageBox.Show("OK!");
#else
#endif
                    string verDir = DownloadPath + "\\" + o.Key;
                    Directory.CreateDirectory(verDir);
                    if (!downloading)
                      downloadExe.DownloadFileAsync(new Uri(o.Value), verDir + "\\League of Legends.exe");
                }
            }
        }

        private void downloadExe_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            downloading = true;
            double bytesIn = double.Parse(e.BytesReceived.ToString());
            double totalBytes = double.Parse(e.TotalBytesToReceive.ToString());
            double percentage = bytesIn / totalBytes * 100;

            int value = int.Parse(Math.Truncate(percentage).ToString());
            progressBar1.Value = int.Parse(Math.Truncate(percentage).ToString());
            lblPercent.Text = value.ToString() + " %";
        }

        private void downloadExe_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            if (!e.Cancelled || e.Error != null)
            {
                MessageBox.Show("下載完成! 可按 \"安裝\" 按鈕進行替換");
            }
            downloading = false;
        }

        private void btnInstall_Click(object sender, EventArgs e)
        {
            if (!downloading)
            {
                string version = boxVersion.SelectedItem.ToString().Replace(" (最新版)", "");
                string filePath = DownloadPath + "\\" + version + "\\League of Legends.exe";
                if (File.Exists(filePath))
                {
                    File.Copy(filePath, "C:\\League of Legends.exe", true);
                }
            }
        }
    }
}
