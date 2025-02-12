using BI.SPA_Violation.Enums;
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

namespace BI.SPA_Violation.Utils
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
        /// <param name="receiver"></param>
        /// <param name="approvalModel"></param>
        /// <param name="main"></param>
        /// <param name="userID">目前登入者</param>
        /// <param name="cDate">目前時間</param> 
        internal static void SendNewVerifyMail(UserAccountModel receiver, TET_SPA_ViolationApproval approvalModel, TET_SPA_Violation main, string userID, DateTime cDate)
        {
            UserRoleManager _roleMgr = new UserRoleManager();
            var qsmList = _roleMgr.GetUserListInRole(ApprovalRole.QSM.ToID().Value);
            var qsmMailList = qsmList.Select(obj => obj.EMail).ToList();
            var pageUrl = $"{ModuleConfig.EmailRootUrl}/SupplierApproval/Index";

            EMailContent content = new EMailContent()
            {
                Title = $"[違規紀錄資料審核] {main.Period}",
                Body =
                $@"
                您好,<br/>
                請點擊「<a href=""{pageUrl}"" target=""_blank"">待審清單</a>」，謝謝 <br/>
                <br/>
                流程名稱: 違規紀錄資料審核 <br/>
                流程發起時間: {cDate.ToString("yyyy-MM-dd HH:mm:ss")} <br/>
                審核關卡: {ApprovalLevel.Level_1.ToDisplayText()} <br/>
                審核開始時間: {approvalModel.CreateDate.ToString("yyyy-MM-dd HH:mm:ss")} <br/>
                "
            };

            var Email = new List<string>();
            Email.Add(receiver.EMail);

            //MailPoolManager.WriteMailWithCC(Email, qsmMailList, content, userID, cDate);
            MailPoolManager.WriteMailWithCC(Email, new List<string>(), content, userID, cDate);
        }

        /// <summary> 寄信新的簽核人 </summary>
        /// <param name="leveltext"></param>
        /// <param name="receiver"></param>
        /// <param name="approvalModel"></param>
        /// <param name="main"></param>
        /// <param name="userID">目前登入者</param>
        /// <param name="cDate">目前時間</param> 
        internal static void SendApprovalMail(string leveltext, UserAccountModel receiver, TET_SPA_ViolationApproval approvalModel, string userID, DateTime cDate)
        {
            UserRoleManager _roleMgr = new UserRoleManager();
            var qsmList = _roleMgr.GetUserListInRole(ApprovalRole.QSM.ToID().Value);
            var qsmMailList = qsmList.Select(obj => obj.EMail).ToList();
            var pageUrl = $"{ModuleConfig.EmailRootUrl}/SupplierApproval/Index";

            EMailContent content = new EMailContent()
            {
                Title = $"[違規紀錄資料審核] {approvalModel.Description}",
                Body =
                $@"
                您好,<br/>
                請點擊「<a href=""{pageUrl}"" target=""_blank"">待審清單</a>」，謝謝 <br/>
                <br/>
                流程名稱: 違規紀錄資料審核 <br/>
                流程發起時間: {cDate.ToString("yyyy-MM-dd HH:mm:ss")} <br/>
                審核關卡: {leveltext} <br/>
                審核開始時間: {approvalModel.CreateDate.ToString("yyyy-MM-dd HH:mm:ss")} <br/>
                "
            };

            var Email = new List<string>();
            Email.Add(receiver.EMail);

            //MailPoolManager.WriteMailWithCC(Email, qsmMailList, content, userID, cDate)
            MailPoolManager.WriteMailWithCC(Email, new List<string>(), content, userID, cDate);
        }
    }
}
