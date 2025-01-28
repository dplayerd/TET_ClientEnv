using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using Platform.AbstractionClass;
using Platform.WebSite.Models;

namespace Platform.WebSite.Util
{
    /// <summary> 分頁協助器 </summary>
    public class PagerUtil
    {
        /// <summary> 取得分頁項目清單 </summary>
        /// <param name="calculatedPager"> 已計算後的分頁 </param>
        /// <param name="baseUri">  </param>
        /// <param name="pageIndexName"> 第幾頁的 QueryString 名稱 </param>
        /// <returns></returns>
        public static List<PagerItemViewModel> GetPagerItems(Pager calculatedPager, Uri baseUri = null, string pageIndexName = "PageIndex", int? pageSize = null, string pageSizeName = "PageSize")
        {
            var pager = calculatedPager;
            var url = (baseUri ?? HttpContext.Current.Request.Url);
            var qsCollection = HttpUtility.ParseQueryString(url.Query);


            // 更改Url的PageIndex大於整頁數下方顯示第一頁時之頁碼
            if (pager.PageIndex > pager.PageCount)
            {
                pager.PageIndex = 1;
            }
            //  更改Url的PageIndex變負數下方顯示第一頁時之頁碼
            if (pager.PageIndex < 1)
            {
                pager.PageIndex = 1; //使當前頁數回到1
            }

            List<PagerItemViewModel> list = new List<PagerItemViewModel>();

            // 第一頁、上一頁
            var first = new PagerItemViewModel() { ItemType = PagerItemType.First, Index = 1, Url = BuildUrl(url, qsCollection, 1, pageIndexName, pageSize, pageSizeName) };
            var prev = new PagerItemViewModel() { ItemType = PagerItemType.Prev, Index = pager.PageIndex - 1, Url = BuildUrl(url, qsCollection, pager.PageIndex - 1, pageIndexName, pageSize, pageSizeName) };
           
            if (pager.PageIndex != 1)
                list.Add(first);
            if (pager.PageIndex > 1)
                list.Add(prev);

            // 中間頁數
            // 如果頁碼小於 2 ，需要做平移
            // 如果頁碼大於總頁數，需要去掉後面的

            int start = pager.PageIndex - 2;
            int end = pager.PageIndex + 2;


            //  更改Url的PageIndex變負數下方顯示第一頁時之頁碼
            if (pager.PageIndex < 1)
            {
                pager.PageIndex = 1; //使當前頁數回到1
                start = 1;
                end += 2;
            }


            if (pager.PageIndex == 1)
            {
                start = 1;
                end += 2;
            }
            else if (pager.PageIndex == 2)
            {
                start = 1;
                end += 1;
            }

            if (end > pager.PageCount)
                end = pager.PageCount;

            for (var i = start; i <= end; i++)
            {
                var item = new PagerItemViewModel() { Index = i, Url = BuildUrl(url, qsCollection, i, pageIndexName, pageSize, pageSizeName) };
                if (i == pager.PageIndex)
                    item.IsCurrent = true;
                list.Add(item);
            }






            // 最後一頁、下一頁
            var last = new PagerItemViewModel() { ItemType = PagerItemType.Last, Index = pager.PageCount, Url = BuildUrl(url, qsCollection, pager.PageCount, pageIndexName, pageSize, pageSizeName) };
            var next = new PagerItemViewModel() { ItemType = PagerItemType.Next, Index = pager.PageIndex + 1, Url = BuildUrl(url, qsCollection, pager.PageIndex + 1, pageIndexName, pageSize, pageSizeName) };

            if (pager.PageIndex < (pager.PageCount - 1))
                list.Add(next);
            if (pager.PageIndex < pager.PageCount)
                list.Add(last);

            return list;
        }

        /// <summary> 建立分頁的 Uri (為相對路徑) </summary>
        /// <param name="baseUri"></param>
        /// <param name="collection"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageIndexName"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageSizeName"></param>
        /// <returns></returns>
        private static Uri BuildUrl(Uri baseUri, NameValueCollection collection, int pageIndex, string pageIndexName = "PageIndex", int? pageSize = null, string pageSizeName = "PageSize")
        {
            collection[pageIndexName] = pageIndex.ToString();

            if (pageSize.HasValue)
                collection[pageSizeName] = pageSize.ToString();
            else
                collection.Remove(pageSizeName);

            string newQs = string.Join("&", ConcatQueryString(collection));
            var newRelativeUrl = new Uri(baseUri.LocalPath + "?" + newQs, UriKind.Relative);
            return newRelativeUrl; ;
        }

        private static IEnumerable<string> ConcatQueryString(NameValueCollection collection)
        {
            foreach(var key in collection.AllKeys)
            {
                yield return key + "=" + HttpUtility.UrlEncode(collection[key]); 
            }
        }
    }
}