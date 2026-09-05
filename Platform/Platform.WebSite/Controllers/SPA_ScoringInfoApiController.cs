using BI.SPA_ScoringInfo.Models;
using BI.SPA_ScoringInfo.Validators;
using BI.SPA_ScoringInfo;
using Newtonsoft.Json;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using Platform.WebSite.Models;
using Platform.WebSite.Services;
using Platform.WebSite.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Web;
using System.Web.Http;
using BI.SPA_ScoringInfo.Models.Exporting;
using System.Web.UI.WebControls;
using Platform.AbstractionClass;
using System.Web.Http.Results;

namespace Platform.WebSite.Controllers
{
    public class SPA_ScoringInfoApiController : ApiController
    {
        private const string _fileUploadPrefix = "Attachment_";

        private SPA_ScoringInfoManager _mgr = new SPA_ScoringInfoManager();
        private SPA_ScoringInfoModulesManager _detailMgr = new SPA_ScoringInfoModulesManager();
        private SPA_ScoringInfoImportManager _importMgr = new SPA_ScoringInfoImportManager();
        private SPA_ScoringInfoApprovalManager _approvalMgr = new SPA_ScoringInfoApprovalManager();
        private SPA_ScoringInfoSheetManager _sheetMgr = new SPA_ScoringInfoSheetManager();


        public class TempPager : DataTablePager
        {
            public string period { get; set; }
            public string[] bu { get; set; }
            public string[] serviceFor { get; set; }
            public string[] serviceItem { get; set; }
            public string[] approveStatus { get; set; }
            public string[] belongTo { get; set; }
        }

        #region Query
        [Route("~/api/SPA_ScoringInfoApi/GetDataTableList")]
        [HttpPost]
        public WebApiDataContainer<SPA_ScoringInfoModel> PostToGetList([FromBody] TempPager filter)
        {
            DateTime cTime = DateTime.Now;

            var cUser = UserProfileService.GetCurrentUser();
            if (string.IsNullOrWhiteSpace(cUser.ID))
                throw new UnauthorizedAccessException();


            var pager = filter.ToPager();
            var list = this._mgr.GetList(filter.period, filter.bu, filter.serviceFor, filter.serviceItem, filter.approveStatus, filter.belongTo, cUser.ID, cTime, pager);

            WebApiDataContainer<SPA_ScoringInfoModel> retList = new WebApiDataContainer<SPA_ScoringInfoModel>();
            retList.recordsFiltered = pager.TotalRow;
            retList.recordsTotal = pager.TotalRow;
            retList.data = list;

            return retList;
        }


        [Route("~/api/SPA_ScoringInfoApi/Detail/{id}")]
        [HttpGet]
        // GET api/SPA_ScoringInfoApi/Detail/{id}
        public SPA_ScoringInfoModel GetOne([FromUri] Guid id)
        {
            var result = this._mgr.GetOne(id);
            return result;
        }
        #endregion


        #region CUD
        [Route("~/api/SPA_ScoringInfoApi/Modify_Tab1/{id}")]
        [HttpPost]
        public IHttpActionResult Modify_Tab1(Guid id, SPA_ScoringInfoModel model)
        {
            // 因為需要上傳檔案，此處原始 Http 來處理
            DateTime cTime = DateTime.Now;

            var cUser = UserProfileService.GetCurrentUser();
            if (string.IsNullOrWhiteSpace(cUser.ID))
                throw new UnauthorizedAccessException();


            // 驗證正確性
            List<string> msgList = new List<string>();
            List<string> tempMsgList;
            var sheetSetting = this.GetSheetSetting(model, msgList);
            var validResult = SPA_ScoringInfoValidator.Valid(model, out tempMsgList);
            if (!validResult)
                msgList.AddRange(tempMsgList);
            var validDetailResult = false;
            tempMsgList = new List<string>();
            if (sheetSetting != null)
                validDetailResult = SPA_ScoringInfoModule1Validator.Valid(model.Module1List, sheetSetting, out tempMsgList);
            if (!validDetailResult)
                msgList.AddRange(tempMsgList);

            if (!validResult || !validDetailResult)
                return BadRequest(JsonConvert.SerializeObject(msgList));

            try
            {
                model.SheetSetting = sheetSetting;
                // 修改
                this._detailMgr.Modify_Module1(model, model.Module1List, cUser.ID, cTime);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(JsonConvert.SerializeObject(new string[] { ex.Message }));
            }
        }

