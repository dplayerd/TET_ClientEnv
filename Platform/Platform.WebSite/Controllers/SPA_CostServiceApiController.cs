using BI.Shared.Utils;
using BI.SPA_CostService;
using BI.SPA_CostService.Models;
using BI.SPA_CostService.Models.Exporting;
using BI.SPA_CostService.Validators;
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
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace Platform.WebSite.Controllers
{
    public class SPA_CostServiceApiController : ApiController
    {
        private const string _fileUploadPrefix = "Attachment_";

        private SPA_CostServiceManager _mgr = new SPA_CostServiceManager();
        private SPA_CostServiceApprovalManager _approvalMgr = new SPA_CostServiceApprovalManager();


        public class TempPager : DataTablePager
        {
            public string period { get; set; }
            public string[] approveStatus { get; set; }
        }

        #region Query
        [Route("~/api/SPA_CostServiceApi/GetDataTableList")]
        [HttpPost]
        public WebApiDataContainer<SPA_CostServiceModel> PostToGetList([FromBody] TempPager filter)
        {
            DateTime cTime = DateTime.Now;

            var cUser = UserProfileService.GetCurrentUser();
            if (string.IsNullOrWhiteSpace(cUser.ID))
                throw new UnauthorizedAccessException();


            var pager = filter.ToPager();
            var list = this._mgr.GetList(filter.period, filter.approveStatus, cUser.ID, cTime, pager);

            WebApiDataContainer<SPA_CostServiceModel> retList = new WebApiDataContainer<SPA_CostServiceModel>();
            retList.recordsFiltered = pager.TotalRow;
            retList.recordsTotal = pager.TotalRow;
            retList.data = list;

            return retList;
        }


        [Route("~/api/SPA_CostServiceApi/Detail/{id}")]
        [HttpGet]
        // GET api/SPA_CostServiceApi/Detail/{id}
        public SPA_CostServiceModel GetOne([FromUri] Guid id)
        {
            var result = this._mgr.GetOne(id);
            return result;
        }


        /// <summary> 讀取前期的明細資料 </summary>
        /// <param name="period"> 評鑑期間 </param>
        /// <param name="serviceFor"> 服務對象 </param>
        /// <returns></returns>
        [Route("~/api/SPA_CostServiceApi/ReadPrevPeriodDetail")]
        [HttpGet]
        public List<SPA_CostServiceDetailModel> ReadPrevPeriodDetail(string period, string serviceFor)
        {
            DateTime cTime = DateTime.Now;

            var cUser = UserProfileService.GetCurrentUser();
            if (string.IsNullOrWhiteSpace(cUser.ID))
                throw new UnauthorizedAccessException();

            var datePeriod = PeriodUtil.ParsePeriod(period);
            var serviceForArr = serviceFor.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries).Select(obj => obj.Trim()).ToArray();
            var list = this._mgr.GetPrevPeriodDetails(datePeriod, serviceForArr, cUser.ID, cTime);

            return list;
        }

        /// <summary> 是否存在指定的評鑑期間 </summary>
        /// <param name="period"> 評鑑期間 </param>
        /// <returns></returns>
        [Route("~/api/SPA_CostServiceApi/HasSamePeriod/{period}")]
        [HttpGet]
        public bool HasSamePeriod(string period)
        {
            DateTime cTime = DateTime.Now;

            var cUser = UserProfileService.GetCurrentUser();
            if (string.IsNullOrWhiteSpace(cUser.ID))
                throw new UnauthorizedAccessException();

            var result = this._mgr.HasSamePeriod(period, cUser.ID, cTime);
            return result;
        }
        #endregion


        #region CUD
        [Route("~/api/SPA_CostServiceApi/Create")]
        [HttpPost]
        public IHttpActionResult Create()
        {
            // 因為需要上傳檔案，此處原始 Http 來處理
            DateTime cTime = DateTime.Now;

            var cUser = UserProfileService.GetCurrentUser();
            if (string.IsNullOrWhiteSpace(cUser.ID))
                throw new UnauthorizedAccessException();

            var inp = HttpContext.Current.Request.Form["Main"];
            SPA_CostServiceModel model;

            // 嘗試做反序列化，如果錯誤的話丟 Bad Request
            try
            {
                model = JsonConvert.DeserializeObject<SPA_CostServiceModel>(inp);
                if (model == null)
                    return BadRequest("SPA CostService is required.");
            }
            catch (Exception ex)
            {
                return BadRequest("SPA CostService is required.");
            }

            // 取得本次上傳的附件
            if (HttpContext.Current.Request.Files.AllKeys.Any())
            {
                foreach (var key in HttpContext.Current.Request.Files.AllKeys)
                {
                    var httpPostedFile = HttpContext.Current.Request.Files[key];
                    var fileContent = UploadUtil.ConvertToFileContent(httpPostedFile);
                    var indexText = key.Replace(_fileUploadPrefix, "");

                    var indexArr = indexText.Split('_');
                    if (indexArr?.Length != 2)
                        continue;

                    if (int.TryParse(indexArr[0], out int tempDetailIndex) &&
                        int.TryParse(indexArr[1], out int tmpFileIndex))
                    {
                        var detail = model.DetailList.ElementAtOrDefault(tempDetailIndex);
                        detail.UploadFileList.Add(fileContent);
                    }
                }
            }


            // 驗證正確性
            List<string> msgList = new List<string>();
            List<string> tempMsgList;
            var validResult = SPA_CostServiceValidator.Valid(model, out tempMsgList);
            if (!validResult)
                msgList.AddRange(tempMsgList);
            var validDetailResult = SPA_CostServiceDetailValidator.Valid(model.DetailList, out tempMsgList);
            if (!validDetailResult)
                msgList.AddRange(tempMsgList);

            if (!validResult || !validDetailResult)
                return BadRequest(JsonConvert.SerializeObject(msgList));

            // 新增
            try
            {
                var newID = this._mgr.Create(model, cUser.ID, cTime);
                return Ok(newID);
            }
            catch (Exception ex)
            {
                return BadRequest(JsonConvert.SerializeObject(new string[] { ex.Message }));
            }
        }


        [Route("~/api/SPA_CostServiceApi/Modify/{id}")]
        [HttpPost]
        public IHttpActionResult Modify(Guid id)
        {
            // 因為需要上傳檔案，此處原始 Http 來處理
            DateTime cTime = DateTime.Now;

            var cUser = UserProfileService.GetCurrentUser();
            if (string.IsNullOrWhiteSpace(cUser.ID))
                throw new UnauthorizedAccessException();

            var inp = HttpContext.Current.Request.Form["Main"];
            SPA_CostServiceModel model;

            // 嘗試做反序列化，如果錯誤的話丟 Bad Request
            try
            {
                model = JsonConvert.DeserializeObject<SPA_CostServiceModel>(inp);
                if (model == null)
                    return BadRequest("SPA CostService is required.");
            }
            catch (Exception ex)
            {
                return BadRequest("SPA CostService is required.");
            }

            // 取得本次上傳的附件
            if (HttpContext.Current.Request.Files.AllKeys.Any())
            {
                foreach (var key in HttpContext.Current.Request.Files.AllKeys)
                {
                    var httpPostedFile = HttpContext.Current.Request.Files[key];
                    var fileContent = UploadUtil.ConvertToFileContent(httpPostedFile);
                    var indexText = key.Replace(_fileUploadPrefix, "");

                    var indexArr = indexText.Split('_');
                    if (indexArr?.Length != 2)
                        continue;

                    if (int.TryParse(indexArr[0], out int tempDetailIndex) &&
                        int.TryParse(indexArr[1], out int tmpFileIndex))
                    {
                        var detail = model.DetailList.ElementAtOrDefault(tempDetailIndex);
                        detail.UploadFileList.Add(fileContent);
                    }
                }
            }


            // 驗證正確性
            List<string> msgList = new List<string>();
            List<string> tempMsgList;
            var validResult = SPA_CostServiceValidator.Valid(model, out tempMsgList);
            if (!validResult)
                msgList.AddRange(tempMsgList);
            var validDetailResult = SPA_CostServiceDetailValidator.Valid(model.DetailList, out tempMsgList);
            if (!validResult)
                msgList.AddRange(tempMsgList);

            if (!validResult || !validDetailResult)
                return BadRequest(JsonConvert.SerializeObject(msgList));

            // 修改
            try
            {
                this._mgr.Modify(model, cUser.ID, cTime);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(JsonConvert.SerializeObject(new string[] { ex.Message }));
            }
        }
        #endregion

        #region Approval
        [Route("~/api/SPA_CostServiceApi/Submit")]
        [HttpPost]
        public IHttpActionResult Submit()
        {
            // 因為需要上傳檔案，此處原始 Http 來處理
            DateTime cTime = DateTime.Now;

            var cUser = UserProfileService.GetCurrentUser();
            if (string.IsNullOrWhiteSpace(cUser.ID))
                throw new UnauthorizedAccessException();

            var inp = HttpContext.Current.Request.Form["Main"];
            SPA_CostServiceModel model;

            // 嘗試做反序列化，如果錯誤的話丟 Bad Request
            try
            {
                model = JsonConvert.DeserializeObject<SPA_CostServiceModel>(inp);
                if (model == null)
                    return BadRequest("Supplier is required.");
            }
            catch (Exception ex)
            {
                return BadRequest("Supplier is required.");
            }

            // 取得本次上傳的附件
            if (HttpContext.Current.Request.Files.AllKeys.Any())
            {
                foreach (var key in HttpContext.Current.Request.Files.AllKeys)
                {
                    var httpPostedFile = HttpContext.Current.Request.Files[key];
                    var fileContent = UploadUtil.ConvertToFileContent(httpPostedFile);
                    var indexText = key.Replace(_fileUploadPrefix, "");

                    var indexArr = indexText.Split('_');
                    if (indexArr?.Length != 2)
                        continue;

                    if (int.TryParse(indexArr[0], out int tempDetailIndex) &&
                        int.TryParse(indexArr[1], out int tmpFileIndex))
                    {
                        var detail = model.DetailList.ElementAtOrDefault(tempDetailIndex);
                        detail.UploadFileList.Add(fileContent);
                    }
                }
            }

            // 驗證正確性
            List<string> msgList = new List<string>();
            List<string> tempMsgList;
            var validResult = SPA_CostServiceValidator.Valid(model, out tempMsgList);
            if (!validResult)
                msgList.AddRange(tempMsgList);
            var validDetailResult = SPA_CostServiceDetailValidator.Valid(model.DetailList, out tempMsgList);
            if (!validResult)
                msgList.AddRange(tempMsgList);

            if (!validResult || !validDetailResult)
                return BadRequest(JsonConvert.SerializeObject(msgList));


            // 如果還沒新增過，就先新增
            Guid csId;
            if (model.ID.HasValue)
            {
                // 查不到資料也視為要新增
                if (this._mgr.GetOne(model.ID.Value) == null)
                {
                    csId = this._mgr.Create(model, cUser.ID, cTime);
                    model.ID = csId;
                }
                else
                {
                    // 如果查得到，就先存檔，避免漏資料
                    this._mgr.Modify(model, cUser.ID, cTime);
                }
            }
            else
            {
                csId = this._mgr.Create(model, cUser.ID, cTime);
                model.ID = csId;
            }

            // 送出
            try
            {
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

        [Route("~/api/SPA_CostServiceApi/Abord/{id}")]
        [HttpPost]
        public IHttpActionResult Abord([FromBody] AbordTempInputClass input)
        {
            // 因為需要上傳檔案，此處原始 Http 來處理
            DateTime cTime = DateTime.Now;

            var cUser = UserProfileService.GetCurrentUser();
            if (string.IsNullOrWhiteSpace(cUser.ID))
                throw new UnauthorizedAccessException();

            // 中止
            try
            {
                this._approvalMgr.Abord(input.id, input.reason, cUser.ID, cTime);
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
        [Route("~/api/SPA_CostServiceApi/ExportExcel")]
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
            var list = this._mgr.GetExportList(filter.period, filter.approveStatus, cUser.ID, cTime);

            MemoryStream newMsOutput = this.BuildOutputExcel(list);

            // 提供下載新的 Excel 檔案
            var response = Request.CreateResponse(HttpStatusCode.OK);
            response.Content = new StreamContent(newMsOutput);
            response.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment");
            response.Content.Headers.ContentDisposition.FileName = "供應商Cost & Service資料匯出.xlsx";
            response.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/octet-stream");


            return ResponseMessage(response);
        }

        /// <summary> 產生 Excel </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        private MemoryStream BuildOutputExcel(List<SPA_CostServiceExportModel> list)
        {
            // 讀取範本 xlsx 檔案
            var templatePath = HttpContext.Current.Server.MapPath("~/ModuleResources/Other/SPA_CostService/供應商CostService資料匯出範本.xlsx");
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


            // 取得工作表
            ISheet sheet_1 = workbook.GetSheetAt(0);
            // 從第 3 列開始填入資料
            int rowIndex = 1;
            foreach (var item in list)
            {
                rowIndex += 1;
                IRow row = sheet_1.CreateRow(rowIndex);

                var periodText = $"{item.Period} ({item.PeriodStart} ~ {item.PeriodEnd})";

                row.CreateCell(00).SetStyle(normalStyle).SetCellValue(periodText);
                row.CreateCell(01).SetStyle(normalStyle).SetCellValue(item.FillerFullName);
                row.CreateCell(02).SetStyle(normalStyle).SetCellValue(item.IsEvaluate);
                row.CreateCell(03).SetStyle(normalStyle).SetCellValue(item.BU);
                row.CreateCell(04).SetStyle(normalStyle).SetCellValue(item.ServiceFor);
                row.CreateCell(05).SetStyle(normalStyle).SetCellValue(item.BelongTo);
                row.CreateCell(06).SetStyle(normalStyle).SetCellValue(item.POSource);
                row.CreateCell(07).SetStyle(normalStyle).SetCellValue(item.AssessmentItem);
                row.CreateCell(08).SetStyle(normalStyle).SetCellValue(item.PriceDeflator);
                row.CreateCell(09).SetStyle(normalStyle).SetCellValue(item.PaymentTerm);
                row.CreateCell(10).SetStyle(normalStyle).SetCellValue(item.Cooperation);
                row.CreateCell(11).SetStyle(normalStyle).SetCellValue(item.Advantage);
                row.CreateCell(12).SetStyle(normalStyle).SetCellValue(item.Improved);
                row.CreateCell(13).SetStyle(normalStyle).SetCellValue(item.Comment);
                row.CreateCell(14).SetStyle(normalStyle).SetCellValue(item.Remark);
            }

            // 儲存新的 Excel 檔案
            var msOutput = new MemoryStream();
            workbook.Write(msOutput);

            var msNewOutput = new MemoryStream(msOutput.ToArray());
            return msNewOutput;
        }
        #endregion
    }
}