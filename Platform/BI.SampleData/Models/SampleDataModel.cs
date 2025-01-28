using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BI.SampleData.Models
{
    public class SampleDataModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
        public string ImageUrl { get; set; }


        #region 管理用欄位
        /// <summary> 是否啟用 </summary>
        public bool IsEnable { get; set; }
        /// <summary> 建立者帳號代碼 </summary>
        public string CreateUser { get; set; }
        /// <summary> 建立時間 </summary>
        public DateTime CreateDate { get; set; }
        /// <summary> 修改者帳號代碼 </summary>
        public string ModifyUser { get; set; }
        /// <summary> 修改時間 </summary>
        public DateTime? ModifyDate { get; set; }
        /// <summary> 刪除者帳號代碼 </summary>
        public string DeleteUser { get; set; }
        /// <summary> 刪除時間 </summary>
        public DateTime? DeleteDate { get; set; }
        #endregion

        public static SampleDataModel GetDefault()
        {
            return new SampleDataModel()
            {
                Id = -1,
                Name = String.Empty,
                Title = String.Empty,
                ImageUrl = String.Empty,
            };
        }
    }
}