        [Route("~/api/SPA_ScoringInfoApi/Modify_Tab2/{id}")]
        [HttpPost]
        public IHttpActionResult Modify_Tab2(Guid id, SPA_ScoringInfoModel model)
        {
            // 因為需要上傳檔案，此處原始 Http 來處理
            DateTime cTime = DateTime.Now;

            var cUser = UserProfileService.GetCurrentUser();
            if (string.IsNullOrWhiteSpace(cUser.ID))
                throw new UnauthorizedAccessException();


            // 驗證正確性
            List<string> msgList = new List<string>();
            List<string> tempMsgList;
            var sheetSetting = this.GetSheetSetting(model, msgList);
            var validResult = SPA_ScoringInfoValidator.Valid(model, out tempMsgList);
            if (!validResult)
                msgList.AddRange(tempMsgList);
            var validDetailResult = false;
            tempMsgList = new List<string>();
            if (sheetSetting != null)
                validDetailResult = SPA_ScoringInfoModule2Validator.Valid(model, model.Module2List, sheetSetting, out tempMsgList);
            if (!validDetailResult)
                msgList.AddRange(tempMsgList);

            if (!validResult || !validDetailResult)
                return BadRequest(JsonConvert.SerializeObject(msgList));

            try
            {
                model.SheetSetting = sheetSetting;
                // 修改
                this._detailMgr.Modify_Module2(model, model.Module2List, cUser.ID, cTime);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(JsonConvert.SerializeObject(new string[] { ex.Message }));
            }
        }


        [Route("~/api/SPA_ScoringInfoApi/Modify_Tab3/{id}")]
        [HttpPost]
        public IHttpActionResult Modify_Tab3(Guid id, SPA_ScoringInfoModel model)
        {
            // 因為需要上傳檔案，此處原始 Http 來處理
            DateTime cTime = DateTime.Now;

            var cUser = UserProfileService.GetCurrentUser();
            if (string.IsNullOrWhiteSpace(cUser.ID))
                throw new UnauthorizedAccessException();


            // 驗證正確性
            List<string> msgList = new List<string>();
            List<string> tempMsgList;
            var sheetSetting = this.GetSheetSetting(model, msgList);
            var validResult = false;
            tempMsgList = new List<string>();
            if (sheetSetting != null)
                validResult = SPA_ScoringInfoValidator.Valid_Tab3(model, sheetSetting, out tempMsgList);
            if (!validResult)
                msgList.AddRange(tempMsgList);
            var validDetailResult = false;
            tempMsgList = new List<string>();
            if (sheetSetting != null)
                validDetailResult = SPA_ScoringInfoModule3Validator.Valid(model.Module3List, sheetSetting, out tempMsgList);
            if (!validDetailResult)
                msgList.AddRange(tempMsgList);

            if (!validResult || !validDetailResult)
                return BadRequest(JsonConvert.SerializeObject(msgList));

            try
            {
                model.SheetSetting = sheetSetting;
                // 修改
                this._detailMgr.Modify_Module3(model, model.Module3List, cUser.ID, cTime);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(JsonConvert.SerializeObject(new string[] { ex.Message }));
            }
        }

        [Route("~/api/SPA_ScoringInfoApi/Import_Tab1/{id}")]
        [HttpPost]
        public IHttpActionResult Import_Tab1(Guid id)
        {
            return SharedImportTab1(id, true);
        }


        [Route("~/api/SPA_ScoringInfoApi/Import_Tab1_Force/{id}")]
        [HttpPost]
        public IHttpActionResult Import_Tab1_Force(Guid id)
        {
            return SharedImportTab1(id, false);
        }


        /// <summary> 共用匯入 Tab1 資料 </summary>
        /// <param name="id"></param>
        /// <param name="validNameDuplicate"> 是否檢查姓名重覆檢查 </param>
        /// <returns></returns>
        /// <exception cref="UnauthorizedAccessException"></exception>
        private IHttpActionResult SharedImportTab1(Guid id, bool validNameDuplicate)
        {
            DateTime cTime = DateTime.Now;

            var cUser = UserProfileService.GetCurrentUser();
            if (string.IsNullOrWhiteSpace(cUser.ID))
                throw new UnauthorizedAccessException();

            try
            {
                var file = this.GetUploadFile();
                var result = this._importMgr.ImportTab1(id, file.InputStream, validNameDuplicate, cUser.ID, cTime);
                return this.ToImportActionResult(result);
            }
            catch (Exception ex)
            {
                return BadRequest(JsonConvert.SerializeObject(new string[] { ex.Message }));
            }
        }

