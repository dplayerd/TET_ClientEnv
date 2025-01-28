using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Platform.AbstractionClass
{
    /// <summary> 分頁元件 </summary>
    public class Pager
    {
        private int _pageSize = 10;
        private int _pageIndex = 1;

        /// <summary> 總筆數 ( > 0) </summary>
        public int PageSize
        {
            get { return _pageSize; }
            set
            {
                if (value <= 1)
                    _pageSize = 1;
                else
                    _pageSize = value;
            }
        }

        /// <summary> 目前頁數 ( > 0) </summary>
        public int PageIndex
        {
            get { return _pageIndex; }
            set
            {
                if (value < 0)
                    _pageIndex = 0;
                else
                    _pageIndex = value;
            }
        }

        /// <summary> 是否允許分頁 </summary>
        public bool AllowPaging { get; set; } = true;

        #region Methods
        /// <summary> 取得預設的分頁 </summary>
        /// <returns></returns>
        public static Pager GetDefaultPager()
        {
            return new Pager()
            {
                PageSize = 10,
                PageIndex = 1,
                AllowPaging = true,
            };
        }
        #endregion

        #region Computed
        /// <summary> 總筆數 </summary>
        public int TotalRow { get; set; } = 0;

        /// <summary> 總頁數 (計算結果，不必輸入) </summary>
        public int PageCount
        {
            get
            {
                return (int)Math.Ceiling((double)TotalRow / (double)PageSize);
            }
        }

        /// <summary>
        /// 取得當頁第一筆筆數
        /// </summary>
        /// <returns></returns>
        public int GetStartIndex()
        {
            if (PageIndex - 1 < 0)
                return 0;

            return PageSize * (PageIndex - 1);
        }


        public int GetEndIndex()
        {
            var endIndex = this.GetStartIndex() + this.PageSize;

            if (endIndex > this.TotalRow)
                endIndex = this.TotalRow;

            return endIndex;
        }
        #endregion


    }
}
