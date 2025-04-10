using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Platform.WebSite.Models
{
    /// <summary> 登入來源 </summary>
    public enum LoginSource
    {
    /// <summary> 標準 </summary>
        Standard,
        /// <summary> AD </summary>
        AD
    }

    /// <summary> 網站用的設定 </summary>
    public class WebSiteConfig
    {
        /// <summary> 登入系統種類 </summary>
        public static LoginSource LoginType { get; set; } = LoginSource.Standard;
    }
}