        [Route("~/api/SPA_ScoringInfoApi/Import_Tab2/{id}")]
        [HttpPost]
        public IHttpActionResult Import_Tab2(Guid id)
        {
            DateTime cTime = DateTime.Now;

            var cUser = UserProfileService.GetCurrentUser();
            if (string.IsNullOrWhiteSpace(cUser.ID))
                throw new UnauthorizedAccessException();

            try
            {
                var file = this.GetUploadFile();
                var result = this._importMgr.ImportTab2(id, file.InputStream, cUser.ID, cTime);
                return this.ToImportActionResult(result);
            }
            catch (Exception ex)
            {
                return BadRequest(JsonConvert.SerializeObject(new string[] { ex.Message }));
            }
        }

        [Route("~/api/SPA_ScoringInfoApi/Import_Tab3/{id}")]
        [HttpPost]
        public IHttpActionResult Import_Tab3(Guid id)
        {
            DateTime cTime = DateTime.Now;

            var cUser = UserProfileService.GetCurrentUser();
            if (string.IsNullOrWhiteSpace(cUser.ID))
                throw new UnauthorizedAccessException();

            try
            {
                var file = this.GetUploadFile();
                var workerCountText = HttpContext.Current.Request.Form["WorkerCount"];
                var result = this._importMgr.ImportTab3(id, file.InputStream, workerCountText, cUser.ID, cTime);
                return this.ToImportActionResult(result);
            }
            catch (Exception ex)
            {
                return BadRequest(JsonConvert.SerializeObject(new string[] { ex.Message }));
            }
        }

        private HttpPostedFile GetUploadFile()
        {
            if (!HttpContext.Current.Request.Files.AllKeys.Any())
                throw new ArgumentException("請選擇匯入檔案。");

            var file = HttpContext.Current.Request.Files[0];
            if (file == null || file.ContentLength == 0)
                throw new ArgumentException("請選擇匯入檔案。");

            return file;
        }

        private IHttpActionResult ToImportActionResult(SPA_ScoringInfoImportResult result)
        {
            if (result.Code == SPA_ScoringInfoImportResult.NameDuplicateCode)
            {
                return Content(HttpStatusCode.Conflict, new
                {
                    Code = result.Code,
                    Messages = result.Messages
                });
            }

            if (result.Messages.Any())
                return BadRequest(JsonConvert.SerializeObject(result.Messages));

            return Ok();
        }

        [Route("~/api/SPA_ScoringInfoApi/Modify_Tab4/{id}")]
        [HttpPost]
        public IHttpActionResult Modify_Tab4(Guid id, SPA_ScoringInfoModel model)
        {
            // 因為需要上傳檔案，此處原始 Http 來處理
            DateTime cTime = DateTime.Now;

            var cUser = UserProfileService.GetCurrentUser();
            if (string.IsNullOrWhiteSpace(cUser.ID))
                throw new UnauthorizedAccessException();


            // 驗證正確性
            List<string> msgList = new List<string>();
            List<string> tempMsgList;
            var sheetSetting = this.GetSheetSetting(model, msgList);
            var validResult = false;
            tempMsgList = new List<string>();
            if (sheetSetting != null)
                validResult = SPA_ScoringInfoValidator.Valid_Tab4(model, sheetSetting, out tempMsgList);
            if (!validResult)
                msgList.AddRange(tempMsgList);

            if (!validResult || sheetSetting == null)
                return BadRequest(JsonConvert.SerializeObject(msgList));

            try
            {
                model.SheetSetting = sheetSetting;
                // 修改
                this._mgr.Modify_Tab4(model, cUser.ID, cTime);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(JsonConvert.SerializeObject(new string[] { ex.Message }));
            }
        }


