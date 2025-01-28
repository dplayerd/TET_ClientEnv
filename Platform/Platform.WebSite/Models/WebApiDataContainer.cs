using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Platform.WebSite.Models
{
    /// <summary> jQuery DataTable 用的回傳物件 </summary>
    /// <typeparam name="T"></typeparam>
    public class WebApiDataContainer<T>
    {
        /// <summary> 總筆數 </summary>
        public int recordsTotal { get; set; }

        /// <summary> 過濾後剩下的總筆數 </summary>
        public int recordsFiltered { get; set; }

        /// <summary> 過濾後的資料 </summary>
        public IEnumerable<T> data { get; set; }
    }
}