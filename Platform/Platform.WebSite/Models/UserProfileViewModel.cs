using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Platform.ORM;

namespace Platform.WebSite.Models
{
    /// <summary> 個人資訊 </summary>
    public class UserProfileViewModel
    {
        /// <summary> ID </summary>
        public string ID { get; set; }

        /// <summary> 帳號 </summary>
        public string Account { get; set; }

        /// <summary> 名 </summary>
        public string FirstNameCH { get; set; }

        /// <summary> 姓 </summary>
        public string LastNameCH { get; set; }

        /// <summary> 英文名 </summary>
        public string FirstNameEN { get; set; }

        /// <summary> 英文姓 </summary>
        public string LastNameEN { get; set; }

        /// <summary> Email </summary>
        public string Email { get; set; }

        /// <summary> 職稱 </summary>
        public string Title { get; set; }

        /// <summary> UnitCode </summary>
        public string UnitCode { get; set; }

        /// <summary> UnitName </summary>
        public string UnitName { get; set; }

        /// <summary> LeaderID </summary>
        public string LeaderID { get; set; }

        /// <summary> 是否有登入 </summary>
        public bool HasLogined { get; set; } = true;

        /// <summary> 大頭貼 </summary>
        public string AvatorUrl { get; set; }

        #region Methods
        /// <summary> 建立預設物件 </summary>
        /// <returns></returns>
        public static UserProfileViewModel GetDefault()
        {
            return new UserProfileViewModel()
            {
                ID = string.Empty,
                FirstNameCH = "Unknow",
                LastNameCH = "User",
                Account = "UnknowUser",
                Email = "",
                Title = "Unknow User",
                HasLogined = false,
                FirstNameEN = "Unknow",
                LastNameEN = "Unknow",
                LeaderID = string.Empty,
                UnitCode = string.Empty,
                UnitName = string.Empty,
                AvatorUrl = "/Content/assets/media/users/300_21.jpg"
            };
        }

        /// <summary> 檢查是否為空物件 </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static bool IsDefault(UserProfileViewModel model)
        {
            if (string.IsNullOrEmpty(model.ID))
                return true;
            else
                return false;
        }
        #endregion
    }
}