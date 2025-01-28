using BI.SPA.Models;
using BI.SPA.Enums;
using Platform.AbstractionClass;
using Platform.Infra;
using Platform.ORM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Platform.Messages;
using System.Xml.Linq;

namespace BI.SPA.Utils
{
    /// <summary> 簽核相關信件發送器 </summary>
    internal class ApprovalMailUtil
    {
        #region New Supplier
        /// <summary> 寄信給簽核過的人 </summary>
        /// <param name="mailList"></param>
        /// <param name="dbModel"></param>
        /// <param name="userID">目前登入者</param>
        /// <param name="cDate">目前時間</param>
        internal static void SendAbordMail(List<string> mailList, TET_SupplierSPA dbModel, string userID, DateTime cDate)
        {
            EMailContent content = new EMailContent()
            {
                Title = $"[審核中止通知] 新增SPA資料審核_{dbModel.BelongTo}",
                Body =
                $@"
                您好，<br/>
                <br/>
                此流程已由申請人中止，謝謝<br/>              
                "
            };

            MailPoolManager.WriteMailWithCC(mailList, content, userID, cDate);
        }


        /// <summary> 寄信新的簽核人 </summary>
        /// <param name="receiverMail"></param>
        /// <param name="approvalModel"></param>
        /// <param name="dbModel"></param>
        /// <param name="userID">目前登入者</param>
        /// <param name="cDate">目前時間</param>
        internal static void SendNewApprovalMail(string receiverMail, TET_SupplierSPAApproval approvalModel, TET_SupplierSPA dbModel, string userID, DateTime cDate)
        {
            var pageUrl = $"{ModuleConfig.EmailRootUrl}/SupplierApproval/Index";

            EMailContent content = new EMailContent()
            {
                Title = $"[審核通知] 新增SPA資料審核_{dbModel.Period}_{dbModel.BelongTo}",
                Body =
                $@"
您好,<br/>
請點「<a href=""{pageUrl}"" target=""_blank"">待審清單</a>」，謝謝 <br/>
<br/>
流程名稱: 新增SPA資料審核 <br/>
流程發起時間: {cDate.ToString("yyyy-MM-dd HH:mm:ss")} <br/>
審核關卡: {approvalModel.Level} <br/>
審核開始時間: {approvalModel.CreateDate.ToString("yyyy-MM-dd HH:mm:ss")} <br/>
                "
            };

            MailPoolManager.WritePool(receiverMail, content, userID, cDate);
        }
        #endregion
    }
}
