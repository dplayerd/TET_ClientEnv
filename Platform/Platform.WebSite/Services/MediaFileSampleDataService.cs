using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using Platform.AbstractionClass;
using Platform.Auth;
using Platform.Infra;
using Platform.WebSite.Models;

namespace Platform.WebSite.Services
{
    public class MediaFileSampleDataService
    {
        private static List<MediaFileViewModel> _mediaList { get; set; }

        #region ME
        /// <summary>
        /// 寫入新檔案
        /// </summary>
        /// <param name="file"></param>
        /// <param name="cUser"></param>
        /// <param name="cTime"></param>
        public static void CreateMediaData(List<HttpPostedFileBase> file, Guid? cUser, DateTime cTime)
        {
            var list = MediaFileSampleDataService.BuildSampleDataList();

            foreach (var item in file)
            {
                var fileName = Path.GetFileName(item.FileName);
                var newName = $"{DateTime.Now.ToString("ddmmss")}{fileName}";
                var path = Path.Combine(System.Web.HttpContext.Current.Server.MapPath("~/MediaFileUpload"), newName);
                item.SaveAs(path);

                MediaFileViewModel mdView = new MediaFileViewModel();
                mdView.ID = UserProfileService.GetCurrentUser().ID;
                mdView.OrgFileName = fileName;
                mdView.FilePath = $"MediaFileUpload/{fileName}";
                mdView.MimeType = MimeMapping.GetMimeMapping(item.ToString());

                list.Add(mdView);
            }
        }

        /// <summary>
        /// 編輯現有檔案
        /// </summary>
        /// <param name="file"></param>
        /// <param name="cUser"></param>
        /// <param name="cTime"></param>
        public static void ModifyMediaData(List<HttpPostedFileBase> file, Guid? cUser, DateTime cTime)
        {
            foreach (var item in file)
            {
                var fileName = Path.GetFileName(item.FileName);
                var newName = $"{DateTime.Now.ToString("ddmmss")}{fileName}";
                var path = Path.Combine(System.Web.HttpContext.Current.Server.MapPath("~/MediaFileUpload"), newName);
                item.SaveAs(path);

                MediaFileViewModel mdView = new MediaFileViewModel();
                mdView.ID = UserProfileService.GetCurrentUser().ID;
                mdView.OrgFileName = fileName;
                mdView.FilePath = $"MediaFileUpload/{fileName}";
                mdView.MimeType = MimeMapping.GetMimeMapping(item.ToString());
                MediaFileSampleDataService.ModifyMedia(mdView, cUser.Value, cTime);
            }
        }

        /// <summary> 編輯資料 </summary>
        /// <param name="model"></param>
        /// <param name="userID"> 修改者帳號代碼 </param>
        /// <param name="cTime"> 修改時間 </param>
        /// <exception cref="Exception"></exception>
        public static void ModifyMedia(MediaFileViewModel model, Guid userID, DateTime cTime)
        {
            var mediafile = MediaFileSampleDataService.BuildSampleDataList().Where(obj => obj.ID == model.ID).FirstOrDefault();

            mediafile.FilePath = model.FilePath;
            mediafile.OrgFileName = model.OrgFileName;
            mediafile.MimeType = model.MimeType;
            mediafile.RequireAuth = model.RequireAuth;
            mediafile.IsEnable = model.IsEnable;
        }
        #endregion

        #region private
        private static List<MediaFileViewModel> BuildSampleDataList()
        {
            if (_mediaList != null)
            {
                var list = new List<MediaFileViewModel>();
                for (int i = 1; i <= 50; i++)
                {
                    list.Add(new MediaFileViewModel()
                    {
                        ID = $"ID_{i}",
                        ModuleName = $"ModuleName{i}",
                        MouduleID = $"ModuleName{i}",
                        OrgFileName = "OrgFileName" + i,
                        MimeType = "MimeType" + i,
                        FilePath = $"~/MediaFileUpload/{i}",
                        RequireAuth = true,
                        IsEnable = true,
                    });
                }
                _mediaList = list;
            }
            return _mediaList;
        }
        #endregion
    }
}