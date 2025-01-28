using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Platform.AbstractionClass;
using Platform.WebSite.Filters;
using Platform.WebSite.Models;
using Platform.WebSite.Services;

namespace Platform.WebSite.Controllers
{
    [AuthorizeCore]
    public class UserManagerController : BaseMVCController
    {
        // GET: UserManager
        public ActionResult Index()
        {
            this.InitAction();

            var pager = Pager.GetDefaultPager();
            var list = UserProfileService.GetUserList(pager);

            return View(list);
        }

        // GET: UserManager/Edit/5
        public ActionResult Edit(string id)
        {
            this.InitAction();
            var user = UserProfileService.GetUser(id);

            return View(user);
        }

        // POST: UserManager/Edit/5
        [HttpPost]
        public ActionResult Edit(string id, FormCollection collection)
        {
            var user = UserProfileService.GetUser(id);

            user.Title = collection["Title"];
            user.Email = collection["Email"];
            user.FirstNameCH = collection["FirstName"];
            user.LastNameCH = collection["LastName"];

            if (!UserProfileService.CheckInput(user))
            {
                this.AddTipMessage("尚有未填項目"); ;
                return RedirectToAction("Edit");
            }

            UserProfileService.ModifyUser(user);
            this.AddTipMessage("更新成功");
            return RedirectToAction("Index");
        }

        // GET: UserManager/Create
        public ActionResult Create()
        {
            this.InitAction();
            var obj = UserProfileViewModel.GetDefault();

            return View("Edit", obj);
        }

        // POST: UserManager/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            var user = new UserProfileViewModel();

            user.FirstNameCH = collection["FirstName"];
            user.LastNameCH = collection["LastName"];
            user.Title = collection["Title"];
            user.Email = collection["Email"];
            user.Account = collection["Account"];

            string pwd = "A123456";

            if (!UserProfileService.CheckInput(user))
            {
                this.AddTipMessage("尚有未填項目"); ;
                return RedirectToAction("Create");
            }

            UserProfileService.CreateUser(user, pwd);
            this.AddTipMessage("新增完成");
            return RedirectToAction("Index");
        }

        // GET: UserManager/Delete/5
        [HttpPost]
        public ActionResult Delete(string id)
        {
            UserProfileService.DeleteUser(id);
            this.AddTipMessage("刪除成功");
            return RedirectToAction("Index");
        }

        // GET: UserManager/Password/5
        public ActionResult Password(string id)
        {
            var user = UserProfileService.GetUser(id);
            this.InitAction();
            return View(user);
        }

        // GET: UserManager/Password/5
        [HttpPost]
        public ActionResult Password(string id, FormCollection collection)
        {
            string newPWD = collection["NewPassword"];
            string newPWD_confirm = collection["ConfirmPassword"];
            var user = UserProfileService.GetUser(id);
            string msg;

            if (newPWD == string.Empty || newPWD_confirm == string.Empty)
            {
                this.AddTipMessage("尚有密碼未填");
                return RedirectToAction("Password");
            }

            if (newPWD != newPWD_confirm)
            {
                this.AddTipMessage("密碼不一致");
                return RedirectToAction("Password");
            }
            else
            {
                UserProfileService.ChangePassword(id, newPWD, out msg);
                if (msg != null)
                {
                    this.AddTipMessage(msg);
                    this.InitAction();
                    return RedirectToAction("Password");
                }
                else
                {
                    this.AddTipMessage("密碼更新完成");
                    this.InitAction();
                    return RedirectToAction("Index");
                }
            }
        }
    }
}
