using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Platform.AbstractionClass;
using Platform.Infra;
using Platform.LogService;
using Platform.ORM;
using BI.STQA.Models;
using BI.STQA.Enums;
using BI.STQA.Utils;
using Platform.Auth;
using BI.STQA.Validators;

namespace BI.STQA
{
    public class TET_STQAManager
    {
        private Logger _logger = new Logger();

        //private TET_SupplierContactManager _contactMgr = new TET_SupplierContactManager();
        //private TET_SupplierAttachmentManager _attachmentMgr = new TET_SupplierAttachmentManager();
        private UserManager _userMgr = new UserManager();
        private UserRoleManager _userRoleMgr = new UserRoleManager();

        #region Read
        /// <summary>
        /// 取得 供應商 STQA 清單
        /// </summary>
        /// <param name="belongTo">供應商</param>
        /// <param name="businessTerm">業務類別</param>
        /// <param name="type">STQA 方式</param>
        /// <param name="dateStart">完成日期區間-起頭</param>
        /// <param name="dateEnd">完成日期區間-結束</param>
        /// <param name="userID"> 目前登入者 </param>
        /// <param name="cDate"> 目前時間 </param>
        /// <param name="pager"></param>
        /// <returns></returns>
        public List<TET_SupplierSTQAModel> GetSTQAList(string[] belongTo, string[] businessTerm, string[] type, DateTime? dateStart, DateTime? dateEnd, string userID, DateTime cDate, Pager pager)
        {
            try
            {
                using (PlatformContextModel context = new PlatformContextModel())
                {
                    string completedText = ApprovalStatus.Completed.ToText();

                    var orgQuery =
                        from item in context.TET_SupplierSTQA
                        from item1 in context.TET_Parameters
                        from item2 in context.TET_Parameters
                        from item3 in context.TET_Parameters
                        from item4 in context.TET_Parameters
                        from item5 in context.TET_Parameters
                        from item6 in context.TET_Parameters
                        where item.ApproveStatus == completedText &&
                              item.Purpose == item1.ID.ToString() &&
                              item.BusinessTerm == item2.ID.ToString() &&
                              item.UnitALevel == item3.ID.ToString() &&
                              item.UnitCLevel == item4.ID.ToString() &&
                              item.UnitDLevel == item5.ID.ToString() &&
                              item.Type==item6.ID.ToString()
                        select new
                        {
                            item.ID,
                            item.BelongTo,
                            Purpose = item1.Item,
                            BusinessTermID = item2.ID,
                            BusinessTerm = item2.Item,
                            item.Date,
                            TypeID = item6.ID,
                            Type =item6.Item,
                            UnitALevel = item3.Item,
                            UnitCLevel = item4.Item,
                            UnitDLevel = item5.Item,
                            item.Comment,
                            item.ApproveStatus,
                            item.CreateUser,
                            item.CreateDate,
                            item.ModifyUser,
                            item.ModifyDate
                        };

                    orgQuery =
                        orgQuery.Union(
                            from item in context.TET_SupplierSTQA
                            from item1 in context.TET_Parameters
                            from item2 in context.TET_Parameters
                            from item3 in context.TET_Parameters
                            from item4 in context.TET_Parameters
                            from item5 in context.TET_Parameters
                            from item6 in context.TET_Parameters
                            where
                                item.ApproveStatus != completedText &&
                                (item.CreateUser == userID || item.ModifyUser == userID) &&
                                item.Purpose == item1.ID.ToString() &&
                                item.BusinessTerm == item2.ID.ToString() &&
                                item.UnitALevel == item3.ID.ToString() &&
                                item.UnitCLevel == item4.ID.ToString() &&
                                item.UnitDLevel == item5.ID.ToString() &&
                                item.Type == item6.ID.ToString()
                            select new
                            {
                                item.ID,
                                item.BelongTo,
                                Purpose = item1.Item,
                                BusinessTermID = item2.ID,
                                BusinessTerm = item2.Item,
                                item.Date,
                                TypeID = item6.ID,
                                Type = item6.Item,
                                UnitALevel = item3.Item,
                                UnitCLevel = item4.Item,
                                UnitDLevel = item5.Item,
                                item.Comment,
                                item.ApproveStatus,
                                item.CreateUser,
                                item.CreateDate,
                                item.ModifyUser,
                                item.ModifyDate
                            });


                    //--- 組合過濾條件 ---
                    if (belongTo != null && belongTo.Length > 0)
                        orgQuery = orgQuery.Where(obj => belongTo.Contains(obj.BelongTo));

                    if (type != null && type.Length > 0)
                        orgQuery = orgQuery.Where(obj => type.Contains(obj.TypeID.ToString()));

                    if (businessTerm != null && businessTerm.Length > 0)
                        orgQuery = orgQuery.Where(obj => businessTerm.Contains(obj.BusinessTermID.ToString()));

                    if (dateStart.HasValue && dateEnd.HasValue)
                        orgQuery = orgQuery.Where(obj => dateEnd.Value >= obj.Date && dateStart.Value <= obj.Date);
                    else if (dateStart.HasValue)
                        orgQuery = orgQuery.Where(obj => dateStart.Value <= obj.Date);
                    else if (dateEnd.HasValue)
                        orgQuery = orgQuery.Where(obj => dateEnd.Value >= obj.Date);
                    //--- 組合過濾條件 ---


                    var query =
                        from item in orgQuery
                        select
                            new TET_SupplierSTQAModel()
                            {
                                ID = item.ID,
                                BelongTo = item.BelongTo,
                                Purpose = item.Purpose.ToUpper(),
                                BusinessTerm = item.BusinessTerm.ToUpper(),
                                Date = item.Date,
                                Type = item.Type,
                                UnitALevel = item.UnitALevel.ToUpper(),
                                UnitCLevel = item.UnitCLevel.ToUpper(),
                                UnitDLevel = item.UnitDLevel.ToUpper(),
                                STQAComment = item.Comment,
                                ApproveStatus = item.ApproveStatus,
                                CreateUser = item.CreateUser,
                                CreateDate = item.CreateDate,
                                ModifyUser = item.ModifyUser,
                                ModifyDate = item.ModifyDate,
                            };

                    query = query.OrderByDescending(obj => obj.CreateDate);
                    var list = query.ProcessPager(pager).ToList();
                    return list;
                }
            }
            catch (Exception ex)
            {
                this._logger.WriteError(ex);
                return default;
            }
        }

