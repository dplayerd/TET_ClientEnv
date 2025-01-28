using BI.Shared;
using BI.Shared.Models;
using BI.SPA_ApproverSetup.Enums;
using BI.SPA_ApproverSetup.Models;
using BI.SPA_ApproverSetup.Validators;
using Platform.AbstractionClass;
using Platform.Auth;
using Platform.Infra;
using Platform.LogService;
using Platform.ORM;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BI.SPA_ApproverSetup
{
    public class SPA_ScoringRatioManager
    {
        private const string _paraKey_AssessmentItems = "SPA評鑑項目";

        private Logger _logger = new Logger();
        private UserManager _userMgr = new UserManager();
        private TET_ParametersManager _paramMgr = new TET_ParametersManager();


        #region Read
        /// <summary> 取得 供應商SPA評鑑審核者 清單 </summary>
        /// <param name="userID">目前登入者</param>
        /// <param name="cDate">目前時間</param>
        /// <param name="pager"></param>
        /// <returns></returns>
        public List<SPA_ScoringRatioModel> GetList(string userID, DateTime cDate, Pager pager)
        {
            try
            {
                using (PlatformContextModel context = new PlatformContextModel())
                {
                    var baseQuery =
                        from item in context.TET_SPA_ScoringRatio
                        from item1 in context.TET_Parameters
                        where item.ServiceItemID.ToString().ToUpper() == item1.ID.ToString().ToUpper()
                        orderby item1.Item, item.POSource
                        select item;


                    var query = this.ConvertToModel(baseQuery);
                    var list = query.ToList();

                    var paramList = this._paramMgr.GetTET_ParametersList(SPA_ScoringRatioManager._paraKey_AssessmentItems);
                    var completedList = this.BuildCompletedList(list, paramList, userID, cDate);

                    return completedList;
                }
            }
            catch (Exception ex)
            {
                this._logger.WriteError(ex);
                return default;
            }
        }

        private IQueryable<SPA_ScoringRatioModel> ConvertToModel(IQueryable<TET_SPA_ScoringRatio> query)
        {
            var result =
                from item in query
                select new SPA_ScoringRatioModel
                {
                    ID = item.ID,
                    ServiceItemID = item.ServiceItemID,
                    POSource = item.POSource,
                    TRatio1 = item.TRatio1,
                    TRatio2 = item.TRatio2,
                    DRatio1 = item.DRatio1,
                    DRatio2 = item.DRatio2,
                    QRatio1 = item.QRatio1,
                    QRatio2 = item.QRatio2,
                    CRatio1 = item.CRatio1,
                    CRatio2 = item.CRatio2,
                    SRatio1 = item.SRatio1,
                    SRatio2 = item.SRatio2,
                    CreateUser = item.CreateUser,
                    CreateDate = item.CreateDate,
                    ModifyUser = item.ModifyUser,
                    ModifyDate = item.ModifyDate,
                };

            return result;
        }


        ///// <summary> 查詢 供應商SPA評鑑審核者 資料 </summary>
        ///// <param name="ID"> 供應商SPA評鑑審核者 代碼 </param>
        ///// <returns></returns>
        //public SPA_ScoringRatioModel GetDetail(Guid ID)
        //{
        //    try
        //    {
        //        using (PlatformContextModel context = new PlatformContextModel())
        //        {
        //            var query =
        //                from item in context.TET_SPA_ScoringRatio
        //                where item.ID == ID
        //                select new SPA_ScoringRatioModel
        //                {
        //                    ID = item.ID,
        //                    ServiceItemID = item.ServiceItemID,
        //                    BUID = item.BUID,
        //                    InfoFill = item.InfoFill,
        //                    InfoConfirm = item.InfoConfirm,
        //                    Lv1Apprvoer = item.Lv1Apprvoer,
        //                    Lv2Apprvoer = item.Lv2Apprvoer,
        //                    CreateUser = item.CreateUser,
        //                    CreateDate = item.CreateDate,
        //                    ModifyUser = item.ModifyUser,
        //                    ModifyDate = item.ModifyDate,
        //                };

        //            var result = query.FirstOrDefault();
        //            return result;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        this._logger.WriteError(ex);
        //        throw;
        //    }
        //}
        #endregion


        #region CUD
        ///// <summary> 新增 供應商SPA評鑑審核者 </summary>
        ///// <param name="model"></param>
        ///// <param name="userID">目前登入者</param>
        ///// <param name="cDate">目前時間</param>
        //public void Create(List<SPA_ScoringRatioModel> model, string userID, DateTime cDate)
        //{
        //    if (model == null)
        //        throw new ArgumentNullException("Model is required.");

        //    // 新增前，先檢查是否能通過商業邏輯
        //    if (!SPA_ApproverSetupValidator.Valid(model, out List<string> msgList))
        //        throw new ArgumentException(string.Join(Environment.NewLine, msgList));

        //    try
        //    {
        //        using (PlatformContextModel context = new PlatformContextModel())
        //        {
        //            var dbModel =
        //                (from item in context.TET_SPA_ScoringRatio
        //                 where item.ServiceItemID == model.ServiceItemID && item.BUID == model.BUID
        //                 select item).FirstOrDefault();

        //            if (dbModel != null)
        //            {
        //                string serviceItemText = this._paramMgr.GetItem(model.ServiceItemID) ?? model.ServiceItemID.ToString();
        //                string buText = this._paramMgr.GetItem(model.BUID) ?? model.BUID.ToString();

        //                throw new Exception($"{serviceItemText}, {buText} 為重覆資料.");
        //            }


        //            var entity = new TET_SPA_ApproverSetup()
        //            {
        //                ID = Guid.NewGuid(),
        //                ServiceItemID = model.ServiceItemID,
        //                BUID = model.BUID,
        //                InfoFill = model.InfoFill,
        //                InfoConfirm = model.InfoConfirm,
        //                Lv1Apprvoer = model.Lv1Apprvoer,
        //                Lv2Apprvoer = model.Lv2Apprvoer,

        //                CreateUser = userID,
        //                CreateDate = cDate,
        //                ModifyUser = userID,
        //                ModifyDate = cDate,
        //            };

        //            context.TET_SPA_ScoringRatio.Add(entity);
        //            context.SaveChanges();
        //            return model;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        this._logger.WriteError(ex);
        //        throw;
        //    }
        //}

        /// <summary> 修改 供應商SPA評鑑審核者 </summary>
        /// <param name="model"></param>
        /// <param name="userID">目前登入者</param>
        public void Modify(List<SPA_ScoringRatioModel> inpList, string userID, DateTime cDate)
        {
            if (inpList == null)
                throw new ArgumentNullException("Model is required.");

            // SPA 評鑑項目
            var paramList = this._paramMgr.GetTET_ParametersList(SPA_ScoringRatioManager._paraKey_AssessmentItems);
            foreach (var item in inpList)
            {
                item.ServiceItem = paramList.Where(obj => obj.ID == item.ServiceItemID).FirstOrDefault()?.Item;
            }

            // 修改前，先檢查是否能通過商業邏輯
            if (!SPA_ScoringRatioValidator.Valid(inpList, out List<string> msgList))
                throw new ArgumentException(string.Join(Environment.NewLine, msgList));

            try
            {
                using (PlatformContextModel context = new PlatformContextModel())
                {
                    var dbEntityList = context.TET_SPA_ScoringRatio.ToList();

                    var completedList = this.BuildCompletedList(inpList, paramList, userID, cDate);

                    // 全部跑一次，如果資料不存在於資料庫中就新增，如果存在就是更新
                    foreach (var item in completedList)
                    {
                        var dbEntity = dbEntityList.Where(obj => obj.ServiceItemID == item.ServiceItemID && obj.POSource == item.POSource).FirstOrDefault();
                        if (dbEntity == null)
                        {
                            dbEntity = new TET_SPA_ScoringRatio()
                            {
                                ID = Guid.NewGuid(),

                                ServiceItemID = item.ServiceItemID,
                                POSource = item.POSource,

                                CreateUser = userID,
                                CreateDate = cDate,
                                ModifyUser = userID,
                                ModifyDate = cDate,
                            };

                            context.TET_SPA_ScoringRatio.Add(dbEntity);
                        }

                        dbEntity.TRatio1 = item.TRatio1;
                        dbEntity.TRatio2 = item.TRatio2;
                        dbEntity.DRatio1 = item.DRatio1;
                        dbEntity.DRatio2 = item.DRatio2;
                        dbEntity.QRatio1 = item.QRatio1;
                        dbEntity.QRatio2 = item.QRatio2;
                        dbEntity.CRatio1 = item.CRatio1;
                        dbEntity.CRatio2 = item.CRatio2;
                        dbEntity.SRatio1 = item.SRatio1;
                        dbEntity.SRatio2 = item.SRatio2;
                    }

                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                this._logger.WriteError(ex);
                throw;
            }
        }

        /// <summary> 產生完整的列表 </summary>
        /// <param name="sourceList"></param>
        /// <param name="parametersList"></param>
        /// <param name="cUserId"></param>
        /// <param name="cDate"></param>
        /// <returns></returns>
        private List<SPA_ScoringRatioModel> BuildCompletedList(List<SPA_ScoringRatioModel> sourceList, List<TET_ParametersModel> parametersList, string cUserId, DateTime cDate)
        {
            var newList = sourceList.ToList();

            foreach (var item in parametersList)
            {
                string txtLocal = POSource.Local.ToString();
                string txtFactory = POSource.Factory.ToString();

                var dbEntity_Local = sourceList.Where(obj => obj.ServiceItemID == item.ID && obj.POSource == txtLocal).FirstOrDefault();
                var dbEntity_Factory = sourceList.Where(obj => obj.ServiceItemID == item.ID && obj.POSource == txtFactory).FirstOrDefault();

                if (dbEntity_Local == null)
                {
                    dbEntity_Local = new SPA_ScoringRatioModel()
                    {
                        ID = Guid.Empty,

                        ServiceItemID = item.ID,
                        POSource = txtLocal,

                        TRatio1 = 0,
                        TRatio2 = 0,
                        DRatio1 = 0,
                        DRatio2 = 0,
                        QRatio1 = 0,
                        QRatio2 = 0,
                        CRatio1 = 0,
                        CRatio2 = 0,
                        SRatio1 = 0,
                        SRatio2 = 0,

                        CreateUser = cUserId,
                        CreateDate = cDate,
                        ModifyUser = cUserId,
                        ModifyDate = cDate,
                    };

                    dbEntity_Local.ServiceItem = item.Item;
                    newList.Add(dbEntity_Local);
                }
                else
                {
                    dbEntity_Local.ServiceItem = parametersList.Where(obj => obj.ID == item.ID).FirstOrDefault()?.Item;
                }

                if (dbEntity_Factory == null)
                {
                    dbEntity_Factory = new SPA_ScoringRatioModel()
                    {
                        ID = Guid.Empty,

                        ServiceItemID = item.ID,
                        POSource = txtFactory,

                        TRatio1 = 0,
                        TRatio2 = 0,
                        DRatio1 = 0,
                        DRatio2 = 0,
                        QRatio1 = 0,
                        QRatio2 = 0,
                        CRatio1 = 0,
                        CRatio2 = 0,
                        SRatio1 = 0,
                        SRatio2 = 0,

                        CreateUser = cUserId,
                        CreateDate = cDate,
                        ModifyUser = cUserId,
                        ModifyDate = cDate,
                    };

                    dbEntity_Factory.ServiceItem = item.Item;
                    newList.Add(dbEntity_Factory);
                }
                else
                {
                    dbEntity_Factory.ServiceItem = parametersList.Where(obj => obj.ID == item.ID).FirstOrDefault()?.Item;
                }

            }

            return newList;
        }
        #endregion
    }
}
