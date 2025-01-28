using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BI.SampleData.Models
{
    public class SampleDataFilterConditions
    {
        /// <summary> 過濾 id (留 NULL 則不過濾) </summary>
        public int? ID { get; set; }

        /// <summary> 過濾姓名包含的文字 (留空則不過濾) </summary>
        public string Name { get; set; }

        /// <summary> 過濾職稱包含的文字 (留空則不過濾) </summary>
        public string Title { get; set; }

        /// <summary> 過濾起始日期大於指定時間 (留空則不過濾) </summary>
        public DateTime? StartDate { get; set; }

        /// <summary> 過濾結束日期小於指定時間 (留空則不過濾) </summary>
        public DateTime? EndDate { get; set; }
    }
}
