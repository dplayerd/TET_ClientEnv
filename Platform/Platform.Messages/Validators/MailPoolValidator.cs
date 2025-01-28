using Platform.Messages.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Platform.Messages.Validators
{
    public class MailPoolValidator
    {
        /// <summary> 驗證必填 </summary>
        /// <param name="model"> 原資料 </param>
        /// <param name="msgList"> 錯誤訊息 </param>
        /// <returns></returns>
        public static bool Valid(MailPoolModel model, out List<string> msgList)
        {
            msgList = new List<string>();

            if (string.IsNullOrWhiteSpace(model.RecipientEmail))
                msgList.Add(" 收件人 Email 為必填 ");

            if (string.IsNullOrWhiteSpace(model.Subject))
                msgList.Add(" 主旨 為必填 ");

            if (string.IsNullOrWhiteSpace(model.Body))
                msgList.Add(" 內文 為必填 ");

            if (msgList.Count > 0)
                return false;

            return true;
        }

        /// <summary> 驗證必填 </summary>
        /// <param name="model"> 原資料 </param>
        /// <param name="msgList"> 錯誤訊息 </param>
        /// <returns></returns>
        public static bool Valid(MailPoolWithCCModel model, out List<string> msgList)
        {
            msgList = new List<string>();

            if (!model.Receivers.Any())
                msgList.Add(" 收件人 Email 為必填 ");

            if (string.IsNullOrWhiteSpace(model.Subject))
                msgList.Add(" 主旨 為必填 ");

            if (string.IsNullOrWhiteSpace(model.Body))
                msgList.Add(" 內文 為必填 ");

            if (msgList.Count > 0)
                return false;

            return true;
        }
    }
}
