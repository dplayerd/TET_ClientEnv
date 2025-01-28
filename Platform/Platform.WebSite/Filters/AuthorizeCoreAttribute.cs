using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Platform.WebSite.Services;

namespace Platform.WebSite.Filters
{
    // REF:
    // https://ithelp.ithome.com.tw/questions/10195967
    public class AuthorizeCoreAttribute : AuthorizeAttribute
    {
        // AuthorizeAttribute包含三個關鍵方法供您override

        //// 核心Function
        //// 可以自定義module邏輯，若沒有要自己處理整個module架構，基本上不覆寫
        //// Source Code: (https://github.com/mono/aspnetwebstack/blob/master/src/System.Web.Mvc/AuthorizeAttribute.cs) 
        //public override void OnAuthorization(AuthorizationContext filterContext) { }

        // 驗證邏輯，成功回傳True，失敗回傳False
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            // 目前的帳號系統
            bool isPass = UserProfileService.HasLogin();

            // TET 帳號系統


            return isPass;
        }

        // 驗證失敗要做什麼
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            // 設定要執行的Result
            var temp = new RouteValueDictionary()
            {
                { "controller", "Account"},
                { "action", "Login" }
            };
            filterContext.Result = new RedirectToRouteResult(temp);
            //filterContext.Result = new RedirectResult("https://tethome.asia.tel.com/NoPermission.aspx");
        }
    }
}