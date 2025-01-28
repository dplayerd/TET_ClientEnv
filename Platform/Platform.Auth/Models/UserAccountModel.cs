using Platform.ORM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Platform.Auth.Models
{
    /// <summary> 帳號資訊 </summary>
    public class UserAccountModel
    {
        /// <summary> ID </summary>
        public string ID { get; set; }

        /// <summary> 工號 </summary>
        public string EmpID { get; set; }

        /// <summary> 名 </summary>
        public string FirstNameCH { get; set; }

        /// <summary> 姓 </summary>
        public string LastNameCH { get; set; }

        /// <summary> EMail </summary>
        public string EMail { get; set; }

        /// <summary> 是否啟用 </summary>
        public bool IsEnable { get; set; }

        /// <summary> FirstNameEN </summary>
        public string FirstNameEN { get; set; }

        /// <summary> LastNameEN </summary>
        public string LastNameEN { get; set; }

        /// <summary> UnitCode </summary>
        public string UnitCode { get; set; }

        /// <summary> UnitName </summary>
        public string UnitName { get; set; }

        /// <summary> LeaderID </summary>
        public string LeaderID { get; set; }

        #region Other
        public string Title
        {
            get 
            {
                return $"{this.FirstNameEN} {this.LastNameEN} ({this.EmpID}) - {this.UnitName}";
            }
        }
        #endregion


    }
}
