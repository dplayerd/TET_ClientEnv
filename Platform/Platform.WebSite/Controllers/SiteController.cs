using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Platform.AbstractionClass;
using Platform.FileSystem;
using Platform.Portal;
using Platform.WebSite.Filters;
using Platform.WebSite.Models;
using Platform.WebSite.Services;
using Platform.WebSite.Util;

namespace Platform.WebSite.Controllers
{
    [AuthorizeCore]
    public class SiteController : BaseMVCController
    {
        private static FileValidateConfig _uploadConfig = ModuleConfig.FileConfig;


        // GET: /Site/Index
        public ActionResult Index()
        {
            this.InitAction();

            var site = SiteManager.GetSite(SiteService.DefaultSiteID);
            if (site == null)
                return new HttpNotFoundResult();

            SiteViewModel vModel = new SiteViewModel()
            {
                ID = site.ID.ToString(),
                Name = site.Name,
                FullName = site.Title,
                HeaderText = site.HeaderText,
                FooterText = site.FooterText,
                ImageUrl = site.ImageUrl,
                MediaFileID = site.MediaFileID.Value
            };

            return View(vModel);
        }

        // POST: /Site/Edit
        [HttpPost]
        public ActionResult Edit(SiteViewModel vModel, HttpPostedFileBase mediaFile)
        {
            this.InitAction();

            //檢查圖片格式
            FileContent fileContent = null;

            if (mediaFile != null)
            {
                fileContent = UploadUtil.ConvertToFileContent(mediaFile);
                if (!FileValidator.ValidFile(fileContent, _uploadConfig))
                {
                    this.AddTipMessage("照片更新失敗，請確認檔案格式是否正確");
                    return View("Index", vModel);
                }
            }

            string userID = UserProfileService.GetCurrentUserID();
            SiteService.ModifySiteInfo(vModel, fileContent, userID);

            this.AddTipMessage("站台資訊更新完成");
            return RedirectToAction("Index");
        }
    }
}