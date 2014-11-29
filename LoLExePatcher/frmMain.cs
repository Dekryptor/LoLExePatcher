using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace LoLExePatcher
{
    public partial class frmMain : Form
    {
        readonly string DownloadPath = Application.StartupPath + "\\files";
        bool downloading = false;
        frmWait wait;
        RootObject versionObj;
        Dictionary<string, string> VersionDict = new Dictionary<string, string>();
        WebClient downloadExe = new WebClient();

        public frmMain()
        {
            InitializeComponent();

            downloadExe.DownloadFileCompleted += new AsyncCompletedEventHandler(downloadExe_DownloadFileCompleted);
            downloadExe.DownloadProgressChanged += new DownloadProgressChangedEventHandler(downloadExe_DownloadProgressChanged);
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            if (!File.Exists(Application.StartupPath + "\\Newtonsoft.Json.dll"))
            {
                MessageBox.Show("程式類別庫檔案遺失，請重新下載...", "檔案遺失", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Environment.Exit(0);
            }
            wait = new frmWait();
            wait.Show();
            wait.Refresh();
            wait.Update();
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
            else
            {
                MessageBox.Show("下載版本資訊失敗，按\"確定\"關閉 LoLExePatcher", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Environment.Exit(0);
            }

            if (wait != null)
            {
                wait.Dispose();
                this.Refresh();
            }

            textBox1.Text = GetReg.TwPath();
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
            if (!downloading)
            {
                foreach (var o in VersionDict)
                {
                    if (o.Key == boxVersion.SelectedItem.ToString().Replace(" (最新版)", ""))
                    {
                        string verDir = DownloadPath + "\\" + o.Key;
                        Directory.CreateDirectory(verDir);
                        downloadExe.DownloadFileAsync(new Uri(o.Value), verDir + "\\League of Legends.exe");
                        return;
                    }
                }
            }
            else
            {
                MessageBox.Show("下載正在進行中...");
                return;
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
            if (!e.Cancelled || (e.Error == null))
            {
                MessageBox.Show("下載完成! 可按 \"安裝\" 按鈕進行替換");
            }
            else
            {
                MessageBox.Show("下載失敗!", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            downloading = false;
        }

        private void btnInstall_Click(object sender, EventArgs e)
        {
            if (!downloading)
            {
                if (PathCheck())
                {
                    string version = boxVersion.SelectedItem.ToString().Replace(" (最新版)", "");
                    string filePath = DownloadPath + "\\" + version + "\\League of Legends.exe";
                    if (File.Exists(filePath))
                    {
                        try
                        {
                            File.Copy(filePath, textBox1.Text + "\\Game\\League of Legends.exe", true);
                            MessageBox.Show("文件替換完成!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        catch 
                        {
                            MessageBox.Show("文件替換失敗...","錯誤",MessageBoxButtons.OK,MessageBoxIcon.Error);
                            return;
                        }
                    }
                    else
                    {
                        MessageBox.Show("找不到下載文件，請先下載...", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                }
                else
                {
                    MessageBox.Show("目錄設定失敗，請檢查...", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            else
            {
                    MessageBox.Show("下載正在進行中...");
                    return;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.Description = "請選擇 LoLTW 目錄";
            if ((DialogResult.Cancel) != dialog.ShowDialog())
            {
                textBox1.Text = dialog.SelectedPath;
            }
        }
        private bool PathCheck()
        {
            if (File.Exists(textBox1.Text + "\\Game\\League of Legends.exe"))
                return true;
            else
                return false;
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://nitroxenon.com");
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://github.com/NitroXenon/LoLExePatcher");
        }

        private void btnBak_Click(object sender, EventArgs e)
        {
            BackupRestore bak = new BackupRestore(textBox1.Text);
            bak.Backup();
        }

        private void btnRes_Click(object sender, EventArgs e)
        {
            BackupRestore res = new BackupRestore(textBox1.Text);
            res.Restore();
        }
    }
}
