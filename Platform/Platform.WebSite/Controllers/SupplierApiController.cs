using BI.STQA;
using BI.STQA.Models;
using BI.Suppliers;
using BI.Suppliers.Enums;
using BI.Suppliers.Models;
using BI.Suppliers.Utils;
using BI.Suppliers.Validators;
using BI.Shared;
using Newtonsoft.Json;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using Platform.AbstractionClass;
using Platform.Auth;
using Platform.WebSite.Filters;
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
using System.Web.UI.WebControls;

namespace Platform.WebSite.Controllers
{
    [WebApiAuthorizeCore]
    public class SupplierApiController : ApiController
    {
        private TET_SupplierManager _mgr = new TET_SupplierManager();
        private TET_SupplierContactManager _contactMgr = new TET_SupplierContactManager();
        private TET_STQAManager _stqaMgr = new TET_STQAManager();
        private SupplierTradeManager _tradeManager = new SupplierTradeManager();
        private UserManager _userMgr = new UserManager();

        public class TempPager : DataTablePager, ISupplierListQueryInput
        {
            public string caption { get; set; }
            public string taxNo { get; set; }
            public string[] belongTo { get; set; }
            public string[] supplierCategory { get; set; }
            public string[] businessCategory { get; set; }
            public string[] businessAttribute { get; set; }
            public string mainProduct { get; set; }
            public string searchKey { get; set; }
            public string keySupplier { get; set; }
            public string stqaCertified { get; set; }
            public string[] buyer { get; set; }
        }

        public class AbordTempInputClass
        {
            public Guid id { get; set; }
            public string reason { get; set; }
        }

        [Route("~/api/SupplierApi/GetDataTableList")]
        [HttpPost]
        public WebApiDataContainer<SupplierListModel> PostToGetList([FromBody] TempPager inputParameters)
        {
            DateTime cTime = DateTime.Now;

            var cUser = UserProfileService.GetCurrentUser();
            if (string.IsNullOrWhiteSpace(cUser.ID))
                throw new UnauthorizedAccessException();


            var pager = inputParameters.ToPager();
            var list = this._mgr.GetTET_SupplierList(inputParameters, cUser.ID, pager);

            WebApiDataContainer<SupplierListModel> retList = new WebApiDataContainer<SupplierListModel>();
            retList.recordsFiltered = pager.TotalRow;
            retList.recordsTotal = pager.TotalRow;
            retList.data = list;

            return retList;
        }

        [Route("~/api/SupplierApi/QueryListSS")]
        [HttpPost]
        public WebApiDataContainer<TET_SupplierModel> QueryListSS([FromBody] TempPager inputParameters)
        {
            DateTime cTime = DateTime.Now;

            var cUser = UserProfileService.GetCurrentUser();
            if (string.IsNullOrWhiteSpace(cUser.ID))
                throw new UnauthorizedAccessException();


            var pager = inputParameters.ToPager();
            var list = this._mgr.GetTET_SupplierLastVersionList(inputParameters, cUser.ID, pager);

            WebApiDataContainer<TET_SupplierModel> retList = new WebApiDataContainer<TET_SupplierModel>();
            retList.recordsFiltered = pager.TotalRow;
            retList.recordsTotal = pager.TotalRow;
            retList.data = list;

            return retList;
        }

        [Route("~/api/SupplierApi/QueryList")]
        [HttpPost]
        public WebApiDataContainer<TET_SupplierModel> QueryList([FromBody] TempPager inputParameters)
        {
            DateTime cTime = DateTime.Now;

            var cUser = UserProfileService.GetCurrentUser();
            if (string.IsNullOrWhiteSpace(cUser.ID))
                throw new UnauthorizedAccessException();


            var pager = inputParameters.ToPager();
            var list = this._mgr.GetTET_SupplierActiveLastVersionList(inputParameters, cUser.ID, pager);

            WebApiDataContainer<TET_SupplierModel> retList = new WebApiDataContainer<TET_SupplierModel>();
            retList.recordsFiltered = pager.TotalRow;
            retList.recordsTotal = pager.TotalRow;
            retList.data = list;

            return retList;
        }


        [Route("~/api/SupplierApi/Detail/{id}")]
        [HttpGet]
        // GET api/SupplierApi/Detail/{id}
        public TET_SupplierModel GetOne([FromUri] Guid id, bool includeApprovalList = false, bool includeSTQAList = false, bool includeTradeList = false, bool isSS = false)
        {
            var result = this._mgr.GetTET_Supplier(id, includeApprovalList, includeSTQAList, includeTradeList, isSS);
            return result;
        }


        [Route("~/api/SupplierApi/Create")]
        [HttpPost]
        public IHttpActionResult Create()
        {
            // 因為需要上傳檔案，此處原始 Http 來處理
            DateTime cTime = DateTime.Now;

            var cUser = UserProfileService.GetCurrentUser();
            if (string.IsNullOrWhiteSpace(cUser.ID))
                throw new UnauthorizedAccessException();

            var inp = HttpContext.Current.Request.Form["Main"];
            TET_SupplierModel model;

            // 嘗試做反序列化，如果錯誤的話丟 Bad Request
            try
            {
                model = JsonConvert.DeserializeObject<TET_SupplierModel>(inp);
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

                    model.UploadFiles.Add(fileContent);
                }
            }

