using BI.Shared.Utils;
using BI.SPA_Evaluation;
using BI.SPA_Evaluation.Models;
using BI.SPA_Evaluation.Models.Exporting;
using BI.SPA_Evaluation.Validators;
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
    public class SPA_EvaluationApiController : ApiController
    {
        private const string _fileUploadPrefix = "Attachment_";

        private SPA_EvaluationManager _mgr = new SPA_EvaluationManager();


        public class TempPager : DataTablePager
        {
            public string period { get; set; }
        }

        #region Query
        //[Route("~/api/SPA_EvaluationApi/GetDataTableList")]
        //[HttpPost]
        //public WebApiDataContainer<SPA_EvaluationModel> PostToGetList([FromBody] TempPager filter)
        //{
        //    DateTime cTime = DateTime.Now;

        //    var cUser = UserProfileService.GetCurrentUser();
        //    if (string.IsNullOrWhiteSpace(cUser.ID))
        //        throw new UnauthorizedAccessException();


        //    var pager = filter.ToPager();
        //    var list = this._mgr.GetList(filter.period, filter.approveStatus, cUser.ID, cTime, pager);

        //    WebApiDataContainer<SPA_EvaluationModel> retList = new WebApiDataContainer<SPA_EvaluationModel>();
        //    retList.recordsFiltered = pager.TotalRow;
        //    retList.recordsTotal = pager.TotalRow;
        //    retList.data = list;

        //    return retList;
        //}


        [Route("~/api/SPA_EvaluationApi/Detail/{id}")]
        [HttpGet]
        // GET api/SPA_EvaluationApi/Detail/{id}
        public SPA_Eva_PeriodModel GetOne([FromUri] Guid id)
        {
            var result = this._mgr.GetOnePeriod(id);
            return result;
        }
        #endregion

        #region Calculate
        /// <summary> 計算分數 </summary>
        /// <param name="inputParameters"></param>
        /// <returns></returns>
        [Route("~/api/SPA_EvaluationApi/Calculate/{period}")]
        [HttpPost]
        public IHttpActionResult Calculate(string period)
        {
            DateTime cTime = DateTime.Now;

            var cUser = UserProfileService.GetCurrentUser();
            if (string.IsNullOrWhiteSpace(cUser.ID))
                throw new UnauthorizedAccessException();

            try
            {
                this._mgr.ComputeScore(period, cUser.ID, cTime);
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
        [Route("~/api/SPA_EvaluationApi/ExportExcel")]
        [HttpGet]
        public IHttpActionResult ExportExcel([FromUri] TempPager filter)
        {
            DateTime cTime = DateTime.Now;

            var cUser = UserProfileService.GetCurrentUser();
            if (string.IsNullOrWhiteSpace(cUser.ID))
                throw new UnauthorizedAccessException();


            if (filter == null)
                filter = new TempPager();

            // 要填入 Excel 的資料
            var list = this._mgr.GetExportList(filter.period, cUser.ID, cTime);

            MemoryStream newMsOutput = this.BuildOutputExcel(list);

            // 提供下載新的 Excel 檔案
            var response = Request.CreateResponse(HttpStatusCode.OK);
            response.Content = new StreamContent(newMsOutput);
            response.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment");
            response.Content.Headers.ContentDisposition.FileName = "供應商SPA評鑑資料匯出範本.xlsx";
            response.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/octet-stream");

            return ResponseMessage(response);
        }

        /// <summary> 產生 Excel </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        private MemoryStream BuildOutputExcel(List<SPA_EvaluationExportModel> list)
        {
            // 讀取範本 xlsx 檔案
            var templatePath = HttpContext.Current.Server.MapPath("~/ModuleResources/Other/SPA_Evaluation/供應商SPA評鑑資料匯出範本.xlsx");
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

                row.CreateCell(00).SetStyle(normalStyle).SetCellValue(periodText);              // 評鑑期間 
                row.CreateCell(01).SetStyle(normalStyle).SetCellValue(item.BU);                 // 評鑑單位
                row.CreateCell(02).SetStyle(normalStyle).SetCellValue(item.POSource);           // PO Source
                row.CreateCell(03).SetStyle(normalStyle).SetCellValue(item.ServiceFor);         // 服務對象
                row.CreateCell(04).SetStyle(normalStyle).SetCellValue(item.BelongTo);           // 受評供應商

                row.CreateCell(05).SetStyle(normalStyle).SetCellValue(item.PerformanceLevel);   // Perfornamce Level
                row.CreateCell(06).SetStyle(normalStyle).SetCellValue(item.TotalScore);         // Total Score
                row.CreateCell(07).SetStyle(normalStyle).SetCellValue(item.TScore);             // T Score
                row.CreateCell(08).SetStyle(normalStyle).SetCellValue(item.DScore);             // D Score
                row.CreateCell(09).SetStyle(normalStyle).SetCellValue(item.QScore);             // Q Score
                row.CreateCell(10).SetStyle(normalStyle).SetCellValue(item.CScore);             // C Score
                row.CreateCell(11).SetStyle(normalStyle).SetCellValue(item.SScore);             // S Score

                row.CreateCell(12).SetStyle(normalStyle).SetCellValue(item.ServiceItem);        // 評鑑項目

                row.CreateCell(13).SetStyle(normalStyle).SetCellValue(item.TScore1);             // T Score1
                row.CreateCell(14).SetStyle(normalStyle).SetCellValue(item.TScore2);             // T Score2
                row.CreateCell(15).SetStyle(normalStyle).SetCellValue(item.DScore1);             // D Score1
                row.CreateCell(16).SetStyle(normalStyle).SetCellValue(item.DScore2);             // D Score2
                row.CreateCell(17).SetStyle(normalStyle).SetCellValue(item.QScore1);             // Q Score1
                row.CreateCell(18).SetStyle(normalStyle).SetCellValue(item.QScore2);             // Q Score2
                row.CreateCell(19).SetStyle(normalStyle).SetCellValue(item.CScore1);             // C Score1
                row.CreateCell(20).SetStyle(normalStyle).SetCellValue(item.CScore2);             // C Score2
                row.CreateCell(21).SetStyle(normalStyle).SetCellValue(item.SScore1);             // S Score1
                row.CreateCell(22).SetStyle(normalStyle).SetCellValue(item.SScore2);             // S Score2
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