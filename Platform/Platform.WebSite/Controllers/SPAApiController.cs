using System;
using System.Web.Http;
using System.Collections.Generic;
using System.Linq;
using Platform.Infra;
using Platform.WebSite.Filters;
using Platform.WebSite.Models;
using Platform.WebSite.Services;
using Newtonsoft.Json;
using System.Web.Http.Results;
using Platform.Auth.Models;
using System.IO;
using System.Web;
using Platform.WebSite.Util;
using BI.SPA;
using BI.SPA.Models;
using BI.SPA.Validators;
using Platform.AbstractionClass;
using BI.SPA.Utils;
using System.Data;


namespace Platform.WebSite.Controllers
{
    [WebApiAuthorizeCore]
    public class SPAApiController : ApiController
    {
        private TET_SPAManager _mgr = new TET_SPAManager();
        
        // 最多一次幾則評鑑期間
        private const int _periodLength = 5;

        #region Input / Output Classes
        public class TempPager : DataTablePager
        {
            public string[] belongTo { get; set; } = new string[0];
            public string[] period { get; set; } = new string[0];
            public string[] bu { get; set; } = new string[0];
            public string[] assessmentItem { get; set; } = new string[0];
            public string[] performanceLevel { get; set; } = new string[0];
        }

        /// <summary> 單期查詢用 </summary>
        public class SingleQueryApiDataContainer : WebApiDataContainer<TET_SupplierSPAModel>
        {
            public string AssessmentItem { get; set; }
            public string Period { get; set; }
            public string SDate_Text { get; set; }
            public string EDate_Text { get; set; }
        }

        public class PeriodObj
        {
            public string Text { get; set; }
            public string Period { get; set; }
            public string SDate_Text { get; set; }
            public string EDate_Text { get; set; }
        }

        /// <summary> 多期查詢用 </summary>
        public class MultiQueryApiDataContainer
        {
            /// <summary> 期別資料內容 </summary>
            public DataTable data { get; set; }

            /// <summary> 所有期別 </summary>
            public List<PeriodObj> totalPeriods { get; set; }

            /// <summary> 評鑑項目 </summary>
            public string AssessmentItem { get; set; }

            /// <summary> 評鑑項目文字 </summary>
            public string AssessmentItem_Text { get; set; }
        }
        #endregion

        #region Read
        [Route("~/api/SPAApi/GetDataTableList")]
        [HttpPost]
        public WebApiDataContainer<TET_SupplierSPAModel> PostToGetList([FromBody] TempPager filter)
        {
            DateTime cTime = DateTime.Now;

            var cUser = UserProfileService.GetCurrentUser();
            if (string.IsNullOrWhiteSpace(cUser.ID))
                throw new UnauthorizedAccessException();


            var pager = filter.ToPager();
            var list = this._mgr.GetSPAList(filter.belongTo, filter.period, filter.bu, filter.assessmentItem, filter.performanceLevel, cUser.ID, cTime, pager);

            WebApiDataContainer<TET_SupplierSPAModel> retList = new WebApiDataContainer<TET_SupplierSPAModel>();
            retList.recordsFiltered = pager.TotalRow;
            retList.recordsTotal = pager.TotalRow;
            retList.data = list;

            return retList;
        }

        [Route("~/api/SPAApi/SingleQueryList")]
        [HttpGet]
        public SingleQueryApiDataContainer GetSingleQueryList([FromUri] TempPager filter)
        {
            DateTime cTime = DateTime.Now;

            var cUser = UserProfileService.GetCurrentUser();
            if (string.IsNullOrWhiteSpace(cUser.ID))
                throw new UnauthorizedAccessException();

            // 必填檢查
            if (filter == null)
            {
                HttpContext.Current.Response.StatusCode = 400;
                return null;
            }

            filter.period = this.ProcessArray(filter.period);                       // 評鑑期間  單選、必選
            filter.assessmentItem = this.ProcessArray(filter.assessmentItem);       // 評鑑項目  單選、必選
            filter.belongTo = this.ProcessArray(filter.belongTo);                   // 供應商    單選
            filter.bu = this.ProcessArray(filter.bu);                               // 評鑑單位  單選
            filter.performanceLevel = this.ProcessArray(filter.performanceLevel);   // Performance Level 單選

            // 必填及格式檢查
            if (filter.period.Length == 0 ||
                filter.assessmentItem.Length == 0 ||
                !PeriodUtil.IsPeriodFormat(filter.period[0]))
            {
                HttpContext.Current.Response.StatusCode = 400;
                return null;
            }

            // 計算時間區間
            var period = PeriodUtil.ParsePeriod(filter.period[0]);

            var pager = new Pager() { AllowPaging = false };
            var list = this._mgr.GetSingleQuerySPAList(filter.belongTo, filter.period, filter.bu, filter.assessmentItem, filter.performanceLevel, cUser.ID, cTime, pager);

            SingleQueryApiDataContainer retList = new SingleQueryApiDataContainer();
            retList.recordsFiltered = pager.TotalRow;
            retList.recordsTotal = pager.TotalRow;
            retList.data = list;

            retList.AssessmentItem = filter.assessmentItem[0];
            retList.Period = filter.period[0];
            retList.SDate_Text = period?.StartDate?.ToString("yyyy-MM-dd");
            retList.EDate_Text = period?.EndDate?.ToString("yyyy-MM-dd");

            return retList;
        }

