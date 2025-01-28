using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Platform.Infra
{
    public class GuidExtension
    {
        /// <summary>
        /// 判斷Guid是否為空值
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static bool IsNullorEmpty(Guid? id)
        {
            if (id == null || id == Guid.Empty)
                return true;     
            else
                return false;             
        }
    }
}
