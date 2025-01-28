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
        /// <summary> 模組名稱 </summary>
        public static string ModuleName = "Portal";

        /// <summary> 站台圖片存檔路徑 </summary>
        public static string SiteLogoFolderPath = "Site";

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
