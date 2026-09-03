using Newtonsoft.Json;
using Platform.Infra;
using Platform.Messages.Enums;
using Platform.Messages.Models;
using Platform.Messages.Validators;
using Platform.ORM;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace Platform.Messages
{
    /// <summary> 信件待發清單寫入服務 </summary>
    public class MailQueueService : IMailQueueService
    {
        /// <summary> 寫入一封含 CC 的待發信件 </summary>
        /// <param name="mail">信件內容</param>
        /// <param name="userID">建立者</param>
        /// <param name="cDate">建立時間</param>
        public void Enqueue(MailPoolWithCCModel mail, string userID, DateTime cDate)
        {
            this.EnqueueBatch(new List<MailPoolWithCCModel>() { mail }, userID, cDate);
        }

        /// <summary> 批次寫入含 CC 的待發信件，整批成功或整批失敗 </summary>
        /// <param name="mails">信件內容清單</param>
        /// <param name="userID">建立者</param>
        /// <param name="cDate">建立時間</param>
        public void EnqueueBatch(IEnumerable<MailPoolWithCCModel> mails, string userID, DateTime cDate)
        {
            using (PlatformContextModel context = new PlatformContextModel())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    this.EnqueueBatch(context, mails, userID, cDate);
                    context.SaveChanges();
                    transaction.Commit();
                }
            }
        }

        /// <summary> 使用外部交易中的 DbContext 批次寫入含 CC 的待發信件 </summary>
        /// <param name="context">外部交易使用的 DbContext</param>
        /// <param name="mails">信件內容清單</param>
        /// <param name="userID">建立者</param>
        /// <param name="cDate">建立時間</param>
        public void EnqueueBatch(PlatformContextModel context, IEnumerable<MailPoolWithCCModel> mails, string userID, DateTime cDate)
        {
            if (context == null)
                throw new ArgumentNullException("context");

            if (mails == null)
                throw new ArgumentNullException("mails");

            var mailList = mails.ToList();
            var config = EmailConfig.GetDefault();

            if (config == null)
                throw new ConfigurationErrorsException("EmailConfig is required.");

            if (string.IsNullOrWhiteSpace(config.SmtpAccount))
                throw new ConfigurationErrorsException("SmtpAccount (Config) is required.");

            // 僅加入 DbContext，不在此 SaveChanges，讓外部可與業務 log 共用同一筆交易。
            foreach (var mail in mailList)
            {
                if (mail == null)
                    throw new ArgumentNullException("Mail is required.");

                if (!MailPoolValidator.Valid(mail, out List<string> msgList))
                    throw new ArgumentException(string.Join(Environment.NewLine, msgList));

                var entity = new MailPoolWithCC()
                {
                    SenderEmail = config.SmtpAccount,
                    SenderName = config.SenderName,
                    Receivers = JsonConvert.SerializeObject(mail.Receivers.Distinct().ToList()),
                    CCs = JsonConvert.SerializeObject((mail.CCs ?? new List<string>()).Distinct().ToList()),
                    Subject = mail.Subject,
                    Body = mail.Body,
                    Priority = (byte)mail.Priority,
                    Status = (byte)MailStatusEnum.Default,
                    RetryCount = 0,
                    IsSent = false,
                    CreateDate = cDate,
                    CreateUser = userID,
                };

                if (!config.WillSendMail)
                {
                    entity.IsSent = true;
                    entity.Status = (byte)MailStatusEnum.Complete;
                    entity.SendDateTime = cDate;
                    entity.ErrorMessage = "--- Completed, mail doesn't send because WillSendMail is 'N'.";
                }

                context.MailPoolWithCCs.Add(entity);
            }
        }
    }
}
