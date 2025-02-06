using BI.Shared.Extensions;
using Platform.AbstractionClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BI.Shared.Utils
{
    /// <summary> 換算評鑑期間的工具 </summary>
    public class PeriodUtil
    {
        /// <summary> 取得預設的評鑑期間
        /// </summary>
        /// <returns></returns>
        public static DatePeriod GetCurrentPeriod()
        {
            int year = DateTime.Today.Year;
            var period = new DatePeriod()
            {
                StartDate = new DateTime(year + 1, 4, 1),
                EndDate = new DateTime(year + 1, 9, 30)
            };


            if (DateTime.Today >= period.StartDate && DateTime.Today <= period.EndDate)
                return period;
            else if (DateTime.Today < period.StartDate)
                return period.GetPrevPeriod();
            else if (DateTime.Today > period.EndDate)
                return period.GetNextPeriod();

            // 基本上不應該走到這路徑，到這裡一定有大問題
            return null;    
        }


        /// <summary> 將日期取出 
        /// <para> 依照評鑑期間換算出開始日期，評鑑期間的格式為FY@yy-@termH </para>
        /// <para> 若 @term = 1: @sdate為 @yy-1/04/01，若 @term = 2: @sdate為 @yy-1/10/01 </para>
        /// <para>   Ex: 若評鑑期間 = FY23-1H，@sdate = 2022/04/01 </para>
        /// <para>       若評鑑期間 = FY23-2H，@sdate = 2022/10/01 </para>
        /// </summary>
        /// <param name="period"></param>
        /// <returns></returns>
        public static DatePeriod ParsePeriod(string period)
        {
            int year = 2000 + int.Parse(period.Substring(2, 2));
            string range = period.Substring(5, 1);


            if (range == "1")
            {
                return new DatePeriod()
                {
                    StartDate = new DateTime(year - 1, 4, 1),
                    EndDate = new DateTime(year - 1, 9, 30)
                };
            }
            else
            {
                return new DatePeriod()
                {
                    StartDate = new DateTime(year - 1, 10, 1),
                    EndDate = new DateTime(year, 3, 31)
                };
            }
        }

        /// <summary> 檢查輸入值 (ex. FY23-1H) </summary>
        /// <param name="period"></param>
        /// <returns></returns>
        public static bool IsPeriodFormat(string period)
        {
            if (string.IsNullOrWhiteSpace(period))
                return false;

            // 正則表達式模式
            string pattern = @"^FY\d{2}-(1|2)H$";

            // 使用 Regex.Match 方法進行匹配
            Match match = Regex.Match(period, pattern);

            // 檢查匹配結果
            return match.Success;
        }


        /// <summary> 檢查輸入值 (ex. FY23-1H) </summary>
        /// <param name="period"></param>
        /// <returns></returns>
        public static bool IsPeriodFormat(string period, out List<string> msgList)
        {
            msgList = new List<string>();

            if (!IsPeriodFormat(period))
            {
                msgList.Add("評鑑期間 格式不正確，必須為 FY23-1H 的格式");
                return false;
            }

            return true;
        }
    }
}
