using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Platform.WebSite.Models
{
    /// <summary> 分頁項目 </summary>
    public class PagerItemViewModel
    {
        /// <summary> 要顯示的頁碼 </summary>
        public int Index { get; set; }

        /// <summary> 完整超連結 </summary>
        public Uri Url { get; set; }

        /// <summary> 項目種類 </summary>
        public PagerItemType ItemType { get; set; } = PagerItemType.Item;

        /// <summary> 是否為目前頁數 </summary>
        public bool IsCurrent { get; set; }
    }

    public enum PagerItemType
    {
        First,
        Last,
        Prev,
        Next,
        Item
    }
}