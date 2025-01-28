using Platform.Infra;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace Platform.WebSite.Util
{
    public class ConfigLoader
    {
        public static void Init()
        {
            BI.Suppliers.ModuleConfig.AllowSizeMB = ReadIntConfig("SupplierFileSize");
            BI.Suppliers.ModuleConfig.AllowTotalSizeMB = ReadIntConfig("SupplierTotalFileSize");

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

            BI.SPA_Evaluation.ModuleConfig.EmailRootUrl = EmailRootUrl;
            BI.SPA_Violation.ModuleConfig.EmailRootUrl = EmailRootUrl;
            BI.SPA_ScoringInfo.ModuleConfig.EmailRootUrl = EmailRootUrl;
            BI.SPA_CostService.ModuleConfig.EmailRootUrl = EmailRootUrl;
            BI.SPA_ApproverSetup.ModuleConfig.EmailRootUrl = EmailRootUrl;
            BI.Suppliers.ModuleConfig.EmailRootUrl = EmailRootUrl;
            BI.STQA.ModuleConfig.EmailRootUrl = EmailRootUrl;
            BI.SPA.ModuleConfig.EmailRootUrl = EmailRootUrl;
            BI.PaymentSuppliers.ModuleConfig.EmailRootUrl = EmailRootUrl;
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