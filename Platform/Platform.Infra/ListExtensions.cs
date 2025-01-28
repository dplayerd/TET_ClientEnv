using Platform.AbstractionClass;
using System.Collections.Generic;
using System.Linq;

namespace Platform.Infra
{
    public static class ListExtensions
    {
        /// <summary> 如果物件還不存在清單中，加入該清單，否則跳過
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sourceList"> 原清單 </param>
        /// <param name="obj"> 要加入的內容 </param>
        /// <returns></returns>
        public static bool AddWhenNotContains<T>(this List<T> sourceList, T obj)
            where T : class
        {
            if (sourceList == null)
                return false;

            if (sourceList.Contains(obj))
                return false;

            sourceList.Add(obj);
            return true;
        }

        /// <summary> 如果物件還不存在清單中，加入該清單，否則跳過
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sourceList"> 原清單 </param>
        /// <param name="objs"> 要加入的內容 </param>
        /// <returns></returns>
        public static bool AddWhenNotContains<T>(this List<T> sourceList, IEnumerable<T> objs)
            where T : class
        {
            if (sourceList == null)
                return false;

            
            foreach (var obj in objs)
            {
                if (sourceList.Contains(obj))
                    continue;

                sourceList.Add(obj);
            }

            return true;
        }
    }
}