        [Route("~/api/SPAApi/MultiQueryList")]
        [HttpGet]
        public IEnumerable<MultiQueryApiDataContainer> GetMultiQueryList([FromUri] TempPager filter)
        {
            DateTime cTime = DateTime.Now;

            var cUser = UserProfileService.GetCurrentUser();
            if (string.IsNullOrWhiteSpace(cUser.ID))
                throw new UnauthorizedAccessException();

            // 必填檢查
            if (filter == null)
            {
                HttpContext.Current.Response.StatusCode = 400;
                return null;
            }

            filter.period = this.ProcessArray(filter.period);                       // 評鑑期間  複選、必選
            filter.assessmentItem = this.ProcessArray(filter.assessmentItem);       // 評鑑項目  複選、必選
            filter.belongTo = this.ProcessArray(filter.belongTo);                   // 供應商    複選
            filter.bu = this.ProcessArray(filter.bu);                               // 評鑑單位  複選

            // 必填及格式檢查
            if (filter.period.Length == 0 ||
                filter.assessmentItem.Length == 0 ||
                filter.period.Any(obj => !PeriodUtil.IsPeriodFormat(obj)))
            {
                HttpContext.Current.Response.StatusCode = 400;
                return null;
            }

            // 找出所有資料
            var pager = new Pager() { AllowPaging = false };
            var list = this._mgr.GetMultiQuerySPAList(filter.belongTo, filter.period, filter.bu, filter.assessmentItem, cUser.ID, cTime, pager);

            // 評鑑項目每則一筆
            List<MultiQueryApiDataContainer> retList = new List<MultiQueryApiDataContainer>();
            foreach (var assesmentItem in filter.assessmentItem)
            {
                int start = 0;

                // 計算找出評鑑項目
                var subList = list.Where(obj => string.Compare(assesmentItem.ToUpper(), obj.AssessmentItemID.ToUpper(), true) == 0).ToList();
                if (subList.Count == 0)
                    continue;

                var periodList = subList.Select(obj => obj.Period).Distinct();
                var periodTextList = periodList.ToList();
                do
                {
                    var enumTempPeriod = periodTextList.Take(_periodLength).ToList();
                    var sourceList = enumTempPeriod.Select(obj => new { PeriodName = obj, PeriodObject = PeriodUtil.ParsePeriod(obj) }).OrderBy(obj => obj.PeriodObject.StartDate).ToList();
                    periodTextList.RemoveRange(0, enumTempPeriod.Count());

                    //--- 格式轉換 (標題) ---
                    var columnList = new List<PeriodObj>();
                    for (var i = 0; i < sourceList.Count; i++)
                    {
                        var item = sourceList.ElementAt(i);
                        columnList.Add(new PeriodObj()
                        {
                            Period = item.PeriodName,
                            Text = "Period" + (i + 1),
                            SDate_Text = item.PeriodObject.StartDate?.ToString("yyyy-MM-dd"),
                            EDate_Text = item.PeriodObject.EndDate?.ToString("yyyy-MM-dd"),
                        });
                    }
                    //--- 格式轉換 (標題) ---


                    //--- 格式轉換 (內容) ---
                    DataTable dt = new DataTable();

                    dt.Columns.Add("BU", typeof(string));
                    dt.Columns.Add("ServiceFor", typeof(string));
                    dt.Columns.Add("BelongTo", typeof(string));

                    // 處理動態欄位 Column
                    foreach (var item in enumTempPeriod)
                    {
                        dt.Columns.Add(item + ".PerformanceLevel", typeof(string));
                        dt.Columns.Add(item + ".TotalScore", typeof(string));
                    }

                    var dicInsertedItem = new Dictionary<string, DataRow>();
                    var groupedList = subList.GroupBy(obj => new { obj.BU, obj.ServiceFor, obj.BelongTo });
                    foreach (var gItem in groupedList)
                    {
                        var newKey = $"{gItem.Key.BU}_{gItem.Key.ServiceFor}_{gItem.Key.BelongTo}";
                        DataRow dr;

                        if (!dicInsertedItem.ContainsKey(newKey))
                        {
                            dr = dt.NewRow();
                            dt.Rows.Add(dr);
                            dicInsertedItem.Add(newKey, dr);
                            dr["BU"] = gItem.Key.BU;
                            dr["ServiceFor"] = gItem.Key.ServiceFor;
                            dr["BelongTo"] = gItem.Key.BelongTo;
                        }
                        else
                            dr = dicInsertedItem[newKey];

                        foreach (var item2 in gItem)
                        {
                            dr[item2.Period + ".PerformanceLevel"] = item2.PerformanceLevel;
                            dr[item2.Period + ".TotalScore"] = item2.TotalScore;
                        }
                    }
                    //--- 格式轉換 (內容) ---

                    var retObj = new MultiQueryApiDataContainer()
                    {
                        data = dt,
                        totalPeriods = columnList,
                        AssessmentItem = assesmentItem,
                        AssessmentItem_Text = TET_ParameterService.GetTET_ParametersList(Guid.Parse(assesmentItem))[0].Text
                };

                    retList.Add(retObj);
                }
                while (periodTextList.Any());
            }

            return retList;
        }


