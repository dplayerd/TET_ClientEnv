using Platform.AbstractionClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Platform.Infra
{
    /// <summary> 字串常用方法 </summary>
    public static class StringExtension
    {
        /// <summary> 處理換行符號 (如果原值為 null ，就回傳 null) </summary>
        /// <param name="val"></param>
        /// <param name="toHtmlNewLine"></param>
        /// <returns></returns>
        public static string ReplaceNewLine(this string val, bool toHtmlNewLine = false)
        {
            if (val == null) 
                return val;

            string repleced = val;
            string outputText = (toHtmlNewLine) ? "<br/>" : Environment.NewLine;

            repleced = repleced.Replace("\r\n", outputText);
            repleced = repleced.Replace("\n", outputText);

            return repleced;
        }
    }
}