            // 驗證正確性
            List<string> msgList = new List<string>();
            List<string> tempMsgList;
            var validResult = SupplierValidator.ValidCreate(model, out tempMsgList);
            if (!validResult)
                msgList.AddRange(tempMsgList);
            var validFileResult = SupplierAttachmentValidator.Valid(model.AttachmentList, model.UploadFiles, out tempMsgList);
            if (!validFileResult)
                msgList.AddRange(tempMsgList);

            if (!validResult || !validFileResult)
                return BadRequest(JsonConvert.SerializeObject(msgList));

            // 新增
            var newID = _mgr.CreateTET_Supplier(model, cUser.ID, cTime);
            return Ok(newID);
        }

        [Route("~/api/SupplierApi/Submit")]
        [HttpPost]
        public IHttpActionResult Submit()
        {
            // 因為需要上傳檔案，此處原始 Http 來處理
            DateTime cTime = DateTime.Now;

            var cUser = UserProfileService.GetCurrentUser();
            if (string.IsNullOrWhiteSpace(cUser.ID))
                throw new UnauthorizedAccessException();

            var inp = HttpContext.Current.Request.Form["Main"];
            TET_SupplierModel model;

            // 嘗試做反序列化，如果錯誤的話丟 Bad Request
            try
            {
                model = JsonConvert.DeserializeObject<TET_SupplierModel>(inp);
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

                    model.UploadFiles.Add(fileContent);
                }
            }

            // 驗證正確性
            List<string> msgList = new List<string>();
            List<string> tempMsgList;
            var validResult = SupplierValidator.ValidCreate(model, out tempMsgList);
            if (!validResult)
                msgList.AddRange(tempMsgList);
            var validFileResult = SupplierAttachmentValidator.Valid(model.AttachmentList, model.UploadFiles, out tempMsgList);
            if (!validFileResult)
                msgList.AddRange(tempMsgList);

            if (!validResult || !validFileResult)
                return BadRequest(JsonConvert.SerializeObject(msgList));

            // 如果還沒新增過，就先新增
            Guid supplierID;
            if (model.ID.HasValue)
            {
                // 查不到資料也視為要新增
                if (this._mgr.GetTET_Supplier(model.ID.Value) == null)
                {
                    supplierID = this._mgr.CreateTET_Supplier(model, cUser.ID, cTime);
                    model.ID = supplierID;
                }
                else
                {
                    // 如果查得到，就先存檔，避免漏資料
                    this._mgr.ModifyTET_Supplier(model, cUser.ID, cTime);
                }
            }
            else
            {
                supplierID = this._mgr.CreateTET_Supplier(model, cUser.ID, cTime);
                model.ID = supplierID;
            }


