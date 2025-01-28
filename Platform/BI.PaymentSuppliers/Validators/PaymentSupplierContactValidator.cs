using BI.PaymentSuppliers.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BI.PaymentSuppliers.Validators
{
    /// <summary> 一般付款對象聯絡人欄位驗證設定 </summary>
    public class PaymentSupplierContactValidator
    {
        private const string _reqText = "為必填欄位";

        /// <summary> 設定資料 </summary>
        private static List<ValidConfig> _validConfigs = new List<ValidConfig>()
        {
            new ValidConfig() { RequiredOnCreate =  true, RequiredOnModify =  true, ShowOnCreate =  true, ShowOnModify =  true, ColumnName = nameof(TET_PaymentSupplierContactModel.ContactName), ColumnTitle = "姓名" },
            new ValidConfig() { RequiredOnCreate =  true, RequiredOnModify =  true, ShowOnCreate =  true, ShowOnModify =  true, ColumnName = nameof(TET_PaymentSupplierContactModel.ContactTel),  ColumnTitle = "電話" },
        };

        /// <summary> 取得全部設定 </summary>
        /// <returns></returns>
        public static List<ValidConfig> GetValidConfigs()
        {
            return _validConfigs;
        }

        /// <summary> 驗證新增資料 </summary>
        /// <param name="model"> 輸入資料 </param>
        /// <param name="msgList"> 錯誤訊息 </param>
        /// <returns></returns>
        public static bool ValidCreate(TET_PaymentSupplierContactModel model, out List<string> msgList)
        {
            var result = CheckRequired(model, true, out msgList);

            if (msgList.Count > 0)
                return false;
            else
                return true;
        }


        /// <summary> 驗證編輯資料 </summary>
        /// <param name="model"> 輸入資料 </param>
        /// <param name="msgList"> 錯誤訊息 </param>
        /// <returns></returns>
        public static bool ValidModify(TET_PaymentSupplierContactModel model, out List<string> msgList)
        {
            var result = CheckRequired(model, false, out msgList);

            if (msgList.Count > 0)
                return false;
            else
                return true;
        }

        /// <summary> 根據設定，驗證每個欄位是否都有填值 </summary>
        /// <param name="model"> 原始資料 </param>
        /// <param name="isCreateMode"> 是否為新增模式 </param>
        /// <param name="msgList"> 錯誤訊息 </param>
        /// <returns></returns>
        private static bool CheckRequired(TET_PaymentSupplierContactModel model, bool isCreateMode, out List<string> msgList)
        {
            msgList = new List<string>();
            PropertyInfo[] properties = typeof(TET_PaymentSupplierContactModel).GetProperties();

            // 依設定驗證每個欄位
            foreach (var config in _validConfigs)
            {
                var prop = properties.Where(obj => obj.Name == config.ColumnName).FirstOrDefault();
                if (prop == null)
                    continue;

                // 如果是新增模式，且必填
                // 如果是編輯模式，且必填
                bool isCheckInThisMode =
                    (isCreateMode && config.RequiredOnCreate) ||
                    (!isCreateMode && config.RequiredOnModify);

                // 檢查屬性值
                if (isCheckInThisMode)
                {
                    if (prop.PropertyType == typeof(string))
                    {
                        string value = (string)prop.GetValue(model);
                        if (string.IsNullOrEmpty(value))
                        {
                            msgList.Add(config.ColumnTitle + _reqText);
                        }
                    }
                    else if (prop.PropertyType == typeof(DateTime?))
                    {
                        DateTime? value = (DateTime?)prop.GetValue(model);
                        if (!value.HasValue)
                        {
                            msgList.Add(config.ColumnTitle + _reqText);
                        }
                    }
                    else if (prop.PropertyType == typeof(DateTime))
                    {
                        DateTime value = (DateTime)prop.GetValue(model);
                        if (value == DateTime.MinValue)
                        {
                            msgList.Add(config.ColumnTitle + _reqText);
                        }
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
