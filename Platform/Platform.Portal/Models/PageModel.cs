using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Platform.Portal.Models
{
    public class PageModel
    {
        public Guid ID { get; set; }
        public Guid SiteID { get; set; }
        public Guid? ParentID { get; set; }
        public string Name { get; set; }
        public string PageNo { get; set; }
        public string PageTitle { get; set; }
        public byte MenuType { get; set; }
        public string OuterLink { get; set; }
        public Guid? ModuleID { get; set; }
        public string PageIcon { get; set; }
        public int SortNo { get; set; }
        public bool IsEnable { get; set; }
        public string CreateUser { get; set; }
        public DateTime CreateDate { get; set; }
        public string ModifyUser { get; set; }
        public DateTime? ModifyDate { get; set; }

        #region Custom
        public MenuTypeEnum GetMenuTypeEnum()
        {
            return (MenuTypeEnum)this.MenuType;
        }
        public string ModuleName { get; set; }
        #endregion
    }
}
