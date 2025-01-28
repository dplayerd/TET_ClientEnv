using Platform.AbstractionClass;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;

namespace Platform.LogService
{
        /// <summary> 預設實作 FileLogger (輸出至指定路徑) </summary>
    internal class FileLoggerProvider : ILoggerProvider
    {
        private static object _locker = new object();

        public void WriteError(Exception ex)
        {
            string message = ex.ToString();

            // 如果是 EF 錯誤，把內容再錄下來
            if(ex is DbEntityValidationException)
            {
                message += Environment.NewLine + "------";
                var efEx = ex as DbEntityValidationException;

                foreach (var item in efEx.EntityValidationErrors)
                { 
                    foreach(var subItem in item.ValidationErrors)
                    {
                        message += Environment.NewLine + $"{subItem.PropertyName}: {subItem.ErrorMessage}" ;
                    }
                }
            }

            WriteMessage(message);
        }

        public void WriteLog(string message)
        {
            WriteMessage(message);
        }

        private static void WriteMessage(string message)
        {
            string folderPath = ConfigurationManager.AppSettings["FileLogPath"];
            string fileName = DateTime.Today.ToString("yyyyMMdd") + ".log";
            string fullPath = Path.Combine(folderPath, fileName);

            string cTime = DateTime.Now.ToString("yyyyMMdd HH:mm:ss.FFFFFF");
            string content = $"{Environment.NewLine}{Environment.NewLine}[{cTime}] {message}";

            lock (_locker)
            {
                if (!Directory.Exists(folderPath))
                    Directory.CreateDirectory(folderPath);

                File.AppendAllText(
                    fullPath,
                    content
                );
            }
        }
    }
}
