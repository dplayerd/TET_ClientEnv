using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BI.SPA_ScoringInfo
{
    /// <summary> 模組主要設定 </summary>
    public class ModuleConfig
    {
        /// <summary> 個人資訊圖片存檔路徑 </summary>
        public static string FolderPath = "SPA_ScoringInfoAttachments";

        /// <summary> 單檔允許容量 </summary>
        public static int AllowSizeMB = 10;

        /// <summary> 檔案總合允許容量 </summary>
        public static int AllowTotalSizeMB = 50;

        /// <summary> 寄信的超連結 </summary>
        public static string EmailRootUrl = "https://FakeDomain/Supplier/";

        /// <summary> 模組名稱 </summary>
        public const string ModuleName = "SPA_ScoringInfo";

        /// <summary> 模組名稱 - SPA評鑑計分資料頁籤顯示設定 </summary>
        public const string ModuleName_SPA_ScoringInfoSheetSetup = "SPA_ScoringInfoSheetSetup";
    }
}
