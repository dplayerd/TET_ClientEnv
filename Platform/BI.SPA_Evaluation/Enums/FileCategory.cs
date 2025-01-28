using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BI.SPA_Evaluation.Enums
{
    public enum FileCategory
    {
        Empty,
        QSM,
        All
    }


    /// <summary> 審核狀態擴充 </summary>
    public static class FileCategoryExtension
    {
        /// <summary> 轉換為文字
        /// </summary>
        /// <param name="enm"> 審核狀態 </param>
        /// <returns></returns>
        public static string ToText(this FileCategory enm)
        {
            switch (enm)
            {
                case FileCategory.Empty:
                    return null;

                case FileCategory.All:
                    return "All";

                case FileCategory.QSM:
                    return "QSM";

                default:
                    return null;
            }
        }
    }
}
