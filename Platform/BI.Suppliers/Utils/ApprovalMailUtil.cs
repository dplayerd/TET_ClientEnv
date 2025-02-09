using BI.Suppliers.Enums;
using Platform.AbstractionClass;
using Platform.Auth;
using Platform.Infra;
using Platform.Messages;
using Platform.ORM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BI.Suppliers.Utils
{
    /// <summary> 簽核相關信件發送器 </summary>
    internal class ApprovalMailUtil
    {
        #region New Supplier
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

            MailPoolManager.WriteMailWithCC(mailList, content, userID, cDate);
        }


        /// <summary> 寄信新的簽核人 </summary>
        /// <param name="receiverMail"></param>
        /// <param name="approvalModel"></param>
        /// <param name="levelName"></param>
        /// <param name="userID">目前登入者</param>
        /// <param name="cDate">目前時間</param> 
        internal static void SendNewVerifyMail(string receiverMail, TET_SupplierApproval approvalModel, string levelName, string userID, DateTime cDate)
        {
            var pageUrl = $"{ModuleConfig.EmailRootUrl}/SupplierApproval/Index";

            EMailContent content = new EMailContent()
            {
                Title = $"[審核通知] {approvalModel.Description}",
                Body =
                $@"
                您好,<br/>
                請點「<a href=""{pageUrl}"" target=""_blank"">待審清單</a>」，謝謝 <br/>
                <br/>
                流程名稱: 新增供應商審核 <br/>
                流程發起時間: {cDate.ToString("yyyy-MM-dd HH:mm:ss")} <br/>
                審核關卡: {levelName} <br/>
                審核開始時間: {approvalModel.CreateDate.ToString("yyyy-MM-dd HH:mm:ss")} <br/>
                "
            };

            MailPoolManager.WritePool(receiverMail, content, userID, cDate);
        }
        #endregion


        #region Modify Supplier
        /// <summary> 寄信新的簽核人 </summary>
        /// <param name="receiverMailList"></param>
        /// <param name="approvalModel"></param>
        /// <param name="levelName"></param>
        /// <param name="userID">目前登入者</param>
        /// <param name="cDate">目前時間</param>
        internal static void SendRevisionVerifyMail(List<string> receiverMailList, TET_SupplierApproval approvalModel, string levelName, string userID, DateTime cDate)
        {
            var pageUrl = $"{ModuleConfig.EmailRootUrl}/SupplierApproval/Index";

            EMailContent content = new EMailContent()
            {
                Title = $"[審核通知] {approvalModel.Description}",
                Body =
                $@"
                您好,<br/>
                請點「<a href=""{pageUrl}"" target=""_blank"">待審清單</a>」，謝謝 <br/>
                <br/>
                流程名稱: {ApprovalType.Modify.ToText()} <br/>
                流程發起時間: {cDate.ToString("yyyy-MM-dd HH:mm:ss")} <br/>
                審核關卡: {levelName} <br/>
                審核開始時間: {approvalModel.CreateDate.ToString("yyyy-MM-dd HH:mm:ss")} <br/>
                "
            };

            MailPoolManager.WritePool(receiverMailList, content, userID, cDate);
        }
        #endregion
    }
}
