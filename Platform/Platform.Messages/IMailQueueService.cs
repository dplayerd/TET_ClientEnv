using Platform.Messages.Models;
using Platform.ORM;
using System;
using System.Collections.Generic;

namespace Platform.Messages
{
    /// <summary> 信件待發清單寫入服務 </summary>
    public interface IMailQueueService
    {
        /// <summary> 寫入一封含 CC 的待發信件 </summary>
        /// <param name="mail">信件內容</param>
        /// <param name="userID">建立者</param>
        /// <param name="cDate">建立時間</param>
        void Enqueue(MailPoolWithCCModel mail, string userID, DateTime cDate);

        /// <summary> 批次寫入含 CC 的待發信件，整批成功或整批失敗 </summary>
        /// <param name="mails">信件內容清單</param>
        /// <param name="userID">建立者</param>
        /// <param name="cDate">建立時間</param>
        void EnqueueBatch(IEnumerable<MailPoolWithCCModel> mails, string userID, DateTime cDate);

        /// <summary> 使用外部交易中的 DbContext 批次寫入含 CC 的待發信件 </summary>
        /// <param name="context">外部交易使用的 DbContext</param>
        /// <param name="mails">信件內容清單</param>
        /// <param name="userID">建立者</param>
        /// <param name="cDate">建立時間</param>
        void EnqueueBatch(PlatformContextModel context, IEnumerable<MailPoolWithCCModel> mails, string userID, DateTime cDate);
    }
}
