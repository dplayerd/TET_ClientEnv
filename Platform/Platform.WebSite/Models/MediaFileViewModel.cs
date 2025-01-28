using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Platform.WebSite.Models
{
    /// <summary>個人多媒體資訊</summary>
    public class MediaFileViewModel
    {
        /// <summary> ID </summary>
        public string ID { get; set; }

        /// <summary> 模組名稱 </summary>
        public string ModuleName { get; set; }

        /// <summary> 模組ID </summary>
        public string MouduleID { get; set; }

        /// <summary> 檔案路徑 </summary>
        public string FilePath { get; set; }

        /// <summary> 原始檔案名稱 </summary>
        public string OrgFileName { get; set; }

        /// <summary> 輸出檔案名稱 </summary>
        public string OutputFileName { get; set; }

        /// <summary> 輸出格式 </summary>
        public string MimeType { get; set; }

        /// <summary> 是否需要授權 </summary>
        public bool RequireAuth { get; set; }

        /// <summary> 是否啟用 </summary>
        public bool IsEnable { get; set; } = true;

        #region Methods
        /// <summary> 檢查是否為空物件 </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static bool IsDefault(UserProfileViewModel model)
        {
            if (string.IsNullOrEmpty(model.ID))
                return true;
            else
                return false;
        }
        #endregion
    }
}