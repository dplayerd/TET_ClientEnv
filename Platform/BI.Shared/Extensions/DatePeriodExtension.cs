using Platform.AbstractionClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BI.Shared.Extensions
{
    public static class DatePeriodExtension
    {
        /// <summary> 取得下一期 </summary>
        /// <param name="period"></param>
        /// <returns></returns>
        public static DatePeriod GetNextPeriod(this DatePeriod period)
        {
            if (!period.StartDate.HasValue || !period.EndDate.HasValue)
                return null;

            var endDate = period.EndDate.Value;

            // 第一期
            if (endDate.Month == 9 && endDate.Day == 30)
            {
                return new DatePeriod()
                {
                    StartDate = new DateTime(endDate.Year, 10, 1),
                    EndDate = new DateTime(endDate.Year + 1, 3, 31)
                };
            }
            // 第二期
            else if (endDate.Month == 3 && endDate.Day == 31)
            {
                return new DatePeriod()
                {
                    StartDate = new DateTime(endDate.Year, 4, 1),
                    EndDate = new DateTime(endDate.Year, 9, 30)
                };
            }
            else
                return null;
        }

        /// <summary> 取得上一期 </summary>
        /// <param name="period"></param>
        /// <returns></returns>
        public static DatePeriod GetPrevPeriod(this DatePeriod period)
        {
            if (!period.StartDate.HasValue || !period.EndDate.HasValue)
                return null;

            var endDate = period.EndDate.Value;

            // 第一期
            if (endDate.Month == 9 && endDate.Day == 30)
            {
                return new DatePeriod()
                {
                    StartDate = new DateTime(endDate.Year - 1, 10, 1),
                    EndDate = new DateTime(endDate.Year, 3, 31)
                };
            }
            // 第二期
            else if (endDate.Month == 3 && endDate.Day == 31)
            {
                return new DatePeriod()
                {
                    StartDate = new DateTime(endDate.Year - 1, 4, 1),
                    EndDate = new DateTime(endDate.Year - 1, 9, 30)
                };
            }
            else
                return null;
        }
    }
}