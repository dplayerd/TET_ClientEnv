using Platform.AbstractionClass;
using Platform.Infra;
using Platform.LogService;
using Platform.Messages.Enums;
using Platform.Messages.Models;
using Platform.Messages.Validators;
using Platform.ORM;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using Newtonsoft.Json;


namespace Platform.Messages
{
    public class MailPoolManager
    {
        /// <summary> 重試上限 </summary>
        private const int _retryLimit = 3;


        private Logger _logger = new Logger();

        #region Read
        /// <summary> 取得 MailPool 清單 </summary>
        /// <returns></returns>
        public List<MailPoolModel> GetList()
        {
            try
            {
                using (PlatformContextModel context = new PlatformContextModel())
                {
                    var orgQuery =
                        from item in context.MailPools
                        select item;

                    IQueryable<MailPoolModel> query = ConvertEntityToModel(orgQuery);

                    var list = query.OrderByDescending(obj => obj.CreateDate).ToList();
                    return list;
                }
            }
            catch (Exception ex)
            {
                this._logger.WriteError(ex);
                return default;
            }
        }

        private IQueryable<MailPoolModel> ConvertEntityToModel(IQueryable<MailPool> orgQuery)
        {
            var query =
                from item in orgQuery
                select
                new MailPoolModel()
                {
                    ID = item.ID,
                    SenderName = item.SenderName,
                    SenderEmail = item.SenderEmail,
                    RecipientName = item.RecipientName,
                    RecipientEmail = item.RecipientEmail,
                    Subject = item.Subject,
                    Body = item.Body,
                    Priority = (MailPriorityEnum)item.Priority,
                    Status = (MailStatusEnum)item.Status,
                    SendDateTime = item.SendDateTime,
                    IsSent = item.IsSent,
                    ErrorMessage = item.ErrorMessage,
                    RetryCount = item.RetryCount,
                    LastRetryDateTime = item.LastRetryDateTime,
                    CreateDate = item.CreateDate,
                    CreateUser = item.CreateUser,
                };

            return query;
        }


        private IQueryable<MailPoolWithCCModel> ConvertEntityToModel(IQueryable<MailPoolWithCC> orgQuery)
        {
            var query =
                from item in orgQuery
                select
                new MailPoolWithCCModel()
                {
                    ID = item.ID,
                    SenderName = item.SenderName,
                    SenderEmail = item.SenderEmail,
                    Receivers = new List<string>() { item.Receivers },
                    CCs = new List<string>() { item.CCs },
                    Subject = item.Subject,
                    Body = item.Body,
                    Priority = (MailPriorityEnum)item.Priority,
                    Status = (MailStatusEnum)item.Status,
                    SendDateTime = item.SendDateTime,
                    IsSent = item.IsSent,
                    ErrorMessage = item.ErrorMessage,
                    RetryCount = item.RetryCount,
                    LastRetryDateTime = item.LastRetryDateTime,
                    CreateDate = item.CreateDate,
                    CreateUser = item.CreateUser,
                };

            return query;
        }

        /// <summary> 取得待寄件清單 </summary>
        /// <returns></returns>
        public List<MailPoolModel> GetWaitingList()
        {
            try
            {
                using (PlatformContextModel context = new PlatformContextModel())
                {
                    var orgQuery =
                        from item in context.MailPools
                        where !item.IsSent && item.RetryCount < _retryLimit
                        orderby item.Priority descending
                        select item;

                    IQueryable<MailPoolModel> query = ConvertEntityToModel(orgQuery);
                    var list = query.ToList();
                    return list;
                }
            }
            catch (Exception ex)
            {
                this._logger.WriteError(ex);
                return default;
            }
        }