        [Route("~/api/SPAApi/Detail/{id}")]
        [HttpGet]
        // GET api/SPAApi/Detail/{id}
        public TET_SupplierSPAModel GetOne([FromUri] Guid id, bool includeApprovalList = false)
        {
            var result = this._mgr.GetSPA(id, includeApprovalList);
            return result;
        }
        #endregion

        [Route("~/api/SPAApi/Create")]
        [HttpPost]
        public IHttpActionResult Create()
        {
            // 因為需要上傳檔案，此處原始 Http 來處理
            DateTime cTime = DateTime.Now;

            var cUser = UserProfileService.GetCurrentUser();
            if (string.IsNullOrWhiteSpace(cUser.ID))
                throw new UnauthorizedAccessException();

            var inp = HttpContext.Current.Request.Form["Main"];
            TET_SupplierSPAModel model;

            // 嘗試做反序列化，如果錯誤的話丟 Bad Request
            try
            {
                model = JsonConvert.DeserializeObject<TET_SupplierSPAModel>(inp);
                if (model == null)
                    return BadRequest("Supplier is required.");
            }
            catch (Exception ex)
            {
                return BadRequest("Supplier is required.");
            }

            // 驗證正確性
            List<string> msgList = new List<string>();
            List<string> tempMsgList;
            var validResult = SupplierSPAValidator.Valid(model, out tempMsgList);
            if (!validResult)
                msgList.AddRange(tempMsgList);

            if (!validResult)
                return BadRequest(JsonConvert.SerializeObject(msgList));

            // 新增
            var newID = _mgr.CreateSPA(model, cUser.ID, cTime);
            return Ok(newID);
        }

