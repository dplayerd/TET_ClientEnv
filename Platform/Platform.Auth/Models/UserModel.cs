using System;

namespace Platform.Auth.Models
{
    public class UserModel
    {
        /// <summary> UserID </summary>
        public string UserID { get; set; }

        /// <summary> EmpID </summary>
        public string EmpID { get; set; }

        /// <summary> FirstNameEN </summary>
        public string FirstNameEN { get; set; }

        /// <summary> LastNameEN </summary>
        public string LastNameEN { get; set; }

        /// <summary> FirstNameCH </summary>
        public string FirstNameCH { get; set; }

        /// <summary> LastNameCH </summary>
        public string LastNameCH { get; set; }

        /// <summary> UnitCode </summary>
        public string UnitCode { get; set; }

        /// <summary> UnitName </summary>
        public string UnitName { get; set; }

        /// <summary> LeaderID </summary>
        public string LeaderID { get; set; }

        /// <summary> EMail </summary>
        public string EMail { get; set; }

        /// <summary> IsEnabled </summary>
        public string IsEnabled { get; set; }



        #region 其它欄位
        /// <summary> 將是否啟用欄位轉為布林，較好操作 </summary>
        public bool IsEnabled_Bool
        {
            get { return IsEnabled == "Y"; }
            set { IsEnabled = (value) ? "Y" : "N"; }
        }
        #endregion
    }
}
