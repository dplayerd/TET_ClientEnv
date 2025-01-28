using System;
using System.Collections.Generic;
using System.Linq;
using BI.Shared;
using BI.SPA_ApproverSetup.Models;
using BI.SPA_ApproverSetup.Validators;
using Platform.AbstractionClass;
using Platform.Auth;
using Platform.Infra;
using Platform.LogService;
using Platform.ORM;

namespace BI.SPA_ApproverSetup
{
    public class SPA_ApproverSetupManager
    {
        private Logger _logger = new Logger();
        private UserRoleManager _roleMgr = new UserRoleManager();
        private TET_ParametersManager _paramMgr = new TET_ParametersManager();

        #region Read
        /// <summary> 取得 供應商SPA評鑑審核者 清單 </summary>
        /// <param name="serviceItemIDs">評鑑項目</param>
        /// <param name="buIDs">評鑑單位</param>
        /// <param name="userID">目前登入者</param>
        /// <param name="cDate">目前時間</param>
        /// <param name="pager"></param>
        /// <returns></returns>
        public List<TET_SPA_ApproverSetupModel> GetList(List<Guid> serviceItemIDs, List<Guid> buIDs, string userID, DateTime cDate, Pager pager)
        {
            try
            {
                using (PlatformContextModel context = new PlatformContextModel())
                {
                    var baseQuery =
                        from item in context.TET_SPA_ApproverSetup
                        select item;

                    if (serviceItemIDs.Any())
                        baseQuery = baseQuery.Where(obj => serviceItemIDs.Contains(obj.ServiceItemID));

                    if (buIDs.Any())
                        baseQuery = baseQuery.Where(obj => buIDs.Contains(obj.BUID));

                    
                    var query =
                        from item in baseQuery
                        orderby item.CreateDate descending
                        select new TET_SPA_ApproverSetupModel
                        {
                            ID = item.ID,
                            ServiceItemID = item.ServiceItemID,
                            BUID = item.BUID,
                            InfoFill = item.InfoFill,
                            InfoConfirm = item.InfoConfirm,
                            Lv1Apprvoer = item.Lv1Apprvoer,
                            Lv2Apprvoer = item.Lv2Apprvoer,
                            CreateUser = item.CreateUser,
                            CreateDate = item.CreateDate,
                            ModifyUser = item.ModifyUser,
                            ModifyDate = item.ModifyDate,
                        };

                    var list = query.ProcessPager(pager).ToList();


                    // 調整 UI 顯示用欄位
                    var serviceItemList = this._paramMgr.GetTET_ParametersList("SPA評鑑項目");
                    var buList = this._paramMgr.GetTET_ParametersList("SPA評鑑單位");
                    foreach (var item in list)
                    {
                        item.ServiceItemText = serviceItemList.Where(obj => obj.ID == item.ServiceItemID).FirstOrDefault()?.Item;
                        item.BUText = buList.Where(obj => obj.ID == item.BUID).FirstOrDefault()?.Item;
                    }
                    
                    return list;
                }
            }
            catch (Exception ex)
            {
                this._logger.WriteError(ex);
                return default;
            }
        }

