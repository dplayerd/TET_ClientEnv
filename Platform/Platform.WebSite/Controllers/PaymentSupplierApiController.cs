using BI.PaymentSuppliers;
using BI.PaymentSuppliers.Enums;
using BI.PaymentSuppliers.Models;
using BI.PaymentSuppliers.Utils;
using BI.PaymentSuppliers.Validators;
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
    public class PaymentSupplierApiController : ApiController
    {
        private TET_PaymentSupplierManager _mgr = new TET_PaymentSupplierManager();
        private TET_PaymentSupplierContactManager _contactMgr = new TET_PaymentSupplierContactManager();
        private UserManager _userMgr = new UserManager();

        public class TempPager : DataTablePager, ISupplierListQueryInput
        {
            public string caption { get; set; }
            public string taxNo { get; set; }
        }

        public class AbordTempInputClass
        {
            public Guid id { get; set; }
            public string reason { get; set; }
        }

        [Route("~/api/PaymentSupplierApi/GetDataTableList")]
        [HttpPost]
        public WebApiDataContainer<PaymentSupplierListModel> PostToGetList([FromBody] TempPager inputParameters)
        {
            DateTime cTime = DateTime.Now;

            var cUser = UserProfileService.GetCurrentUser();
            if (string.IsNullOrWhiteSpace(cUser.ID))
                throw new UnauthorizedAccessException();


            var pager = inputParameters.ToPager();
            var list = this._mgr.GetTET_PaymentSupplierList(inputParameters, cUser.ID, pager);

            WebApiDataContainer<PaymentSupplierListModel> retList = new WebApiDataContainer<PaymentSupplierListModel>();
            retList.recordsFiltered = pager.TotalRow;
            retList.recordsTotal = pager.TotalRow;
            retList.data = list;

            return retList;
        }

        [Route("~/api/PaymentSupplierApi/QueryList")]
        [HttpPost]
        public WebApiDataContainer<TET_PaymentSupplierModel> QueryList([FromBody] TempPager inputParameters)
        {
            DateTime cTime = DateTime.Now;

            var cUser = UserProfileService.GetCurrentUser();
            if (string.IsNullOrWhiteSpace(cUser.ID))
                throw new UnauthorizedAccessException();


            var pager = inputParameters.ToPager();
            var list = this._mgr.GetTET_PaymentSupplierLastVersionList(inputParameters, cUser.ID, pager);

            WebApiDataContainer<TET_PaymentSupplierModel> retList = new WebApiDataContainer<TET_PaymentSupplierModel>();
            retList.recordsFiltered = pager.TotalRow;
            retList.recordsTotal = pager.TotalRow;
            retList.data = list;

            return retList;
        }

        [Route("~/api/PaymentSupplierApi/Detail/{id}")]
        [HttpGet]
        // GET api/PaymentSupplierApi/Detail/{id}
        public TET_PaymentSupplierModel GetOne([FromUri] Guid id)
        {
            var result = this._mgr.GetTET_PaymentSupplier(id);
            return result;
        }


        [Route("~/api/PaymentSupplierApi/Create")]
        [HttpPost]
        public IHttpActionResult Create()
        {
            // 因為需要上傳檔案，此處原始 Http 來處理
            DateTime cTime = DateTime.Now;

            var cUser = UserProfileService.GetCurrentUser();
            if (string.IsNullOrWhiteSpace(cUser.ID))
                throw new UnauthorizedAccessException();

            var inp = HttpContext.Current.Request.Form["Main"];
            TET_PaymentSupplierModel model;

            // 嘗試做反序列化，如果錯誤的話丟 Bad Request
            try
            {
                model = JsonConvert.DeserializeObject<TET_PaymentSupplierModel>(inp);
                if (model == null)
                    return BadRequest("Payment Supplier is required.");
            }
            catch (Exception ex)
            {
                return BadRequest("Payment Supplier is required.");
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
            var validResult = PaymentSupplierValidator.ValidCreate(model, out tempMsgList);
            if (!validResult)
                msgList.AddRange(tempMsgList);
            var validFileResult = PaymentSupplierAttachmentValidator.Valid(model.AttachmentList, model.UploadFiles, out tempMsgList);
            if (!validFileResult)
                msgList.AddRange(tempMsgList);

            if (!validResult || !validFileResult)
                return BadRequest(JsonConvert.SerializeObject(msgList));

            // 新增
            var newID = _mgr.CreateTET_PaymentSupplier(model, cUser.ID, cTime);
            return Ok(newID);
        }

        [Route("~/api/PaymentSupplierApi/Submit")]
        [HttpPost]
        public IHttpActionResult Submit()
        {
            // 因為需要上傳檔案，此處原始 Http 來處理
            DateTime cTime = DateTime.Now;

            var cUser = UserProfileService.GetCurrentUser();
            if (string.IsNullOrWhiteSpace(cUser.ID))
                throw new UnauthorizedAccessException();

            var inp = HttpContext.Current.Request.Form["Main"];
            TET_PaymentSupplierModel model;

            // 嘗試做反序列化，如果錯誤的話丟 Bad Request
            try
            {
                model = JsonConvert.DeserializeObject<TET_PaymentSupplierModel>(inp);
                if (model == null)
                    return BadRequest("Payment Supplier is required.");
            }
            catch (Exception ex)
            {
                return BadRequest("Payment Supplier is required.");
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
            var validResult = PaymentSupplierValidator.ValidCreate(model, out tempMsgList);
            if (!validResult)
                msgList.AddRange(tempMsgList);
            var validFileResult = PaymentSupplierAttachmentValidator.Valid(model.AttachmentList, model.UploadFiles, out tempMsgList);
            if (!validFileResult)
                msgList.AddRange(tempMsgList);

            if (!validResult || !validFileResult)
                return BadRequest(JsonConvert.SerializeObject(msgList));

            // 如果還沒新增過，就先新增
            Guid supplierID;
            if (model.ID.HasValue)
            {
                // 查不到資料也視為要新增
                if (this._mgr.GetTET_PaymentSupplier(model.ID.Value) == null)
                {
                    supplierID = this._mgr.CreateTET_PaymentSupplier(model, cUser.ID, cTime);
                    model.ID = supplierID;
                }
                else
                {
                    // 如果查得到，就先存檔，避免漏資料
                    this._mgr.ModifyTET_PaymentSupplier(model, cUser.ID, cTime);
                }
            }
            else
            {
                supplierID = this._mgr.CreateTET_PaymentSupplier(model, cUser.ID, cTime);
                model.ID = supplierID;
            }


            // 送出
            this._mgr.SubmitTET_PaymentSupplierApproval(model, cUser.ID, cTime);
            return Ok(model.ID);
        }

        [Route("~/api/PaymentSupplierApi/Modify/{id}")]
        [HttpPost]
        public IHttpActionResult Modify(Guid id)
        {
            // 因為需要上傳檔案，此處原始 Http 來處理
            DateTime cTime = DateTime.Now;

            var cUser = UserProfileService.GetCurrentUser();
            if (string.IsNullOrWhiteSpace(cUser.ID))
                throw new UnauthorizedAccessException();

            var inp = HttpContext.Current.Request.Form["Main"];
            TET_PaymentSupplierModel model;

            // 嘗試做反序列化，如果錯誤的話丟 Bad Request
            try
            {
                model = JsonConvert.DeserializeObject<TET_PaymentSupplierModel>(inp);
                if (model == null)
                    return BadRequest("Payment Supplier is required.");
            }
            catch (Exception ex)
            {
                return BadRequest("Payment Supplier is required.");
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
            var validResult = PaymentSupplierValidator.ValidModify(model, out msgList);
            var validFileResult = PaymentSupplierAttachmentValidator.Valid(model.AttachmentList, model.UploadFiles, out msgList2);

            if (!validResult || !validFileResult)
                return BadRequest(JsonConvert.SerializeObject(msgList.Union(msgList2)));

            // 修改
            try
            {
                this._mgr.ModifyTET_PaymentSupplier(model, cUser.ID, cTime);
            }
            catch (Exception ex)
            {
                return BadRequest(JsonConvert.SerializeObject(new string[] { ex.Message }));
            }
            return Ok();
        }

        [Route("~/api/PaymentSupplierApi/Delete/{id}")]
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
                this._mgr.DeleteTET_PaymentSupplier(id, cUser.ID, cTime);
            }
            catch (Exception ex)
            {
                return BadRequest(JsonConvert.SerializeObject(new string[] { ex.Message }));
            }
            return Ok();
        }

        [Route("~/api/PaymentSupplierApi/Abord/")]
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
                this._mgr.AbordTET_PaymentSupplier(input.id, input.reason, cUser.ID, cTime);
            }
            catch (Exception ex)
            {
                return BadRequest(JsonConvert.SerializeObject(new string[] { ex.Message }));
            }
            return Ok();
        }

        [Route("~/api/PaymentSupplierApi/ParseExcel")]
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
                TET_PaymentSupplierModel model = ReadExcel(HttpContext.Current.Request.Files[0]);

                if (model.ContactList != null)
                {
                    var msgList = new List<string>();
                    foreach (var item in model.ContactList)
                    {
                        if (!PaymentSupplierContactValidator.ValidCreate(item, out List<string> tempList))
                            msgList.AddRange(tempList);
                    }

                    if (msgList.Count > 0)
                    {
                        msgList = msgList.Distinct().ToList();
                        return BadRequest(JsonConvert.SerializeObject(msgList));
                    }
                }

                return Json<TET_PaymentSupplierModel>(model);
            }
            catch (Exception ex)
            {
                return BadRequest("Payment Supplier is required.");
            }
        }

        [Route("~/api/PaymentSupplierApi/Active/{id}")]
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
                this._mgr.ActiveTET_PaymentSupplier(id, cUser.ID, cTime);
            }
            catch (Exception ex)
            {
                return BadRequest(JsonConvert.SerializeObject(new string[] { ex.Message }));
            }
            return Ok();
        }

        [Route("~/api/PaymentSupplierApi/Inactive/{id}")]
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
                this._mgr.InactiveTET_PaymentSupplier(id, cUser.ID, cTime);
            }
            catch (Exception ex)
            {
                return BadRequest(JsonConvert.SerializeObject(new string[] { ex.Message }));
            }
            return Ok();
        }


        #region Query
        [Route("~/api/PaymentSupplierApi/ModifyQuery/{id}")]
        [HttpPost]
        public IHttpActionResult ModifyQuery(Guid id)
        {
            // 因為需要上傳檔案，此處原始 Http 來處理
            DateTime cTime = DateTime.Now;

            var cUser = UserProfileService.GetCurrentUser();
            if (string.IsNullOrWhiteSpace(cUser.ID))
                throw new UnauthorizedAccessException();

            var inp = HttpContext.Current.Request.Form["Main"];
            TET_PaymentSupplierModel model;

            // 嘗試做反序列化，如果錯誤的話丟 Bad Request
            try
            {
                model = JsonConvert.DeserializeObject<TET_PaymentSupplierModel>(inp);
                if (model == null)
                    return BadRequest("Payment Supplier is required.");
            }
            catch (Exception ex)
            {
                return BadRequest("Payment Supplier is required.");
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


            var dbModel = this._mgr.GetTET_PaymentSupplier(model.ID.Value);
            if (dbModel == null)
                return BadRequest("Payment Supplier is required.");

            dbModel.PaymentTerm = model.PaymentTerm;
            dbModel.Country = model.Country;
            dbModel.Address = model.Address;
            dbModel.OfficeTel = model.OfficeTel;
            dbModel.Incoterms = model.Incoterms;
            dbModel.Remark = model.Remark;
            dbModel.BillingDocument = model.BillingDocument;
            dbModel.IsActive = model.IsActive;

            dbModel.AttachmentList = model.AttachmentList;
            dbModel.UploadFiles = model.UploadFiles;
            dbModel.ContactList = model.ContactList;

            // 驗證正確性
            List<string> msgList, msgList2;
            var validResult = PaymentSupplierValidator.ValidModify(dbModel, out msgList);
            var validFileResult = PaymentSupplierAttachmentValidator.Valid(model.AttachmentList, model.UploadFiles, out msgList2);

            if (!validResult || !validFileResult)
                return BadRequest(JsonConvert.SerializeObject(msgList.Union(msgList2)));

            // 修改         
            try
            {
                this._mgr.ModifyTET_PaymentSupplier_Query(model, cUser.ID, cTime);
            }
            catch (Exception ex)
            {
                return BadRequest(JsonConvert.SerializeObject(new string[] { ex.Message }));
            }
            return Ok();
        }

        [HttpGet]
        public IHttpActionResult ExportPaymentSupplierExcel([FromUri] TempPager inputParameters)
        {
            DateTime cTime = DateTime.Now;

            var cUser = UserProfileService.GetCurrentUser();
            if (string.IsNullOrWhiteSpace(cUser.ID))
                throw new UnauthorizedAccessException();


            // 模擬要填入 Excel 的資料
            var pager = new Pager() { AllowPaging = false };
            var supplierList = this._mgr.GetTET_PaymentSupplierLastVersionList(inputParameters, cUser.ID, pager);
            var contactList = this._contactMgr.GetTET_PaymentSupplierContactList(supplierList.Select(obj => obj.ID.Value));

            MemoryStream newMsOutput = this.BuildOutputExcel(supplierList, contactList);

            // 提供下載新的 Excel 檔案
            var response = Request.CreateResponse(HttpStatusCode.OK);
            response.Content = new StreamContent(newMsOutput);
            response.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment");
            response.Content.Headers.ContentDisposition.FileName = "一般付款對象資料匯出.xlsx";
            response.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/octet-stream");


            return ResponseMessage(response);
        }

        private MemoryStream BuildOutputExcel(List<TET_PaymentSupplierModel> list, List<TET_PaymentSupplierContactModel> contactList)
        {
            // 讀取範本 xlsx 檔案
            var templatePath = HttpContext.Current.Server.MapPath("~/ModuleResources/Other/PaymentSupplier/一般付款對象資料匯出範本.xlsx");
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
            ISheet sheet_1 = workbook.GetSheet("一般付款對象資料");
            // 從第 3 列開始填入資料
            int rowIndex = 1;
            foreach (var item in list)
            {
                rowIndex += 1;
                IRow row = sheet_1.CreateRow(rowIndex);

                row.CreateCell(00).SetStyle(normalStyle).SetCellValue(item.RegisterDate);                             // 登錄日期
                row.CreateCell(01).SetStyle(normalStyle).SetCellValue(item.IsActive);                                 // 是否啟用
                row.CreateCell(02).SetStyle(normalStyle).SetCellValue(item.VenderCode);                               // 供應商代碼
                row.CreateCell(03).SetStyle(normalStyle).SetCellValue(item.CName);                                    // 一般付款對象中文名稱
                row.CreateCell(04).SetStyle(normalStyle).SetCellValue(item.EName);                                    // 一般付款對象英文名稱
                row.CreateCell(05).SetStyle(normalStyle).SetCellValue(string.Join(",", item.CountryFullNameList));          // 國家別
                row.CreateCell(06).SetStyle(normalStyle).SetCellValue(item.TaxNo);                                    // 統一編號
                row.CreateCell(07).SetStyle(normalStyle).SetCellValue(item.IdNo);                                     // 身分證字號
                row.CreateCell(08).SetStyle(normalStyle).SetCellValue(item.Address);                                  // 一般付款對象地址
                row.CreateCell(09).SetStyle(normalStyle).SetCellValue(item.OfficeTel);                                // 一般付款對象電話
                row.CreateCell(10).SetStyle(normalStyle).SetCellValue(item.Charge);                                   // 公司負責人
                row.CreateCell(11).SetStyle(normalStyle).SetCellValue(string.Join(",", item.PaymentTermFullNameList));      // 付款條件
                row.CreateCell(12).SetStyle(normalStyle).SetCellValue(string.Join(",", item.IncotermsFullNameList));      // 交易條件
                row.CreateCell(13).SetStyle(normalStyle).SetCellValue(string.Join(",", item.BillingDocumentFullNameList));      // 請款憑證
                row.CreateCell(14).SetStyle(wrapStyle).SetCellValue(item.Remark);                                     // 一般付款對象相關備註
                row.CreateCell(15).SetStyle(normalStyle).SetCellValue(item.BankName);                                 // 銀行名稱
                row.CreateCell(16).SetStyle(normalStyle).SetCellValue(item.BankCode);                                 // 銀行代碼
                row.CreateCell(17).SetStyle(normalStyle).SetCellValue(item.BankBranchName);                           // 分行名稱
                row.CreateCell(18).SetStyle(normalStyle).SetCellValue(item.BankBranchCode);                           // 分行代碼
                row.CreateCell(19).SetStyle(normalStyle).SetCellValue(item.BankAccountNo);                            // 匯款帳號
                row.CreateCell(20).SetStyle(normalStyle).SetCellValue(item.BankAccountName);                          // 帳戶名稱
                row.CreateCell(21).SetStyle(normalStyle).SetCellValue(string.Join(",", item.CurrencyFullNameList));   // 匯款幣別
                row.CreateCell(22).SetStyle(normalStyle).SetCellValue(string.Join(",", item.BankCountryFullNameList));          // 銀行國別
                row.CreateCell(23).SetStyle(normalStyle).SetCellValue(item.BankAddress);                              // 銀行地址
                row.CreateCell(24).SetStyle(normalStyle).SetCellValue(item.SwiftCode);                                // SWIFT CODE
                row.CreateCell(25).SetStyle(normalStyle).SetCellValue(item.CompanyCity);                              // 公司註冊地城市
            }

            // 取得工作表
            ISheet sheet_2 = workbook.GetSheet("一般付款對象聯絡人");
            // 從第 3 列開始填入資料
            rowIndex = 0;
            foreach (var item in contactList)
            {
                rowIndex += 1;
                IRow row = sheet_2.CreateRow(rowIndex);

                var supplier = list.Where(obj => obj.ID == item.PSID).FirstOrDefault();

                row.CreateCell(0).SetStyle(normalStyle).SetCellValue(supplier.VenderCode);    // 供應商代碼 
                row.CreateCell(1).SetStyle(normalStyle).SetCellValue(supplier.CName);         // 一般付款對象中文名稱
                row.CreateCell(2).SetStyle(normalStyle).SetCellValue(item.ContactName);       // 姓名
                row.CreateCell(3).SetStyle(normalStyle).SetCellValue(item.ContactTitle);      // 職稱
                row.CreateCell(4).SetStyle(normalStyle).SetCellValue(item.ContactTel);        // 電話
                row.CreateCell(5).SetStyle(normalStyle).SetCellValue(item.ContactEmail);      // Email
                row.CreateCell(6).SetStyle(wrapStyle).SetCellValue(item.ContactRemark);       // 備註（產品／區域）
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
        private TET_PaymentSupplierModel ReadExcel(HttpPostedFile file)
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

                var model = new TET_PaymentSupplierModel();

                //取「一般付款對象資料」頁籤   
                ISheet sheet_Main = wb.GetSheet("一般付款對象資料");
                ReadPaymentSupplierMain(sheet_Main, model);

                //取「一般付款對象聯絡人」頁籤   
                ISheet sheet_Contact = wb.GetSheet("一般付款對象聯絡人");
                ReadPaymentSupplierContact(sheet_Contact, model);

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
        private void ReadPaymentSupplierMain(ISheet sheet, TET_PaymentSupplierModel model)
        {
            TET_ParametersManager _pm = new TET_ParametersManager();

            // Sheet: 一般付款對象資料

            // Title			   Name				ExcelColumn	ColumnIndex	RowIndex
            // 中文名稱           CName				D4			3			3
            // 英文名稱	          EName				D5			4			3
            // 國家別			  Country			D6			5			3
            // 統一編號			  TaxNo				D7			6			3
            // 公司負責人		  Charge		    D8			7			3
            // 交易條件			  Incoterms			G4			3			6
            // 請款憑證			  BillingDocument	G5			4			6
            // 付款條件           PaymentTerm       G6          5           6
            // 公司地址			  Address			G7			6			6
            // 公司電話			  OfficeTel			G8			7			6
            // 身分證字號	      IdNo  			G9			8			6

            // 銀行名稱 		  BankName			D12			11			3
            // 分行名稱 		  BankBranchName	D13			12			3
            // 帳戶名稱 		  BankAccountName	D14			13			3
            // 匯款帳號 		  BankAccountNo		D15			14			3
            // 銀行代碼 		  BankCode			G12			11			6
            // 分行代碼			  BankBranchCode	G13 		12			6
            // 銀行國別			  BankCountry		G14			13			6
            // 幣別				  Currency			G15			14			6

            // 公司註冊地城市	  CompanyCity		D17			16			3
            // 匯款銀行地址		  BankAddress		D18			17			3
            // SWIFT CODE		  SwiftCode			G17			16			6

            model.CName = this.ReadColumnValue(sheet, 03, 3);   // D4	
            model.EName = this.ReadColumnValue(sheet, 04, 3);   // D5	
            model.Country = _pm.GetItem("國家別", this.ReadColumnValue(sheet, 05, 3));   // D6	
            model.TaxNo = this.ReadColumnValue(sheet, 06, 3);   // D7	
            model.Charge = this.ReadColumnValue(sheet, 07, 3);   // D8	

            model.Incoterms = _pm.GetItem("交易條件",this.ReadColumnValue(sheet, 03, 6));   // G4	
            model.BillingDocument = _pm.GetItem("請款憑證", this.ReadColumnValue(sheet, 04, 6));   // G5	
            model.PaymentTerm = _pm.GetItem("付款條件", this.ReadColumnValue(sheet, 05, 6));   // G6	
            model.Address = this.ReadColumnValue(sheet, 06, 6);   // G7
            model.OfficeTel = this.ReadColumnValue(sheet, 07, 6);   // G8	
            model.IdNo = this.ReadColumnValue(sheet, 08, 6);   // G9	

            model.BankName = this.ReadColumnValue(sheet, 11, 3);   // D12	
            model.BankBranchName = this.ReadColumnValue(sheet, 12, 3);   // D13	
            model.BankAccountName = this.ReadColumnValue(sheet, 13, 3);   // D14	
            model.BankAccountNo = this.ReadColumnValue(sheet, 14, 3);   // D15	

            model.BankCode = this.ReadColumnValue(sheet, 11, 6);   // G12	
            model.BankBranchCode = this.ReadColumnValue(sheet, 12, 6);   // G13	
            model.BankCountry = _pm.GetItem("銀行國別", this.ReadColumnValue(sheet, 13, 6));   // G14	
            model.Currency = _pm.GetItem("幣別", this.ReadColumnValue(sheet, 14, 6));   // G15	

            model.CompanyCity = this.ReadColumnValue(sheet, 16, 3);   // D17	
            model.BankAddress = this.ReadColumnValue(sheet, 17, 3);   // D18	
            model.SwiftCode = this.ReadColumnValue(sheet, 16, 6);   // G17	
        }

        /// <summary> 讀取聯絡人資料 </summary>
        /// <param name="sheet"></param>
        /// <param name="model"></param>
        private void ReadPaymentSupplierContact(ISheet sheet, TET_PaymentSupplierModel model)
        {
            // Sheet: 一般付款對象聯絡人

            // Title	Name	ExcelColumn	ColumnIndex	RowIndex
            // 姓名		Name	A3~A9		0			2~8
            // 職稱		Title	B3~B9		1			2~8
            // 電話		Tel		C3~C9		2			2~8
            // 電子郵件	EMail	D3~D9		3			2~8
            // 備註		Remark	E3~E9		4			2~8

            for (var i = 2; i <= 8; i++)
            {
                var contact = new TET_PaymentSupplierContactModel()
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

        #endregion
    }
}
