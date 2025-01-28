using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Configuration;
using System.Data.SqlClient;
using System.Threading;
using Platform.WebSite.Services;

namespace Platform.WebSite.Controllers
{
    [AllowAnonymous]
    public class TestController : Controller
    {
        // GET: Test
        public ActionResult Index()
        {
            var connConfig = ConfigurationManager.ConnectionStrings["DefaultConnection"];
            if (connConfig == null)
            {
                this.ViewBag.Result = "No Connection String.";
                return View();
            }

            using (SqlConnection conn = new SqlConnection(connConfig.ConnectionString))
            {
                try
                {
                    conn.Open();
                    Thread.Sleep(300);
                    conn.Close();

                    this.ViewBag.Result = "Connect success";
                    return View();
                }
                catch (Exception ex)
                {
                    this.ViewBag.Result = "Connect fail: " + ex.ToString();
                    return View();
                }
            }
        }


        public ActionResult LoginStatus()
        {
            bool isLogined = UserProfileService.HasLogin();
            this.ViewBag.IsCookieLogined = isLogined;

            if (isLogined)
            {
                var item = UserProfileService.GetCurrentUser();
                this.ViewBag.CookieModel = item;
            }

            var empID = System.Web.HttpContext.Current.Session["EmpID"] as string;
            this.ViewBag.IsSessionLogined = !string.IsNullOrEmpty(empID);

            if (!string.IsNullOrEmpty(empID))
            {
                this.ViewBag.SessionModel = empID;
            }

            return View();
        }
    }
}