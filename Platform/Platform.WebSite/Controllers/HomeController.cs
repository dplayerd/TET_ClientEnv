using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Platform.Portal.Models;
using Platform.WebSite.Filters;
using Platform.WebSite.Models;

namespace Platform.WebSite.Controllers
{
    [AuthorizeCore]
    public class HomeController : BaseMVCController
    {
        // GET: Home
        //[SessionAuthorizeAttribute]
        public ActionResult Index()
        {
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
                                    j -= 1;
                                }
                            }
                        }

                        if (!subItem.IsKeep)
                        {
                            item.Children.Remove(subItem);
                            i -= 1;
                        }
                    }
                }

                if(item.IsKeep && item.MenuTypeEnum == MenuTypeEnum.Folder)
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
    }
}
