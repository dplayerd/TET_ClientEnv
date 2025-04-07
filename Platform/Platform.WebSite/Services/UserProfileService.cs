using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Security;
using Platform.AbstractionClass;
using Platform.Auth;
using Platform.Auth.Models;
using Platform.FileSystem;
using Platform.WebSite.Models;
using System.Runtime.CompilerServices;

namespace Platform.WebSite.Services
{
    public class UserProfileService
    {
        private const string _currentUserKey = "_CurrentLoginedUser";
        private const string _roleName = "一般使用者";

        #region Private Methods
        /// <summary> 轉換為使用者資訊 </summary>
        /// <param name="userInfo"></param>
        /// <returns></returns>
        private static UserProfileViewModel BuildUserViewModel(UserAccountModel userInfo)
        {
            var viewModel = new UserProfileViewModel()
            {
                ID = userInfo.ID.ToString(),
                FirstNameCH = userInfo.FirstNameCH,
                LastNameCH = userInfo.LastNameCH,
                FirstNameEN = userInfo.FirstNameEN,
                LastNameEN = userInfo.LastNameEN,
                Account = userInfo.EmpID,
                Email = userInfo.EMail,
                Title = userInfo.Title,
                UnitCode = userInfo.UnitCode,
                UnitName = userInfo.UnitName,
                LeaderID = userInfo.LeaderID,
                HasLogined = true,
                AvatorUrl = null
            };


            MediaFileManager mediaFileManager = new MediaFileManager();
            var mediaFile = mediaFileManager.GetMediaFile(ModuleConfig.ModuleName, viewModel.ID);
            viewModel.AvatorUrl = mediaFile?.FilePath;

            return viewModel;
        }

        /// <summary> 轉換為使用者資訊 </summary>
        /// <param name="userInfo"></param>
        /// <returns></returns>
        private static UserProfileViewModel BuildUserViewModel(UserModel userInfo)
        {
            var viewModel = new UserProfileViewModel()
            {
                ID = userInfo.UserID,
                FirstNameCH = userInfo.FirstNameCH,
                LastNameCH = userInfo.LastNameCH,
                FirstNameEN = userInfo.FirstNameEN,
                LastNameEN = userInfo.LastNameEN,
                Account = userInfo.EmpID,
                Email = userInfo.EMail,
                UnitCode = userInfo.UnitCode,
                UnitName = userInfo.UnitName,
                LeaderID = userInfo.LeaderID,
                HasLogined = true,
                AvatorUrl = null
            };


            MediaFileManager mediaFileManager = new MediaFileManager();
            var mediaFile = mediaFileManager.GetMediaFile(ModuleConfig.ModuleName, viewModel.ID);
            viewModel.AvatorUrl = mediaFile?.FilePath;

            return viewModel;
        }

        /// <summary> 取得目前登入者資訊 </summary>
        /// <returns></returns>
        private static UserAccountModel GetIdentityModel()
        {
            if (HttpContext.Current == null)
                return null;

            // 設定快取
            if (HttpContext.Current.Items != null && HttpContext.Current.Items[_currentUserKey] != null)
                return (UserAccountModel)HttpContext.Current.Items[_currentUserKey];

            if (HttpContext.Current.User == null)
                return null;

            var user = HttpContext.Current.User;

            if (user == null)
                return null;

            if (HttpContext.Current.Request == null)
                return null;

            bool isAuthed = HttpContext.Current.Request.IsAuthenticated;
            if (isAuthed && user != null)
            {
                var identity = HttpContext.Current.User.Identity as FormsIdentity;
                var usermodel = JsonConvert.DeserializeObject<UserAccountModel>(identity.Ticket.UserData);

                // 設定快取
                HttpContext.Current.Items[_currentUserKey] = usermodel;
                return usermodel;
            }
            else
            {
                return null;
            }
        }
        #endregion

        #region Login & Status
        /// <summary> 嘗試登入 </summary>
        /// <param name="account"> 帳號 </param>
        /// <param name="password"> 密碼 </param>
        /// <param name="msg"> 錯誤訊息 </param>
        /// <returns></returns>
        public static bool Signin(string account, string password, out string msg)
        {
            var loginMgr = new LoginManager();
            var roleMgr = new RoleManager();
            var userRoleMgr = new UserRoleManager();
            var userModel = loginMgr.TryLogin(account, password, out msg);

            if (userModel == null)
            {
                return false;
            }
            else
            {
                bool isPersistance = false;
                string[] roles = { };                           // 無角色
                DateTime ticketTime = DateTime.Now;             // 發證時間
                DateTime expireTime = ticketTime.AddHours(12);  // 逾期時間
                string userModelText = JsonConvert.SerializeObject(userModel);

                FormsAuthentication.SetAuthCookie(userModel.EmpID, isPersistance);
                FormsAuthenticationTicket ticket =
                    new FormsAuthenticationTicket(
                        1,                  // 版本
                        userModel.EmpID,  // 帳號
                        ticketTime,         // 發證時間
                        expireTime,         // 逾期時間
                        isPersistance,      // 是否存在50年
                        userModelText       // 其它個人資訊
                    );

                FormsIdentity identity = new FormsIdentity(ticket);
                HttpCookie cookie =
                    new HttpCookie(
                        FormsAuthentication.FormsCookieName,
                        FormsAuthentication.Encrypt(ticket)
                    );

                cookie.HttpOnly = true;
                cookie.Expires = expireTime;
                HttpContext.Current.User = new GenericPrincipal(identity, roles);
                HttpContext.Current.Response.Cookies.Add(cookie);

                // 當登入者不具備一般使用者身份時，為其加入該角色
                var roleList = roleMgr.GetRoleAdminList(_roleName, Pager.GetDefaultPager());
                if (roleList.Any())
                {
                    if (!userRoleMgr.IsRole(userModel.ID, roleList[0].ID))
                    {
                        userRoleMgr.MapUserAndRole(new string[] { userModel.ID }, new Guid[] { roleList[0].ID }, userModel.EmpID, DateTime.Now);
                    }
                }


                return true;
            }
        }

