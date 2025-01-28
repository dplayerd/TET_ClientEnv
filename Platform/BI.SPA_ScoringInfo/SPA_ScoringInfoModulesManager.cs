using BI.SPA_ScoringInfo.Models;
using BI.SPA_ScoringInfo.Validators;
using Platform.AbstractionClass;
using Platform.Infra;
using Platform.LogService;
using Platform.ORM;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BI.SPA_ScoringInfo
{
    public class SPA_ScoringInfoModulesManager
    {
        private Logger _logger = new Logger();
        private const string _prevText = "前期匯入";
        private const string _yesText = "Yes";
        private const string _noText = "No";
        private const string _EmpStatus1_Text = "在職";
        private const string _EmpStatus2_Text = "離職";
        private const string _EmpStatus3_Text = "新進";
        private const string _EmpStatus4_Text = "其他";

        private const string _hasDamageText = "有抱怨，且造成客戶或TEL損失";
        private const string _noDamageText = "有抱怨，未造成客戶或TEL損失";
        private const string _nothingText = "無";


        #region Read
        /// <summary> 取得 人力盤點 清單 </summary>
        /// <param name="SIID">評鑑計分資料系統辨識碼</param>
        /// <returns></returns>
        public List<SPA_ScoringInfoModule1Model> GetList_Module1(params Guid[] SIID)
        {
            try
            {
                using (PlatformContextModel context = new PlatformContextModel())
                {
                    //人力盤點頁簽: 明細排序調整(員工狀態("在職"最上、再"離職"、"新進"、"其他")、主要負責作業(升冪)->能否獨立作業->本社 / 協力廠商(升冪)->建立時間順序(降冪)
                    var orgQuery =
                        from item in context.TET_SPA_ScoringInfoModule1
                        let EmpStatusSortIndex =
                            (item.EmpStatus == _EmpStatus1_Text) ? 1 :
                            (item.EmpStatus == _EmpStatus2_Text) ? 2 :
                            (item.EmpStatus == _EmpStatus3_Text) ? 3 :
                            (item.EmpStatus == _EmpStatus4_Text) ? 4 :
                            99
                        where SIID.Contains(item.SIID)
                        orderby EmpStatusSortIndex, item.MajorJob, item.IsIndependent, item.Type, item.CreateDate descending
                        select item;
                    
                    var query =
                        from item in orgQuery
                        select
                            new SPA_ScoringInfoModule1Model()
                            {
                                ID = item.ID,
                                SIID = item.SIID,
                                Source = item.Source,
                                Type = item.Type,
                                Supplier = item.Supplier,
                                EmpName = item.EmpName,
                                MajorJob = item.MajorJob,
                                IsIndependent = item.IsIndependent,
                                SkillLevel = item.SkillLevel,
                                EmpStatus = item.EmpStatus,
                                TELSeniorityY = item.TELSeniorityY,
                                TELSeniorityM = item.TELSeniorityM,
                                Remark = item.Remark,
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

        /// <summary> 取得 人力盤點  </summary>
        /// <param name="id">Key</param>
        /// <returns></returns>
        public SPA_ScoringInfoModule1Model GetOne_Module1(Guid id)
        {
            try
            {
                using (PlatformContextModel context = new PlatformContextModel())
                {

                    var query =
                        from item in context.TET_SPA_ScoringInfoModule1
                        where item.ID == id
                        select
                            new SPA_ScoringInfoModule1Model()
                            {
                                ID = item.ID,
                                SIID = item.SIID,
                                Source = item.Source,
                                Type = item.Type,
                                Supplier = item.Supplier,
                                EmpName = item.EmpName,
                                MajorJob = item.MajorJob,
                                IsIndependent = item.IsIndependent,
                                SkillLevel = item.SkillLevel,
                                EmpStatus = item.EmpStatus,
                                TELSeniorityY = item.TELSeniorityY,
                                TELSeniorityM = item.TELSeniorityM,
                                Remark = item.Remark,
                                CreateUser = item.CreateUser,
                                CreateDate = item.CreateDate,
                                ModifyUser = item.ModifyUser,
                                ModifyDate = item.ModifyDate,
                            };

                    var result = query.FirstOrDefault();
                    return result;

                }
            }
            catch (Exception ex)
            {
                this._logger.WriteError(ex);
                return default;
            }
        }

        /// <summary> 取得 施工達交狀況盤點 清單 </summary>
        /// <param name="SIID">評鑑計分資料系統辨識碼</param>
        /// <returns></returns>
        public List<SPA_ScoringInfoModule2Model> GetList_Module2(params Guid[] SIID)
        {
            try
            {
                using (PlatformContextModel context = new PlatformContextModel())
                {
                    var orgQuery =
                        from item in context.TET_SPA_ScoringInfoModule2
                        where SIID.Contains(item.SIID)
                        select item;

                    var query =
                        from item in orgQuery
                        select
                            new SPA_ScoringInfoModule2Model()
                            {
                                ID = item.ID,
                                SIID = item.SIID,
                                ServiceFor = item.ServiceFor,
                                WorkItem = item.WorkItem,
                                MachineName = item.MachineName,
                                MachineNo = item.MachineNo,
                                OnTime = item.OnTime,
                                Remark = item.Remark,
                                CreateUser = item.CreateUser,
                                CreateDate = item.CreateDate,
                                ModifyUser = item.ModifyUser,
                                ModifyDate = item.ModifyDate,
                            };

                    query = query.OrderByDescending(obj => obj.CreateDate).ThenBy(obj => obj.ServiceFor).ThenBy(obj => obj.WorkItem).ThenBy(obj => obj.MachineNo).ThenBy(obj => obj.MachineName);
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

        /// <summary> 取得 施工達交狀況盤點  </summary>
        /// <param name="id">Key</param>
        /// <returns></returns>
        public SPA_ScoringInfoModule2Model GetOne_Module2(Guid id)
        {
            try
            {
                using (PlatformContextModel context = new PlatformContextModel())
                {
                    var query =
                        from item in context.TET_SPA_ScoringInfoModule2
                        where item.ID == id
                        select
                            new SPA_ScoringInfoModule2Model()
                            {
                                ID = item.ID,
                                SIID = item.SIID,
                                ServiceFor = item.ServiceFor,
                                WorkItem = item.WorkItem,
                                MachineName = item.MachineName,
                                MachineNo = item.MachineNo,
                                OnTime = item.OnTime,
                                Remark = item.Remark,
                                CreateUser = item.CreateUser,
                                CreateDate = item.CreateDate,
                                ModifyUser = item.ModifyUser,
                                ModifyDate = item.ModifyDate,
                            };

                    var result = query.FirstOrDefault();
                    return result;
                }
            }
            catch (Exception ex)
            {
                this._logger.WriteError(ex);
                return default;
            }
        }

        /// <summary> 取得 施工正確性 清單 </summary>
        /// <param name="SIID">評鑑計分資料系統辨識碼</param>
        /// <returns></returns>
        public List<SPA_ScoringInfoModule3Model> GetList_Module3(params Guid[] SIID)
        {
            try
            {
                using (PlatformContextModel context = new PlatformContextModel())
                {
                    var orgQuery =
                        from item in context.TET_SPA_ScoringInfoModule3
                        where SIID.Contains(item.SIID)
                        select item;

                    var query =
                        from item in orgQuery
                        select
                            new SPA_ScoringInfoModule3Model()
                            {
                                ID = item.ID,
                                SIID = item.SIID,
                                Date = item.Date,
                                Location = item.Location,
                                TELLoss = item.TELLoss,
                                CustomerLoss = item.CustomerLoss,
                                Accident = item.Accident,
                                Description = item.Description,
                                CreateUser = item.CreateUser,
                                CreateDate = item.CreateDate,
                                ModifyUser = item.ModifyUser,
                                ModifyDate = item.ModifyDate,
                            };

                    query = query.OrderByDescending(obj => obj.CreateDate);
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

        /// <summary> 取得 施工正確性  </summary>
        /// <param name="id">Key</param>
        /// <returns></returns>
        public SPA_ScoringInfoModule3Model GetOne_Module3(Guid id)
        {
            try
            {
                using (PlatformContextModel context = new PlatformContextModel())
                {
                    var query =
                        from item in context.TET_SPA_ScoringInfoModule3
                        where item.ID == id
                        select
                            new SPA_ScoringInfoModule3Model()
                            {
                                ID = item.ID,
                                SIID = item.SIID,
                                Date = item.Date,
                                Location = item.Location,
                                TELLoss = item.TELLoss,
                                CustomerLoss = item.CustomerLoss,
                                Accident = item.Accident,
                                Description = item.Description,
                                CreateUser = item.CreateUser,
                                CreateDate = item.CreateDate,
                                ModifyUser = item.ModifyUser,
                                ModifyDate = item.ModifyDate,
                            };

                    var result = query.FirstOrDefault();
                    return result;
                }
            }
            catch (Exception ex)
            {
                this._logger.WriteError(ex);
                return default;
            }
        }


        /// <summary> 取得 作業正確性&人員備齊貢獻度 清單 </summary>
        /// <param name="SIID">評鑑計分資料系統辨識碼</param>
        /// <returns></returns>
        public List<SPA_ScoringInfoModule4Model> GetList_Module4(params Guid[] SIID)
        {
            try
            {
                using (PlatformContextModel context = new PlatformContextModel())
                {
                    var orgQuery =
                        from item in context.TET_SPA_ScoringInfoModule4
                        where SIID.Contains(item.SIID)
                        select item;

                    var query =
                        from item in orgQuery
                        select
                            new SPA_ScoringInfoModule4Model()
                            {
                                ID = item.ID,
                                SIID = item.SIID,
                                Date = item.Date,
                                Location = item.Location,
                                IsDamage = item.IsDamage,
                                Description = item.Description,
                                CreateUser = item.CreateUser,
                                CreateDate = item.CreateDate,
                                ModifyUser = item.ModifyUser,
                                ModifyDate = item.ModifyDate,
                            };

                    query = query.OrderByDescending(obj => obj.CreateDate);
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

        /// <summary> 取得 作業正確性&人員備齊貢獻度  </summary>
        /// <param name="id">Key</param>
        /// <returns></returns>
        public SPA_ScoringInfoModule4Model GetOne_Module4(Guid id)
        {
            try
            {
                using (PlatformContextModel context = new PlatformContextModel())
                {
                    var query =
                        from item in context.TET_SPA_ScoringInfoModule4
                        where item.ID == id
                        select
                            new SPA_ScoringInfoModule4Model()
                            {
                                ID = item.ID,
                                SIID = item.SIID,
                                Date = item.Date,
                                Location = item.Location,
                                IsDamage = item.IsDamage,
                                Description = item.Description,
                                CreateUser = item.CreateUser,
                                CreateDate = item.CreateDate,
                                ModifyUser = item.ModifyUser,
                                ModifyDate = item.ModifyDate,
                            };

                    var result = query.FirstOrDefault();
                    return result;
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
        /// <summary> 修改 人力盤點 </summary>
        /// <param name="mainModel"></param>
        /// <param name="module1List"></param>
        /// <param name="userID">目前登入者</param>
        /// <param name="cDate">目前時間</param>
        public void Modify_Module1(SPA_ScoringInfoModel mainModel, List<SPA_ScoringInfoModule1Model> module1List, string userID, DateTime cDate)
        {
            if (module1List == null)
                throw new ArgumentNullException("Module1 is required.");

            try
            {
                using (PlatformContextModel context = new PlatformContextModel())
                {

                    //--- 先檢查是否能通過商業邏輯 ---
                    List<string> tempMsgList;
                    List<string> msgList = new List<string>();
                    var validResult = SPA_ScoringInfoValidator.Valid(mainModel, out tempMsgList);
                    if (!validResult)
                        msgList.AddRange(tempMsgList);
                    var validDetailResult = SPA_ScoringInfoModule1Validator.Valid(module1List, out tempMsgList);
                    if (!validResult)
                        msgList.AddRange(tempMsgList);

                    if (!validResult || !validDetailResult)
                        throw new ArgumentException(string.Join(Environment.NewLine, msgList));
                    //--- 先檢查是否能通過商業邏輯 ---


                    var dbModel =
                    (from item in context.TET_SPA_ScoringInfo
                     where item.ID == mainModel.ID
                     select item).FirstOrDefault();

                    if (dbModel == null)
                        throw new NullReferenceException($"{mainModel.ID} don't exists.");

                    //dbModel.Period = module1List.Period;
                    //dbModel.ModifyUser = userID;
                    //dbModel.ModifyDate = cDate;

                    // 明細清單
                    var detailList = context.TET_SPA_ScoringInfoModule1.Where(item => item.SIID == mainModel.ID).ToList();
                    var detailIdList = detailList.Select(obj => obj.ID);

                    var inpDetailIdList = module1List.Where(obj => obj.ID.HasValue).Select(obj => obj.ID.Value).ToList();


                    //--- 找到已移除的明細及附檔，並刪除 ---
                    var notExistDetailId = detailIdList.Except(inpDetailIdList).ToList();

                    context.TET_SPA_ScoringInfoModule1.RemoveRange(detailList.Where(item => notExistDetailId.Contains(item.ID)));
                    //--- 找到已移除的明細及附檔，並刪除 ---


                    //--- 找到已存在的明細及附檔，並更新 ---
                    foreach (var item in module1List)
                    {
                        if (!item.ID.HasValue)
                            continue;

                        // 以下欄位在前期匯入時，是不允許編輯的
                        // 評鑑單位、服務對象、受評供應商、PO Source、評鑑項目
                        var isPrevImport = (string.Compare(_prevText, item.Source, true) == 0);
                        var orgDetail = detailList.Where(obj => obj.ID == item.ID.Value).FirstOrDefault();

                        if (orgDetail != null)
                        {
                            orgDetail.Source = item.Source;
                            if (!isPrevImport)
                            {
                                orgDetail.Type = item.Type;
                                orgDetail.Supplier = item.Supplier;
                                orgDetail.EmpName = item.EmpName;
                            }
                            orgDetail.MajorJob = item.MajorJob;
                            orgDetail.IsIndependent = item.IsIndependent;
                            orgDetail.SkillLevel = item.SkillLevel;
                            orgDetail.EmpStatus = item.EmpStatus;
                            orgDetail.TELSeniorityY = item.TELSeniorityY;
                            orgDetail.TELSeniorityM = item.TELSeniorityM;
                            orgDetail.Remark = item.Remark;

                            orgDetail.ModifyUser = userID;
                            orgDetail.ModifyDate = cDate;
                        }
                    }
                    //--- 找到已存在的明細及附檔，並更新 ---


                    //--- 找到新增的明細及附檔，並建立 ---
                    var willCreateDetails = module1List.Where(obj => !obj.ID.HasValue || Guid.Empty == obj.ID.Value).ToList();
                    foreach (var item in willCreateDetails)
                    {
                        var detailEntity = new TET_SPA_ScoringInfoModule1()
                        {
                            ID = Guid.NewGuid(),
                            SIID = mainModel.ID.Value,

                            Source = item.Source,
                            Type = item.Type,
                            Supplier = item.Supplier,
                            EmpName = item.EmpName,
                            MajorJob = item.MajorJob,
                            IsIndependent = item.IsIndependent,
                            SkillLevel = item.SkillLevel,
                            EmpStatus = item.EmpStatus,
                            TELSeniorityY = item.TELSeniorityY,
                            TELSeniorityM = item.TELSeniorityM,
                            Remark = item.Remark,

                            CreateUser = userID,
                            CreateDate = cDate,
                            ModifyUser = userID,
                            ModifyDate = cDate,
                        };


                        context.TET_SPA_ScoringInfoModule1.Add(detailEntity);
                    }
                    //--- 找到新增的明細及附檔，並建立 ---

                    context.SaveChanges();

                }
            }
            catch (Exception ex)
            {
                this._logger.WriteError(ex);
                throw;
            }
        }


        /// <summary> 修改 施工達交狀況盤點 </summary>
        /// <param name="mainModel"></param>
        /// <param name="Module2List"></param>
        /// <param name="userID">目前登入者</param>
        /// <param name="cDate">目前時間</param>
        public void Modify_Module2(SPA_ScoringInfoModel mainModel, List<SPA_ScoringInfoModule2Model> Module2List, string userID, DateTime cDate)
        {
            if (Module2List == null)
                throw new ArgumentNullException("Module2 is required.");

            try
            {
                using (PlatformContextModel context = new PlatformContextModel())
                {
                    //--- 先檢查是否能通過商業邏輯 ---
                    List<string> tempMsgList;
                    List<string> msgList = new List<string>();
                    var validResult = SPA_ScoringInfoValidator.Valid(mainModel, out tempMsgList);
                    if (!validResult)
                        msgList.AddRange(tempMsgList);
                    var validDetailResult = SPA_ScoringInfoModule2Validator.Valid(mainModel, Module2List, out tempMsgList);
                    if (!validResult)
                        msgList.AddRange(tempMsgList);

                    if (!validResult || !validDetailResult)
                        throw new ArgumentException(string.Join(Environment.NewLine, msgList));
                    //--- 先檢查是否能通過商業邏輯 ---


                    var dbModel =
                    (from item in context.TET_SPA_ScoringInfo
                     where item.ID == mainModel.ID
                     select item).FirstOrDefault();

                    if (dbModel == null)
                        throw new NullReferenceException($"{mainModel.ID} don't exists.");

                    //dbModel.Period = Module2List.Period;
                    //dbModel.ModifyUser = userID;
                    //dbModel.ModifyDate = cDate;

                    // 明細清單
                    var detailList = context.TET_SPA_ScoringInfoModule2.Where(item => item.SIID == mainModel.ID).ToList();
                    var detailIdList = detailList.Select(obj => obj.ID);

                    var inpDetailIdList = Module2List.Where(obj => obj.ID.HasValue).Select(obj => obj.ID.Value).ToList();


                    //--- 找到已移除的明細及附檔，並刪除 ---
                    var notExistDetailId = detailIdList.Except(inpDetailIdList).ToList();

                    context.TET_SPA_ScoringInfoModule2.RemoveRange(detailList.Where(item => notExistDetailId.Contains(item.ID)));
                    //--- 找到已移除的明細及附檔，並刪除 ---


                    //--- 找到已存在的明細及附檔，並更新 ---
                    foreach (var item in Module2List)
                    {
                        if (!item.ID.HasValue)
                            continue;

                        var orgDetail = detailList.Where(obj => obj.ID == item.ID.Value).FirstOrDefault();

                        if (orgDetail != null)
                        {
                            orgDetail.ServiceFor = item.ServiceFor;
                            orgDetail.WorkItem = item.WorkItem;
                            orgDetail.MachineName = item.MachineName;
                            orgDetail.MachineNo = item.MachineNo;
                            orgDetail.OnTime = item.OnTime;
                            orgDetail.Remark = item.Remark;

                            orgDetail.ModifyUser = userID;
                            orgDetail.ModifyDate = cDate;
                        }
                    }
                    //--- 找到已存在的明細及附檔，並更新 ---


                    //--- 找到新增的明細及附檔，並建立 ---
                    var willCreateDetails = Module2List.Where(obj => !obj.ID.HasValue || Guid.Empty == obj.ID.Value).ToList();
                    foreach (var item in willCreateDetails)
                    {
                        var detailEntity = new TET_SPA_ScoringInfoModule2()
                        {
                            ID = Guid.NewGuid(),
                            SIID = mainModel.ID.Value,

                            ServiceFor = item.ServiceFor,
                            WorkItem = item.WorkItem,
                            MachineName = item.MachineName,
                            MachineNo = item.MachineNo,
                            OnTime = item.OnTime,
                            Remark = item.Remark,

                            CreateUser = userID,
                            CreateDate = cDate,
                            ModifyUser = userID,
                            ModifyDate = cDate,
                        };


                        context.TET_SPA_ScoringInfoModule2.Add(detailEntity);
                    }
                    //--- 找到新增的明細及附檔，並建立 ---

                    context.SaveChanges();

                }
            }
            catch (Exception ex)
            {
                this._logger.WriteError(ex);
                throw;
            }


        }

        /// <summary> 修改 施工正確性 </summary>
        /// <param name="mainModel"></param>
        /// <param name="module3List"></param>
        /// <param name="userID">目前登入者</param>
        /// <param name="cDate">目前時間</param>
        public void Modify_Module3(SPA_ScoringInfoModel mainModel, List<SPA_ScoringInfoModule3Model> module3List, string userID, DateTime cDate)
        {
            if (module3List == null)
                throw new ArgumentNullException("Module3 is required.");

            try
            {
                using (PlatformContextModel context = new PlatformContextModel())
                {

                    //--- 先檢查是否能通過商業邏輯 ---
                    List<string> tempMsgList;
                    List<string> msgList = new List<string>();
                    var validResult = SPA_ScoringInfoValidator.Valid_Tab3(mainModel, out tempMsgList);
                    if (!validResult)
                        msgList.AddRange(tempMsgList);
                    var validDetailResult = SPA_ScoringInfoModule3Validator.Valid(module3List, out tempMsgList);
                    if (!validResult)
                        msgList.AddRange(tempMsgList);

                    if (!validResult || !validDetailResult)
                        throw new ArgumentException(string.Join(Environment.NewLine, msgList));
                    //--- 先檢查是否能通過商業邏輯 ---


                    var dbModel =
                    (from item in context.TET_SPA_ScoringInfo
                     where item.ID == mainModel.ID
                     select item).FirstOrDefault();

                    if (dbModel == null)
                        throw new NullReferenceException($"{mainModel.ID} don't exists.");

                    dbModel.MOCount = module3List.Count.ToString();
                    dbModel.TELLoss = module3List.Where(obj => string.Compare(_yesText, obj.TELLoss, true) == 0).Any() ? _yesText : _noText;
                    dbModel.CustomerLoss = module3List.Where(obj => string.Compare(_yesText, obj.CustomerLoss, true) == 0).Any() ? _yesText : _noText;
                    dbModel.Accident = module3List.Where(obj => string.Compare(_yesText, obj.Accident, true) == 0).Any() ? _yesText : _noText;
                    dbModel.WorkerCount = mainModel.WorkerCount;
                    dbModel.ModifyUser = userID;
                    dbModel.ModifyDate = cDate;

                    // 明細清單
                    var detailList = context.TET_SPA_ScoringInfoModule3.Where(item => item.SIID == mainModel.ID).ToList();
                    var detailIdList = detailList.Select(obj => obj.ID);

                    var inpDetailIdList = module3List.Where(obj => obj.ID.HasValue).Select(obj => obj.ID.Value).ToList();


                    //--- 找到已移除的明細及附檔，並刪除 ---
                    var notExistDetailId = detailIdList.Except(inpDetailIdList).ToList();

                    context.TET_SPA_ScoringInfoModule3.RemoveRange(detailList.Where(item => notExistDetailId.Contains(item.ID)));
                    //--- 找到已移除的明細及附檔，並刪除 ---


                    //--- 找到已存在的明細及附檔，並更新 ---
                    foreach (var item in module3List)
                    {
                        if (!item.ID.HasValue)
                            continue;

                        var orgDetail = detailList.Where(obj => obj.ID == item.ID.Value).FirstOrDefault();

                        if (orgDetail != null)
                        {
                            orgDetail.Date = item.Date;
                            orgDetail.Location = item.Location;
                            orgDetail.TELLoss = item.TELLoss;
                            orgDetail.CustomerLoss = item.CustomerLoss;
                            orgDetail.Accident = item.Accident;
                            orgDetail.Description = item.Description ?? string.Empty;

                            orgDetail.ModifyUser = userID;
                            orgDetail.ModifyDate = cDate;
                        }
                    }
                    //--- 找到已存在的明細及附檔，並更新 ---


                    //--- 找到新增的明細及附檔，並建立 ---
                    var willCreateDetails = module3List.Where(obj => !obj.ID.HasValue || Guid.Empty == obj.ID.Value).ToList();
                    foreach (var item in willCreateDetails)
                    {
                        var detailEntity = new TET_SPA_ScoringInfoModule3()
                        {
                            ID = Guid.NewGuid(),
                            SIID = mainModel.ID.Value,

                            Date = item.Date,
                            Location = item.Location,
                            TELLoss = item.TELLoss,
                            CustomerLoss = item.CustomerLoss,
                            Accident = item.Accident,
                            Description = item.Description ?? string.Empty,

                            CreateUser = userID,
                            CreateDate = cDate,
                            ModifyUser = userID,
                            ModifyDate = cDate,
                        };


                        context.TET_SPA_ScoringInfoModule3.Add(detailEntity);
                    }
                    //--- 找到新增的明細及附檔，並建立 ---

                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                this._logger.WriteError(ex);
                throw;
            }

        }


        /// <summary> 修改 作業正確性&人員備齊貢獻度 </summary>
        /// <param name="mainModel"></param>
        /// <param name="module4List"></param>
        /// <param name="userID">目前登入者</param>
        /// <param name="cDate">目前時間</param>
        public void Modify_Module4(SPA_ScoringInfoModel mainModel, List<SPA_ScoringInfoModule4Model> module4List, string userID, DateTime cDate)
        {
            if (module4List == null)
                throw new ArgumentNullException("Module4 is required.");

            try
            {
                using (PlatformContextModel context = new PlatformContextModel())
                {
                    //--- 先檢查是否能通過商業邏輯 ---
                    List<string> tempMsgList;
                    List<string> msgList = new List<string>();
                    var validResult = SPA_ScoringInfoValidator.Valid_Tab6(mainModel, out tempMsgList);
                    if (!validResult)
                        msgList.AddRange(tempMsgList);
                    var validDetailResult = SPA_ScoringInfoModule4Validator.Valid(module4List, out tempMsgList);
                    if (!validResult)
                        msgList.AddRange(tempMsgList);

                    if (!validResult || !validDetailResult)
                        throw new ArgumentException(string.Join(Environment.NewLine, msgList));
                    //--- 先檢查是否能通過商業邏輯 ---


                    var dbModel =
                    (from item in context.TET_SPA_ScoringInfo
                     where item.ID == mainModel.ID
                     select item).FirstOrDefault();

                    if (dbModel == null)
                        throw new NullReferenceException($"{mainModel.ID} don't exists.");

                    if (module4List.Any())
                        dbModel.Complain = (module4List.Where(obj => string.Compare(_yesText, obj.IsDamage, true) == 0).Any()) ? _hasDamageText : _noDamageText;
                    else
                        dbModel.Complain = _nothingText;

                    dbModel.Cooperation = mainModel.Cooperation;
                    dbModel.Advantage = mainModel.Advantage;
                    dbModel.Improved = mainModel.Improved;
                    dbModel.Comment = mainModel.Comment;
                    dbModel.ModifyUser = userID;
                    dbModel.ModifyDate = cDate;


                    // 明細清單
                    var detailList = context.TET_SPA_ScoringInfoModule4.Where(item => item.SIID == mainModel.ID).ToList();
                    var detailIdList = detailList.Select(obj => obj.ID);

                    var inpDetailIdList = module4List.Where(obj => obj.ID.HasValue).Select(obj => obj.ID.Value).ToList();


                    //--- 找到已移除的明細及附檔，並刪除 ---
                    var notExistDetailId = detailIdList.Except(inpDetailIdList).ToList();

                    context.TET_SPA_ScoringInfoModule4.RemoveRange(detailList.Where(item => notExistDetailId.Contains(item.ID)));
                    //--- 找到已移除的明細及附檔，並刪除 ---


                    //--- 找到已存在的明細及附檔，並更新 ---
                    foreach (var item in module4List)
                    {
                        if (!item.ID.HasValue)
                            continue;

                        var orgDetail = detailList.Where(obj => obj.ID == item.ID.Value).FirstOrDefault();

                        if (orgDetail != null)
                        {
                            orgDetail.Date = item.Date;
                            orgDetail.Location = item.Location;
                            orgDetail.IsDamage = item.IsDamage;
                            orgDetail.Description = item.Description ?? string.Empty;

                            orgDetail.ModifyUser = userID;
                            orgDetail.ModifyDate = cDate;
                        }
                    }
                    //--- 找到已存在的明細及附檔，並更新 ---


                    //--- 找到新增的明細及附檔，並建立 ---
                    var willCreateDetails = module4List.Where(obj => !obj.ID.HasValue || Guid.Empty == obj.ID.Value).ToList();
                    foreach (var item in willCreateDetails)
                    {
                        var detailEntity = new TET_SPA_ScoringInfoModule4()
                        {
                            ID = Guid.NewGuid(),
                            SIID = mainModel.ID.Value,

                            Date = item.Date,
                            Location = item.Location,
                            IsDamage = item.IsDamage,
                            Description = item.Description ?? string.Empty,

                            CreateUser = userID,
                            CreateDate = cDate,
                            ModifyUser = userID,
                            ModifyDate = cDate,
                        };


                        context.TET_SPA_ScoringInfoModule4.Add(detailEntity);
                    }
                    //--- 找到新增的明細及附檔，並建立 ---

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
