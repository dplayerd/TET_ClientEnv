using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using System.Web.SessionState;
using Platform.LogService;
using Platform.WebSite.Services;
using Platform.WebSite.Util;

namespace Platform.WebSite
{
    public class Global : System.Web.HttpApplication
    {
        static object _locker = new object();

        protected void Application_Start(object sender, EventArgs e)
        {
            // 應用程式啟動時執行的程式碼
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            RouteConfig.RegisterRoutes(RouteTable.Routes);


            // 初始化時，讀取設定檔
            ConfigLoader.Init();
        }

        protected void Application_PostAuthorizeRequest()
        {
            //UserProfileService.InitUserSession();
        }

        protected void Session_Start(object sender, EventArgs e)
        {

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {
            var error = HttpContext.Current.Error;

            Logger logger = new Logger();
            logger.WriteError(error);
        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {

        }
    }
}