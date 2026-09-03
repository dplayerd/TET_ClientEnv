using Newtonsoft.Json;
using Platform.WebSite.Services.ScheduledMail;
using System;
using System.Net;
using System.Web.Http;

namespace Platform.WebSite.Controllers
{
    /// <summary>
    /// 排程信件產生 WebAPI，供 Edge 搭配 Windows 工作排程呼叫。
    /// </summary>
    public class ScheduledMailApiController : ApiController
    {
        /// <summary>
        /// 排程建立資料時使用的系統帳號。
        /// </summary>
        private const string SystemUser = "System";

        private readonly IReminderMailService _approvalReminderMailService = new ApprovalReminderMailService();
        private readonly IReminderMailService _spaScoringInfoReminderMailService = new SPAScoringInfoReminderMailService();

        /// <summary>
        /// 產生審核提醒信。
        /// </summary>
        /// <param name="token">排程呼叫 Token。</param>
        /// <returns>提醒信產生結果。</returns>
        [Route("~/api/ScheduledMailApi/GenerateApprovalReminder")]
        [AcceptVerbs("GET", "POST")]
        public IHttpActionResult GenerateApprovalReminder(string token = null)
        {
            return this.Generate(token, this._approvalReminderMailService);
        }

        /// <summary>
        /// 產生 SPA 評鑑計分資料填寫提醒信。
        /// </summary>
        /// <param name="token">排程呼叫 Token。</param>
        /// <returns>提醒信產生結果。</returns>
        [Route("~/api/ScheduledMailApi/GenerateSPAScoringInfoReminder")]
        [AcceptVerbs("GET", "POST")]
        public IHttpActionResult GenerateSPAScoringInfoReminder(string token = null)
        {
            return this.Generate(token, this._spaScoringInfoReminderMailService);
        }

        /// <summary>
        /// 共用提醒信產生流程，集中處理 Token 驗證及錯誤回應格式。
        /// </summary>
        /// <param name="token">排程呼叫 Token。</param>
        /// <param name="service">提醒信服務。</param>
        /// <returns>提醒信產生結果或錯誤訊息。</returns>
        private IHttpActionResult Generate(string token, IReminderMailService service)
        {
            try
            {
                ScheduledMailConfig.ValidateToken(token);

                var result = service.Generate(SystemUser, DateTime.Now);
                return Ok(result);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Content(HttpStatusCode.Unauthorized, ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(JsonConvert.SerializeObject(new string[] { ex.Message }));
            }
        }
    }
}
