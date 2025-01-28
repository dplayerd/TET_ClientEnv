using BI.SPA_CostService.Enums;
using Platform.AbstractionClass;
using Platform.Auth;
using Platform.Auth.Models;
using Platform.Infra;
using Platform.Messages;
using Platform.ORM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BI.SPA_CostService.Utils
{
    /// <summary> 簽核相關信件發送器 </summary>
    internal class ApprovalMailUtil
    {
        /// <summary> 寄信給簽核過的人 </summary>
        /// <param name="mailList"></param>
        /// <param name="titleText"></param>
        /// <param name="reason"></param>
        /// <param name="userID">目前登入者</param>
        /// <param name="cDate">目前時間</param>
        internal static void SendAbordMail(List<string> mailList, string titleText, string reason, string userID, DateTime cDate)
        {
            EMailContent content = new EMailContent()
            {
                Title = $"[審核中止通知] {titleText}",
                Body =
                $@"
                您好，<br/>
                <br/>
                此流程已由申請人中止，謝謝<br/>
                <br/>
                中止原因<br/>
                {reason.ReplaceNewLine(true)}
                "
            };

            MailPoolManager.WritePool(mailList, content, userID, cDate);
        }


        /// <summary> 寄信新的簽核人 </summary>
        /// <param name="applicant"> 申請人 </param>
        /// <param name="receiver"> 簽核人 </param>
        /// <param name="approvalModel"> 簽核資訊 </param>
        /// <param name="main"></param>
        /// <param name="userID">目前登入者</param>
        /// <param name="cDate">目前時間</param> 
        internal static void SendNewVerifyMail(UserModel applicant, UserAccountModel receiver, TET_SPA_CostServiceApproval approvalModel, TET_SPA_CostService main, string userID, DateTime cDate)
        {
            var pageUrl = $"{ModuleConfig.EmailRootUrl}/SupplierApproval/Index";

            EMailContent content = new EMailContent()
            {
                Title = $"[Cost&Service資料審核] {main.Period}_{applicant.FirstNameEN} {applicant.LastNameEN} ({applicant.EmpID})",
                Body =
                $@"
                您好,<br/>
                請點「<a href=""{pageUrl}"" target=""_blank"">待審清單</a>」，謝謝 <br/>
                <br/>
                流程名稱: Cost&Service資料審核 <br/>
                流程發起時間: {cDate.ToString("yyyy-MM-dd HH:mm:ss")} <br/>
                審核關卡: {approvalModel.Level} <br/>
                審核開始時間: {approvalModel.CreateDate.ToString("yyyy-MM-dd HH:mm:ss")} <br/>
                "
            };

            MailPoolManager.WritePool(receiver.EMail, content, userID, cDate);
        }

    }
}