        [Route("~/api/SPA_ScoringInfoApi/Modify_Tab5/{id}")]
        [HttpPost]
        public IHttpActionResult Modify_Tab5(Guid id, SPA_ScoringInfoModel model)
        {
            // 因為需要上傳檔案，此處原始 Http 來處理
            DateTime cTime = DateTime.Now;

            var cUser = UserProfileService.GetCurrentUser();
            if (string.IsNullOrWhiteSpace(cUser.ID))
                throw new UnauthorizedAccessException();


            // 驗證正確性
            List<string> msgList = new List<string>();
            List<string> tempMsgList;
            var sheetSetting = this.GetSheetSetting(model, msgList);
            var validResult = false;
            tempMsgList = new List<string>();
            if (sheetSetting != null)
                validResult = SPA_ScoringInfoValidator.Valid_Tab5(model, sheetSetting, out tempMsgList);
            if (!validResult)
                msgList.AddRange(tempMsgList);

            if (!validResult || sheetSetting == null)
                return BadRequest(JsonConvert.SerializeObject(msgList));

            try
            {
                model.SheetSetting = sheetSetting;
                // 修改
                this._mgr.Modify_Tab5(model, cUser.ID, cTime);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(JsonConvert.SerializeObject(new string[] { ex.Message }));
            }
        }



        [Route("~/api/SPA_ScoringInfoApi/Modify_Tab6/{id}")]
        [HttpPost]
        public IHttpActionResult Modify_Tab6(Guid id, SPA_ScoringInfoModel model)
        {
            // 因為需要上傳檔案，此處原始 Http 來處理
            DateTime cTime = DateTime.Now;

            var cUser = UserProfileService.GetCurrentUser();
            if (string.IsNullOrWhiteSpace(cUser.ID))
                throw new UnauthorizedAccessException();


            // 驗證正確性
            List<string> msgList = new List<string>();
            List<string> tempMsgList;
            var sheetSetting = this.GetSheetSetting(model, msgList);
            var validResult = false;
            tempMsgList = new List<string>();
            if (sheetSetting != null)
                validResult = SPA_ScoringInfoValidator.Valid_Tab6(model, sheetSetting, out tempMsgList);
            if (!validResult)
                msgList.AddRange(tempMsgList);

            var validDetailResult = sheetSetting != null && SPA_ScoringInfoModule4Validator.Valid(model.Module4List, sheetSetting, out tempMsgList);
            if (!validDetailResult)
                msgList.AddRange(tempMsgList);

            if (!validResult || !validDetailResult || sheetSetting == null)
                return BadRequest(JsonConvert.SerializeObject(msgList));

            try
            {
                model.SheetSetting = sheetSetting;
                // 修改
                this._detailMgr.Modify_Module4(model, model.Module4List, cUser.ID, cTime);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(JsonConvert.SerializeObject(new string[] { ex.Message }));
            }
        }


        [Route("~/api/SPA_ScoringInfoApi/Modify_Tab7/{id}")]
        [HttpPost]
        public IHttpActionResult Modify_Tab7(Guid id)
        {
            // 因為需要上傳檔案，此處原始 Http 來處理
            DateTime cTime = DateTime.Now;

            var cUser = UserProfileService.GetCurrentUser();
            if (string.IsNullOrWhiteSpace(cUser.ID))
                throw new UnauthorizedAccessException();


            var inp = HttpContext.Current.Request.Form["Main"];
            SPA_ScoringInfoModel model;

            // 嘗試做反序列化，如果錯誤的話丟 Bad Request
            try
            {
                model = JsonConvert.DeserializeObject<SPA_ScoringInfoModel>(inp);
                if (model == null)
                    return BadRequest("SPA CostService is required.");
            }
            catch (Exception ex)
            {
                return BadRequest("SPA CostService is required.");
            }


            // 取得本次上傳的附件
            List<FileContent> fileUploads = new List<FileContent>();
            if (HttpContext.Current.Request.Files.AllKeys.Any())
            {
                foreach (var key in HttpContext.Current.Request.Files.AllKeys)
                {
                    var httpPostedFile = HttpContext.Current.Request.Files[key];
                    var fileContent = UploadUtil.ConvertToFileContent(httpPostedFile);

                    fileUploads.Add(fileContent);
                }
            }


            // 驗證正確性
            List<string> msgList = new List<string>();
            List<string> tempMsgList;
            var sheetSetting = this.GetSheetSetting(model, msgList);
            var validResult = SPA_ScoringInfoValidator.Valid(model, out tempMsgList);
            if (!validResult)
                msgList.AddRange(tempMsgList);

            if (!validResult || sheetSetting == null)
                return BadRequest(JsonConvert.SerializeObject(msgList));

            try
            {
                model.SheetSetting = sheetSetting;
                // 修改
                this._mgr.Modify_Tab7(model, fileUploads, cUser.ID, cTime);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(JsonConvert.SerializeObject(new string[] { ex.Message }));
            }
        }
        #endregion


