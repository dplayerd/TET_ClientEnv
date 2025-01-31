using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Platform.Portal.Models;

namespace Platform.WebSite.Models
{
    /// <summary> 導覽列項目 </summary>
    public class NavigateItemViewModel
    {
        /// <summary> ID </summary>
        public string ID { get; set; }

        /// <summary> 父節點 ID </summary>
        public string ParentID { get; set; }

        /// <summary> 名稱 </summary>
        public string Name { get; set; }

        /// <summary> 使用的 ICON </summary>
        public string IconName { get; set; }

        /// <summary> 連結 </summary>
        public string Url { get; set; }

        /// <summary> 選單種類 </summary>
        public byte MenuType { get; set; }

        /// <summary> 選單種類 </summary>
        public MenuTypeEnum MenuTypeEnum { get { return (MenuTypeEnum)this.MenuType; } }

        /// <summary> 排序 </summary>
        public int? SortIndex { get; set; }

        /// <summary> 是否為外連結 </summary>
        public bool IsOuterLink { get; set; } = false;

        /// <summary> 是否為目前頁面 </summary>
        public bool IsCurrentPage { get; set; } = false;

        /// <summary> 提示文字 </summary>
        public string TipText { get; set; }

        /// <summary> 提示文字種類 </summary>
        public NavigateItemTipType TipType { get; set; }

        /// <summary> 父節點 </summary>
        [JsonIgnore]
        public NavigateItemViewModel ParentNode { get; set; }

        /// <summary> 子項目列表 </summary>
        [JsonIgnore]
        public List<NavigateItemViewModel> Children { get; set; } = new List<NavigateItemViewModel>();

        #region Nodes Tree Access
        /// <summary> 尋找節點 </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public NavigateItemViewModel FindNode(string ID)
        {
            if (ID == null)
                return null;

            if (this.ID == ID)
                return this;

            foreach (var subNode in this.Children)
            {
                var findedNode = subNode.FindNode(ID);
                if (findedNode != null)
                    return findedNode;
            }
            return null;
        }

        /// <summary> 取得自己節點，但轉為 Enum </summary>
        /// <returns></returns>
        private IEnumerable<NavigateItemViewModel> GetSelfNodeEnum()
        {
            return new NavigateItemViewModel[] { this };
        }

        /// <summary> 讀取自己節點至直根節點的完整路徑 </summary>
        /// <returns></returns>
        public IEnumerable<NavigateItemViewModel> GetParentUtilRoot()
        {
            if (this.ParentNode == null)
                return this.GetSelfNodeEnum();
            else
                return this.ParentNode.GetParentUtilRoot().Union(this.GetSelfNodeEnum());
        }

        /// <summary> 攤平結構，改為清單 </summary>
        /// <returns></returns>
        public IEnumerable<NavigateItemViewModel> FlattenNodeList()
        {
            if (this.Children?.Count > 0)
                return this.GetSelfNodeEnum().Union(this.Children);
            else
                return this.GetSelfNodeEnum();
        }

        /// <summary> 是否有子節點 </summary>
        /// <returns></returns>
        public bool HasChildren()
        {
            if (this.Children?.Count > 0)
                return true;
            return false;
        }
        #endregion

        #region Custom Property
        /// <summary> 是否要保留 </summary>
        public bool IsKeep { get; set; } = true;

        /// <summary> 是否包含有要保留的子節點 </summary>
        public bool WillKeep()
        {
            if (this.Children?.Count > 0)
            {
                var result = this.Children.Where(obj => obj.IsKeep).Any();
                return this.IsKeep && result;
            }
            return this.IsKeep;
        }


        public string FullPathName
        {
            get
            {
                if (this.ParentNode == null)
                    return this.Name;

                return this.ParentNode.FullPathName + " > " + this.Name;
            }
        }


        /// <summary> 圖片路徑 </summary>
        public string ImagePath { get; set; }
        #endregion
    }
}