        /// <summary> 取得待寄件清單 </summary>
        /// <returns></returns>
        public List<MailPoolWithCCModel> GetWaitingListWithCC()
        {
            try
            {
                using (PlatformContextModel context = new PlatformContextModel())
                {
                    var orgQuery =
                        from item in context.MailPoolWithCCs
                        where !item.IsSent && item.RetryCount < _retryLimit
                        orderby item.Priority descending
                        select item;

                    IQueryable<MailPoolWithCCModel> query = ConvertEntityToModel(orgQuery);
                    var list = query.ToList();

                    foreach (var item in list)
                    {
                        if (item.Receivers.Any())
                            item.Receivers = JsonConvert.DeserializeObject<List<string>>(item.Receivers[0]);

                        if (item.CCs.Any())
                            item.CCs = JsonConvert.DeserializeObject<List<string>>(item.CCs[0]);
                    }

                    return list;

                    //var list = context.MailPoolWithCCs.SqlQuery($"SELECT * FROM dbo.MailPoolWithCC WHERE IsSent = 'false' AND RetryCount < {_retryLimit}").ToList();

                    //var retList = new List<MailPoolWithCCModel>();
                    //foreach (MailPoolWithCC item in list)
                    //{
                    //    var model = 
                    //        new MailPoolWithCCModel()
                    //        {
                    //            ID = item.ID,
                    //            SenderName = item.SenderName,
                    //            SenderEmail = item.SenderEmail,
                    //            Receivers = new List<string>() { item.Receivers },
                    //            CCs = new List<string>() { item.CCs },
                    //            Subject = item.Subject,
                    //            Body = item.Body,
                    //            Priority = (MailPriorityEnum)item.Priority,
                    //            Status = (MailStatusEnum)item.Status,
                    //            SendDateTime = item.SendDateTime,
                    //            IsSent = item.IsSent,
                    //            ErrorMessage = item.ErrorMessage,
                    //            RetryCount = item.RetryCount,
                    //            LastRetryDateTime = item.LastRetryDateTime,
                    //            CreateDate = item.CreateDate,
                    //            CreateUser = item.CreateUser,
                    //        };

                    //    if (item.Receivers.Any())
                    //        model.Receivers = JsonConvert.DeserializeObject<List<string>>(model.Receivers[0]);

                    //    if (item.CCs.Any())
                    //        model.CCs = JsonConvert.DeserializeObject<List<string>>(model.CCs[0]);

                    //    retList.Add(model);
                    //}

                    //return retList;
                }
            }
            catch (Exception ex)
            {
                this._logger.WriteError(ex);
                return default;
            }
        }
        #endregion


