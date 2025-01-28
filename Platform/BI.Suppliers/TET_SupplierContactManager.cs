using BI.Suppliers.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Platform.AbstractionClass;
using Platform.Infra;
using Platform.LogService;
using Platform.ORM;
using System.Xml.Linq;
using BI.Suppliers.Validators;

namespace BI.Suppliers
{
    public class TET_SupplierContactManager
    {
        private Logger _logger = new Logger();

        #region Read
        /// <summary>
        /// 取得 供應商聯絡人 清單
        /// </summary>
        /// <param name="context">  </param>
        /// <param name="supplierID"> 供應商 ID </param>
        /// <returns></returns>
        public List<TET_SupplierContactModel> GetTET_SupplierContactList(PlatformContextModel context, Guid supplierID)
        {
            var query =
                from item in context.TET_SupplierContact
                where item.SupplierID == supplierID
                orderby item.CreateDate
                select
                new TET_SupplierContactModel()
                {
                    ID = item.ID,
                    SupplierID = item.SupplierID,
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
        /// 取得 供應商聯絡人 清單
        /// </summary>
        /// <param name="supplierIDs"> 供應商 ID </param>
        /// <returns></returns>
        public List<TET_SupplierContactModel> GetTET_SupplierContactList(IEnumerable<Guid> supplierIDs)
        {
            try
            {
                using (PlatformContextModel context = new PlatformContextModel())
                {
                    var query =
                        from item in context.TET_SupplierContact
                        where supplierIDs.Contains(item.SupplierID)
                        orderby item.CreateDate
                        select
                        new TET_SupplierContactModel()
                        {
                            ID = item.ID,
                            SupplierID = item.SupplierID,
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
        /// <summary> 新增 供應商聯絡人 </summary>
        /// <param name="context"> ORM 核心 </param>
        /// <param name="supplierID"> 供應商 ID </param>
        /// <param name="modelList"> 供應商聯絡人 List </param>
        /// <param name="userID">目前登入者</param>
        /// <param name="cDate">目前時間</param>
        internal void WriteTET_SupplierContact(PlatformContextModel context, Guid supplierID, List<TET_SupplierContactModel> modelList, string userID, DateTime cDate)
        {
            if (modelList == null)
                throw new ArgumentNullException("Contact is required.");

            // 新增前，先檢查是否能通過商業邏輯
            foreach (var model in modelList)
            {
                if (!SupplierContactValidator.ValidCreate(model, out List<string> msgList))
                    throw new ArgumentException(string.Join(Environment.NewLine, msgList));
            }

            // 先刪除，然後才新增
            var currentList =
                (from item in context.TET_SupplierContact
                 where item.SupplierID == supplierID
                 select item).ToList();

            context.TET_SupplierContact.RemoveRange(currentList);


            foreach (var model in modelList)
            {
                var entity = new TET_SupplierContact()
                {
                    ID = Guid.NewGuid(),
                    SupplierID = supplierID,
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

                context.TET_SupplierContact.Add(entity);
            }

        }

        /// <summary> 複製 供應商聯絡人 </summary>
        /// <param name="context"> ORM 核心 </param>
        /// <param name="supplierID"> 供應商 ID </param>
        /// <param name="modelList"> 供應商聯絡人 List </param>
        /// <param name="userID">目前登入者</param>
        /// <param name="cDate">目前時間</param>
        internal void CopyTET_SupplierContact(PlatformContextModel context, Guid supplierID, List<TET_SupplierContactModel> modelList, string userID, DateTime cDate)
        {
            foreach (var model in modelList)
            {
                var entity = new TET_SupplierContact()
                {
                    ID = Guid.NewGuid(),
                    SupplierID = supplierID,
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

                context.TET_SupplierContact.Add(entity);
            }
        }


        /// <summary> 刪除 供應商裡所有聯絡人 </summary>
        /// <param name="context"> ORM 核心 </param>
        /// <param name="supplierID"> 供應商 ID </param>
        /// <param name="userID">目前登入者</param>
        /// <param name="cDate">目前時間</param>
        internal void DeleteTET_SupplierContact(PlatformContextModel context, Guid supplierID, string userID, DateTime cDate)
        {
            // 先刪除，然後才新增
            var currentList =
                (from item in context.TET_SupplierContact
                 where item.SupplierID == supplierID
                 select item).ToList();

            context.TET_SupplierContact.RemoveRange(currentList);
        }
        #endregion
    }
}
