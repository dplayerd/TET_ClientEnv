using BI.SPA_Evaluation.Enums;
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

namespace BI.SPA_Evaluation.Utils
{
    /// <summary> 簽核相關信件發送器 </summary>
    internal class MailUtil
    {
        /// <summary> 寄信給需要評鑑的人 </summary>
        /// <param name="receivers"></param>
        /// <param name="main"></param>
        /// <param name="userID">目前登入者</param>
        /// <param name="cDate">目前時間</param> 
        internal static void SendMessageMail(List<UserAccountModel> receivers, TET_SPA_EvaluationReport main, string userID, DateTime cDate)
        {
            var pageUrl = $"{ModuleConfig.EmailRootUrl}/SPA_EvaluationReport/Index";

            EMailContent content = new EMailContent()
            {
                Title = $"[績效評鑑報告] {main.Period}_{main.BU})",
                Body =
                $@"
您好,<br/>
<br/>
檢視績效評鑑報告請點擊「<a href=""{pageUrl}"" target=""_blank"">SPA 績效評鑑報告維護</a>」連結，謝謝 <br/>
                "
            };

            var emailList = receivers.Select(obj => obj.EMail).ToList();
            MailPoolManager.WriteMailWithCC(emailList, new List<string>(), content, userID, cDate);
        }

    }
}