        #region Approval
        [Route("~/api/SPA_ScoringInfoApi/Submit")]
        [HttpPost]
        public IHttpActionResult Submit(SPA_ScoringInfoModel model)
        {
            // 因為需要上傳檔案，此處原始 Http 來處理
            DateTime cTime = DateTime.Now;

            var cUser = UserProfileService.GetCurrentUser();
            if (string.IsNullOrWhiteSpace(cUser.ID))
                throw new UnauthorizedAccessException();

            try
            {
                var dbModel = this._mgr.GetOne(model.ID.Value);
                List<string> msgList = new List<string>();
                var sheetSetting = this.GetSheetSetting(dbModel, msgList);
                if (sheetSetting == null)
                    return BadRequest(JsonConvert.SerializeObject(msgList));

                // 送出
                this._approvalMgr.Submit(model.ID.Value, cUser.ID, cTime);
                return Ok(model.ID);
            }
            catch (Exception ex)
            {
                return BadRequest(JsonConvert.SerializeObject(new string[] { ex.Message }));
            }
        }

        public class AbordTempInputClass
        {
            public Guid id { get; set; }
            public string reason { get; set; }
        }

        [Route("~/api/SPA_ScoringInfoApi/Abord/{id}")]
        [HttpPost]
        public IHttpActionResult Abord([FromBody] AbordTempInputClass input)
        {
            // 因為需要上傳檔案，此處原始 Http 來處理
            DateTime cTime = DateTime.Now;

            var cUser = UserProfileService.GetCurrentUser();
            if (string.IsNullOrWhiteSpace(cUser.ID))
                throw new UnauthorizedAccessException();

            try
            {
                // 中止
                this._approvalMgr.Abord(input.id, input.reason, cUser.ID, cTime);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(JsonConvert.SerializeObject(new string[] { ex.Message }));
            }
        }

        [Route("~/api/SPA_ScoringInfoApi/NotEvaluate/{id}")]
        [HttpPost]
        public IHttpActionResult NotEvaluate(Guid id)
        {
            DateTime cTime = DateTime.Now;

            var cUser = UserProfileService.GetCurrentUser();
            if (string.IsNullOrWhiteSpace(cUser.ID))
                throw new UnauthorizedAccessException();

            try
            {
                this._approvalMgr.MarkAsNotEvaluate(id, cUser.ID, cTime);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(JsonConvert.SerializeObject(new string[] { ex.Message }));
            }
        }
        #endregion

        #region Export
        /// <summary> 列表頁匯出 Excel </summary>
        /// <param name="inputParameters"></param>
        /// <returns></returns>
        [Route("~/api/SPA_ScoringInfoApi/ExportExcel")]
        [HttpGet]
        public IHttpActionResult ExportExcel([FromUri] TempPager filter)
        {
            DateTime cTime = DateTime.Now;

            var cUser = UserProfileService.GetCurrentUser();
            if (string.IsNullOrWhiteSpace(cUser.ID))
                throw new UnauthorizedAccessException();


            if (filter == null)
                filter = new TempPager();

            if (filter.approveStatus != null)
                filter.approveStatus = filter.approveStatus.Where(obj => obj != null).ToArray();

            // 要填入 Excel 的資料
            var report = this._mgr.GetExportingReport(filter.period, filter.bu, filter.serviceFor, filter.serviceItem, filter.belongTo, filter.approveStatus, cUser.ID, cTime);

            MemoryStream newMsOutput = this.BuildOutputExcel(report);

            // 提供下載新的 Excel 檔案
            var response = Request.CreateResponse(HttpStatusCode.OK);
            response.Content = new StreamContent(newMsOutput);
            response.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment");
            response.Content.Headers.ContentDisposition.FileName = "供應商SPA評鑑計分資料匯出.xlsx";
            response.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/octet-stream");


            return ResponseMessage(response);
        }

