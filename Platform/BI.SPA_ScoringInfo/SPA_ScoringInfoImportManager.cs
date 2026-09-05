using BI.Shared;
using BI.SPA_ScoringInfo.Models;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace BI.SPA_ScoringInfo
{
    /// <summary> 供應商 SPA 評鑑計分資料匯入 Manager </summary>
    public class SPA_ScoringInfoImportManager
    {
        #region 欄位
        private readonly SPA_ScoringInfoManager _mgr = new SPA_ScoringInfoManager();
        private readonly SPA_ScoringInfoModulesManager _detailMgr = new SPA_ScoringInfoModulesManager();
        private readonly SPA_ScoringInfoSheetManager _sheetMgr = new SPA_ScoringInfoSheetManager();
        private readonly TET_ParametersManager _parameterMgr = new TET_ParametersManager();
        #endregion

        #region 公開方法
        /// <summary> 匯入人員名冊頁籤，並依供應商與員工姓名合併既有資料 </summary>
        /// <param name="id">SPA評鑑計分資料識別碼。</param>
        /// <param name="input">匯入 Excel 檔案串流。</param>
        /// <param name="validNameDuplicate">是否檢查員工姓名重複。</param>
        /// <param name="userID">目前登入者。</param>
        /// <param name="cDate">目前時間。</param>
        /// <returns>匯入結果。</returns>
        public SPA_ScoringInfoImportResult ImportTab1(Guid id, Stream input, bool validNameDuplicate, string userID, DateTime cDate)
        {
            var result = new SPA_ScoringInfoImportResult();
            var model = this._mgr.GetOne(id);
            var sheetSetting = this.GetSheetSetting(model, result.Messages);
            if (sheetSetting == null)
                return result;

            var importedList = this.ReadImportTab1(input, model, sheetSetting, result.Messages);
            if (result.Messages.Any())
                return result;

            if (validNameDuplicate)
            {
                var duplicateMsgList = new List<string>();
                bool isDuplicated = this.ValidDuplicate(importedList.GroupBy(obj => obj.Supplier + "___" + obj.EmpName), "員工姓名", duplicateMsgList);
                if (isDuplicated)
                {
                    result.Code = SPA_ScoringInfoImportResult.NameDuplicateCode;
                    result.Messages.AddRange(duplicateMsgList);
                    return result;
                }
            }

            var detailList = this._detailMgr.GetList_Module1(id) ?? new List<SPA_ScoringInfoModule1Model>();
            foreach (var imported in importedList)
            {
                var exists = detailList.FirstOrDefault(obj => obj.Supplier == imported.Supplier && obj.EmpName == imported.EmpName);
                if (exists == null)
                {
                    detailList.Add(imported);
                }
                else
                {
                    imported.ID = exists.ID;
                    var index = detailList.IndexOf(exists);
                    detailList[index] = imported;
                }
            }

            model.SheetSetting = sheetSetting;
            model.Module1List = detailList;
            this._detailMgr.Modify_Module1(model, detailList, userID, cDate, false);
            return result;
        }

        /// <summary> 匯入機台服務頁籤，並依機台名稱與序號合併既有資料 </summary>
        /// <param name="id">SPA評鑑計分資料識別碼。</param>
        /// <param name="input">匯入 Excel 檔案串流。</param>
        /// <param name="userID">目前登入者。</param>
        /// <param name="cDate">目前時間。</param>
        /// <returns>匯入結果。</returns>
        public SPA_ScoringInfoImportResult ImportTab2(Guid id, Stream input, string userID, DateTime cDate)
        {
            var result = new SPA_ScoringInfoImportResult();
            var model = this._mgr.GetOne(id);
            var sheetSetting = this.GetSheetSetting(model, result.Messages);
            if (sheetSetting == null)
                return result;

            var importedList = this.ReadImportTab2(input, model, sheetSetting, result.Messages);
            if (result.Messages.Any())
                return result;

            var detailList = this._detailMgr.GetList_Module2(id) ?? new List<SPA_ScoringInfoModule2Model>();
            foreach (var imported in importedList)
            {
                var exists = detailList.FirstOrDefault(obj => obj.MachineName == imported.MachineName && obj.MachineNo == imported.MachineNo);
                if (exists == null)
                {
                    detailList.Add(imported);
                }
                else
                {
                    imported.ID = exists.ID;
                    var index = detailList.IndexOf(exists);
                    detailList[index] = imported;
                }
            }

            model.SheetSetting = sheetSetting;
            model.Module2List = detailList;
            this._detailMgr.Modify_Module2(model, detailList, userID, cDate, false);
            return result;
        }

        /// <summary> 匯入工安事件頁籤，並依時間、地點與事件說明合併既有資料 </summary>
        /// <param name="id">SPA評鑑計分資料識別碼。</param>
        /// <param name="input">匯入 Excel 檔案串流。</param>
        /// <param name="workerCountText">出工人數文字。</param>
        /// <param name="userID">目前登入者。</param>
        /// <param name="cDate">目前時間。</param>
        /// <returns>匯入結果。</returns>
        public SPA_ScoringInfoImportResult ImportTab3(Guid id, Stream input, string workerCountText, string userID, DateTime cDate)
        {
            var result = new SPA_ScoringInfoImportResult();
            var model = this._mgr.GetOne(id);
            var sheetSetting = this.GetSheetSetting(model, result.Messages);
            if (sheetSetting == null)
                return result;

            this.ApplyWorkerCount(model, workerCountText, result.Messages);
            if (result.Messages.Any())
                return result;

            var importedList = this.ReadImportTab3(input, model, sheetSetting, result.Messages);
            if (result.Messages.Any())
                return result;

            var detailList = this._detailMgr.GetList_Module3(id) ?? new List<SPA_ScoringInfoModule3Model>();
            foreach (var imported in importedList)
            {
                var exists = detailList.FirstOrDefault(obj => obj.Date?.Date == imported.Date?.Date && obj.Location == imported.Location && obj.Description == imported.Description);
                if (exists == null)
                {
                    detailList.Add(imported);
                }
                else
                {
                    imported.ID = exists.ID;
                    var index = detailList.IndexOf(exists);
                    detailList[index] = imported;
                }
            }

            model.SheetSetting = sheetSetting;
            model.Module3List = detailList;
            this._detailMgr.Modify_Module3(model, detailList, userID, cDate, false);
            return result;
        }
        #endregion

        #region 匯入讀取
        /// <summary> 取得計分資料對應的頁籤顯示設定 </summary>
        private SPA_ScoringInfoSheetModel GetSheetSetting(SPA_ScoringInfoModel model, List<string> msgList)
        {
            if (model == null)
            {
                msgList.Add("SPA評鑑計分資料不存在。");
                return null;
            }

            var sheetSetting = this._sheetMgr.GetDetail(model.ServiceItem, model.POSource);
            if (sheetSetting == null)
                msgList.Add($"找不到 SPA評鑑計分資料頁籤顯示設定，評鑑項目：{model.ServiceItem}，PO Source：{model.POSource}。請先維護設定後再存檔。");

            return sheetSetting;
        }

        /// <summary> 由上傳串流建立 Excel 工作簿 </summary>
        private IWorkbook GetUploadWorkbook(Stream input)
        {
            if (input == null || !input.CanRead)
                throw new ArgumentException("請選擇匯入檔案。");

            return new XSSFWorkbook(input);
        }

        /// <summary> 讀取人員名冊頁籤資料並執行欄位驗證 </summary>
        private List<SPA_ScoringInfoModule1Model> ReadImportTab1(Stream input, SPA_ScoringInfoModel mainModel, SPA_ScoringInfoSheetModel sheetSetting, List<string> msgList)
        {
            var result = new List<SPA_ScoringInfoModule1Model>();
            var sheet = this.GetUploadWorkbook(input).GetSheetAt(0);
            for (int i = 1; i <= sheet.LastRowNum; i++)
            {
                var row = sheet.GetRow(i);
                if (this.IsEmptyRow(row))
                    continue;

                int rowNo = i;
                this.ValidMainColumns(mainModel, row, rowNo, msgList);

                result.Add(new SPA_ScoringInfoModule1Model()
                {
                    SIID = mainModel.ID.Value,
                    Source = this.GetCellString(row, 5),
                    Type = this.GetCellString(row, 6),
                    Supplier = this.GetCellString(row, 7),
                    EmpName = this.GetCellString(row, 8),
                    MajorJob = this.GetCellString(row, 9),
                    IsIndependent = this.GetCellString(row, 10),
                    SkillLevel = this.GetCellString(row, 11),
                    EmpStatus = this.GetCellString(row, 12),
                    TELSeniorityY = this.GetCellString(row, 13),
                    TELSeniorityM = this.GetCellString(row, 14),
                    Remark = this.GetCellString(row, 15),
                });
            }

            this.ValidImportValues_Tab1(result, mainModel, sheetSetting, msgList);
            return result;
        }

        /// <summary> 讀取機台服務頁籤資料並執行欄位驗證 </summary>
        private List<SPA_ScoringInfoModule2Model> ReadImportTab2(Stream input, SPA_ScoringInfoModel mainModel, SPA_ScoringInfoSheetModel sheetSetting, List<string> msgList)
        {
            var result = new List<SPA_ScoringInfoModule2Model>();
            var sheet = this.GetUploadWorkbook(input).GetSheetAt(0);
            for (int i = 1; i <= sheet.LastRowNum; i++)
            {
                var row = sheet.GetRow(i);
                if (this.IsEmptyRow(row))
                    continue;

                int rowNo = i;
                this.ValidMainColumns(mainModel, row, rowNo, msgList);

                result.Add(new SPA_ScoringInfoModule2Model()
                {
                    SIID = mainModel.ID.Value,
                    ServiceFor = this.GetCellString(row, 5),
                    WorkItem = this.GetCellString(row, 6),
                    MachineName = this.GetCellString(row, 7),
                    MachineNo = this.GetCellString(row, 8),
                    OnTime = this.GetCellString(row, 9),
                    Remark = this.GetCellString(row, 10),
                });
            }

            this.ValidImportValues_Tab2(result, sheetSetting, msgList);
            return result;
        }

        /// <summary> 讀取工安事件頁籤資料並執行欄位驗證 </summary>
        private List<SPA_ScoringInfoModule3Model> ReadImportTab3(Stream input, SPA_ScoringInfoModel mainModel, SPA_ScoringInfoSheetModel sheetSetting, List<string> msgList)
        {
            var result = new List<SPA_ScoringInfoModule3Model>();
            var sheet = this.GetUploadWorkbook(input).GetSheetAt(0);
            for (int i = 1; i <= sheet.LastRowNum; i++)
            {
                var row = sheet.GetRow(i);
                if (this.IsEmptyRow(row))
                    continue;

                int rowNo = i;
                this.ValidMainColumns(mainModel, row, rowNo, msgList);

                var dateText = this.GetCellString(row, 5);
                var date = this.GetCellDate(row, 5);
                if (!string.IsNullOrWhiteSpace(dateText) && !date.HasValue)
                    msgList.Add($"第{rowNo}筆資料 欄位 時間 欄位值錯誤");

                var imported = new SPA_ScoringInfoModule3Model()
                {
                    SIID = mainModel.ID.Value,
                    Date = date,
                    Location = this.GetCellString(row, 6),
                    TELLoss = this.GetCellString(row, 7),
                    CustomerLoss = this.GetCellString(row, 8),
                    Accident = this.GetCellString(row, 9),
                    Description = this.GetCellString(row, 10),
                };

                this.ValidImportRequired_Tab3(imported, dateText, sheetSetting, rowNo, msgList);
                this.ValidImportYesNo(imported.TELLoss, "TEL財損", rowNo, msgList);
                this.ValidImportYesNo(imported.CustomerLoss, "客戶財損", rowNo, msgList);
                this.ValidImportYesNo(imported.Accident, "人身事故", rowNo, msgList);
                result.Add(imported);
            }

            this.ValidDuplicate_Tab3(result, msgList);
            return result;
        }

        /// <summary> 套用出工人數並驗證格式 </summary>
        private void ApplyWorkerCount(SPA_ScoringInfoModel model, string workerCountText, List<string> msgList)
        {
            if (workerCountText == null)
                return;

            if (string.IsNullOrWhiteSpace(workerCountText))
            {
                model.WorkerCount = null;
                return;
            }

            if (int.TryParse(workerCountText, out int workerCount))
            {
                model.WorkerCount = workerCount;
                return;
            }

            msgList.Add("出工人數必須為正整數");
        }
        #endregion

        #region 匯入驗證
        /// <summary> 驗證工安事件頁籤必填欄位 </summary>
        private void ValidImportRequired_Tab3(SPA_ScoringInfoModule3Model model, string dateText, SPA_ScoringInfoSheetModel sheetSetting, int rowNo, List<string> msgList)
        {
            if (sheetSetting.IsSheet3DateFill && string.IsNullOrWhiteSpace(dateText))
                msgList.Add($"第{rowNo}筆資料 欄位 時間 為必填欄位");

            if (sheetSetting.IsSheet3LocationFill && string.IsNullOrWhiteSpace(model.Location))
                msgList.Add($"第{rowNo}筆資料 欄位 地點 為必填欄位");

            if (sheetSetting.IsSheet3TELLossFill && string.IsNullOrWhiteSpace(model.TELLoss))
                msgList.Add($"第{rowNo}筆資料 欄位 TEL財損 為必填欄位");

            if (sheetSetting.IsSheet3CustomerLossFill && string.IsNullOrWhiteSpace(model.CustomerLoss))
                msgList.Add($"第{rowNo}筆資料 欄位 客戶財損 為必填欄位");

            if (sheetSetting.IsSheet3AccidentFill && string.IsNullOrWhiteSpace(model.Accident))
                msgList.Add($"第{rowNo}筆資料 欄位 人身事故 為必填欄位");

            if (sheetSetting.IsSheet3DescriptionFill && string.IsNullOrWhiteSpace(model.Description))
                msgList.Add($"第{rowNo}筆資料 欄位 事件說明 為必填欄位");
        }

        /// <summary> 驗證工安事件頁籤時間、地點與事件說明是否重複 </summary>
        private void ValidDuplicate_Tab3(List<SPA_ScoringInfoModule3Model> list, List<string> msgList)
        {
            var repeated = list.GroupBy(obj => obj.Date?.ToString("yyyy-MM-dd") + "___" + obj.Location + "___" + obj.Description)
                .Where(obj => !string.IsNullOrWhiteSpace(obj.Key.Replace("___", string.Empty)) && obj.Count() > 1)
                .ToList();

            foreach (var item in repeated)
            {
                var parts = item.Key.Split(new[] { "___" }, StringSplitOptions.None);
                msgList.Add($"時間{parts[0]} + 地點{parts[1]} + 事件說明{parts[2]} 重複");
            }
        }

        /// <summary> 驗證匯入檔主檔欄位是否與目前單據一致 </summary>
        private void ValidMainColumns(SPA_ScoringInfoModel mainModel, IRow row, int rowNo, List<string> msgList)
        {
            var period = this.GetCellString(row, 0);
            var bu = this.GetCellString(row, 1);
            var serviceFor = this.GetCellString(row, 2);
            var serviceItem = this.GetCellString(row, 3);
            var belongTo = this.GetCellString(row, 4);

            bool periodMatch = period == mainModel.Period || period.StartsWith(mainModel.Period + " ", StringComparison.OrdinalIgnoreCase);
            if (!periodMatch || bu != mainModel.BU || serviceFor != mainModel.ServiceFor || serviceItem != mainModel.ServiceItem || belongTo != mainModel.BelongTo)
                msgList.Add($"第{rowNo}筆資料 評鑑期間、評鑑單位、服務對象、評鑑項目、受評供應商欄位值與本單據不符");
        }

        /// <summary> 檢查指定群組是否有重複資料 </summary>
        private bool ValidDuplicate<T>(IEnumerable<IGrouping<string, T>> groups, string title, List<string> msgList)
        {
            bool isDuplicated = false;

            var repeated = groups.Where(obj => !string.IsNullOrWhiteSpace(obj.Key.Replace("___", string.Empty)) && obj.Count() > 1).ToList();
            foreach (var item in repeated)
            {
                msgList.Add($"{title}{item.Key.Replace("___", " + ")}重複");
                isDuplicated = true;
            }

            return isDuplicated;
        }

        /// <summary> 驗證人員名冊頁籤的欄位值與跨欄位商業規則 </summary>
        private void ValidImportValues_Tab1(List<SPA_ScoringInfoModule1Model> list, SPA_ScoringInfoModel mainModel, SPA_ScoringInfoSheetModel sheetSetting, List<string> msgList)
        {
            var majorJobs = this._parameterMgr.GetParametersKeyTextList("SPA主要負責作業").Select(obj => obj.Text).ToArray();

            int maxTELSeniorityY = 40;
            int maxTELSeniorityM = 11;
            string[] types = new[] { "本社社員", "協力廠商" };
            string[] sources = new[] { "本期新增", "前期匯入" };
            string[] isIndependent = new[] { "O", "X", "NA" };
            string[] empStatus = new[] { "新進", "在職", "離職", "其他" };
            string[] newEmpStatus = new[] { "新進", "其他" };

            int rowNo = 0;
            foreach (var item in list)
            {
                rowNo += 1;

                if (sheetSetting.IsSheet1TypeFill)
                    this.ValidImportText(item.Type, "本社/協力廠商", rowNo, msgList, types);

                if (sheetSetting.IsSheet1SupplierFill)
                    this.ValidImportText(item.Supplier, "供應商名稱", rowNo, msgList);

                if (sheetSetting.IsSheet1SourceFill)
                    this.ValidImportText(item.Source, "資料來源", rowNo, msgList, sources);

                if (sheetSetting.IsSheet1EmpStatusFill)
                    this.ValidImportText(item.EmpStatus, "員工狀態", rowNo, msgList, empStatus);

                if (sheetSetting.IsSheet1EmpNameFill)
                    this.ValidImportText(item.EmpName, "員工姓名", rowNo, msgList);

                if (sheetSetting.IsSheet1MajorJobFill)
                    this.ValidImportText(item.MajorJob, "主要負責作業", rowNo, msgList, majorJobs);

                if (sheetSetting.IsSheet1IsIndependentFill)
                    this.ValidImportText(item.IsIndependent, "能否獨立作業", rowNo, msgList, isIndependent);

                if (sheetSetting.IsSheet1SkillLevelFill)
                    this.ValidImportText(item.SkillLevel, "Skill Level", rowNo, msgList);

                if (sheetSetting.IsSheet1TELSeniorityYFill)
                    this.ValidImportInt(item.TELSeniorityY, "派工至TEL的年資(年)", rowNo, msgList, 0, maxTELSeniorityY);

                if (sheetSetting.IsSheet1TELSeniorityMFill)
                    this.ValidImportInt(item.TELSeniorityM, "派工至TEL的年資(月)", rowNo, msgList, 0, maxTELSeniorityM);

                if (sheetSetting.IsSheet1RemarkFill)
                    this.ValidImportText(item.Remark, "備註", rowNo, msgList);

                if (sheetSetting.IsSheet1TypeFill && sheetSetting.IsSheet1SupplierFill)
                {
                    bool isSelfCompany = string.Compare("本社社員", item.Type, true) == 0;
                    bool isSameSupplier = string.Compare(mainModel.BelongTo, item.Supplier, true) == 0;

                    if (isSelfCompany && !isSameSupplier)
                        msgList.Add($"第{rowNo}筆資料 欄位 供應商名稱 欄位值錯誤");
                    else if (!isSelfCompany && isSameSupplier)
                        msgList.Add($"第{rowNo}筆資料 欄位 供應商名稱 欄位值錯誤");
                }

                if (sheetSetting.IsSheet1SourceFill && sheetSetting.IsSheet1EmpStatusFill)
                {
                    bool isThisPeriod = string.Compare("本期新增", item.Source, true) == 0;
                    bool isNewEmpStatus = newEmpStatus.Contains(item.EmpStatus);

                    if (isThisPeriod && !isNewEmpStatus)
                        msgList.Add($"第{rowNo}筆資料 欄位 員工狀態 欄位值錯誤");
                }
            }
        }

        /// <summary> 驗證機台服務頁籤的欄位值 </summary>
        private void ValidImportValues_Tab2(List<SPA_ScoringInfoModule2Model> list, SPA_ScoringInfoSheetModel sheetSetting, List<string> msgList)
        {
            var serviceFor = this._parameterMgr.GetParametersKeyTextList("SPA服務對象").Select(obj => obj.Text).ToArray();
            var workItem = this._parameterMgr.GetParametersKeyTextList("SPA作業項目").Select(obj => obj.Text).ToArray();

            int rowNo = 0;
            foreach (var item in list)
            {
                rowNo += 1;

                if (sheetSetting.IsSheet2ServiceForFill)
                    this.ValidImportText(item.ServiceFor, "服務對象", rowNo, msgList, serviceFor);

                if (sheetSetting.IsSheet2WorkItemFill)
                    this.ValidImportText(item.WorkItem, "作業項目", rowNo, msgList, workItem);

                if (sheetSetting.IsSheet2MachineNameFill)
                    this.ValidImportText(item.MachineName, "承攬機台名稱", rowNo, msgList);

                if (sheetSetting.IsSheet2MachineNoFill)
                    this.ValidImportText(item.MachineNo, "機台Serial No.", rowNo, msgList);

                if (sheetSetting.IsSheet2OnTimeFill)
                    this.ValidImportYesNo(item.OnTime, "是否準時交付", rowNo, msgList);

                if (sheetSetting.IsSheet2RemarkFill)
                    this.ValidImportText(item.Remark, "備註", rowNo, msgList);
            }
        }

        /// <summary> 驗證必填整數欄位與上下限 </summary>
        private void ValidImportInt(string value, string title, int rowNo, List<string> msgList, int? min = null, int? max = null)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                msgList.Add($"第{rowNo}筆資料 欄位 {title} 為必填欄位");
                return;
            }

            if (!int.TryParse(value, out int parsedInt))
            {
                msgList.Add($"第{rowNo}筆資料 欄位 {title} 欄位值錯誤");
                return;
            }

            if (min.HasValue && parsedInt < min.Value)
                msgList.Add($"第{rowNo}筆資料 欄位 {title} 欄位值錯誤");

            if (max.HasValue && parsedInt > max.Value)
                msgList.Add($"第{rowNo}筆資料 欄位 {title} 欄位值錯誤");
        }

        /// <summary> 驗證必填文字欄位與允許值清單 </summary>
        private void ValidImportText(string value, string title, int rowNo, List<string> msgList, params string[] itemArray)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                msgList.Add($"第{rowNo}筆資料 欄位 {title} 為必填欄位");
                return;
            }

            if (itemArray?.Length > 0 && !itemArray.Contains(value))
                msgList.Add($"第{rowNo}筆資料 欄位 {title} 欄位值錯誤");
        }

        /// <summary> 驗證 YES/NO 欄位格式 </summary>
        private void ValidImportYesNo(string value, string title, int rowNo, List<string> msgList)
        {
            if (string.IsNullOrWhiteSpace(value))
                return;

            var isYes = string.Compare("YES", value, true) == 0;
            var isNo = string.Compare("NO", value, true) == 0;

            if (!isYes && !isNo)
                msgList.Add($"第{rowNo}筆資料 欄位 {title} 欄位值錯誤");
        }
        #endregion

        #region 儲存格工具
        /// <summary> 讀取儲存格文字，日期格式會轉為 yyyy-MM-dd </summary>
        private string GetCellString(IRow row, int cellIndex)
        {
            var cell = row?.GetCell(cellIndex);
            if (cell == null)
                return string.Empty;

            if (cell.CellType == CellType.Numeric && DateUtil.IsCellDateFormatted(cell))
                return cell.DateCellValue.ToString("yyyy-MM-dd");

            return cell.ToString()?.Trim() ?? string.Empty;
        }

        /// <summary> 讀取儲存格日期，文字日期會嘗試轉換為日期 </summary>
        private DateTime? GetCellDate(IRow row, int cellIndex)
        {
            var cell = row?.GetCell(cellIndex);
            if (cell == null)
                return null;

            if (cell.CellType == CellType.Numeric && DateUtil.IsCellDateFormatted(cell))
                return cell.DateCellValue.Date;

            DateTime result;
            if (DateTime.TryParse(this.GetCellString(row, cellIndex), out result))
                return result.Date;

            return null;
        }

        /// <summary> 判斷 Excel 資料列是否為空白列 </summary>
        private bool IsEmptyRow(IRow row)
        {
            if (row == null)
                return true;

            for (int i = 0; i < row.LastCellNum; i++)
            {
                if (!string.IsNullOrWhiteSpace(this.GetCellString(row, i)))
                    return false;
            }

            return true;
        }
        #endregion
    }

    #region 結果模型
    /// <summary> SPA評鑑計分資料匯入結果 </summary>
    public class SPA_ScoringInfoImportResult
    {
        /// <summary> 員工姓名重複錯誤代碼 </summary>
        public const string NameDuplicateCode = "NameDuplicate";

        /// <summary> 匯入結果代碼 </summary>
        public string Code { get; set; }

        /// <summary> 匯入訊息清單 </summary>
        public List<string> Messages { get; set; } = new List<string>();
    }
    #endregion
}