        /// <summary> 查詢指定供應商的 STQA 清單 </summary>
        /// <param name="belongTos"></param>
        /// <returns></returns>
        public List<TET_SupplierSTQAModel> GetSTQAList(IEnumerable<string> belongTos)
        {
            try
            {
                using (PlatformContextModel context = new PlatformContextModel())
                {
                    var query =
                        from item in context.TET_SupplierSTQA
                        from item1 in context.TET_Parameters
                        from item2 in context.TET_Parameters
                        from item3 in context.TET_Parameters
                        from item4 in context.TET_Parameters
                        from item5 in context.TET_Parameters
                        from item6 in context.TET_Parameters
                        where belongTos.Contains(item.BelongTo) &&
                              item.Purpose == item1.ID.ToString() &&
                              item.BusinessTerm == item2.ID.ToString() &&
                              item.UnitALevel == item3.ID.ToString() &&
                              item.UnitCLevel == item4.ID.ToString() &&
                              item.UnitDLevel == item5.ID.ToString() &&
                              item.Type == item6.ID.ToString()
                        orderby item.CreateDate 
                        select
                            new TET_SupplierSTQAModel()
                            {
                                ID = item.ID,
                                BelongTo = item.BelongTo,
                                Purpose = item1.Item,
                                BusinessTerm = item2.Item,
                                Date = item.Date,
                                Type = item6.Item,
                                UnitALevel = item3.Item,
                                UnitCLevel = item4.Item,
                                UnitDLevel = item5.Item,
                                STQAComment = item.Comment,
                                ApproveStatus = item.ApproveStatus,
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
                return default;
            }
        }

        /// <summary> 查詢供應商 STQA </summary>
        /// <param name="ID"> 供應商 ID </param>
        /// <param name="includeApprovalList"> 是否要包含審核歷程 </param>
        /// <returns></returns>
        public TET_SupplierSTQAModel GetSTQA(Guid ID, bool includeApprovalList = false)
        {
            try
            {
                using (PlatformContextModel context = new PlatformContextModel())
                {
                    var result =
                    (from item in context.TET_SupplierSTQA
                     where
                        item.ID == ID
                     select
                     new TET_SupplierSTQAModel()
                     {
                         ID = item.ID,
                         BelongTo = item.BelongTo,
                         Purpose = item.Purpose,
                         BusinessTerm = item.BusinessTerm,
                         Date = item.Date,
                         Type = item.Type,
                         UnitALevel = item.UnitALevel,
                         UnitCLevel = item.UnitCLevel,
                         UnitDLevel = item.UnitDLevel,
                         STQAComment = item.Comment,
                         ApproveStatus = item.ApproveStatus,
                         CreateUser = item.CreateUser,
                         CreateDate = item.CreateDate,
                         ModifyUser = item.ModifyUser,
                         ModifyDate = item.ModifyDate,
                     }
                    ).FirstOrDefault();

                    if (includeApprovalList)
                    {
                        var approvalQuery =
                            from item in context.TET_SupplierSTQAApproval
                            where
                                item.STQAID == ID &&
                                item.Result != null
                            orderby item.CreateDate
                            select
                                new TET_SupplierSTQAApprovalModel()
                                {
                                    ID = item.ID,
                                    STQAID = item.STQAID,
                                    Type = item.Type,
                                    Description = item.Description,
                                    Level = item.Level,
                                    Approver = item.Approver,
                                    Result = item.Result,
                                    Comment = item.Comment,
                                    CreateUser = item.CreateUser,
                                    CreateDate = item.CreateDate,
                                    ModifyUser = item.ModifyUser,
                                    ModifyDate = item.ModifyDate,
                                };

                        result.ApprovalList = approvalQuery.ToList();
                    }

                    return result;
                }
            }
            catch (Exception ex)
            {
                this._logger.WriteError(ex);
                throw;
            }
        }
        #endregion

        #region OtherRead
        /// <summary> 是否能開啟 STQA 資料 </summary>
        /// <param name="ID"> STQA 代碼 </param>
        /// <param name="cUser"> 目前登入者 </param>
        /// <returns></returns>
        public bool CanRead(Guid ID, string cUser)
        {
            try
            {
                using (PlatformContextModel context = new PlatformContextModel())
                {
                    var supplier = this.GetSTQA(ID);

                    if (supplier == null)
                        return false;

                    // 檢查是否為建立或修改者
                    var creatorResult = false;
                    if (string.Compare(cUser, supplier.CreateUser, true) == 0 ||
                       string.Compare(cUser, supplier.ModifyUser, true) == 0)
                        creatorResult = true;


                    // 只有審核中的需要檢查
                    bool approvalResult = true;
                    if (string.Compare(ApprovalStatus.Verify.ToText(), supplier.ApproveStatus, true) == 0)
                    {
                        var approvalQuery =
                            from item in context.TET_SupplierSTQAApproval
                            where
                                item.STQAID == ID && item.Approver == cUser
                            select item.ID;
                        approvalResult = approvalQuery.Any();
                    }

                    var result = creatorResult || approvalResult;
                    return result;
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
        /// <summary> 新增 供應商 STQA </summary>
        /// <param name="model"></param>
        /// <param name="userID">目前登入者</param>
        /// <param name="cDate">目前時間</param>
        public Guid CreateSTQA(TET_SupplierSTQAModel model, string userID, DateTime cDate)
        {
            if (model == null)
                throw new ArgumentNullException("Supplier is required.");

            // 新增前，先檢查是否能通過商業邏輯
            if (!SupplierSTQAValidator.Valid(model, out List<string> msgList))
                throw new ArgumentException(string.Join(Environment.NewLine, msgList));

            try
            {
                using (PlatformContextModel context = new PlatformContextModel())
                {
                    var entity = new TET_SupplierSTQA()
                    {
                        ID = Guid.NewGuid(),
                        BelongTo = model.BelongTo,
                        Purpose = model.Purpose,
                        BusinessTerm = model.BusinessTerm,
                        Date = model.Date.Value,
                        Type = model.Type,
                        UnitALevel = model.UnitALevel,
                        UnitCLevel = model.UnitCLevel,
                        UnitDLevel = model.UnitDLevel,
                        Comment = model.STQAComment,
                        ApproveStatus = model.ApproveStatus,

                        CreateUser = userID,
                        CreateDate = cDate,
                        ModifyUser = userID,
                        ModifyDate = cDate,
                    };

                    context.TET_SupplierSTQA.Add(entity);
                    context.SaveChanges();
                    return entity.ID;
                }
            }
            catch (Exception ex)
            {
                this._logger.WriteError(ex);
                throw;
            }
        }

        /// <summary> 修改 供應商 STQA </summary>
        /// <param name="model"></param>
        /// <param name="userID">目前登入者</param>
        public void ModifySTQA(TET_SupplierSTQAModel model, string userID, DateTime cDate)
        {
            if (model == null)
                throw new ArgumentNullException("Model is required.");

            // 修改前，先檢查是否能通過商業邏輯
            if (!SupplierSTQAValidator.Valid(model, out List<string> msgList))
                throw new ArgumentException(string.Join(Environment.NewLine, msgList));

            try
            {
                using (PlatformContextModel context = new PlatformContextModel())
                {
                    var dbModel =
                    (from item in context.TET_SupplierSTQA
                     where item.ID == model.ID
                     select item).FirstOrDefault();

                    if (dbModel == null)
                        throw new NullReferenceException($"{model.ID} don't exists.");

                    dbModel.BelongTo = model.BelongTo;
                    dbModel.Purpose = model.Purpose;
                    dbModel.BusinessTerm = model.BusinessTerm;
                    dbModel.Date = model.Date.Value;
                    dbModel.Type = model.Type;
                    dbModel.UnitALevel = model.UnitALevel;
                    dbModel.UnitCLevel = model.UnitCLevel;
                    dbModel.UnitDLevel = model.UnitDLevel;
                    dbModel.Comment = model.STQAComment;
                    dbModel.ApproveStatus = model.ApproveStatus;
                    dbModel.ModifyUser = userID;
                    dbModel.ModifyDate = cDate;

                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                this._logger.WriteError(ex);
                throw;
            }
        }

        /// <summary> 改版 供應商 STQA </summary>
        /// <param name="id">主鍵</param>
        /// <param name="userID">目前登入者</param>
        /// <param name="cDate">目前時間</param>
        public void Revision(Guid id, string userID, DateTime cDate)
        {
            if (id == Guid.Empty || string.IsNullOrWhiteSpace(userID))
                throw new ArgumentNullException("ID, UserID is required");

            try
            {
                using (PlatformContextModel context = new PlatformContextModel())
                {
                    var dbModel =
                    (from item in context.TET_SupplierSTQA
                     where item.ID == id
                     select item).FirstOrDefault();

                    // 資料如果不存在，就視為已刪除
                    if (dbModel == null)
                        throw new NullReferenceException($"{id} don't exists.");

                    dbModel.ApproveStatus = null;
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                this._logger.WriteError(ex);
                throw;
            }
        }
        #endregion

        #region Applicant
        /// <summary> 送出申請 </summary>
        /// <param name="model"></param>
        /// <param name="userID">目前登入者</param>
        /// <param name="cDate">目前時間</param>
        public void SubmitSTQA(TET_SupplierSTQAModel model, string userID, DateTime cDate)
        {
            if (model == null)
                throw new ArgumentNullException("Model is required.");

            var user = _userMgr.GetUser(userID);
            if (user == null)
                throw new Exception("User is required.");

            var leader = this._userMgr.GetUserLeader(userID);
            if (leader == null)
                throw new Exception($"{userID} 沒有主管資料");


            try
            {
                using (PlatformContextModel context = new PlatformContextModel())
                {
                    // 先查出原資料
                    var dbModel =
                    (from item in context.TET_SupplierSTQA
                     where item.ID == model.ID
                     select item).FirstOrDefault();

                    if (dbModel == null)
                        throw new NullReferenceException($"{model.ID} don't exists.");

                    //--- 調整原資料的審核狀態 ---
                    // 如果審核狀態為 NULL 或是已退回，才允許送出申請
                    if (!string.IsNullOrWhiteSpace(dbModel.ApproveStatus) &&
                        ApprovalUtils.ParseApprovalStatus(dbModel.ApproveStatus) != ApprovalStatus.Rejected)
                        throw new Exception("送審狀態必須為空，或是已退回.");

                    // 更改原資料的審核狀態
                    dbModel.ApproveStatus = ApprovalStatus.Verify.ToText();
                    //--- 調整原資料的審核狀態 ---


                    //--- 新增審核資料 ---
                    // 建立新的申請資訊
                    var entity = new TET_SupplierSTQAApproval()
                    {
                        ID = Guid.NewGuid(),
                        STQAID = dbModel.ID,
                        Type = ApprovalType.New.ToText(),
                        Level = ApprovalLevel.User_GL.ToText(),
                        Description = $"{ApprovalType.New.ToText()}_{dbModel.BelongTo}",
                        Approver = leader.ID,
                        CreateUser = userID,
                        CreateDate = cDate,
                        ModifyUser = userID,
                        ModifyDate = cDate,
                    };


                    // 寄送通知信
                    ApprovalMailUtil.SendNewApprovalMail(leader.EMail, entity, dbModel, userID, cDate);

                    context.TET_SupplierSTQAApproval.Add(entity);
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                this._logger.WriteError(ex);
                throw;
            }
        }

        /// <summary> 刪除 STQA </summary>
        /// <param name="id">主鍵</param>
        /// <param name="userID">目前登入者</param>
        /// <param name="cDate">目前時間</param>
        public void DeleteTET_STQA(Guid id, string userID, DateTime cDate)
        {
            if (id == Guid.Empty || string.IsNullOrWhiteSpace(userID))
                throw new ArgumentNullException("ID, UserID is required");

            try
            {
                using (PlatformContextModel context = new PlatformContextModel())
                {
                    var dbModel =
                        (from item in context.TET_SupplierSTQA
                         where item.ID == id
                         select item).FirstOrDefault();

                    // 資料如果不存在，就視為已刪除
                    if (dbModel == null)
                        return;

                    context.TET_SupplierSTQA.Remove(dbModel);
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                this._logger.WriteError(ex);
                throw;
            }
        }

        /// <summary> 中止 供應商 審核</summary>
        /// <param name="id">主鍵</param>
        /// <param name="userID">目前登入者</param>
        /// <param name="cDate">目前時間</param>
        public void AbordTET_STQA(Guid id, string userID, DateTime cDate)
        {
            if (id == Guid.Empty || string.IsNullOrWhiteSpace(userID))
                throw new ArgumentNullException("ID, UserID is required");

            try
            {
                using (PlatformContextModel context = new PlatformContextModel())
                {
                    var dbModel =
                        (from item in context.TET_SupplierSTQA
                         where item.ID == id
                         select item).FirstOrDefault();

                    // 資料如果不存在
                    if (dbModel == null)
                        return;

                    // 只有審核中的能中止
                    if (ApprovalUtils.ParseApprovalStatus(dbModel.ApproveStatus) != ApprovalStatus.Verify)
                        return;

                    // 改為未送出的狀態
                    dbModel.ApproveStatus = null;

                    // 審核資料
                    var approvalList =
                        (from item in context.TET_SupplierSTQAApproval
                         where item.STQAID == id
                         select item).ToList();

                    // 刪除尚未審核的審核資料
                    var nonResultApprovalList = approvalList.Where(item => string.IsNullOrEmpty(item.Result)).ToList();
                    context.TET_SupplierSTQAApproval.RemoveRange(nonResultApprovalList);
                    context.SaveChanges();

                    // 寄送通知信
                    var receivers = approvalList.Select(obj => obj.Approver).ToList();
                    var userList = this._userMgr.GetUserList(receivers);
                    var mailList = userList.Select(obj => obj.EMail).ToList();


                    ApprovalMailUtil.SendAbordMail(mailList, dbModel, userID, cDate);
                }
            }
            catch (Exception ex)
            {
                this._logger.WriteError(ex);
                throw;
            }
        }
        #endregion
    }
}


