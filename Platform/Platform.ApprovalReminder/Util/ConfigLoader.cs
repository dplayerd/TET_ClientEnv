using Platform.Infra;
using System.Configuration;

namespace Platform.ApprovalReminder.Util
{
    /// <summary>Console 執行前所需的系統設定載入器。</summary>
    public class ConfigLoader
    {
        #region 公開方法
        /// <summary>初始化信件設定，供提醒信 Manager 寫入待發信清單時使用。</summary>
        public static void Init()
        {
            // Reminder Console 不直接寄信，但寫入待發信清單時仍需寄件者與系統網址設定。
            var config = new EmailConfig()
            {
                WillSendMail = (ReadStringConfig("WillSendMail") == "Y"),
                EmailRootUrl = ReadStringConfig("EmailRootUrl"),
                SmtpHost = ReadStringConfig("SmtpHost"),
                SmtpPort = ReadIntConfig("SmtpPort"),
                SenderName = ReadStringConfig("SenderName"),
                SmtpAccount = ReadStringConfig("SmtpAccount"),
                SmtpPassword = ReadStringConfig("SmtpPassword"),
                ExpireDays = ReadIntConfig("EmailExpireDays"),
            };

            EmailConfig.RegisterDefault(config);
        }
        #endregion

        #region 私有方法
        /// <summary>讀取整數設定；格式不正確時回傳 -1。</summary>
        /// <param name="configName">設定鍵。</param>
        /// <returns>設定值。</returns>
        private static int ReadIntConfig(string configName)
        {
            var config = ConfigurationManager.AppSettings[configName];
            if (int.TryParse(config, out int temp))
                return temp;

            return -1;
        }

        /// <summary>讀取字串設定；未設定時回傳空字串。</summary>
        /// <param name="configName">設定鍵。</param>
        /// <returns>設定值。</returns>
        private static string ReadStringConfig(string configName)
        {
            return ConfigurationManager.AppSettings[configName] ?? string.Empty;
        }
        #endregion
    }
}
