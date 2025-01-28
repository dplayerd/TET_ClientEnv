using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Platform.WebSite.Models
{
    /// <summary> 站台資訊 </summary>
    public class SiteViewModel
    {
        /// <summary> 站台代碼 </summary>
        public string ID { get; set; }

        /// <summary> 站台名稱 </summary>
        public string Name { get; set; }

        /// <summary> 站台全名 </summary>
        public string FullName { get; set; }

        /// <summary> LOGO 路徑 </summary>
        public string ImageUrl { get; set; } = "https://www.Sample.tw/";

        /// <summary> 站台副標題 </summary>
        public string HeaderText { get; set; }

        /// <summary> 站台結尾文字 </summary>
        public string FooterText { get; set; }

        /// <summary> 檔案代碼 </summary>
        public Guid MediaFileID { get; set; }

        /// <summary> 開發者連結 </summary>
        public string MaintainerUrl { get; set; }

        /// <summary> 目前開啟的頁面 </summary>
        public NavigateItemViewModel CurrentPage { get; set; }

        /// <summary> 完整的頁面結構 </summary>
        public List<NavigateItemViewModel> MainMenus { get; set; }

        /// <summary> 頁首的選單連結 </summary>
        public List<NavigateItemViewModel> HeaderMenus { get; set; }

        /// <summary> 頁尾選單連結 </summary>
        public List<NavigateItemViewModel> FooterMenus { get; set; }
    }
}