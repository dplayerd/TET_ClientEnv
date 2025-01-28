using Platform.AbstractionClass;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Platform.LogService
{
    /// <summary> 預設實作 Logger (輸出至 System.Diagnostics.Debug) </summary>
    internal class DefaultLoggerProvider : ILoggerProvider
    {
        public void WriteError(Exception ex)
        {
            string cTime = DateTime.Now.ToString("yyyyMMdd HH:mm:ss.FFFFFF");
            string msg = $"{Environment.NewLine}{Environment.NewLine}[{cTime}] {ex.ToString()}";


            // 如果是 EF 錯誤，把內容再錄下來
            if (ex is DbEntityValidationException)
            {
                msg += Environment.NewLine + "------";
                var efEx = ex as DbEntityValidationException;

                foreach (var item in efEx.EntityValidationErrors)
                {
                    foreach (var subItem in item.ValidationErrors)
                    {
                        msg += Environment.NewLine + $"{subItem.PropertyName}: {subItem.ErrorMessage}";
                    }
                }
            }

            System.Diagnostics.Debug.WriteLine(msg);
        }

        public void WriteLog(string message)
        {
            string cTime = DateTime.Now.ToString("yyyyMMdd HH:mm:ss.FFFFFF");
            string content = $"{Environment.NewLine}{Environment.NewLine}[{cTime}] {message}";
            System.Diagnostics.Debug.WriteLine(content);
        }
    }
}