        /// <summary> 查詢 供應商SPA評鑑審核者 資料 </summary>
        /// <param name="ID"> 供應商SPA評鑑審核者 代碼 </param>
        /// <returns></returns>
        public TET_SPA_ApproverSetupModel GetDetail(Guid ID)
        {
            try
            {
                using (PlatformContextModel context = new PlatformContextModel())
                {
                    var query =
                        from item in context.TET_SPA_ApproverSetup
                        where item.ID == ID
                        select new TET_SPA_ApproverSetupModel
                        {
                            ID = item.ID,
                            ServiceItemID = item.ServiceItemID,
                            BUID = item.BUID,
                            InfoFill = item.InfoFill,
                            InfoConfirm = item.InfoConfirm,
                            Lv1Apprvoer = item.Lv1Apprvoer,
                            Lv2Apprvoer = item.Lv2Apprvoer,
                            CreateUser = item.CreateUser,
                            CreateDate = item.CreateDate,
                            ModifyUser = item.ModifyUser,
                            ModifyDate = item.ModifyDate,
                        };

                    var result = query.FirstOrDefault();



                    // 調整 UI 顯示用欄位
                    var serviceItemList = this._paramMgr.GetTET_ParametersList("SPA評鑑項目");
                    var buList = this._paramMgr.GetTET_ParametersList("SPA評鑑單位");
                    if(result != null)
                    {
                        result.ServiceItemText = serviceItemList.Where(obj => obj.ID == result.ServiceItemID).FirstOrDefault()?.Item;
                        result.BUText = buList.Where(obj => obj.ID == result.BUID).FirstOrDefault()?.Item;
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


        #region CUD
        /// <summary> 新增 供應商SPA評鑑審核者 </summary>
        /// <param name="model"></param>
        /// <param name="userID">目前登入者</param>
        /// <param name="cDate">目前時間</param>
        public TET_SPA_ApproverSetupModel Create(TET_SPA_ApproverSetupModel model, string userID, DateTime cDate)
        {
            if (model == null)
                throw new ArgumentNullException("Model is required.");

            // 新增前，先檢查是否能通過商業邏輯
            if (!SPA_ApproverSetupValidator.Valid(model, out List<string> msgList))
                throw new ArgumentException(string.Join(Environment.NewLine, msgList));

            try
            {
                using (PlatformContextModel context = new PlatformContextModel())
                {
                    var dbModel =
                        (from item in context.TET_SPA_ApproverSetup
                         where item.ServiceItemID == model.ServiceItemID && item.BUID == model.BUID
                         select item).FirstOrDefault();

                    if (dbModel != null)
                    {
                        string serviceItemText = this._paramMgr.GetItem(model.ServiceItemID) ?? model.ServiceItemID.ToString();
                        string buText = this._paramMgr.GetItem(model.BUID) ?? model.BUID.ToString();

                        throw new Exception($"此評鑑項目與評鑑單位的審核者設定資料已存在.");
                    }


                    var entity = new TET_SPA_ApproverSetup()
                    {
                        ID = Guid.NewGuid(),
                        ServiceItemID = model.ServiceItemID,
                        BUID = model.BUID,
                        InfoFill = model.InfoFill,
                        InfoConfirm = model.InfoConfirm,
                        Lv1Apprvoer = model.Lv1Apprvoer,
                        Lv2Apprvoer = model.Lv2Apprvoer,

                        CreateUser = userID,
                        CreateDate = cDate,
                        ModifyUser = userID,
                        ModifyDate = cDate,
                    };

                    context.TET_SPA_ApproverSetup.Add(entity);
                    context.SaveChanges();
                    return model;
                }
            }
            catch (Exception ex)
            {
                this._logger.WriteError(ex);
                throw;
            }
        }

        /// <summary> 修改 供應商SPA評鑑審核者 </summary>
        /// <param name="model"></param>
        /// <param name="userID">目前登入者</param>
        public void Modify(TET_SPA_ApproverSetupModel model, string userID, DateTime cDate)
        {
            if (model == null)
                throw new ArgumentNullException("Model is required.");

            // 修改前，先檢查是否能通過商業邏輯
            if (!SPA_ApproverSetupValidator.Valid(model, out List<string> msgList))
                throw new ArgumentException(string.Join(Environment.NewLine, msgList));

            try
            {
                using (PlatformContextModel context = new PlatformContextModel())
                {
                    var dbModel =
                        (from item in context.TET_SPA_ApproverSetup
                         where item.ID == model.ID
                         select item).FirstOrDefault();

                    if (dbModel == null)
                        throw new NullReferenceException($"{model.ID} doesn't exists.");

                    // 檢查重複資料
                    var dbModel_existed = context.TET_SPA_ApproverSetup.Where(item => item.ID != model.ID && item.ServiceItemID == model.ServiceItemID && item.BUID == model.BUID).FirstOrDefault();
                    if (dbModel_existed != null)
                        throw new Exception($"此評鑑項目與評鑑單位的審核者設定資料已存在.");

                    dbModel.ServiceItemID = model.ServiceItemID;
                    dbModel.BUID = model.BUID;
                    dbModel.InfoFill = model.InfoFill;
                    dbModel.InfoConfirm = model.InfoConfirm;
                    dbModel.Lv1Apprvoer = model.Lv1Apprvoer;
                    dbModel.Lv2Apprvoer = model.Lv2Apprvoer;

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
        #endregion
    }
}
