using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Platform.Infra
{
    public class DateTimeExtensions
    {
        public static string GetJS_V_Text()
        {
            return DateTime.Now.ToString("yyyyMMddHHmm");
        }
    }
}
