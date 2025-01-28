using Platform.AbstractionClass;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Platform.Infra
{
    /// <summary> 欄位必填驗證器 </summary>
    public class ColumnValidator
    {
        private const string _reqText = "為必填欄位";

        /// <summary> 依設定驗證類別填寫狀況 </summary>
        /// <typeparam name="T"> 自行指定型別 </typeparam>
        /// <param name="model"> 要驗證的類別 </param>
        /// <param name="configs"> Property 要驗證的條件設定 </param>
        /// <param name="msgList"> 回傳錯誤訊息 </param>
        /// <returns></returns>
        public static bool ValidProperty<T>(T model, IEnumerable<ValidateConfig> configs, out Dictionary<string, string> msgList)
        {
            msgList = new Dictionary<string, string>();
            PropertyInfo[] properties = typeof(T).GetProperties();

            // 依設定驗證每個欄位
            foreach (var config in configs)
            {
                if (!config.Required)
                    continue;

                var prop = properties.Where(obj => obj.Name == config.Name).FirstOrDefault();
                if (prop == null)
                    continue;

                // 檢查屬性值
                if (prop.PropertyType == typeof(string))
                {
                    string value = (string)prop.GetValue(model);
                    if (string.IsNullOrEmpty(value))
                    {
                        msgList.Add(config.Name, config.Title + _reqText);
                    }
                }
                else if (prop.PropertyType == typeof(string[]))
                {
                    string[] value = (string[])prop.GetValue(model);
                    if (value == null || value.Length == 0)
                    {
                        msgList.Add(config.Name, config.Title + _reqText);
                    }
                }
                else if (prop.PropertyType == typeof(DateTime?))
                {
                    DateTime? value = (DateTime?)prop.GetValue(model);
                    if (!value.HasValue)
                    {
                        msgList.Add(config.Name, config.Title + _reqText);
                    }
                }
                else if (prop.PropertyType == typeof(DateTime))
                {
                    DateTime value = (DateTime)prop.GetValue(model);
                    if (value == DateTime.MinValue)
                    {
                        msgList.Add(config.Name, config.Title + _reqText);
                    }
                }
                else if (prop.PropertyType == typeof(Guid?))
                {
                    Guid? value = (Guid?)prop.GetValue(model);
                    if (!value.HasValue)
                    {
                        msgList.Add(config.Name, config.Title + _reqText);
                    }
                }
                else if (prop.PropertyType == typeof(Guid))
                {
                    Guid value = (Guid)prop.GetValue(model);
                    if (value == Guid.Empty)
                    {
                        msgList.Add(config.Name, config.Title + _reqText);
                    }
                }
            }

            // 回傳結果
            if (msgList.Count > 0)
                return false;
            else
                return true;
        }
    }
}
