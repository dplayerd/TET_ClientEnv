using BI.PaymentSuppliers.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Platform.AbstractionClass;
using Platform.Infra;
using Platform.LogService;
using Platform.ORM;
using System.Xml.Linq;
using BI.PaymentSuppliers.Validators;

namespace BI.PaymentSuppliers
{
    public class TET_PaymentSupplierContactManager
    {
        private Logger _logger = new Logger();

        #region Read
        /// <summary>
        /// 取得 一般付款對象聯絡人 清單
        /// </summary>
        /// <param name="context">  </param>
        /// <param name="psID"> 一般付款對象 ID </param>
        /// <returns></returns>
        public List<TET_PaymentSupplierContactModel> GetTET_PaymentSupplierContactList(PlatformContextModel context, Guid psID)
        {
            var query =
                from item in context.TET_PaymentSupplierContact
                where item.PSID == psID
                orderby item.CreateDate
                select
                new TET_PaymentSupplierContactModel()
                {
                    ID = item.ID,
                    PSID = item.PSID,
                    ContactName = item.Name,
                    ContactTitle = item.Title,
                    ContactTel = item.Tel,
                    ContactEmail = item.Email,
                    ContactRemark = item.Remark,
                    CreateUser = item.CreateUser,
                    CreateDate = item.CreateDate,
                    ModifyUser = item.ModifyUser,
                    ModifyDate = item.ModifyDate,
                };

            var list = query.ToList();
            return list;
        }

        /// <summary>
        /// 取得 一般付款對象聯絡人 清單
        /// </summary>
        /// <param name="psIDs"> 一般付款對象 ID </param>
        /// <returns></returns>
        public List<TET_PaymentSupplierContactModel> GetTET_PaymentSupplierContactList(IEnumerable<Guid> psIDs)
        {
            try
            {
                using (PlatformContextModel context = new PlatformContextModel())
                {
                    var query =
                        from item in context.TET_PaymentSupplierContact
                        where psIDs.Contains(item.PSID)
                        orderby item.CreateDate
                        select
                        new TET_PaymentSupplierContactModel()
                        {
                            ID = item.ID,
                            PSID = item.PSID,
                            ContactName = item.Name,
                            ContactTitle = item.Title,
                            ContactTel = item.Tel,
                            ContactEmail = item.Email,
                            ContactRemark = item.Remark,
                            CreateUser = item.CreateUser,
                            CreateDate = item.CreateDate,
                            ModifyUser = item.ModifyUser,
                            ModifyDate = item.ModifyDate,
                        };

                    var list = query.ToList();
                    return list;
                }
            }
            catch (Exception ex)
            {
                this._logger.WriteError(ex);
                throw;
            }
        }
        #endregion

        #region CUD
        /// <summary> 新增 一般付款對象聯絡人 </summary>
        /// <param name="context"> ORM 核心 </param>
        /// <param name="psID"> 一般付款對象 ID </param>
        /// <param name="modelList"> 一般付款對象聯絡人 List </param>
        /// <param name="userID">目前登入者</param>
        /// <param name="cDate">目前時間</param>
        internal void WriteTET_PaymentSupplierContact(PlatformContextModel context, Guid psID, List<TET_PaymentSupplierContactModel> modelList, string userID, DateTime cDate)
        {
            if (modelList == null)
                throw new ArgumentNullException("Contact is required.");

            // 新增前，先檢查是否能通過商業邏輯
            foreach (var model in modelList)
            {
                if (!PaymentSupplierContactValidator.ValidCreate(model, out List<string> msgList))
                    throw new ArgumentException(string.Join(Environment.NewLine, msgList));
            }

            // 先刪除，然後才新增
            var currentList =
                (from item in context.TET_PaymentSupplierContact
                 where item.PSID == psID
                 select item).ToList();

            context.TET_PaymentSupplierContact.RemoveRange(currentList);


            foreach (var model in modelList)
            {
                var entity = new TET_PaymentSupplierContact()
                {
                    ID = Guid.NewGuid(),
                    PSID = psID,
                    Name = model.ContactName,
                    Title = model.ContactTitle,
                    Tel = model.ContactTel,
                    Email = model.ContactEmail,
                    Remark = model.ContactRemark,
                    CreateUser = userID,
                    CreateDate = cDate,
                    ModifyUser = userID,
                    ModifyDate = cDate,
                };

                context.TET_PaymentSupplierContact.Add(entity);
            }

        }

        /// <summary> 複製 一般付款對象聯絡人 </summary>
        /// <param name="context"> ORM 核心 </param>
        /// <param name="psID"> 一般付款對象 ID </param>
        /// <param name="modelList"> 一般付款對象聯絡人 List </param>
        /// <param name="userID">目前登入者</param>
        /// <param name="cDate">目前時間</param>
        internal void CopyTET_PaymentSupplierContact(PlatformContextModel context, Guid psID, List<TET_PaymentSupplierContactModel> modelList, string userID, DateTime cDate)
        {
            foreach (var model in modelList)
            {
                var entity = new TET_PaymentSupplierContact()
                {
                    ID = Guid.NewGuid(),
                    PSID = psID,
                    Name = model.ContactName,
                    Title = model.ContactTitle,
                    Tel = model.ContactTel,
                    Email = model.ContactEmail,
                    Remark = model.ContactRemark,
                    CreateUser = userID,
                    CreateDate = cDate,
                    ModifyUser = userID,
                    ModifyDate = cDate,
                };

                context.TET_PaymentSupplierContact.Add(entity);
            }
        }


        /// <summary> 刪除 一般付款對象裡所有聯絡人 </summary>
        /// <param name="context"> ORM 核心 </param>
        /// <param name="psID"> 一般付款對象 ID </param>
        /// <param name="userID">目前登入者</param>
        /// <param name="cDate">目前時間</param>
        internal void DeleteTET_PaymentSupplierContact(PlatformContextModel context, Guid psID, string userID, DateTime cDate)
        {
            // 先刪除，然後才新增
            var currentList =
                (from item in context.TET_PaymentSupplierContact
                 where item.PSID == psID
                 select item).ToList();

            context.TET_PaymentSupplierContact.RemoveRange(currentList);
        }
        #endregion
    }
}