        /// <summary> 產生 Excel </summary>
        /// <param name="reportModel"></param>
        /// <returns></returns>
        private MemoryStream BuildOutputExcel(SPA_ScoringInfoExportModel reportModel)
        {
            // 讀取範本 xlsx 檔案
            var templatePath = HttpContext.Current.Server.MapPath("~/ModuleResources/Other/SPA_ScoringInfo/供應商SPA評鑑計分資料匯出範本.xlsx");
            var templateStream = new FileStream(templatePath, FileMode.Open, FileAccess.Read);

            // 創建新的工作簿
            IWorkbook workbook = new XSSFWorkbook(templateStream);

            var font1 = workbook.CreateFont();
            font1.FontHeightInPoints = 10;
            ICellStyle wrapStyle = workbook.CreateCellStyle();
            wrapStyle.WrapText = true;
            wrapStyle.SetFont(font1);

            var font2 = workbook.CreateFont();
            font2.FontHeightInPoints = 10;
            ICellStyle normalStyle = workbook.CreateCellStyle();
            normalStyle.SetFont(font2);

            // 取得工作表 - 頁籤 0 - 計分資料主檔
            ISheet sheet_0 = workbook.GetSheetAt(0);
            // 從第 3 列開始填入資料
            int rowIndex_tab0 = 0;
            foreach (var item in reportModel.BaseList)
            {
                rowIndex_tab0 += 1;
                IRow row = sheet_0.CreateRow(rowIndex_tab0);

                var periodText = $"{item.Period} ({item.PeriodStart} ~ {item.PeriodEnd})";

                row.CreateCell(00).SetStyle(normalStyle).SetCellValue(periodText);
                row.CreateCell(01).SetStyle(normalStyle).SetCellValue(item.BU);
                row.CreateCell(02).SetStyle(normalStyle).SetCellValue(item.ServiceItem);
                row.CreateCell(03).SetStyle(normalStyle).SetCellValue(item.ServiceFor);
                row.CreateCell(04).SetStyle(normalStyle).SetCellValue(item.BelongTo);

                row.CreateCell(05).SetStyle(normalStyle).SetCellValue(item.POSource);
                row.CreateCell(06).SetStyle(normalStyle).SetCellValue(item.MOCount);
                row.CreateCell(07).SetStyle(normalStyle).SetCellValue(item.TELLoss);
                row.CreateCell(08).SetStyle(normalStyle).SetCellValue(item.CustomerLoss);
                row.CreateCell(09).SetStyle(normalStyle).SetCellValue(item.Accident);

                row.CreateCell(10).SetStyle(normalStyle).SetCellValue(item.WorkerCount?.ToString());
                row.CreateCell(11).SetStyle(normalStyle).SetCellValue(item.Cooperation);
                row.CreateCell(12).SetStyle(normalStyle).SetCellValue(item.Complain);
                row.CreateCell(13).SetStyle(normalStyle).SetCellValue(item.Advantage);
                row.CreateCell(14).SetStyle(normalStyle).SetCellValue(item.Improved);

                row.CreateCell(15).SetStyle(normalStyle).SetCellValue(item.Comment);
            }

            // 取得工作表 - 頁籤 1 - 人力盤點
            ISheet sheet_1 = workbook.GetSheetAt(1);
            // 從第 3 列開始填入資料
            int rowIndex_tab1 = 0;
            foreach (var item in reportModel.Tab1List)
            {
                rowIndex_tab1 += 1;
                IRow row = sheet_1.CreateRow(rowIndex_tab1);

                var periodText = $"{item.Period} ({item.PeriodStart} ~ {item.PeriodEnd})";

                row.CreateCell(00).SetStyle(normalStyle).SetCellValue(periodText);
                row.CreateCell(01).SetStyle(normalStyle).SetCellValue(item.BU);
                row.CreateCell(02).SetStyle(normalStyle).SetCellValue(item.ServiceFor);
                row.CreateCell(03).SetStyle(normalStyle).SetCellValue(item.ServiceItem);
                row.CreateCell(04).SetStyle(normalStyle).SetCellValue(item.BelongTo);

                row.CreateCell(05).SetStyle(normalStyle).SetCellValue(item.Type);
                row.CreateCell(06).SetStyle(normalStyle).SetCellValue(item.Supplier);
                row.CreateCell(07).SetStyle(normalStyle).SetCellValue(item.EmpName);
                row.CreateCell(08).SetStyle(normalStyle).SetCellValue(item.MajorJob);
                row.CreateCell(09).SetStyle(normalStyle).SetCellValue(item.IsIndependent);

                row.CreateCell(10).SetStyle(normalStyle).SetCellValue(item.SkillLevel);
                row.CreateCell(11).SetStyle(normalStyle).SetCellValue(item.EmpStatus);
                row.CreateCell(12).SetStyle(normalStyle).SetCellValue(item.TELSeniorityY);
                row.CreateCell(13).SetStyle(normalStyle).SetCellValue(item.TELSeniorityM);
                row.CreateCell(14).SetStyle(normalStyle).SetCellValue(item.Remark);
            }

            // 取得工作表 - 頁籤 2 - 施工達交狀況
            ISheet sheet_2 = workbook.GetSheetAt(2);
            // 從第 3 列開始填入資料
            int rowIndex_tab2 = 0;
            foreach (var item in reportModel.Tab2List)
            {
                rowIndex_tab2 += 1;
                IRow row = sheet_2.CreateRow(rowIndex_tab2);

                var periodText = $"{item.Period} ({item.PeriodStart} ~ {item.PeriodEnd})";

                row.CreateCell(00).SetStyle(normalStyle).SetCellValue(periodText);
                row.CreateCell(01).SetStyle(normalStyle).SetCellValue(item.BU);
                row.CreateCell(02).SetStyle(normalStyle).SetCellValue(item.ServiceFor);
                row.CreateCell(03).SetStyle(normalStyle).SetCellValue(item.ServiceItem);
                row.CreateCell(04).SetStyle(normalStyle).SetCellValue(item.BelongTo);

                row.CreateCell(05).SetStyle(normalStyle).SetCellValue(item.WorkItem);
                row.CreateCell(06).SetStyle(normalStyle).SetCellValue(item.MachineName);
                row.CreateCell(07).SetStyle(normalStyle).SetCellValue(item.MachineNo);
                row.CreateCell(08).SetStyle(normalStyle).SetCellValue(item.OnTime);
                row.CreateCell(09).SetStyle(normalStyle).SetCellValue(item.Remark);
            }


            // 取得工作表 - 頁籤 3 - 施工正確性
            ISheet sheet_3 = workbook.GetSheetAt(3);
            // 從第 3 列開始填入資料
            int rowIndex_tab3 = 0;
            foreach (var item in reportModel.Tab3List)
            {
                rowIndex_tab3 += 1;
                IRow row = sheet_3.CreateRow(rowIndex_tab3);

                var periodText = $"{item.Period} ({item.PeriodStart} ~ {item.PeriodEnd})";

                row.CreateCell(00).SetStyle(normalStyle).SetCellValue(periodText);
                row.CreateCell(01).SetStyle(normalStyle).SetCellValue(item.BU);
                row.CreateCell(02).SetStyle(normalStyle).SetCellValue(item.ServiceFor);
                row.CreateCell(03).SetStyle(normalStyle).SetCellValue(item.ServiceItem);
                row.CreateCell(04).SetStyle(normalStyle).SetCellValue(item.BelongTo);

                row.CreateCell(05).SetStyle(normalStyle).SetCellValue(item.Date?.ToString("yyyy-MM-dd") ?? string.Empty);
                row.CreateCell(06).SetStyle(normalStyle).SetCellValue(item.Location);
                row.CreateCell(07).SetStyle(normalStyle).SetCellValue(item.TELLoss);
                row.CreateCell(08).SetStyle(normalStyle).SetCellValue(item.CustomerLoss);
                row.CreateCell(09).SetStyle(normalStyle).SetCellValue(item.Accident);

                row.CreateCell(10).SetStyle(normalStyle).SetCellValue(item.Description);
            }

            // 取得工作表 - 頁籤 4 - 作業正確性&人員備齊貢獻度
            ISheet sheet_4 = workbook.GetSheetAt(4);
            // 從第 3 列開始填入資料
            int rowIndex_tab4 = 0;
            foreach (var item in reportModel.Tab4List)
            {
                rowIndex_tab4 += 1;
                IRow row = sheet_4.CreateRow(rowIndex_tab4);

                var periodText = $"{item.Period} ({item.PeriodStart} ~ {item.PeriodEnd})";

                row.CreateCell(00).SetStyle(normalStyle).SetCellValue(periodText);
                row.CreateCell(01).SetStyle(normalStyle).SetCellValue(item.BU);
                row.CreateCell(02).SetStyle(normalStyle).SetCellValue(item.ServiceFor);
                row.CreateCell(03).SetStyle(normalStyle).SetCellValue(item.ServiceItem);
                row.CreateCell(04).SetStyle(normalStyle).SetCellValue(item.BelongTo);

                row.CreateCell(05).SetStyle(normalStyle).SetCellValue(item.Correctness);
                row.CreateCell(06).SetStyle(normalStyle).SetCellValue(item.Contribution);
            }

            // 取得工作表 - 頁籤 5 - 自訓能力
            ISheet sheet_5 = workbook.GetSheetAt(5);
            // 從第 3 列開始填入資料
            int rowIndex_tab5 = 0;
            foreach (var item in reportModel.Tab5List)
            {
                rowIndex_tab5 += 1;
                IRow row = sheet_5.CreateRow(rowIndex_tab5);

                var periodText = $"{item.Period} ({item.PeriodStart} ~ {item.PeriodEnd})";

                row.CreateCell(00).SetStyle(normalStyle).SetCellValue(periodText);
                row.CreateCell(01).SetStyle(normalStyle).SetCellValue(item.BU);
                row.CreateCell(02).SetStyle(normalStyle).SetCellValue(item.ServiceFor);
                row.CreateCell(03).SetStyle(normalStyle).SetCellValue(item.ServiceItem);
                row.CreateCell(04).SetStyle(normalStyle).SetCellValue(item.BelongTo);

                row.CreateCell(05).SetStyle(normalStyle).SetCellValue(item.SelfTraining);
                row.CreateCell(06).SetStyle(normalStyle).SetCellValue(item.SelfTrainingRemark);
            }

            // 取得工作表 - 頁籤 1 - 服務
            ISheet sheet_6 = workbook.GetSheetAt(6);
            // 從第 3 列開始填入資料
            int rowIndex_tab6 = 0;
            foreach (var item in reportModel.Tab6List)
            {
                rowIndex_tab6 += 1;
                IRow row = sheet_6.CreateRow(rowIndex_tab6);

                var periodText = $"{item.Period} ({item.PeriodStart} ~ {item.PeriodEnd})";

                row.CreateCell(00).SetStyle(normalStyle).SetCellValue(periodText);
                row.CreateCell(01).SetStyle(normalStyle).SetCellValue(item.BU);
                row.CreateCell(02).SetStyle(normalStyle).SetCellValue(item.ServiceFor);
                row.CreateCell(03).SetStyle(normalStyle).SetCellValue(item.ServiceItem);
                row.CreateCell(04).SetStyle(normalStyle).SetCellValue(item.BelongTo);

                row.CreateCell(05).SetStyle(normalStyle).SetCellValue(item.Date?.ToString("yyyy-MM-dd") ?? string.Empty);
                row.CreateCell(06).SetStyle(normalStyle).SetCellValue(item.Location);
                row.CreateCell(07).SetStyle(normalStyle).SetCellValue(item.IsDamage);
                row.CreateCell(08).SetStyle(normalStyle).SetCellValue(item.Description);
            }

            // 儲存新的 Excel 檔案
            var msOutput = new MemoryStream();
            workbook.Write(msOutput);

            var msNewOutput = new MemoryStream(msOutput.ToArray());
            return msNewOutput;
        }
        #endregion

        #region Private
        private SPA_ScoringInfoSheetModel GetSheetSetting(SPA_ScoringInfoModel model, List<string> msgList)
        {
            if (model == null)
            {
                msgList.Add("SPA評鑑計分資料不存在。");
                return null;
            }

            if ((string.IsNullOrWhiteSpace(model.ServiceItem) || string.IsNullOrWhiteSpace(model.POSource)) && model.ID.HasValue)
            {
                var dbModel = this._mgr.GetOne(model.ID.Value);
                if (dbModel != null)
                {
                    model.ServiceItem = dbModel.ServiceItem;
                    model.POSource = dbModel.POSource;
                }
            }

            var sheetSetting = this._sheetMgr.GetDetail(model.ServiceItem, model.POSource);
            if (sheetSetting == null)
                msgList.Add($"找不到 SPA評鑑計分資料頁籤顯示設定，評鑑項目：{model.ServiceItem}，PO Source：{model.POSource}。請先維護設定後再存檔。");

            return sheetSetting;
        }
        #endregion

    }
}