        #region CUD
        /// <summary> 新增 MailPool </summary>
        /// <param name="model"></param>
        /// <param name="userID">目前登入者</param>
        /// <param name="cDate">目前時間</param>
        public void Create(MailPoolModel model, string userID, DateTime cDate)
        {
            if (model == null)
                throw new ArgumentNullException("Mail is required.");

            // 新增前，先檢查是否能通過商業邏輯
            if (!MailPoolValidator.Valid(model, out List<string> msgList))
                throw new ArgumentException(string.Join(Environment.NewLine, msgList));

            var config = EmailConfig.GetDefault();

            if (string.IsNullOrWhiteSpace(config.SmtpAccount))
                throw new ConfigurationException("SmtpAccount (Config) is required.");


            try
            {
                using (PlatformContextModel context = new PlatformContextModel())
                {
                    var entity = new MailPool()
                    {
                        ID = model.ID,
                        SenderEmail = config.SmtpAccount,
                        SenderName = config.SenderName,
                        RecipientName = model.RecipientName,
                        RecipientEmail = model.RecipientEmail,
                        Subject = model.Subject,
                        Body = model.Body,
                        Priority = (byte)model.Priority,
                        Status = (byte)MailStatusEnum.Default,

                        CreateDate = cDate,
                        CreateUser = userID,
                    };

                    if (!config.WillSendMail)
                    {
                        entity.IsSent = true;
                        entity.Status = (byte)MailStatusEnum.Complete;
                        entity.SendDateTime = cDate;
                        entity.ErrorMessage = $"--- Completed, mail doesn't send because WillSendMail is 'N'.";
                    }

                    context.MailPools.Add(entity);
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                this._logger.WriteError(ex);
                throw;
            }
        }



        /// <summary> 新增 MailPool </summary>
        /// <param name="model"></param>
        /// <param name="userID">目前登入者</param>
        /// <param name="cDate">目前時間</param>
        public void Create(MailPoolWithCCModel model, string userID, DateTime cDate)
        {
            if (model == null)
                throw new ArgumentNullException("Mail is required.");

            // 新增前，先檢查是否能通過商業邏輯
            if (!MailPoolValidator.Valid(model, out List<string> msgList))
                throw new ArgumentException(string.Join(Environment.NewLine, msgList));

            var config = EmailConfig.GetDefault();

            if (string.IsNullOrWhiteSpace(config.SmtpAccount))
                throw new ConfigurationException("SmtpAccount (Config) is required.");


            try
            {
                using (PlatformContextModel context = new PlatformContextModel())
                {
                    var entity = new MailPoolWithCC()
                    {
                        ID = model.ID,
                        SenderEmail = config.SmtpAccount,
                        SenderName = config.SenderName,

                        Receivers = JsonConvert.SerializeObject(model.Receivers),
                        CCs = JsonConvert.SerializeObject(model.CCs),

                        Subject = model.Subject,
                        Body = model.Body,
                        Priority = (byte)model.Priority,
                        Status = (byte)MailStatusEnum.Default,

                        CreateDate = cDate,
                        CreateUser = userID,
                    };

                    if (!config.WillSendMail)
                    {
                        entity.IsSent = true;
                        entity.Status = (byte)MailStatusEnum.Complete;
                        entity.SendDateTime = cDate;
                        entity.ErrorMessage = $"--- Completed, mail doesn't send because WillSendMail is 'N'.";
                    }

                    context.MailPoolWithCCs.Add(entity);
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                this._logger.WriteError(ex);
                throw;
            }
        }


        /// <summary> 刪除過期 MailPool </summary>
        /// <param name="model"></param>
        /// <param name="userID">目前登入者</param>
        /// <param name="cDate">目前時間</param>
        public int DeleteExpiredMail(string userID, DateTime cDate)
        {
            var config = EmailConfig.GetDefault();
            int[] arrFinished = { (byte)MailStatusEnum.Fail, (byte)MailStatusEnum.Complete };

            try
            {
                using (PlatformContextModel context = new PlatformContextModel())
                {
                    // 計算過期時間
                    var expiredDate = cDate.Date.AddDays(-1 * config.ExpireDays);

                    // 一般信件
                    var orgQuery_MailPool =
                        (from item in context.MailPools
                         where
                             arrFinished.Contains(item.Status) &&
                             item.CreateDate < expiredDate
                         orderby item.Priority descending
                         select item);

                    var mailPoolList = orgQuery_MailPool.ToList();
                    context.MailPools.RemoveRange(mailPoolList);


                    // 附有 CC 的信件
                    var orgQuery_MailPoolWithCC =
                        (from item in context.MailPoolWithCCs
                         where
                             arrFinished.Contains(item.Status) &&
                             item.CreateDate < expiredDate
                         orderby item.Priority descending
                         select item);

                    var mailPoolWithCCList = orgQuery_MailPoolWithCC.ToList();
                    context.MailPoolWithCCs.RemoveRange(mailPoolWithCCList);


                    context.SaveChanges();

                    return mailPoolList.Count + mailPoolWithCCList.Count;
                }
            }
            catch (Exception ex)
            {
                this._logger.WriteError(ex);
                throw;
            }
        }
        #endregion


        #region Static Method
        /// <summary> 寫入待發清單 </summary>
        /// <param name="receiver"> 收件人 </param>
        /// <param name="content"> 正文 </param>
        /// <param name="userID">目前登入者</param>
        /// <param name="cDate">目前時間</param>
        public static void WritePool(string receiver, EMailContent content, string userID, DateTime cDate)
        {
            WritePool(new List<string>() { receiver }, content, userID, cDate);
        }

        /// <summary> 寫入待發清單 </summary>
        /// <param name="receiverList"> 收件人 </param>
        /// <param name="content"> 正文 </param>
        /// <param name="userID">目前登入者</param>
        /// <param name="cDate">目前時間</param>
        public static void WritePool(List<string> receiverList, EMailContent content, string userID, DateTime cDate)
        {
            var mgr = new MailPoolManager();
            var config = EmailConfig.GetDefault();

            foreach (var receiver in receiverList)
            {
                MailPoolModel model = new MailPoolModel()
                {
                    RecipientName = string.Empty,
                    RecipientEmail = receiver,
                    Subject = content.Title,
                    Body = content.Body,
                };

                mgr.Create(model, userID, cDate);
            }
        }

        /// <summary> 加入一個可以寄 CC 的信件 (所有人只收一封信) </summary>
        /// <param name="receiverList"></param>
        /// <param name="ccList"></param>
        /// <param name="content"></param>
        /// <param name="userID"></param>
        /// <param name="cDate"></param>
        public static void WriteMailWithCC(List<string> receiverList, List<string> ccList, EMailContent content, string userID, DateTime cDate)
        {
            var mgr = new MailPoolManager();
            var config = EmailConfig.GetDefault();

            MailPoolWithCCModel model = new MailPoolWithCCModel()
            {
                Receivers = receiverList,
                CCs = ccList,

                Subject = content.Title,
                Body = content.Body,
            };

            mgr.Create(model, userID, cDate);
        }


        /// <summary> 加入一個可以寄 CC 的信件 (所有人只收一封信) </summary>
        /// <param name="receiverList"></param>
        /// <param name="ccList"></param>
        /// <param name="content"></param>
        /// <param name="userID"></param>
        /// <param name="cDate"></param>
        public static void WriteMailWithCC(List<string> receiverList, EMailContent content, string userID, DateTime cDate)
        {
            WriteMailWithCC(receiverList, new List<string>(), content, userID, cDate);
        }
        #endregion


        #region SendAndKeepMessage
        /// <summary> 保留完成紀錄 </summary>
        /// <param name="model"></param>
        /// <param name="userID">目前登入者</param>
        /// <param name="cDate">目前時間</param>
        public void WriteSuccessMessage(MailPoolModel model, string userID, DateTime cDate)
        {
            try
            {
                using (PlatformContextModel context = new PlatformContextModel())
                {
                    var mail = context.MailPools.Find(model.ID);

                    if (mail == null)
                        throw new NullReferenceException($"找不到項目 {model.ID}");

                    mail.IsSent = true;
                    mail.SendDateTime = cDate;
                    mail.Status = (byte)MailStatusEnum.Complete;

                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                this._logger.WriteError(ex);
            }
        }


        /// <summary> 紀錄錯誤 </summary>
        /// <param name="model"></param>
        /// <param name="ex"> 錯誤訊息 </param>
        /// <param name="userID">目前登入者</param>
        /// <param name="cDate">目前時間</param>

        /// <exception cref="NullReferenceException"></exception>
        public void WriteFailResult(MailPoolModel model, Exception ex, string userID, DateTime cDate)
        {
            using (PlatformContextModel context = new PlatformContextModel())
            {
                var mail = context.MailPools.Find(model.ID);

                if (mail == null)
                    throw new NullReferenceException($"找不到項目 {model.ID}");

                mail.ErrorMessage +=
$@"----- 
Send: {mail.RetryCount} {cDate.ToLongTimeString()} 
{ex.Message} 

";
                mail.RetryCount += 1;
                mail.LastRetryDateTime = DateTime.Now;

                if (mail.RetryCount >= _retryLimit)
                    mail.Status = (byte)MailStatusEnum.Fail;

                context.SaveChanges();
            }
        }

        /// <summary> 保留完成紀錄 </summary>
        /// <param name="model"></param>
        /// <param name="userID">目前登入者</param>
        /// <param name="cDate">目前時間</param>
        public void WriteSuccessMessage(MailPoolWithCCModel model, string userID, DateTime cDate)
        {
            try
            {
                using (PlatformContextModel context = new PlatformContextModel())
                {
                    var mail = context.MailPoolWithCCs.Find(model.ID);

                    if (mail == null)
                        throw new NullReferenceException($"找不到項目 {model.ID}");

                    mail.IsSent = true;
                    mail.SendDateTime = cDate;
                    mail.Status = (byte)MailStatusEnum.Complete;

                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                this._logger.WriteError(ex);
            }
        }


        /// <summary> 紀錄錯誤 </summary>
        /// <param name="model"></param>
        /// <param name="ex"> 錯誤訊息 </param>
        /// <param name="userID">目前登入者</param>
        /// <param name="cDate">目前時間</param>
        public void WriteFailResult(MailPoolWithCCModel model, Exception ex, string userID, DateTime cDate)
        {
            using (PlatformContextModel context = new PlatformContextModel())
            {
                var mail = context.MailPoolWithCCs.Find(model.ID);

                if (mail == null)
                    throw new NullReferenceException($"找不到項目 {model.ID}");

                mail.ErrorMessage +=
$@"----- 
Send: {mail.RetryCount} {cDate.ToLongTimeString()} 
{ex.Message} 

";
                mail.RetryCount += 1;
                mail.LastRetryDateTime = DateTime.Now;

                if (mail.RetryCount >= _retryLimit)
                    mail.Status = (byte)MailStatusEnum.Fail;

                context.SaveChanges();
            }
        }
        #endregion
    }
}
