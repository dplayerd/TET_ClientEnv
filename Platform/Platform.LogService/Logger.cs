using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Platform.AbstractionClass;

namespace Platform.LogService
{
    public class Logger
    {
        private ILoggerProvider _core;

        public Logger()
        {
            string loggerType = ConfigurationManager.AppSettings["LoggerType"];

            if (loggerType == null || string.Compare(loggerType, "Default", true) == 0)
                _core = new DefaultLoggerProvider();
            else
                _core = new FileLoggerProvider();
        }

        public Logger(ILoggerProvider core)
        {
            _core = core;
        }

        /// <summary> 紀錄錯誤訊息 </summary>
        /// <param name="ex"></param>
        public void WriteError(Exception ex)
        {
            _core.WriteError(ex);
        }

        /// <summary> 紀錄訊息 </summary>
        /// <param name="message"></param>
        public void WriteLog(string message)
        {
            _core.WriteLog(message);
        }
    }
}
