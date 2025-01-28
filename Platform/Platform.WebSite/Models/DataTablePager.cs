using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Platform.AbstractionClass;

namespace Platform.WebSite.Models
{
    /// <summary> 外掛 jQuery DataTable 的分頁資訊 </summary>
    public class DataTablePager
    {
        /// <summary> 起始頁數 </summary>
        public int? start { get; set; }

        /// <summary> 一頁筆數 </summary>
        public int? length { get; set; }

        /// <summary> anti csrf (捨棄不用此屬性) </summary>
        public string draw { get; set; }


        public Pager ToPager()
        {
            var pager = Pager.GetDefaultPager();

            pager.PageSize = length ?? 10;
            pager.PageIndex = ((start ?? 0) / (length ?? 10)) + 1;

            return pager;
        }
    }
}