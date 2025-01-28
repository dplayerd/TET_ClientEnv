using Platform.AbstractionClass;
using Platform.LogService;
using Platform.ORM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BI.Shared
{
    public class ToolTipManager
    {
        private Logger _logger = new Logger();

        #region Read
        /// <summary> 取得 ToolTip 清單</summary>
        /// <param name="moduleName"> 種類 </param>
        /// <returns></returns>
        public List<KeyTextModel> GetList(string moduleName)
        {
            try
            {
                using (PlatformContextModel context = new PlatformContextModel())
                {
                    var query =
                        from item in context.TET_SPA_Tooltips
                        where
                            string.Compare(moduleName, item.ModuleName, true) == 0
                        orderby
                            item.FieldName ascending
                        select
                            new KeyTextModel()
                            {
                                Key = item.FieldName,
                                Text = item.Description,
                            };

                    var list = query.ToList();
                    return list;
                }
            }
            catch (Exception ex)
            {
                this._logger.WriteError(ex);
                return default;
            }
        }
        #endregion
    }
}