        /// <summary> 登出 </summary>
        public static void Signout()
        {
            if (HasLogin())
                FormsAuthentication.SignOut();
        }

        /// <summary> 是否已登入 </summary>
        /// <returns></returns>
        public static bool HasLogin()
        {
            if (GetIdentityModel() == null)
                return false;
            else
                return true;
        }

        /// <summary> 取得目前登入者的 ID </summary>
        /// <returns></returns>
        public static string GetCurrentUserID()
        {
            var model = GetIdentityModel();

            if (model == null)
                return null;
            else
                return model.ID;
        }

        /// <summary> 取得現在登入者 </summary>
        /// <returns></returns>
        public static UserProfileViewModel GetCurrentUser()
        {
            var model = GetIdentityModel();

            if (model == null)
                return UserProfileViewModel.GetDefault();

            return BuildUserViewModel(model);
        }
        #endregion

        #region Manage
        /// <summary> 查詢使用者 </summary>
        /// <param name="uid">使用者 id </param>
        /// <returns></returns>
        public static UserProfileViewModel GetUser(string uid)
        {
            UserManager mgr = new UserManager();
            var userInfo = mgr.GetUser(uid);

            if (userInfo == null)
                return UserProfileViewModel.GetDefault();
            else
                return BuildUserViewModel(userInfo);
        }

        /// <summary> 取得使用者清單 </summary>
        /// <param name="pager"></param>
        /// <returns></returns>
        public static List<UserProfileViewModel> GetUserList(Pager pager)
        {
            UserManager mgr = new UserManager();
            var list = mgr.GetUserList(pager);

            var retList =
                (from userInfo in list
                 select new UserProfileViewModel()
                 {
                     ID = userInfo.UserID,
                     FirstNameCH = userInfo.FirstNameCH,
                     LastNameCH = userInfo.LastNameCH,
                     FirstNameEN = userInfo.FirstNameEN,
                     LastNameEN = userInfo.LastNameEN,
                     Account = userInfo.EmpID,
                     Email = userInfo.EMail,
                     HasLogined = true
                 }).ToList();

            return retList;
        }

        /// <summary> 建立使用者 </summary>
        /// <param name="user"></param>
        /// <param name="password"></param>
        public static void CreateUser(UserProfileViewModel user, string password)
        {
            if (!HasLogin())
            {
                throw new Exception("Auth Exception");
            }

            UserManager mgr = new UserManager();
            Guid.TryParse(user.ID, out Guid uid);
            var userInfo = new UserAccountModel();

            userInfo.EmpID = user.Account;
            userInfo.EMail = user.Email;
            userInfo.FirstNameCH = user.FirstNameCH;
            userInfo.LastNameCH = user.LastNameCH;

            var currentUserID = GetCurrentUserID();
            //mgr.CreateUser(userInfo, password, currentUserID);
        }

        /// <summary> 修改使用者資訊 </summary>
        /// <param name="user"></param>
        /// <param name="currentUserID"></param>
        public static void ModifyUser(UserProfileViewModel user)
        {
            if (!HasLogin())
            {
                throw new Exception("Auth Exception");
            }

            UserManager mgr = new UserManager();
            Guid.TryParse(user.ID, out Guid uid);
            var userInfo = mgr.GetUser(user.ID);

            userInfo.EMail = user.Email;
            userInfo.FirstNameCH = user.FirstNameCH;
            userInfo.LastNameCH = user.LastNameCH;

            var currentUserID = GetCurrentUserID();
            //mgr.ModifyUser(userInfo, currentUserID);
        }

        /// <summary> 檢查輸入值 </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public static bool CheckInput(UserProfileViewModel user)
        {
            var isValid =
                string.IsNullOrWhiteSpace(user.FirstNameCH) ||
                string.IsNullOrWhiteSpace(user.LastNameCH) ||
                string.IsNullOrWhiteSpace(user.Title) ||
                string.IsNullOrWhiteSpace(user.Email) ||
                string.IsNullOrWhiteSpace(user.Account);

            if (isValid)
                return false;
            else
                return true;
        }

        /// <summary> 刪除使用者 </summary>
        /// <param name="uid"></param>
        public static void DeleteUser(string uid)
        {
            if (!HasLogin())
            {
                throw new Exception("Auth Exception");
            }

            UserManager mgr = new UserManager();
            var userInfo = mgr.GetUser(uid);

            var currentUserID = GetCurrentUserID();
            //mgr.DeleteUser(userInfo, currentUserID);
        }
        #endregion

        #region Password
        public static void ChangePassword(string uid, string newPassword, out string msg)
        {
            if (!HasLogin())
            {
                throw new Exception("Auth Exception");
            }

            UserManager mgr = new UserManager();
            var userInfo = mgr.GetUser(uid);

            var currentUserID = GetCurrentUserID();
            //mgr.ChangePassword(userInfo, currentUserID, newPassword, out msg);
            msg = string.Empty;
        }
        #endregion
    }
}