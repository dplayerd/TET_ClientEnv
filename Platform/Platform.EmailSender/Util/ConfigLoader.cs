using Platform.Infra;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Platform.EmailSenderConsole.Util
{
    public class ConfigLoader
    {
        public static void Init()
        {
            // --- Email Configs ---
            string WillSendMail = ReadStringConfig("WillSendMail");
            string EmailRootUrl = ReadStringConfig("EmailRootUrl");
            string SmtpHost = ReadStringConfig("SmtpHost");
            int SmtpPort = ReadIntConfig("SmtpPort");
            string SmtpAccount = ReadStringConfig("SmtpAccount");
            string SmtpPassword = ReadStringConfig("SmtpPassword");
            string SenderName = ReadStringConfig("SenderName");
            int ExpireDays = ReadIntConfig("EmailExpireDays");

            EmailConfig config = new EmailConfig()
            {
                WillSendMail = (WillSendMail == "Y"),
                EmailRootUrl = EmailRootUrl,
                SmtpHost = SmtpHost,
                SmtpPort = SmtpPort,
                SenderName = SenderName,
                SmtpAccount = SmtpAccount,
                SmtpPassword = SmtpPassword,
                ExpireDays = ExpireDays,
            };

            EmailConfig.RegisterDefault(config);
            // --- Email Configs ---
        }

        private static int ReadIntConfig(string configName)
        {
            var config = ConfigurationManager.AppSettings[configName];
            if (int.TryParse(config, out int temp))
                return temp;
            else
                return -1;
        }

        private static string ReadStringConfig(string configName)
        {
            var config = ConfigurationManager.AppSettings[configName];

            if (config == null)
                return string.Empty;
            else
                return config;
        }
    }
}
