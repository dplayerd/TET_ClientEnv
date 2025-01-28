using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Platform.WebSite.Util
{
    public static class NPOIExtension
    {
        /// <summary>
        /// 判斷Guid是否為空值
        /// </summary>
        /// <param name="cell"></param>
        /// <returns></returns>
        public static ICell SetStyle(this ICell cell, ICellStyle style)
        {
            cell.CellStyle = style;
            return cell;
        }
    }
}