            // 送出
            this._mgr.SubmitTET_SupplierApproval(model, cUser.ID, cTime);
            return Ok(model.ID);
        }

        [Route("~/api/SupplierApi/Modify/{id}")]
        [HttpPost]
        public IHttpActionResult Modify(Guid id)
        {
            // 因為需要上傳檔案，此處原始 Http 來處理
            DateTime cTime = DateTime.Now;

            var cUser = UserProfileService.GetCurrentUser();
            if (string.IsNullOrWhiteSpace(cUser.ID))
                throw new UnauthorizedAccessException();

            var inp = HttpContext.Current.Request.Form["Main"];
            TET_SupplierModel model;

            // 嘗試做反序列化，如果錯誤的話丟 Bad Request
            try
            {
                model = JsonConvert.DeserializeObject<TET_SupplierModel>(inp);
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

                    model.UploadFiles.Add(fileContent);
                }
            }

            // 驗證正確性
            List<string> msgList, msgList2;
            var validResult = SupplierValidator.ValidModify(model, out msgList);
            var validFileResult = SupplierAttachmentValidator.Valid(model.AttachmentList, model.UploadFiles, out msgList2);

            if (!validResult || !validFileResult)
                return BadRequest(JsonConvert.SerializeObject(msgList.Union(msgList2)));

            // 修改
            try
            {
                this._mgr.ModifyTET_Supplier(model, cUser.ID, cTime);
            }
            catch (Exception ex)
            {
                return BadRequest(JsonConvert.SerializeObject(new string[] { ex.Message }));
            }
            return Ok();
        }

        [Route("~/api/SupplierApi/Delete/{id}")]
        [HttpPost]
        public IHttpActionResult Delete(Guid id)
        {
            // 因為需要上傳檔案，此處原始 Http 來處理
            DateTime cTime = DateTime.Now;

            var cUser = UserProfileService.GetCurrentUser();
            if (string.IsNullOrWhiteSpace(cUser.ID))
                throw new UnauthorizedAccessException();

            // 刪除
            try
            {
                this._mgr.DeleteTET_Supplier(id, cUser.ID, cTime);
            }
            catch (Exception ex)
            {
                return BadRequest(JsonConvert.SerializeObject(new string[] { ex.Message }));
            }
            return Ok();
        }

        [Route("~/api/SupplierApi/Abord/")]
        [HttpPost]
        public IHttpActionResult Abord([FromBody] AbordTempInputClass input)
        {
            // 因為需要上傳檔案，此處原始 Http 來處理
            DateTime cTime = DateTime.Now;

            var cUser = UserProfileService.GetCurrentUser();
            if (string.IsNullOrWhiteSpace(cUser.ID))
                throw new UnauthorizedAccessException();

            // 刪除
            try
            {
                this._mgr.AbordTET_Supplier(input.id, input.reason, cUser.ID, cTime);
            }
            catch (Exception ex)
            {
                return BadRequest(JsonConvert.SerializeObject(new string[] { ex.Message }));
            }
            return Ok();
        }

        [Route("~/api/SupplierApi/ParseExcel")]
        [HttpPost]
        public IHttpActionResult ParseExcel()
        {
            // 因為需要上傳檔案，此處原始 Http 來處理
            DateTime cTime = DateTime.Now;

            var cUser = UserProfileService.GetCurrentUser();
            if (string.IsNullOrWhiteSpace(cUser.ID))
                throw new UnauthorizedAccessException();


            // 取得本次上傳的附件
            if (!HttpContext.Current.Request.Files.AllKeys.Any())
                return null;

            try
            {
                // 嘗試轉換
                TET_SupplierModel model = ReadExcel(HttpContext.Current.Request.Files[0]);

                if (model.ContactList != null)
                {
                    var msgList = new List<string>();
                    foreach (var item in model.ContactList)
                    {
                        if (!SupplierContactValidator.ValidCreate(item, out List<string> tempList))
                            msgList.AddRange(tempList);
                    }

                    if (msgList.Count > 0)
                    {
                        msgList = msgList.Distinct().ToList();
                        return BadRequest(JsonConvert.SerializeObject(msgList));
                    }
                }

                return Json<TET_SupplierModel>(model);
            }
            catch (Exception ex)
            {
                return BadRequest("Supplier is required.");
            }
        }

        [Route("~/api/SupplierApi/Active/{id}")]
        [HttpPost]
        public IHttpActionResult Active(Guid id)
        {
            // 因為需要上傳檔案，此處原始 Http 來處理
            DateTime cTime = DateTime.Now;

            var cUser = UserProfileService.GetCurrentUser();
            if (string.IsNullOrWhiteSpace(cUser.ID))
                throw new UnauthorizedAccessException();

            // Active
            try
            {
                this._mgr.ActiveTET_Supplier(id, cUser.ID, cTime);
            }
            catch (Exception ex)
            {
                return BadRequest(JsonConvert.SerializeObject(new string[] { ex.Message }));
            }
            return Ok();
        }

        [Route("~/api/SupplierApi/Inactive/{id}")]
        [HttpPost]
        public IHttpActionResult Inactive(Guid id)
        {
            // 因為需要上傳檔案，此處原始 Http 來處理
            DateTime cTime = DateTime.Now;

            var cUser = UserProfileService.GetCurrentUser();
            if (string.IsNullOrWhiteSpace(cUser.ID))
                throw new UnauthorizedAccessException();

            // Active
            try
            {
                this._mgr.InactiveTET_Supplier(id, cUser.ID, cTime);
            }
            catch (Exception ex)
            {
                return BadRequest(JsonConvert.SerializeObject(new string[] { ex.Message }));
            }
            return Ok();
        }


        #region Query / QuerySS
        [Route("~/api/SupplierApi/ModifyQuerySS/{id}")]
        [HttpPost]
        public IHttpActionResult ModifyQuerySS(Guid id)
        {
            // 因為需要上傳檔案，此處原始 Http 來處理
            DateTime cTime = DateTime.Now;

            var cUser = UserProfileService.GetCurrentUser();
            if (string.IsNullOrWhiteSpace(cUser.ID))
                throw new UnauthorizedAccessException();

            var inp = HttpContext.Current.Request.Form["Main"];
            TET_SupplierModel model;

            // 嘗試做反序列化，如果錯誤的話丟 Bad Request
            try
            {
                model = JsonConvert.DeserializeObject<TET_SupplierModel>(inp);
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

                    model.UploadFiles.Add(fileContent);
                }
            }


            var dbModel = this._mgr.GetTET_Supplier(model.ID.Value);
            if (dbModel == null)
                return BadRequest("Supplier is required.");

            dbModel.RelatedDept = model.RelatedDept;
            dbModel.BelongTo = model.BelongTo;
            dbModel.SupplierCategory = model.SupplierCategory;
            dbModel.BusinessCategory = model.BusinessCategory;
            dbModel.BusinessAttribute = model.BusinessAttribute;
            dbModel.Buyer = model.Buyer;
            dbModel.EName = model.EName;
            dbModel.PaymentTerm = model.PaymentTerm;
            dbModel.NDANo = model.NDANo;
            dbModel.Contract = model.Contract;
            dbModel.Buyer = model.Buyer;
            dbModel.SearchKey = model.SearchKey;
            dbModel.Country = model.Country;
            dbModel.Address = model.Address;
            dbModel.OfficeTel = model.OfficeTel;
            dbModel.Email = model.Email;
            dbModel.Website = model.Website;
            dbModel.ISO = model.ISO;
            dbModel.CapitalAmount = model.CapitalAmount;
            dbModel.Employees = model.Employees;
            dbModel.Incoterms = model.Incoterms;
            dbModel.BillingDocument = model.BillingDocument;
            dbModel.MainProduct = model.MainProduct;
            dbModel.Remark = model.Remark;
            dbModel.SignDate1 = model.SignDate1;
            dbModel.SignDate2 = model.SignDate2;
            dbModel.KeySupplier = model.KeySupplier;
            dbModel.IsActive = model.IsActive;

            dbModel.AttachmentList = model.AttachmentList;
            dbModel.UploadFiles = model.UploadFiles;
            dbModel.ContactList = model.ContactList;

            // 驗證正確性
            List<string> msgList, msgList2;
            var validResult = SupplierValidator.ValidModify(dbModel, out msgList);
            var validFileResult = SupplierAttachmentValidator.Valid(model.AttachmentList, model.UploadFiles, out msgList2);

            if (!validResult || !validFileResult)
                return BadRequest(JsonConvert.SerializeObject(msgList.Union(msgList2)));

            // 修改
            try
            {
                this._mgr.ModifyTET_Supplier_QuerySS(model, cUser.ID, cTime);
            }
            catch (Exception ex)
            {
                return BadRequest(JsonConvert.SerializeObject(new string[] { ex.Message }));
            }
            return Ok();
        }


        [Route("~/api/SupplierApi/WriteTradeExcel")]
        [HttpPost]
        public IHttpActionResult WriteTradeExcel()
        {
            // 因為需要上傳檔案，此處原始 Http 來處理
            DateTime cTime = DateTime.Now;

            var cUser = UserProfileService.GetCurrentUser();
            if (string.IsNullOrWhiteSpace(cUser.ID))
                throw new UnauthorizedAccessException();


            // 取得本次上傳的附件
            if (!HttpContext.Current.Request.Files.AllKeys.Any())
                return null;

            try
            {
                // 嘗試轉換
                List<string> msgList = new List<string>();
                List<TET_SupplierTradeModel> list = ReadTradeExcel(HttpContext.Current.Request.Files[0], out msgList);
                if (msgList.Any())
                {
                    return BadRequest(JsonConvert.SerializeObject(msgList));
                }

                this._tradeManager.WriteTET_SupplierTrade(list, cUser.ID, cTime);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest("Supplier trade is required.");
            }
        }

        [HttpGet]
        public IHttpActionResult ExportSupplierExcel([FromUri] bool isSS, [FromUri] TempPager inputParameters)
        {
            DateTime cTime = DateTime.Now;

            var cUser = UserProfileService.GetCurrentUser();
            if (string.IsNullOrWhiteSpace(cUser.ID))
                throw new UnauthorizedAccessException();


            // 模擬要填入 Excel 的資料
            var pager = new Pager() { AllowPaging = false };
            var supplierList = this._mgr.GetTET_SupplierLastVersionList(inputParameters, cUser.ID, pager);
            var contactList = this._contactMgr.GetTET_SupplierContactList(supplierList.Select(obj => obj.ID.Value));
            var tradeList = this._tradeManager.GetTET_SupplierTradeList(supplierList.Select(obj => obj.VenderCode));
            var stqaList = this._stqaMgr.GetSTQAList(supplierList.Select(obj => obj.BelongTo));

            MemoryStream newMsOutput = this.BuildOutputExcel(supplierList, tradeList, stqaList, contactList, isSS);

            // 提供下載新的 Excel 檔案
            var response = Request.CreateResponse(HttpStatusCode.OK);
            response.Content = new StreamContent(newMsOutput);
            response.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment");
            response.Content.Headers.ContentDisposition.FileName = "供應商資料匯出.xlsx";
            response.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/octet-stream");


            return ResponseMessage(response);
        }

        private MemoryStream BuildOutputExcel(List<TET_SupplierModel> list, List<TET_SupplierTradeModel> tradeList, List<TET_SupplierSTQAModel> stqaList, List<TET_SupplierContactModel> contactList, bool isSS)
        {
            // 讀取範本 xlsx 檔案
            var templatePath =
                isSS
                    ? HttpContext.Current.Server.MapPath("~/ModuleResources/Other/Supplier/供應商資料匯出範本(採購).xlsx")
                    : HttpContext.Current.Server.MapPath("~/ModuleResources/Other/Supplier/供應商資料匯出範本.xlsx");

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
            ISheet sheet_1 = workbook.GetSheet("供應商資料");
            // 從第 3 列開始填入資料
            int rowIndex = 1;
            var supplierlist = list.OrderBy(obj => obj.VenderCode);
            foreach (var item in supplierlist)
            {
                // 交易紀錄，如果是採購就可以看到全部紀錄，否則只能看最新年度
                string tradeText;
                if (isSS)
                {
                    var tempTradeList = this._tradeManager.GetGroupedTradeList(item.VenderCode, isSS).Select(obj => $"{obj.Year}-{obj.TotalAmount.ToString("#,###.00")}({obj.Currency})").ToList();
                    tradeText = string.Join(Environment.NewLine, tempTradeList);
                }
                else
                {
                    tradeText = this._tradeManager.GetLastYearOfTrade(item.VenderCode)?.ToString();
                }

                rowIndex += 1;
                IRow row = sheet_1.CreateRow(rowIndex);

                row.CreateCell(00).SetStyle(normalStyle).SetCellValue(item.RegisterDate);                             // 登錄日期
                row.CreateCell(01).SetStyle(normalStyle).SetCellValue(item.IsActive);                                 // 是否啟用
                row.CreateCell(02).SetStyle(normalStyle).SetCellValue(item.VenderCode);                               // 供應商代碼
                row.CreateCell(03).SetStyle(normalStyle).SetCellValue(item.BelongTo);                                 // 歸屬公司名稱
                row.CreateCell(04).SetStyle(normalStyle).SetCellValue(string.Join(",", item.SupplierCategoryFullNameList));       // 廠商類別
                row.CreateCell(05).SetStyle(normalStyle).SetCellValue(string.Join(",", item.BusinessCategoryFullNameList));       // 交易主類別
                row.CreateCell(06).SetStyle(normalStyle).SetCellValue(string.Join(",", item.BusinessAttributeFullNameList));      // 交易子類別
                row.CreateCell(07).SetStyle(normalStyle).SetCellValue(string.Join(",", item.RelatedDeptFullNameList));            // 相關BU
                row.CreateCell(09).SetStyle(normalStyle).SetCellValue(item.CName);                                    // 供應商中文名稱
                row.CreateCell(10).SetStyle(normalStyle).SetCellValue(item.EName);                                    // 供應商英文名稱
                row.CreateCell(11).SetStyle(normalStyle).SetCellValue(string.Join(",", item.CountryFullNameList));          // 國家別
                row.CreateCell(12).SetStyle(normalStyle).SetCellValue(item.TaxNo);                                    // 統一編號
                row.CreateCell(13).SetStyle(normalStyle).SetCellValue(item.Address);                                  // 供應商地址
                row.CreateCell(14).SetStyle(normalStyle).SetCellValue(item.OfficeTel);                                // 供應商電話
                row.CreateCell(15).SetStyle(normalStyle).SetCellValue(item.Email);                                    // 供應商Email
                row.CreateCell(16).SetStyle(normalStyle).SetCellValue(item.Website);                                  // 供應商網站
                row.CreateCell(17).SetStyle(normalStyle).SetCellValue(item.ISO);                                      // ISO認證
                row.CreateCell(18).SetStyle(normalStyle).SetCellValue(item.CapitalAmount);                            // 資本額
                row.CreateCell(19).SetStyle(normalStyle).SetCellValue(item.Charge);                                   // 公司負責人
                row.CreateCell(20).SetStyle(normalStyle).SetCellValue(item.Employees);                                // 員工人數
                row.CreateCell(21).SetStyle(normalStyle).SetCellValue(string.Join(",", item.PaymentTermFullNameList));      // 付款條件
                row.CreateCell(22).SetStyle(normalStyle).SetCellValue(string.Join(",", item.IncotermsFullNameList));      // 交易條件
                row.CreateCell(23).SetStyle(normalStyle).SetCellValue(string.Join(",", item.BillingDocumentFullNameList));      // 請款憑證
                row.CreateCell(24).SetStyle(wrapStyle).SetCellValue(item.MainProduct);                                // 主要產品/服務項目
                row.CreateCell(25).SetStyle(wrapStyle).SetCellValue(item.Remark);                                     // 供應商相關備註
                row.CreateCell(26).SetStyle(normalStyle).SetCellValue(item.BankName);                                 // 銀行名稱
                row.CreateCell(27).SetStyle(normalStyle).SetCellValue(item.BankCode);                                 // 銀行代碼
                row.CreateCell(28).SetStyle(normalStyle).SetCellValue(item.BankBranchName);                           // 分行名稱
                row.CreateCell(29).SetStyle(normalStyle).SetCellValue(item.BankBranchCode);                           // 分行代碼
                row.CreateCell(30).SetStyle(normalStyle).SetCellValue(item.BankAccountNo);                            // 匯款帳號
                row.CreateCell(31).SetStyle(normalStyle).SetCellValue(item.BankAccountName);                          // 帳戶名稱
                row.CreateCell(32).SetStyle(normalStyle).SetCellValue(string.Join(",", item.CurrencyFullNameList));   // 匯款幣別
                row.CreateCell(33).SetStyle(normalStyle).SetCellValue(string.Join(",", item.BankCountryFullNameList));          // 銀行國別
                row.CreateCell(34).SetStyle(normalStyle).SetCellValue(item.BankAddress);                              // 銀行地址
                row.CreateCell(35).SetStyle(normalStyle).SetCellValue(item.SwiftCode);                                // SWIFT CODE
                row.CreateCell(36).SetStyle(normalStyle).SetCellValue(item.CompanyCity);                              // 公司註冊地城市
                row.CreateCell(37).SetStyle(normalStyle).SetCellValue(item.NDANo);                                    // NDA號碼
                row.CreateCell(38).SetStyle(normalStyle).SetCellValue(item.Contract);                                 // 合約(Y/N)
                row.CreateCell(39).SetStyle(normalStyle).SetCellValue(item.SignDate1?.ToString("yyyy-MM-dd"));        // 行為準則承諾書簽屬日期
                row.CreateCell(40).SetStyle(normalStyle).SetCellValue(item.SignDate2?.ToString("yyyy-MM-dd"));        // 承攬商安全衛生環保承諾書簽屬日期
                row.CreateCell(41).SetStyle(normalStyle).SetCellValue(string.Join(",", item.KeySupplierFullNameList));          // 供應商狀態
                row.CreateCell(42).SetStyle(normalStyle).SetCellValue(item.IsSTQA);                                   // STQA 認證

                string buyerText = string.Join(", ", item.BuyerFullNameList);
                row.CreateCell(08).SetStyle(normalStyle).SetCellValue(buyerText);                                     // 採購擔當

                row.CreateCell(43).SetStyle(wrapStyle).SetCellValue(tradeText);                                       // 交易資訊
            }


            // 取得工作表
            ISheet sheet_2 = workbook.GetSheet("供應商聯絡人");
            // 從第 2 列開始填入資料
            rowIndex = 0;
            foreach (var item in supplierlist)
            {
                var cotactlist = contactList.Where(obj => obj.SupplierID == item.ID).ToList();

                foreach (var item1 in cotactlist)
                {
                    rowIndex += 1;
                    IRow row = sheet_2.CreateRow(rowIndex);

                    row.CreateCell(0).SetStyle(normalStyle).SetCellValue(item.VenderCode);    // 供應商代碼 
                    row.CreateCell(1).SetStyle(normalStyle).SetCellValue(item.CName);         // 供應商中文名稱
                    row.CreateCell(2).SetStyle(normalStyle).SetCellValue(item1.ContactName);       // 姓名
                    row.CreateCell(3).SetStyle(normalStyle).SetCellValue(item1.ContactTitle);      // 職稱
                    row.CreateCell(4).SetStyle(normalStyle).SetCellValue(item1.ContactTel);        // 電話
                    row.CreateCell(5).SetStyle(normalStyle).SetCellValue(item1.ContactEmail);      // Email
                    row.CreateCell(6).SetStyle(wrapStyle).SetCellValue(item1.ContactRemark);       // 備註（產品／區域）
                }
            }

            // 取得工作表
            ISheet sheet_3 = workbook.GetSheet("供應商STQA資訊");

            // 從第 2 列開始填入資料
            rowIndex = 0;
            foreach (var item in supplierlist)
            {
                var stqalist = stqaList.Where(obj => obj.BelongTo == item.BelongTo).ToList();

                foreach (var item2 in stqalist)
                {
                    rowIndex += 1;
                    IRow row = sheet_3.CreateRow(rowIndex);

                    row.CreateCell(0).SetStyle(normalStyle).SetCellValue(item.VenderCode);    // 供應商代碼 
                    row.CreateCell(1).SetStyle(normalStyle).SetCellValue(item.CName);         // 供應商中文名稱
                    row.CreateCell(2).SetStyle(normalStyle).SetCellValue(item2.Purpose);                              // STQA 理由
                    row.CreateCell(3).SetStyle(normalStyle).SetCellValue(item2.BusinessTerm);                         // Purpose 業務類別
                    row.CreateCell(4).SetStyle(normalStyle).SetCellValue(item2.Type);                                 // STQA 方式
                    row.CreateCell(5).SetStyle(normalStyle).SetCellValue(item2.Date?.ToString("yyyy-MM-dd"));         // 完成日期
                    row.CreateCell(6).SetStyle(normalStyle).SetCellValue(item2.UnitALevel);                           // Unit - A Level
                    row.CreateCell(7).SetStyle(normalStyle).SetCellValue(item2.UnitCLevel);                           // Unit - C Level
                    row.CreateCell(8).SetStyle(normalStyle).SetCellValue(item2.UnitDLevel);                           // Unit - D Level
                }
            }

            // 儲存新的 Excel 檔案
            var msOutput = new MemoryStream();
            workbook.Write(msOutput);

            var msNewOutput = new MemoryStream(msOutput.ToArray());
            return msNewOutput;
        }
        #endregion


        #region Read Excel
        /// <summary> 將 Excel 轉為供應商內容 </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        private TET_SupplierModel ReadExcel(HttpPostedFile file)
        {
            if (file == null)
                return null;

            Stream stream = file.InputStream;
            try
            {
                IWorkbook wb;

                // 依版本，呼叫不同 API 載入檔案
                if (file.FileName.ToUpper().EndsWith("XLSX"))
                    wb = new XSSFWorkbook(stream); // excel版本(.xlsx)
                else
                    wb = new HSSFWorkbook(stream); // excel版本(.xls)


                var model = new TET_SupplierModel();

                //取「供應商資料」頁籤   
                ISheet sheet_Main = wb.GetSheet("供應商資料");
                ReadSupplierMain(sheet_Main, model);

                //取「供應商聯絡人」頁籤   
                ISheet sheet_Contact = wb.GetSheet("供應商聯絡人");
                ReadSupplierContact(sheet_Contact, model);

                return model;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                // 釋放佔用資源
                stream.Dispose();
                stream.Close();
            }

        }

        /// <summary> 讀取主要資料 </summary>
        /// <param name="sheet"></param>
        /// <param name="model"></param>
        private void ReadSupplierMain(ISheet sheet, TET_SupplierModel model)
        {
            TET_ParametersManager _pm = new TET_ParametersManager();

            // Sheet: 供應商資料

            // Title			Name				ExcelColumn	ColumnIndex	RowIndex
            // 供應商中文名稱		CName				D4			3			3
            // 供應商英文名稱		EName				D5			4			3
            // 國家別			Country				    D6			5			3
            // 統一編號			TaxNo				D7			6			3
            // 資本額			CapitalAmount		D8			7			3
            // 主要產品/服務項目 	MainProduct			D9			8			3
            // 公司負責人		Charge				D10			9			3
            // 公司地址			Address				D11			10			3
            // 銀行名稱 			BankName			D14			13			3
            // 分行名稱 			BankBranchName		D15			14			3
            // 帳戶名稱 			BankAccountName		D16			15			3
            // 匯款帳號 			BankAccountNo		D17			16			3
            // 公司註冊地城市		CompanyCity			D19			18			3
            // 匯款銀行地址		BankAddress			D20			19			3
            // 交易條件			Incoterms			G4			3			6
            // 請款憑證			BillingDocument		G5			4			6
            // ISO認證			ISO					G6			5			6
            // 公司E-mail		Email				G7			6			6
            // 公司網站			Website				G8			7			6
            // 員工人數			Employees			G9			8			6
            // 公司電話			OfficeTel			G10			9			6
            // 銀行代碼 			BankCode			G14			13			6
            // 分行代碼			BankBranchCode		G15			14			6
            // 銀行國別			BankCountry			G16			15			6
            // 幣別				Currency			G17			16			6
            // SWIFT CODE		SwiftCode			G19			18			6

            model.CName = this.ReadColumnValue(sheet, 03, 3);   // D4	
            model.EName = this.ReadColumnValue(sheet, 04, 3);   // D5	
            model.Country = _pm.GetItem("國家別", this.ReadColumnValue(sheet, 05, 3));   // D6	
            model.TaxNo = this.ReadColumnValue(sheet, 06, 3);   // D7	
            model.CapitalAmount = this.ReadColumnValue(sheet, 07, 3);   // D8	
            model.MainProduct = this.ReadColumnValue(sheet, 08, 3);   // D9	
            model.Charge = this.ReadColumnValue(sheet, 09, 3);   // D10	
            model.Address = this.ReadColumnValue(sheet, 10, 3);   // D11	
            model.BankName = this.ReadColumnValue(sheet, 13, 3);   // D14	
            model.BankBranchName = this.ReadColumnValue(sheet, 14, 3);   // D15	
            model.BankAccountName = this.ReadColumnValue(sheet, 15, 3);   // D16	
            model.BankAccountNo = this.ReadColumnValue(sheet, 16, 3);   // D17	
            model.CompanyCity = this.ReadColumnValue(sheet, 18, 3);   // D19	
            model.BankAddress = this.ReadColumnValue(sheet, 19, 3);   // D20	
            model.Incoterms = _pm.GetItem("交易條件", this.ReadColumnValue(sheet, 03, 6));   // G4	
            model.BillingDocument = _pm.GetItem("請款憑證", this.ReadColumnValue(sheet, 04, 6));   // G5	
            model.ISO = this.ReadColumnValue(sheet, 05, 6);   // G6	
            model.Email = this.ReadColumnValue(sheet, 06, 6);   // G7	
            model.Website = this.ReadColumnValue(sheet, 07, 6);   // G8	
            model.Employees = this.ReadColumnValue(sheet, 08, 6);   // G9	
            model.OfficeTel = this.ReadColumnValue(sheet, 09, 6);   // G10	
            model.BankCode = this.ReadColumnValue(sheet, 13, 6);   // G14	
            model.BankBranchCode = this.ReadColumnValue(sheet, 14, 6);   // G15	
            model.BankCountry = _pm.GetItem("銀行國別", this.ReadColumnValue(sheet, 15, 6));   // G16	
            model.Currency = _pm.GetItem("幣別", this.ReadColumnValue(sheet, 16, 6));   // G17	
            model.SwiftCode = this.ReadColumnValue(sheet, 18, 6);   // G19	
        }

        /// <summary> 讀取聯絡人資料 </summary>
        /// <param name="sheet"></param>
        /// <param name="model"></param>
        private void ReadSupplierContact(ISheet sheet, TET_SupplierModel model)
        {
            // Sheet: 供應商聯絡人

            // Title	Name	ExcelColumn	ColumnIndex	RowIndex
            // 姓名		Name	A3~A9		0			2~8
            // 職稱		Title	B3~B9		1			2~8
            // 電話		Tel		C3~C9		2			2~8
            // 電子郵件	EMail	D3~D9		3			2~8
            // 備註		Remark	E3~E9		4			2~8
            // 

            for (var i = 2; i <= 8; i++)
            {
                var contact = new TET_SupplierContactModel()
                {
                    ContactName = this.ReadColumnValue(sheet, i, 0),
                    ContactTitle = this.ReadColumnValue(sheet, i, 1),
                    ContactTel = this.ReadColumnValue(sheet, i, 2),
                    ContactEmail = this.ReadColumnValue(sheet, i, 3),
                    ContactRemark = this.ReadColumnValue(sheet, i, 4),
                };

                // 如果有任何一個欄位有值，就視為有資料
                if (
                    !string.IsNullOrWhiteSpace(contact.ContactName) ||
                    !string.IsNullOrWhiteSpace(contact.ContactTitle) ||
                    !string.IsNullOrWhiteSpace(contact.ContactTel) ||
                    !string.IsNullOrWhiteSpace(contact.ContactEmail) ||
                    !string.IsNullOrWhiteSpace(contact.ContactRemark)
                    )
                {
                    model.ContactList.Add(contact);
                }
            }
        }


        /// <summary> 取得指定欄位的值，並強制轉為文字 </summary>
        /// <param name="sheet"></param>
        /// <param name="columnIndex"></param>
        /// <param name="rowIndex"></param>
        /// <returns></returns>
        private string ReadColumnValue(ISheet sheet, int rowIndex, int columnIndex)
        {
            var row = sheet.GetRow(rowIndex);
            if (row == null)
                return null;

            var cell = row.GetCell(columnIndex);
            if (cell == null)
                return null;

            string result = null;
            switch (cell.CellType)
            {
                //字串型態欄位
                case CellType.String:
                    result = cell.StringCellValue?.Trim();
                    break;

                //數字型態欄位
                case CellType.Numeric:
                    if (HSSFDateUtil.IsCellDateFormatted(cell)) // 日期格式
                        result = DateTime.FromOADate(cell.NumericCellValue).ToString("yyyy/MM/dd");
                    else                                        // 數字格式
                        result = cell.NumericCellValue.ToString();
                    break;

                //布林值
                case CellType.Boolean:
                    result = (cell.BooleanCellValue) ? "YES" : "NO";
                    break;

                //空值
                case CellType.Blank:
                    result = string.Empty;
                    break;

                // 預設
                default:
                    result = cell.StringCellValue?.Trim();
                    break;
            }

            return result;
        }

        /// <summary> 讀取交易資料 </summary>
        /// <param name="file"></param>
        /// <param name="msgList"> 回傳用錯誤訊息 </param>
        /// <returns></returns>
        private List<TET_SupplierTradeModel> ReadTradeExcel(HttpPostedFile file, out List<string> msgList)
        {
            msgList = new List<string>();

            if (file == null)
                return null;

            Stream stream = file.InputStream;
            try
            {
                IWorkbook wb;

                // 依版本，呼叫不同 API 載入檔案
                if (file.FileName.ToUpper().EndsWith("XLSX"))
                    wb = new XSSFWorkbook(stream); // excel版本(.xlsx)
                else
                    wb = new HSSFWorkbook(stream); // excel版本(.xls)


                ISheet sheet_Main = wb.GetSheetAt(0);
                return ReadTrade(sheet_Main, out msgList);
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                // 釋放佔用資源
                stream.Dispose();
                stream.Close();
            }

        }

        /// <summary> 讀取交易資料 </summary>
        /// <param name="sheet"></param>
        /// <param name="msgList"> 回傳用錯誤訊息 </param>
        /// <returns></returns>
        private List<TET_SupplierTradeModel> ReadTrade(ISheet sheet, out List<string> msgList)
        {
            msgList = new List<string>();

            // Sheet: 供應商聯絡人

            // Title	    Name	        ExcelColumn	ColumnIndex	RowIndex
            // 傳票日期      SubpoenaDate	A2~AN		0			2~N
            // 傳票編號      SubpoenaNo	    B2~BN		1			2~N
            // 供應商代碼    VenderCode		C2~CN		2			2~N
            // 幣別          Currency	    D2~DN		3			2~N
            // 本幣貸方金額  Amount	        E2~EN		4			2~N

            // 如果都沒有任何欄位有值，代表沒有讀到，這時候就停掉
            bool hasReaded = false;
            int i = 1;
            var retList = new List<TET_SupplierTradeModel>();
            var dicNo = new Dictionary<string, string>();

            do
            {
                hasReaded = false;

                string subpoenaDate = this.ReadColumnValue(sheet, i, 0);
                string subpoenaNo = this.ReadColumnValue(sheet, i, 1);
                string venderCode = this.ReadColumnValue(sheet, i, 2);
                string currency = this.ReadColumnValue(sheet, i, 3);
                string amount = this.ReadColumnValue(sheet, i, 4);


                // 如果都沒有資料，不再讀取
                if (string.IsNullOrWhiteSpace(subpoenaDate) &&
                    string.IsNullOrWhiteSpace(subpoenaNo) &&
                    string.IsNullOrWhiteSpace(venderCode) &&
                    string.IsNullOrWhiteSpace(currency) &&
                    string.IsNullOrWhiteSpace(amount))
                {
                    hasReaded = false;
                    break;
                }

                if (string.IsNullOrWhiteSpace(subpoenaDate) ||
                    string.IsNullOrWhiteSpace(subpoenaNo) ||
                    string.IsNullOrWhiteSpace(venderCode) ||
                    string.IsNullOrWhiteSpace(currency) ||
                    string.IsNullOrWhiteSpace(amount))
                {
                    msgList.Add($"第 {i + 1} 行欄位不可為空。");
                    i += 1;
                    hasReaded = true;
                    continue;
                }


                if (!DateTime.TryParse(subpoenaDate, out var date) ||
                    !decimal.TryParse(amount, out var amountD))
                {
                    msgList.Add($"第 {i + 1} 行日期格式或金額格式錯誤。");
                    i += 1;
                    hasReaded = true;
                    continue;
                }

                if (!dicNo.ContainsKey(subpoenaNo))
                    dicNo.Add(subpoenaNo, subpoenaNo);
                else
                    msgList.Add($"第 {i + 1} 行傳票號碼重覆，該傳票號碼為 {subpoenaNo}。");

                hasReaded = true;
                retList.Add(new TET_SupplierTradeModel()
                {
                    SubpoenaNo = subpoenaNo,
                    SubpoenaDate = date,
                    VenderCode = venderCode,
                    Currency = currency,
                    Amount = amountD,
                });

                i += 1;
            }
            while (hasReaded);

            return retList;
        }
        #endregion
    }
}
