namespace LoLExePatcher
{
    class GetReg
    {
        /// <summary>
        /// 從登錄檔取得LoL路徑
        /// </summary>

        public static string installPath = "";  //LoL安裝路徑

        public static string TwPath()   //取得台服LoL路徑
        {

            if (My.Computer.Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Garena\LoLTW", "Path", null) != null)
            {
                string value = My.Computer.Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Garena\LoLTW", "Path", null).ToString();
                if (value.Contains("LoLTW"))
                {
                    installPath = My.Computer.Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Garena\LoLTW", "Path", null).ToString();
                    return installPath;
                }
            }

            if (My.Computer.Registry.GetValue(@"HKEY_CURRENT_USER\SOFTWARE\Garena\LoLTW", "Path", null) != null)
            {
                string value = My.Computer.Registry.GetValue(@"HKEY_CURRENT_USER\SOFTWARE\Garena\LoLTW", "Path", null).ToString();
                if (value.Contains("LoLTW"))
                {
                    installPath = My.Computer.Registry.GetValue(@"HKEY_CURRENT_USER\SOFTWARE\Garena\LoLTW", "Path", null).ToString();
                    return installPath;
                }
            }
            return "";
        }
    }
}
