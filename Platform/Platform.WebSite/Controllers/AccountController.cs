using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Platform.AbstractionClass;
using Platform.Auth;
using Platform.FileSystem;
using Platform.WebSite.Filters;
using Platform.WebSite.Services;
using Platform.WebSite.Util;

namespace Platform.WebSite.Controllers
{
    public class AccountController : BaseMVCController
    {
        private static FileValidateConfig _uploadConfig = ModuleConfig.FileConfig;


        // GET: Account
        [AllowAnonymous]
        public ActionResult Login()
        {
            this.InitAction();
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult Login(string account, string password)
        {
            string msg;
            if (UserProfileService.Signin(account, password, out msg))
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                this.AddTipMessage(msg);
                this.InitAction();
                return RedirectToAction("Login");
            }
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult Logout()
        {
            UserProfileService.Signout();
            return RedirectToAction("Login");
        }

        [AuthorizeCore]
        public ActionResult UserProfile()
        {
            this.InitAction();
            var userInfo = UserProfileService.GetCurrentUser();
            var user = UserProfileService.GetUser(userInfo.ID);
            return View(user);
        }

        //Guid userID, FormCollection collection
        [AuthorizeCore]
        public ActionResult EditUserProfile()
        {
            this.InitAction();
            var userInfo = UserProfileService.GetCurrentUser();
            var user = UserProfileService.GetUser(userInfo.ID);
            return View(user);
        }

        [AuthorizeCore]
        public ActionResult EditUserPassword()
        {
            this.InitAction();
            var userInfo = UserProfileService.GetCurrentUser();
            var user = UserProfileService.GetUser(userInfo.ID);
            return View(user);
        }

        [AuthorizeCore]
        public ActionResult EditUserPhoto()
        {
            this.InitAction();
            var userInfo = UserProfileService.GetCurrentUser();
            var user = UserProfileService.GetUser(userInfo.ID);
            return View(user);
        }

        // POST: Account/EditUserProfile
        [AuthorizeCore]
        [HttpPost]
        public ActionResult EditUserProfile(FormCollection collection)
        {
            var user = UserProfileService.GetUser(UserProfileService.GetCurrentUser().ID);

            user.Title = collection["Title"];
            user.Email = collection["Email"];
            user.FirstNameCH = collection["FirstName"];
            user.LastNameCH = collection["LastName"];

            UserProfileService.ModifyUser(user);
            this.AddTipMessage("個人資訊更新完成");
            return RedirectToAction("UserProfile");
        }

        // POST: Account/EditUserPassword
        [AuthorizeCore]
        [HttpPost]
        public ActionResult EditUserPassword(FormCollection collection)
        {
            string id = UserProfileService.GetCurrentUser().ID;
            var user = UserProfileService.GetUser(id);
            string newPWD = collection["NewPassword"];
            string newPWD_confirm = collection["ConfirmPassword"];
            string msg;

            if (newPWD == string.Empty || newPWD_confirm == string.Empty)
            {
                this.AddTipMessage("請輸入密碼");
                return RedirectToAction("EditUserPassword");
            }

            if (newPWD != newPWD_confirm)
            {
                this.AddTipMessage("密碼不一致");
                return RedirectToAction("EditUserPassword");
            }
            else
            {
                UserProfileService.ChangePassword(id, newPWD, out msg);
                if (msg != null)
                {
                    this.AddTipMessage(msg);
                    this.InitAction();
                    return RedirectToAction("EditUserPassword");
                }
                else
                {
                    this.AddTipMessage("密碼更新完成");
                    this.InitAction();
                    return RedirectToAction("UserProfile");
                }
            }
        }

        // POST: Account/EditUserPhoto
        [AuthorizeCore]
        [HttpPost]
        public ActionResult EditUserPhoto(HttpPostedFileBase photo)
        {
            this.InitAction();

            if (photo == null || photo.ContentLength == 0)
            {
                this.AddTipMessage("取消變更照片");
                return RedirectToAction("UserProfile");
            }

            //檢查圖片格式
            var fileContent = UploadUtil.ConvertToFileContent(photo);
            if (!FileValidator.ValidFile(fileContent, _uploadConfig))
            {
                this.AddTipMessage("照片更新失敗，請確認檔案格式是否正確");
                return RedirectToAction("EditUserPhoto");
            }

            // 上傳圖片
            string currentUserID = UserProfileService.GetCurrentUserID();

            UserAccountManager manager = new UserAccountManager();
            //manager.ChangeProfileImage(currentUserID, fileContent, currentUserID);

            this.AddTipMessage("個人照片更新完成");
            return RedirectToAction("UserProfile");
        }
    }
}
