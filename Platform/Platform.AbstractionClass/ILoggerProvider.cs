using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Platform.AbstractionClass
{
    public interface ILoggerProvider
    {
        void WriteError(Exception ex);

        void WriteLog(string message);
    }
}
