using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace Platform.WebSite.Filters
{
    // REF: 
    // https://dotblogs.com.tw/jesperlai/2018/03/20/170705
    public class PermissionFilter : AuthorizeAttribute
    {
        private static readonly string _unAuthUrl = @"..\Error\Unauthorized401";
        private static readonly string _forbiddenUrl = @"..\Error\Forbidden403";

        //有些頁面也許只想讓admin進入
        public bool IsPageAdminOnly { get; set; } = false;

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            //對有進網域的電腦而言不太可能發生
            if (filterContext == null)
            {
                filterContext.Result = new RedirectResult(_unAuthUrl);
            }

            if (AuthorizeCore(filterContext.HttpContext))
            {
                //驗證有過也不留cache
                SetCachePolicy(filterContext);
            }
            else if (filterContext.HttpContext.User.Identity.IsAuthenticated)
            {
                //通常 AuthorizeCore 沒過都是 403
                filterContext.Result = new RedirectResult(_forbiddenUrl);
            }
            else
            {
                //401 (對有進網域的電腦而言不太可能發生)
                filterContext.Result = new RedirectResult(_unAuthUrl);
            }
        }

        private void SetCachePolicy(AuthorizationContext filterContext)
        {
            //怕下一秒把這個人被改成Unauth，但因為上一秒他成功進來過，被瀏覽器cache permission，導致雖然已unauth卻還是進的來，所以set 0
            var cachePolicy = filterContext.HttpContext.Response.Cache;
            cachePolicy.SetProxyMaxAge(new TimeSpan(0));
            cachePolicy.AddValidationCallback(CacheValidateHandler, null);
        }

        private void CacheValidateHandler(HttpContext context, object data, ref HttpValidationStatus validationStatus)
        {
            validationStatus = OnCacheAuthorization(new HttpContextWrapper(context));
        }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            var adAccount = httpContext.User.Identity.Name;

            //是不是admin only
            if (IsPageAdminOnly && !HasAdminAuth(adAccount))
            {
                return false;
            }
            else
            {
                //其他用個人AD和部門權限判斷
                return HasAuth(adAccount);
            }
        }

        private bool HasAdminAuth(string adAccount)
        {
            ////為了方便講解--------------------------------------------------------
            //var admins = new List<string> { @"MyCompany\John" };
            ////--------------------------------------------------------------------

            //return admins.Contains(adAccount) ? true : false;
            return true;
        }

        private bool HasAuth(string adAccount)
        {
            ////取得允許名單
            //var allowUsers = Users.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            //var allowRoles = Roles.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

            //if (allowUsers.Any() && !allowUsers.Contains(adAccount))
            //{
            //    return false;
            //}

            //if (allowRoles.Any() && !HasRoleAuth(adAccount, allowRoles))
            //{
            //    return false;
            //}

            return true;
        }

        private bool HasRoleAuth(string adAccount, String[] allowRoles)
        {
            return true;
            ////為了方便講解--------------------------------------------------------
            //var employees = new List<Employee>
            //{
            //    new Employee { AD_ACCOUNT=@"MyCompany\John", Department="A部門", Name="John" },
            //    new Employee { AD_ACCOUNT=@"MyCompany\Mary", Department="B部門", Name="Mary" },
            //    new Employee { AD_ACCOUNT=@"MyCompany\Tom", Department="C部門", Name="Tom" },
            //};
            ////--------------------------------------------------------------------

            //var target = employees.Where(q => q.AD_ACCOUNT == adAccount).ToList();

            //if (target.Any() && allowRoles.Contains(target.First().Department))
            //{
            //    return true;
            //}
            //else
            //{
            //    return false;
            //}
        }
    }


}