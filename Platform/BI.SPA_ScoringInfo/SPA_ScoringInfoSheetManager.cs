using BI.Shared;
using BI.Shared.Extensions;
using BI.Shared.Models;
using BI.SPA_ScoringInfo.Models;
using Platform.AbstractionClass;
using Platform.Infra;
using Platform.LogService;
using Platform.ORM;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BI.SPA_ScoringInfo
{
    public class SPA_ScoringInfoSheetManager
    {
        private const string _paraKey_AssessmentItems = "SPA評鑑項目";

        private Logger _logger = new Logger();
        private TET_ParametersManager _paramMgr = new TET_ParametersManager();

        #region Read
        public List<SPA_ScoringInfoSheetModel> GetList(List<Guid> serviceItemIDs, string[] poSource, string userID, DateTime cDate, Pager pager)
        {
            try
            {
                using (PlatformContextModel context = new PlatformContextModel())
                {
                    var enabledServiceItemIDs =
                        context.TET_Parameters
                            .Where(obj => obj.Type == _paraKey_AssessmentItems && obj.IsEnable)
                            .Select(obj => obj.ID);

                    var baseQuery =
                        from item in context.TET_SPA_ScoringInfoSheets
                        where enabledServiceItemIDs.Contains(item.ServiceItemID)
                        select item;

                    if (serviceItemIDs != null && serviceItemIDs.Any())
                        baseQuery = baseQuery.Where(obj => serviceItemIDs.Contains(obj.ServiceItemID));

                    if (poSource != null && poSource.Length > 0)
                        baseQuery = baseQuery.Where(obj => poSource.Contains(obj.POSource));

                    baseQuery = baseQuery.OrderBy(obj => obj.ServiceItemID).ThenBy(obj => obj.POSource);

                    var list = this.ConvertToModel(baseQuery).ProcessPager(pager).ToList();
                    var paramList = this._paramMgr.GetTET_ParametersList(_paraKey_AssessmentItems);
                    this.FillServiceItemText(list, paramList);

                    return list;
                }
            }
            catch (Exception ex)
            {
                this._logger.WriteError(ex);
                return default;
            }
        }

        public SPA_ScoringInfoSheetModel GetDetail(Guid serviceItemID, string poSource)
        {
            try
            {
                using (PlatformContextModel context = new PlatformContextModel())
                {
                    var query =
                        from item in context.TET_SPA_ScoringInfoSheets
                        where item.ServiceItemID == serviceItemID && item.POSource == poSource
                        select item;

                    var result = this.ConvertToModel(query).FirstOrDefault();
                    if (result != null)
                    {
                        var paramList = this._paramMgr.GetTET_ParametersList(_paraKey_AssessmentItems);
                        this.FillServiceItemText(new List<SPA_ScoringInfoSheetModel>() { result }, paramList);
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

        public SPA_ScoringInfoSheetModel GetDetail(string serviceItem, string poSource)
        {
            try
            {
                using (PlatformContextModel context = new PlatformContextModel())
                {
                    var query =
                        from item in context.TET_SPA_ScoringInfoSheets
                        join param in context.TET_Parameters on item.ServiceItemID equals param.ID
                        where
                            param.Type == _paraKey_AssessmentItems &&
                            param.IsEnable &&
                            param.Item == serviceItem &&
                            item.POSource == poSource
                        select item;

                    var result = this.ConvertToModel(query).FirstOrDefault();
                    if (result != null)
                        result.ServiceItem = serviceItem;

                    return result;
                }
            }
            catch (Exception ex)
            {
                this._logger.WriteError(ex);
                throw;
            }
        }

        private IQueryable<SPA_ScoringInfoSheetModel> ConvertToModel(IQueryable<TET_SPA_ScoringInfoSheets> query)
        {
            var result =
                from item in query
                select new SPA_ScoringInfoSheetModel
                {
                    ServiceItemID = item.ServiceItemID,
                    POSource = item.POSource,
                    IsSheet1Show = item.IsSheet1Show,
                    IsSheet1TypeFill = item.IsSheet1TypeFill,
                    IsSheet1SupplierFill = item.IsSheet1SupplierFill,
                    IsSheet1SourceFill = item.IsSheet1SourceFill,
                    IsSheet1EmpNameFill = item.IsSheet1EmpNameFill,
                    IsSheet1MajorJobFill = item.IsSheet1MajorJobFill,
                    IsSheet1IsIndependentFill = item.IsSheet1IsIndependentFill,
                    IsSheet1SkillLevelFill = item.IsSheet1SkillLevelFill,
                    IsSheet1EmpStatusFill = item.IsSheet1EmpStatusFill,
                    IsSheet1TELSeniorityYFill = item.IsSheet1TELSeniorityYFill,
                    IsSheet1TELSeniorityMFill = item.IsSheet1TELSeniorityMFill,
                    IsSheet1RemarkFill = item.IsSheet1RemarkFill,
                    IsSheet2Show = item.IsSheet2Show,
                    IsSheet2ServiceForFill = item.IsSheet2ServiceForFill,
                    IsSheet2WorkItemFill = item.IsSheet2WorkItemFill,
                    IsSheet2MachineNameFill = item.IsSheet2MachineNameFill,
                    IsSheet2MachineNoFill = item.IsSheet2MachineNoFill,
                    IsSheet2OnTimeFill = item.IsSheet2OnTimeFill,
                    IsSheet2RemarkFill = item.IsSheet2RemarkFill,
                    IsSheet3Show = item.IsSheet3Show,
                    IsSheet3WorkerCountFill = item.IsSheet3WorkerCountFill,
                    IsSheet3DateFill = item.IsSheet3DateFill,
                    IsSheet3LocationFill = item.IsSheet3LocationFill,
                    IsSheet3TELLossFill = item.IsSheet3TELLossFill,
                    IsSheet3CustomerLossFill = item.IsSheet3CustomerLossFill,
                    IsSheet3AccidentFill = item.IsSheet3AccidentFill,
                    IsSheet3DescriptionFill = item.IsSheet3DescriptionFill,
                    IsSheet4Show = item.IsSheet4Show,
                    IsSheet4CorrectnessFill = item.IsSheet4CorrectnessFill,
                    IsSheet4ContributionFill = item.IsSheet4ContributionFill,
                    IsSheet5Show = item.IsSheet5Show,
                    IsSheet5SelfTrainingFill = item.IsSheet5SelfTrainingFill,
                    IsSheet5SelfTrainingRemarkFill = item.IsSheet5SelfTrainingRemarkFill,
                    IsSheet6Show = item.IsSheet6Show,
                    IsSheet6CooperationFill = item.IsSheet6CooperationFill,
                    IsSheet6DateFill = item.IsSheet6DateFill,
                    IsSheet6LocationFill = item.IsSheet6LocationFill,
                    IsSheet6IsDamageFill = item.IsSheet6IsDamageFill,
                    IsSheet6DescriptionFill = item.IsSheet6DescriptionFill,
                    IsSheet7Show = item.IsSheet7Show,
                    CreateUser = item.CreateUser,
                    CreateDate = item.CreateDate,
                    ModifyUser = item.ModifyUser,
                    ModifyDate = item.ModifyDate,
                };

            return result;
        }
        #endregion

        #region CUD
        public void Create(SPA_ScoringInfoSheetModel model, string userID, DateTime cDate)
        {
            if (model == null)
                throw new ArgumentNullException("Model is required.");

            this.Valid(model);

            try
            {
                using (PlatformContextModel context = new PlatformContextModel())
                {
                    var dbEntity = context.TET_SPA_ScoringInfoSheets.Where(obj => obj.ServiceItemID == model.ServiceItemID && obj.POSource == model.POSource).FirstOrDefault();
                    if (dbEntity != null)
                        throw new Exception("此評鑑項目與 PO Source 的頁籤顯示設定資料已存在.");

                    dbEntity = new TET_SPA_ScoringInfoSheets()
                    {
                        ServiceItemID = model.ServiceItemID,
                        POSource = model.POSource,
                        CreateUser = userID,
                        CreateDate = cDate,
                    };

                    this.AssignEntity(dbEntity, model, userID, cDate);
                    context.TET_SPA_ScoringInfoSheets.Add(dbEntity);
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                this._logger.WriteError(ex);
                throw;
            }
        }

        public void Modify(SPA_ScoringInfoSheetModel model, string userID, DateTime cDate)
        {
            if (model == null)
                throw new ArgumentNullException("Model is required.");

            this.Valid(model);

            try
            {
                using (PlatformContextModel context = new PlatformContextModel())
                {
                    var dbEntity = context.TET_SPA_ScoringInfoSheets.Where(obj => obj.ServiceItemID == model.ServiceItemID && obj.POSource == model.POSource).FirstOrDefault();
                    if (dbEntity == null)
                    {
                        dbEntity = new TET_SPA_ScoringInfoSheets()
                        {
                            ServiceItemID = model.ServiceItemID,
                            POSource = model.POSource,
                            CreateUser = userID,
                            CreateDate = cDate,
                        };

                        context.TET_SPA_ScoringInfoSheets.Add(dbEntity);
                    }

                    this.AssignEntity(dbEntity, model, userID, cDate);
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                this._logger.WriteError(ex);
                throw;
            }
        }

        private void AssignEntity(TET_SPA_ScoringInfoSheets dbEntity, SPA_ScoringInfoSheetModel item, string userID, DateTime cDate)
        {
            dbEntity.IsSheet1Show = item.IsSheet1Show;
            dbEntity.IsSheet1TypeFill = item.IsSheet1TypeFill;
            dbEntity.IsSheet1SupplierFill = item.IsSheet1SupplierFill;
            dbEntity.IsSheet1SourceFill = item.IsSheet1SourceFill;
            dbEntity.IsSheet1EmpNameFill = item.IsSheet1EmpNameFill;
            dbEntity.IsSheet1MajorJobFill = item.IsSheet1MajorJobFill;
            dbEntity.IsSheet1IsIndependentFill = item.IsSheet1IsIndependentFill;
            dbEntity.IsSheet1SkillLevelFill = item.IsSheet1SkillLevelFill;
            dbEntity.IsSheet1EmpStatusFill = item.IsSheet1EmpStatusFill;
            dbEntity.IsSheet1TELSeniorityYFill = item.IsSheet1TELSeniorityYFill;
            dbEntity.IsSheet1TELSeniorityMFill = item.IsSheet1TELSeniorityMFill;
            dbEntity.IsSheet1RemarkFill = item.IsSheet1RemarkFill;
            dbEntity.IsSheet2Show = item.IsSheet2Show;
            dbEntity.IsSheet2ServiceForFill = item.IsSheet2ServiceForFill;
            dbEntity.IsSheet2WorkItemFill = item.IsSheet2WorkItemFill;
            dbEntity.IsSheet2MachineNameFill = item.IsSheet2MachineNameFill;
            dbEntity.IsSheet2MachineNoFill = item.IsSheet2MachineNoFill;
            dbEntity.IsSheet2OnTimeFill = item.IsSheet2OnTimeFill;
            dbEntity.IsSheet2RemarkFill = item.IsSheet2RemarkFill;
            dbEntity.IsSheet3Show = item.IsSheet3Show;
            dbEntity.IsSheet3WorkerCountFill = item.IsSheet3WorkerCountFill;
            dbEntity.IsSheet3DateFill = item.IsSheet3DateFill;
            dbEntity.IsSheet3LocationFill = item.IsSheet3LocationFill;
            dbEntity.IsSheet3TELLossFill = item.IsSheet3TELLossFill;
            dbEntity.IsSheet3CustomerLossFill = item.IsSheet3CustomerLossFill;
            dbEntity.IsSheet3AccidentFill = item.IsSheet3AccidentFill;
            dbEntity.IsSheet3DescriptionFill = item.IsSheet3DescriptionFill;
            dbEntity.IsSheet4Show = item.IsSheet4Show;
            dbEntity.IsSheet4CorrectnessFill = item.IsSheet4CorrectnessFill;
            dbEntity.IsSheet4ContributionFill = item.IsSheet4ContributionFill;
            dbEntity.IsSheet5Show = item.IsSheet5Show;
            dbEntity.IsSheet5SelfTrainingFill = item.IsSheet5SelfTrainingFill;
            dbEntity.IsSheet5SelfTrainingRemarkFill = item.IsSheet5SelfTrainingRemarkFill;
            dbEntity.IsSheet6Show = item.IsSheet6Show;
            dbEntity.IsSheet6CooperationFill = item.IsSheet6CooperationFill;
            dbEntity.IsSheet6DateFill = item.IsSheet6DateFill;
            dbEntity.IsSheet6LocationFill = item.IsSheet6LocationFill;
            dbEntity.IsSheet6IsDamageFill = item.IsSheet6IsDamageFill;
            dbEntity.IsSheet6DescriptionFill = item.IsSheet6DescriptionFill;
            dbEntity.IsSheet7Show = item.IsSheet7Show;
            dbEntity.ModifyUser = userID;
            dbEntity.ModifyDate = cDate;
        }
        #endregion

        #region Private
        private void Valid(SPA_ScoringInfoSheetModel model)
        {
            var msgList = new List<string>();

            if (model.ServiceItemID == Guid.Empty)
                msgList.Add("ServiceItemID is required.");

            if (string.IsNullOrWhiteSpace(model.POSource))
                msgList.Add("POSource is required.");

            if (msgList.Any())
                throw new ArgumentException(string.Join(Environment.NewLine, msgList));
        }

        private void FillServiceItemText(List<SPA_ScoringInfoSheetModel> sourceList, List<TET_ParametersModel> parametersList)
        {
            foreach (var item in sourceList)
            {
                item.ServiceItem = parametersList.Where(obj => obj.ID == item.ServiceItemID).FirstOrDefault()?.Item;
            }
        }
        #endregion
    }
}
