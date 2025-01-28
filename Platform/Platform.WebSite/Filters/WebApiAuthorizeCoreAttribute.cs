using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Routing;
using System.Web.Security;
using Platform.WebSite.Services;

namespace Platform.WebSite.Filters
{
    // REF:
    // https://stackoverflow.com/questions/35271881/customizing-system-web-http-authorizeattribute-within-asp-net-web-api-applicatio
    public class WebApiAuthorizeCoreAttribute : AuthorizeAttribute
    {
        // 驗證邏輯，成功回傳True，失敗回傳False
        protected override bool IsAuthorized(HttpActionContext actionContext)
        {
            // 目前的帳號系統
            bool isPass = UserProfileService.HasLogin();

            // TET 帳號系統


            return isPass;
        }

        protected override void HandleUnauthorizedRequest(HttpActionContext actionContext)
        {
            actionContext.Response = new HttpResponseMessage((HttpStatusCode)401)
            {
                ReasonPhrase = "Unauthorized user"
            };
        }
    }
}