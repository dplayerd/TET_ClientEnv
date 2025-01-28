using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Platform.AbstractionClass
{
    /// <summary> 期間 </summary>
    public class DatePeriod
    {
        public DatePeriod() { }

        /// <summary> 直接給予兩個日期 </summary>
        /// <param name="startDate">起始日期</param>
        /// <param name="endDate">結束日期</param>
        public DatePeriod(DateTime? startDate, DateTime? endDate)
        {
            StartDate = startDate;
            EndDate = endDate;
        }

        /// <summary> 給予起始日期，以及結束日期的日數，用兩個相加得到結束日期 </summary>
        /// <param name="startDate">起始日期</param>
        /// <param name="endDateCounts">結束日的日數</param>
        public DatePeriod(DateTime startDate, int endDateCounts)
        {
            this.StartDate = startDate;
            this.EndDate = startDate.AddDays(endDateCounts);
        }

        /// <summary> 起始日期 </summary>
        public DateTime? StartDate { get; set; }

        /// <summary> 結束日期 </summary>
        public DateTime? EndDate { get; set; }

        /// <summary> 評鑑期間文字 </summary>
        /// <returns></returns>
        public string PeriodText
        {
            get
            {
                if (!this.StartDate.HasValue || !this.EndDate.HasValue)
                    return null;

                var endDate = this.EndDate.Value;
                var y = this.StartDate.Value.Year - 2000 + 1;
                var yText = y.ToString("00");

                // 第一期
                if (endDate.Month == 9 && endDate.Day == 30)
                    return $"FY{yText}-1H";
                // 第二期
                else if (endDate.Month == 3 && endDate.Day == 31)
                    return $"FY{yText}-2H";
                else
                    return null;
            }
        }
    }
}
