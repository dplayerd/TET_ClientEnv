using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Platform.AbstractionClass;
using Platform.Auth;
using Platform.Auth.Models;

namespace Platform.WebSite.Services
{
    public class UserRoleService
    {
        /// <summary> 取得帳號角色清單 </summary>
        /// <param name="pager"></param>
        /// <returns></returns>
        public static List<UserRoleModel> GetUserRoleList(Pager pager)
        {
            return new UserRoleManager().GetUserRoleList(pager);    
        }

        /// <summary> 是否為指定的角色 </summary>
        /// <param name="userID"></param>
        /// <param name="roleID"></param>
        /// <returns></returns>
        public static bool IsRole(string userID, Guid roleID)
        {
            return new UserRoleManager().IsRole(userID, roleID);
        }

        /// <summary> 將帳號和角色設定對應 </summary>
        /// <param name="userIDs"></param>
        /// <param name="roleIDs"></param>
        /// <param name="cUserID"></param>
        /// <param name="time"></param>
        public static void MapUserRole(IEnumerable<string> userIDs, IEnumerable<Guid> roleIDs, string cUserID, DateTime time)
        {
            new UserRoleManager().MapUserAndRole(userIDs, roleIDs, cUserID, time);
        }

        /// <summary> 將帳號和角色取消對應 </summary>
        /// <param name="userIDs"></param>
        /// <param name="roleIDs"></param>
        /// <param name="cUserID"></param>
        /// <param name="time"></param>
        public static void UnmapUserRole(IEnumerable<string> userIDs, IEnumerable<Guid> roleIDs, string cUserID, DateTime time)
        {
            new UserRoleManager().UnmapUserAndRole(userIDs, roleIDs, cUserID, time);
        }

        /// <summary> 查出角色下的所有帳號 (或非該角色的帳號) </summary>
        /// <param name="roleID"></param>
        /// <param name="pager"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static List<UserAccountModel> GetAssignedRoleUserList(Guid roleID, Pager pager, string name)
        {
            var mgr = new UserRoleManager();
            return mgr.GetAssignedRoleUserList(roleID, pager, name);
        }

        /// <summary> 查出角色下的所有帳號 (或非該角色的帳號) </summary>
        /// <param name="roleID"></param>
        /// <param name="pager"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static List<UserAccountModel> GetUnassignedRoleUserList(Guid roleID, Pager pager, string name)
        {
            var mgr = new UserRoleManager();
            return mgr.GetUnassignedRoleUserList(roleID, pager, name);
        }
    }
}