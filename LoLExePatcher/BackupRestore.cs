using System.IO;
using System.Windows.Forms;

namespace LoLExePatcher
{
    class BackupRestore
    {
        readonly string desPath = Application.StartupPath + "\\backup";
        string installPath;
        string exePath;

        public BackupRestore(string _installPath)
        {
            installPath = _installPath;
            exePath = installPath + "\\Game\\League of Legends.exe";

            if (!Directory.Exists(desPath))
            {
                Directory.CreateDirectory(desPath);
            }
        }

        public void Backup()
        {
            try
            {
                if (File.Exists(exePath))
                {
                    if (File.Exists(desPath + "\\League of Legends.exe"))
                    {
                        if (DialogResult.Yes == MessageBox.Show("已經存在已備份文件，要覆蓋已備份文件嗎?", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation))
                        {
                            File.Copy(exePath, desPath + "\\League of Legends.exe", true);
                            MessageBox.Show("備份完成!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return;
                        }
                        else return;
                    }
                    else
                    {
                        File.Copy(exePath, desPath + "\\League of Legends.exe", true);
                        MessageBox.Show("備份完成!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                }
                else
                {
                    MessageBox.Show("\"League of Legends.exe\" 檔案遺失! 備份失敗!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            catch
            {
                MessageBox.Show("發生錯誤，備份失敗!", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        public void Restore()
        {
            try
            {
                if (File.Exists(desPath + "\\League of Legends.exe"))
                {
                    File.Copy(desPath + "\\League of Legends.exe", exePath, true);
                    MessageBox.Show("還原完成!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("沒有備份!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
            }
            catch
            {
                MessageBox.Show("發生錯誤，還原失敗!", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }
    }
}

