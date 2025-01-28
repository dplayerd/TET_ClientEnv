using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Text;
using System.Threading.Tasks;
using Platform.AbstractionClass;

namespace Platform.Infra
{
    public static class PagerExtension
    {
        /// <summary> 附加分頁處理
        /// <para>(應該是所有查詢都處理完後，再呼叫)</para>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"> 原內容 </param>
        /// <param name="pager"> 分頁資訊 </param>
        /// <returns></returns>
        public static IQueryable<T> ProcessPager<T>(this IQueryable<T> query, Pager pager)
        {
            var skip = pager.GetStartIndex();
            pager.TotalRow = query.Count();

            var result =
                (pager.AllowPaging)
                    ? query.Skip(skip).Take(pager.PageSize)
                    : query;

            return result;
        }


        /// <summary> 附加分頁處理 
        /// <para>(應該是所有查詢都處理完後，再呼叫)</para>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enm"> 原內容 </param>
        /// <param name="pager"> 分頁資訊 </param>
        /// <returns></returns>
        public static IEnumerable<T> ProcessPager<T>(this IEnumerable<T> enm, Pager pager)
        {

            pager.TotalRow = enm.Count();

            if (pager.PageIndex > pager.PageCount)
            {
                pager.PageIndex = 1;             
            }


            var skip = pager.GetStartIndex();

            var result =
                (pager.AllowPaging)
                    ? enm.Skip(skip).Take(pager.PageSize)
                    : enm;

            return result;
        }


        private static bool IsOrdered<T>(this IQueryable<T> queryable)
        {
            return queryable.Expression.Type == typeof(IOrderedQueryable<T>);
        }
    }
}
