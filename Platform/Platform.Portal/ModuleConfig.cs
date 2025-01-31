using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Platform.AbstractionClass;

namespace Platform.Portal
{    
    /// <summary> 模組主要設定 </summary>
    public class ModuleConfig
    {
        /// <summary> 模組名稱 - 站台 </summary>
        public static string ModuleName_Site = "Portal";

        /// <summary> 模組名稱 - 頁面 </summary>
        public static string ModuleName_Page = "Page";

        /// <summary> 站台圖片存檔路徑 </summary>
        public static string SiteLogoFolderPath = "Site";

        /// <summary> 頁面圖片存檔路徑 </summary>
        public static string PageFileFolderPath = "PageIcon";

        /// <summary> 允許上傳的檔案限制 </summary>
        public static FileValidateConfig FileConfig = new FileValidateConfig()
        {
            ModuleName = "Portal",
            Purpose = MediaFileModel.DefaultPurpose,
            AllowSizeMB = 5,
            AllowExtensions = new string[] { ".jpg", ".png", ".gif" },
        };
    }
}
