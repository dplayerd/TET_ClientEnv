using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Platform.AbstractionClass;
using Platform.Auth;
using Platform.Auth.Models;

namespace Platform.WebSite.Services
{
    public class RoleService
    {
        /// <summary> 取得角色清單 </summary>
        /// <param name="pager"></param>
        /// <returns></returns>
        public static List<RoleModel> GetList()
        {
            var pager = Pager.GetDefaultPager();
            pager.AllowPaging = false;
            return new RoleManager().GetRoleList(null, pager);
        }

        /// <summary> 取得角色清單 </summary>
        /// <param name="caption"> 包含文字 </param>
        /// <param name="pager"> 分頁資訊 </param>
        /// <returns></returns>
        public static List<RoleModel> GetList(string caption, Pager pager)
        {
            return new RoleManager().GetRoleList(caption, pager);
        }

        /// <summary> 取得角色清單 </summary>
        /// <param name="caption"> 包含文字 </param>
        /// <param name="pager"> 分頁資訊 </param>
        /// <returns></returns>
        public static List<RoleModel> GetAdminList(string caption, Pager pager)
        {
            return new RoleManager().GetRoleAdminList(caption, pager);
        }

        /// <summary> 取得角色 </summary>
        /// <param name="roleID"></param>
        /// <returns></returns>
        public static RoleModel GetDetail(Guid roleID)
        {
            return new RoleManager().GetDetail(roleID);
        }

        /// <summary> 建立角色 </summary>
        /// <param name="model"></param>
        /// <param name="userID"></param>
        /// <param name="time"></param>
        public static void Create(RoleModel model, string userID, DateTime time)
        {
            new RoleManager().CreateRole(model, userID, time);
        }

        /// <summary> 修改角色 </summary>
        /// <param name="model"></param>
        /// <param name="userID"></param>
        /// <param name="time"></param>
        public static void Modify(RoleModel model, string userID, DateTime time)
        {
            new RoleManager().ModifyRole(model, userID, time);
        }

        /// <summary> 刪除角色 </summary>
        /// <param name="id"></param>
        /// <param name="userID"></param>
        /// <param name="time"></param>
        public static void Delete(Guid id, string userID, DateTime time)
        {
            new RoleManager().DeleteRole(id, userID, time);
        }
    }
}