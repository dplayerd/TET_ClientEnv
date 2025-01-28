using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Platform.Infra
{
    public class EmailConfig
    {
        private static EmailConfig _config;

        /// <summary> 是否真的要發送 (如果是否，一建立就視同結束) </summary>
        public bool WillSendMail { get; set; }

        /// <summary> Email 根 Url </summary>
        public string EmailRootUrl { get; set; }

        /// <summary> SMTP 主機 </summary>
        public string SmtpHost { get; set; }

        /// <summary> SMTP 通訊埠 </summary>
        public int SmtpPort { get; set; }

        /// <summary> 寄件者名稱 </summary>
        public string SenderName { get; set; }

        /// <summary> 寄件者 Email </summary>
        public string SmtpAccount { get; set; }

        /// <summary> 寄件者 Email 密碼 </summary>
        public string SmtpPassword { get; set; }

        /// <summary> 信件保存日數 </summary>
        public int ExpireDays { get; set; } = 180;

        public static void RegisterDefault(EmailConfig config)
        {
            _config = config;
        }

        public static EmailConfig GetDefault()
        {
            if (_config == null)
                return default;

            return _config;
        }
    }
}