        [Route("~/api/SPAApi/Submit")]
        [HttpPost]
        public IHttpActionResult Submit()
        {
            // 因為需要上傳檔案，此處原始 Http 來處理
            DateTime cTime = DateTime.Now;

            var cUser = UserProfileService.GetCurrentUser();
            if (string.IsNullOrWhiteSpace(cUser.ID))
                throw new UnauthorizedAccessException();

            var inp = HttpContext.Current.Request.Form["Main"];
            TET_SupplierSPAModel model;

            // 嘗試做反序列化，如果錯誤的話丟 Bad Request
            try
            {
                model = JsonConvert.DeserializeObject<TET_SupplierSPAModel>(inp);
                if (model == null)
                    return BadRequest("Supplier is required.");
            }
            catch (Exception ex)
            {
                return BadRequest("Supplier is required.");
            }

            // 驗證正確性
            List<string> msgList = new List<string>();
            List<string> tempMsgList;
            var validResult = SupplierSPAValidator.Valid(model, out tempMsgList);
            if (!validResult)
                msgList.AddRange(tempMsgList);

            if (!validResult)
                return BadRequest(JsonConvert.SerializeObject(msgList));

            // 如果還沒新增過，就先新增
            Guid supplierID;
            if (model.ID.HasValue)
            {
                // 查不到資料也視為要新增
                if (this._mgr.GetSPA(model.ID.Value) == null)
                {
                    supplierID = this._mgr.CreateSPA(model, cUser.ID, cTime);
                    model.ID = supplierID;
                }
                else
                {
                    // 如果查得到，就先存檔，避免漏資料
                    this._mgr.ModifySPA(model, cUser.ID, cTime);
                }
            }
            else
            {
                supplierID = this._mgr.CreateSPA(model, cUser.ID, cTime);
                model.ID = supplierID;
            }


            // 送出
            this._mgr.SubmitSPA(model, cUser.ID, cTime);
            return Ok(model.ID);
        }

        [Route("~/api/SPAApi/Modify/{id}")]
        [HttpPost]
        public IHttpActionResult Modify(Guid id)
        {
            // 因為需要上傳檔案，此處原始 Http 來處理
            DateTime cTime = DateTime.Now;

            var cUser = UserProfileService.GetCurrentUser();
            if (string.IsNullOrWhiteSpace(cUser.ID))
                throw new UnauthorizedAccessException();

            var inp = HttpContext.Current.Request.Form["Main"];
            TET_SupplierSPAModel model;

            // 嘗試做反序列化，如果錯誤的話丟 Bad Request
            try
            {
                model = JsonConvert.DeserializeObject<TET_SupplierSPAModel>(inp);
                if (model == null)
                    return BadRequest("Supplier is required.");
            }
            catch (Exception ex)
            {
                return BadRequest("Supplier is required.");
            }

            // 驗證正確性
            var validResult = SupplierSPAValidator.Valid(model, out List<string> tempMsgList);

            if (!validResult)
                return BadRequest(JsonConvert.SerializeObject(tempMsgList));

            // 修改
            try
            {
                this._mgr.ModifySPA(model, cUser.ID, cTime);
            }
            catch (Exception ex)
            {
                return BadRequest(JsonConvert.SerializeObject(new string[] { ex.Message }));
            }
            return Ok();
        }

        [Route("~/api/SPAApi/Revision/{id}")]
        [HttpPost]
        public IHttpActionResult Revision(Guid id)
        {
            // 因為需要上傳檔案，此處原始 Http 來處理
            DateTime cTime = DateTime.Now;

            var cUser = UserProfileService.GetCurrentUser();
            if (string.IsNullOrWhiteSpace(cUser.ID))
                throw new UnauthorizedAccessException();

            // 刪除
            try
            {
                this._mgr.Revision(id, cUser.ID, cTime);
            }
            catch (Exception ex)
            {
                return BadRequest(JsonConvert.SerializeObject(new string[] { ex.Message }));
            }
            return Ok();
        }

        [Route("~/api/SPAApi/Delete/{id}")]
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
                this._mgr.DeleteTET_SPA(id, cUser.ID, cTime);
            }
            catch (Exception ex)
            {
                return BadRequest(JsonConvert.SerializeObject(new string[] { ex.Message }));
            }
            return Ok();
        }

        [Route("~/api/SPAApi/Abord/{id}")]
        [HttpPost]
        public IHttpActionResult Abord(Guid id)
        {
            // 因為需要上傳檔案，此處原始 Http 來處理
            DateTime cTime = DateTime.Now;

            var cUser = UserProfileService.GetCurrentUser();
            if (string.IsNullOrWhiteSpace(cUser.ID))
                throw new UnauthorizedAccessException();

            // 中止
            try
            {
                this._mgr.AbordTET_SPA(id, cUser.ID, cTime);
            }
            catch (Exception ex)
            {
                return BadRequest(JsonConvert.SerializeObject(new string[] { ex.Message }));
            }
            return Ok();
        }


        #region Private Methods
        private string[] ProcessArray(string[] source)
        {
            if (source == null)
                return new string[0];
            else
                return source.Where(obj => !string.IsNullOrEmpty(obj)).ToArray();
        }
        #endregion
    }
}
