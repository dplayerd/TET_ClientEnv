using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Platform.Portal.Models
{
    /// <summary> 頁面角色 </summary>
    public class PageRoleModel
    {
        private byte _allowActs;

        /// <summary> 主代碼 </summary>
        public Guid ID { get; set; }

        /// <summary> 頁面代碼 </summary>
        public Guid PageID { get; set; }

        /// <summary> 角色代碼 </summary>
        public Guid RoleID { get; set; }

        /// <summary> 允許的標準行為 </summary>
        public byte AllowActs
        {
            get
            {
                this._allowActs = this.BitsToByte();
                return _allowActs;
            }
            set
            {
                _allowActs = value;
                this.ByteToBits(value);
            }
        }

        /// <summary> 建立者 </summary>
        public Guid CreateUser { get; set; }

        /// <summary> 建立時間 </summary>
        public DateTime CreateDate { get; set; }

        /// <summary> 修改者 </summary>
        public Guid? ModifyUser { get; set; }

        /// <summary> 修改時間 </summary>
        public DateTime? ModifyDate { get; set; }

        #region Custom Property
        /// <summary> 角色名稱 </summary>
        public string RoleName { get; set; }

        /// <summary> 頁面名稱 </summary>
        public string PageName { get; set; }
        #endregion

        #region Standard Permission
        /// <summary> 權限 - 讀取列表 </summary>
        public bool ReadList { get; set; }

        /// <summary> 權限 - 讀取內頁 </summary>
        public bool ReadDetail { get; set; }

        /// <summary> 權限 - 新增 </summary>
        public bool Create { get; set; }

        /// <summary> 權限 - 修改 </summary>
        public bool Modify { get; set; }

        /// <summary> 權限 - 刪除 </summary>
        public bool Delete { get; set; }

        /// <summary> 權限 - 匯出 </summary>
        public bool Export { get; set; }

        /// <summary> 權限 - 管理者功能 </summary>
        public bool Admin { get; set; }
        #endregion

        #region Spesfic Permission
        // TODO: 尚未完成
        /// <summary> 特定權限 </summary>
        public List<SpesficAction> SpesficActionList = new List<SpesficAction>();
        #endregion

        #region Private Methods
        private void ByteToBits(byte val)
        {
            if ((val & (byte)AllowActionEnum.ReadList) != 0)
                this.ReadList = true;

            if ((val & (byte)AllowActionEnum.ReadDetail) != 0)
                this.ReadDetail = true;

            if ((val & (byte)AllowActionEnum.Create) != 0)
                this.Create = true;

            if ((val & (byte)AllowActionEnum.Modify) != 0)
                this.Modify = true;

            if ((val & (byte)AllowActionEnum.Delete) != 0)
                this.Delete = true;

            if ((val & (byte)AllowActionEnum.Export) != 0)
                this.Export = true;

            if ((val & (byte)AllowActionEnum.Admin) != 0)
                this.Admin = true;
        }

        private byte BitsToByte()
        {
            AllowActionEnum actions = 0;

            if (this.ReadList)
                actions |= AllowActionEnum.ReadList;

            if (this.ReadDetail)
                actions |= AllowActionEnum.ReadDetail;

            if (this.Create)
                actions |= AllowActionEnum.Create;

            if (this.Modify)
                actions |= AllowActionEnum.Modify;

            if (this.Delete)
                actions |= AllowActionEnum.Delete;

            if (this.Export)
                actions |= AllowActionEnum.Export;

            if (this.Admin)
                actions |= AllowActionEnum.Admin;

            return (byte)actions;
        }
        #endregion
    }
}
