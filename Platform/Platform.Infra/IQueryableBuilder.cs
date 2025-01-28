using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Platform.AbstractionClass;

namespace Platform.Infra
{
    /// <summary> 查詢組合器 </summary>
    public class IQueryableBuilder
    {
        /// <summary> 組合未刪除的查詢 </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <returns></returns>
        public static IQueryable<T> ProcessDelete<T>(IQueryable<T> query)
            where T : class, IDeleteQuery
        {
            return query.Where(obj => !obj.DeleteDate.HasValue && !obj.DeleteUser.HasValue);
        }

        /// <summary> 組合允許顯示的查詢 </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <returns></returns>
        public static IQueryable<T> ProcessIsEnable<T>(IQueryable<T> query)
            where T : class, IEnabledQuery
        {
            return query.Where(obj => obj.IsEnable);
        }
    }
}
