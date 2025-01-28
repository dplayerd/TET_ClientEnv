using BI.Shared.Utils;
using Platform.AbstractionClass;
using Platform.Auth.Models;
using Platform.Messages;
using Platform.ORM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BI.SPA_ApproverSetup.Utils
{
    internal class MailUtil
    {
        /// <summary> 寄信 </summary>
        /// <param name="mailList"> 收件人 </param>
        /// <param name="period"> 評鑑期間 </param>
        /// <param name="userID">目前登入者</param>
        /// <param name="cDate">目前時間</param>
        /// 
        internal static void Send_SRISS_Mail(List<string> mailList, List<string> ccList, string period, string userID, DateTime cDate)
        {
            var datePeriod = PeriodUtil.ParsePeriod(period);
            var pageUrl = $"{ModuleConfig.EmailRootUrl}/SPA_CostService/Index";

            EMailContent content = new EMailContent()
            {
                Title = $"[Cost & Service資料填寫] {period}",
                Body =
                $@"
                您好，<br/>
                <br/>
                評鑑範圍：{period} ({datePeriod.StartDate?.ToString("yyyy-MM-dd")} ~ {datePeriod.EndDate?.ToString("yyyy-MM-dd")})期間供應商所提供之服務表現。<br/>
                <br/>
                填寫資料請點擊「<a href=""{pageUrl}"" target=""_blank"">Cost & Service資料維護</a>」連結，謝謝。
                "
            };

            MailPoolManager.WriteMailWithCC(mailList, ccList, content, userID, cDate);
        }


        /// <summary> 寄信 </summary>
        /// <param name="mailList"> 收件人 </param>
        /// <param name="period"> 評鑑期間 </param>
        /// <param name="userID">目前登入者</param>
        /// <param name="cDate">目前時間</param>
        /// 
        internal static void Send_SafetyAndEhs_Mail(List<string> mailList, List<string> ccList, string period, string userID, DateTime cDate)
        {
            var datePeriod = PeriodUtil.ParsePeriod(period);
            var pageUrl = $"{ModuleConfig.EmailRootUrl}/SPA_Violation/Index";

            EMailContent content = new EMailContent()
            {
                Title = $"[供應商SPA違規紀錄資料填寫] {period}",
                Body =
                $@"
                您好，<br/>
                <br/>
                評鑑範圍：{period} ({datePeriod.StartDate?.ToString("yyyy-MM-dd")} ~ {datePeriod.EndDate?.ToString("yyyy-MM-dd")})期間供應商的違規紀錄。<br/>
                <br/>
                填寫資料請點擊「<a href=""{pageUrl}"" target=""_blank"">供應商SPA違規紀錄資料填寫</a>」連結，謝謝。
                "
            };

            MailPoolManager.WriteMailWithCC(mailList, ccList, content, userID, cDate);
        }
    }
}
