using Platform.AbstractionClass;
using Platform.LogService;
using Platform.ORM;
using Platform.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using BI.AllApproval.Models;

namespace BI.AllApproval
{
    public class AllApprovalManager
    {
        private Logger _logger = new Logger();

        #region Read
        /// <summary>
        /// 取得 供應商審核資料 清單
        /// </summary>
        /// <param name="userID">目前登入者</param>
        /// <param name="cDate">目前時間</param>
        /// <param name="pager"></param>
        /// <returns></returns>
        public List<ApprovalModel> GetApprovalList(string userID, DateTime cDate, Pager pager)
        {
            try
            {
                using (PlatformContextModel context = new PlatformContextModel())
                {
                    var query =
                    from item in context.vwApprovalList
                    where
                        item.Approver == userID &&
                        item.Result == null
                    orderby
                        item.CreateDate descending
                    select
                        new ApprovalModel()
                        {
                            ID = item.ID,
                            ParentID = item.ParentID,
                            ParentType = item.ParentType,
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

        public object GetApproverList(object approver, DateTime cDate, Pager pager)
        {
            throw new NotImplementedException();
        }

        public List<ApprovalModel> GetApproverChangeList(string approver, DateTime cDate, Pager pager)
        {
            try
            {
                using (PlatformContextModel context = new PlatformContextModel())
                {
                    if (approver != string.Empty && approver!=null)
                    {
                        var query =
                        from item in context.vwApprovalList
                        where
                            item.Result == null &&
                            item.Approver == approver
                        orderby
                            item.CreateDate descending
                        select
                            new ApprovalModel()
                            {
                                ID = item.ID,
                                ParentID = item.ParentID,
                                ParentType = item.ParentType,
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

                        var list = query.ProcessPager(pager).ToList();
                        return list;
                    }
                    else
                    {
                        var query =
                        from item in context.vwApprovalList
                        where
                            item.Result == null
                        orderby
                            item.CreateDate descending
                        select
                            new ApprovalModel()
                            {
                                ID = item.ID,
                                ParentID = item.ParentID,
                                ParentType = item.ParentType,
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

                        var list = query.ProcessPager(pager).ToList();
                        return list;
                    }
                }
            }
            catch (Exception ex)
            {
                this._logger.WriteError(ex);
                return default;
            }
        }
        #endregion
    }
}
