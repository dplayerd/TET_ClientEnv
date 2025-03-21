using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Platform.AbstractionClass;
using Platform.Auth;
using Platform.Auth.Models;
using Platform.Portal.Models;
using Platform.WebSite.Filters;
using Platform.WebSite.Models;
using Platform.WebSite.Services;

namespace Platform.WebSite.Controllers
{
    [AuthorizeCore]
    public class HomeController : BaseMVCController
    {
        private RoleManager _roleMgr = new RoleManager();
        UserRoleManager _userRoleMgr = new UserRoleManager();

        // GET: Home
        //[SessionAuthorizeAttribute]
        public ActionResult Index()
        {
            this.TryAddRole();

            this.InitAction();

            SiteViewModel siteViewModel = (SiteViewModel)this.ViewBag.MasterInfo;

            // 取出可以用的頁面
            List<NavigateItemViewModel> pageList = new List<NavigateItemViewModel>();

            NavigateItemViewModel noFolderItem = new NavigateItemViewModel()
            {
                ID = Guid.Empty.ToString(),
                ParentID = null,
                Name = "未分類",
                IconName = "flaticon-app",
                Url = string.Empty,
                MenuType = (byte)MenuTypeEnum.Folder,
                SortIndex = 99,
                IsOuterLink = false,
                IsCurrentPage = false,
                TipText = string.Empty,
                TipType = NavigateItemTipType.Normal,
                Children = new List<NavigateItemViewModel>(),
            };

            for (var i = 0; i < siteViewModel.MainMenus.Count; i++)
            {
                var item = siteViewModel.MainMenus[i];

                if (item.HasChildren())
                {
                    for (var j = 0; j < item.Children.Count; j++)
                    {
                        var subItem = item.Children[j];

                        if (subItem.HasChildren())
                        {
                            foreach (var subSubItem in subItem.Children)
                            {
                                if (!subSubItem.IsKeep)
                                {
                                    subItem.Children.Remove(subSubItem);

                                    if (j > 0)
                                        j -= 1;
                                }
                            }
                        }

                        if (!subItem.IsKeep)
                        {
                            item.Children.Remove(subItem);
                            if (i > 0)
                                i -= 1;
                        }
                    }
                }

                if (item.IsKeep && item.MenuTypeEnum == MenuTypeEnum.Folder)
                {
                    pageList.Add(item);
                }

                if (item.IsKeep && item.MenuTypeEnum != MenuTypeEnum.Folder)
                {
                    noFolderItem.Children.Add(item);
                }
            }

            pageList.Add(noFolderItem);
            pageList = pageList.Distinct().ToList();

            return View(pageList);
        }

        private void TryAddRole()
        {
            // 取得登入者
            var cUser = UserProfileService.GetCurrentUser();

            if (cUser != null)
            {
                // 當登入者不具備一般使用者身份時，為其加入該角色
                var roleList = this._roleMgr.GetRoleGeneralUserList(Pager.GetDefaultPager());
                if (roleList.Any())
                {
                    if (!_userRoleMgr.IsRole(cUser.ID, roleList[0].ID))
                    {
                        _userRoleMgr.MapUserAndRole(new string[] { cUser.ID }, new Guid[] { roleList[0].ID }, cUser.ID, DateTime.Now);
                    }
                }
            }
        }
    }
}
