using BI.Shared;
using BI.Shared.Models;
using Platform.AbstractionClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Web;
using System.Windows.Input;
using static Platform.WebSite.Models.PageRoleUpdateModel;

namespace Platform.WebSite.Services
{
    public class TET_ParameterService
    {
        private const string _parameterListKey = "__parameterListKey";
        private static TET_ParametersManager _mgr = new TET_ParametersManager();

        /// <summary> 使用 ID 或是 Item 欄位做為 Key </summary>
        public enum KeyType
        {
            Id, Item
        }

        /// <summary>
        /// 取得 共用參數 清單
        ///</summary>
        ///<param name="typeName"> 種類 </param>
        ///<param name="keyType"> Key 使用哪個欄位 (預設為 Item) </param>
        /// <returns></returns>
        public static List<KeyTextModel> GetTET_ParametersList(string typeName, KeyType keyType = KeyType.Item)
        {
            if (HttpContext.Current == null)
                return new List<KeyTextModel>();

            List<TET_ParametersModel> sourceList;

            if (HttpContext.Current.Items[_parameterListKey] == null)
            {
                var list = _mgr.GetAllParametersKeyTextList();
                HttpContext.Current.Items[_parameterListKey] = list;
                sourceList = list;
            }
            else
            {
                sourceList = (List<TET_ParametersModel>)HttpContext.Current.Items[_parameterListKey];
            }

            var retList = sourceList.Where(obj => string.Compare(typeName, obj.Type, true) == 0).ToList();

            if (keyType == KeyType.Item)
                return retList.Select(obj => new KeyTextModel() { Key = obj.Item, Text = obj.Item }).ToList();
            else
                return retList.Select(obj => new KeyTextModel() { Key = obj.ID.ToString(), Text = obj.Item }).ToList();

        }

        public static List<KeyTextModel> GetTET_ParametersList(string typeName)
        {
            if (HttpContext.Current == null)
                return new List<KeyTextModel>();

            List<TET_ParametersModel> sourceList;

            if (HttpContext.Current.Items[_parameterListKey] == null)
            {
                var list = _mgr.GetAllParametersKeyTextList();
                HttpContext.Current.Items[_parameterListKey] = list;
                sourceList = list;
            }
            else
            {
                sourceList = (List<TET_ParametersModel>)HttpContext.Current.Items[_parameterListKey];
            }

            var retList = sourceList.Where(obj => string.Compare(typeName, obj.Type, true) == 0).ToList();
            return retList.Select(obj => new KeyTextModel() { Key = obj.ID.ToString().ToUpper(), Text = obj.Item }).ToList();
        }

        public static List<KeyTextModel> GetTET_ParametersList1(string typeName)
        {
            if (HttpContext.Current == null)
                return new List<KeyTextModel>();

            List<TET_ParametersModel> sourceList;

            if (HttpContext.Current.Items[_parameterListKey] == null)
            {
                var list = _mgr.GetAllParametersKeyTextList();
                HttpContext.Current.Items[_parameterListKey] = list;
                sourceList = list;
            }
            else
            {
                sourceList = (List<TET_ParametersModel>)HttpContext.Current.Items[_parameterListKey];
            }

            var retList = sourceList.Where(obj => string.Compare(typeName, obj.Type, true) == 0).ToList();
            return retList.Select(obj => new KeyTextModel() { Key = obj.Item, Text = obj.Item }).ToList();
        }

        public static List<KeyTextModel> GetTET_ParametersList(Guid id)
        {
            if (HttpContext.Current == null)
                return new List<KeyTextModel>();

            List<TET_ParametersModel> sourceList;

            if (HttpContext.Current.Items[_parameterListKey] == null)
            {
                var list = _mgr.GetAllParametersKeyTextList();
                HttpContext.Current.Items[_parameterListKey] = list;
                sourceList = list;
            }
            else
            {
                sourceList = (List<TET_ParametersModel>)HttpContext.Current.Items[_parameterListKey];
            }

            var retList = sourceList.Where(obj => obj.ID == id).ToList();
            return retList.Select(obj => new KeyTextModel() { Key = obj.ID.ToString().ToUpper(), Text = obj.Item }).ToList();
        }

        /// <summary>
        /// 取得 共用參數種類 清單
        ///</summary>
        /// <returns></returns>
        public static List<string> GetTET_ParametersTypeList()
        {
            return _mgr.GetTET_ParametersTypeList();
        }

        /// <summary> 讀取單筆的資料 </summary>
        /// <param name="typeName"></param>
        /// <param name="itemName"></param>
        /// <returns></returns>
        public static KeyTextModel<Guid> GetTET_Parameters_ID(string typeName, string itemName)
        {
            List<TET_ParametersModel> sourceList;

            if (HttpContext.Current.Items[_parameterListKey] == null)
            {
                var list = _mgr.GetAllParametersKeyTextList();
                HttpContext.Current.Items[_parameterListKey] = list;
                sourceList = list;
            }
            else
            {
                sourceList = (List<TET_ParametersModel>)HttpContext.Current.Items[_parameterListKey];
            }

            var retList = sourceList.Where(obj => string.Compare(typeName, obj.Type, true) == 0 && string.Compare(itemName, obj.Item, true) == 0).ToList();
            return retList.Select(obj => new KeyTextModel<Guid>() { Key = obj.ID, Text = obj.Item }).FirstOrDefault();
        }

        /// <summary> 讀取單筆的資料 </summary>
        /// <param name="typeName"></param>
        /// <param name="itemName"></param>
        /// <returns></returns>
        public static KeyTextModel GetTET_Parameters_Name(string typeName, string itemName)
        {
            List<TET_ParametersModel> sourceList;

            if (HttpContext.Current.Items[_parameterListKey] == null)
            {
                var list = _mgr.GetAllParametersKeyTextList();
                HttpContext.Current.Items[_parameterListKey] = list;
                sourceList = list;
            }
            else
            {
                sourceList = (List<TET_ParametersModel>)HttpContext.Current.Items[_parameterListKey];
            }

            var retList = sourceList.Where(obj => string.Compare(typeName, obj.Type, true) == 0 && string.Compare(itemName, obj.Item, true) == 0).ToList();
            return retList.Select(obj => new KeyTextModel() { Key = obj.Item, Text = obj.Item }).FirstOrDefault();
        }
    }
}