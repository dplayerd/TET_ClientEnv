using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace Platform.WebSite.Services
{
    public class TET_UserService
    {
        public static bool HasLogin()
        {
            Boolean hasSession = false;
            var sessionItem = HttpContext.Current.Session["EmpID"];

            if (sessionItem != null && 
                !string.IsNullOrEmpty(HttpContext.Current.Session["EmpID"].ToString()))
            {
                hasSession = true;
            }

            return hasSession;
        }
    }
}

