using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Platform.Auth.InternalUtility;
using Platform.Auth.Models;
using Platform.LogService;
using Platform.ORM;
using Platform.Security;

namespace Platform.Auth
{
    public class LoginManager
    {
        private Logger _logger = new Logger();

        //private UserAccountManager _userManager = new UserAccountManager();
        private UserManager _userManager = new UserManager();

        /// <summary> 嘗試登入 </summary>
        /// <param name="account"> 帳號 </param>
        /// <param name="pwd"> 密碼 </param>
        /// <param name="msg"> 錯誤訊息 (沒有錯誤時，為空字串) </param>
        /// <returns></returns>
        public UserAccountModel TryLogin(string account, string pwd, out string msg)
        {
            try
            {
                using (PlatformContextModel context = new PlatformContextModel())
                {
                    var user = this._userManager.GetUser(account);

                    if (user == null)
                    {
                        msg = "Account or password is not currect.";
                        return null;
                    }

                    //if (!PWDUtility.ValidatePWD(pwd, user))
                    //{
                    //    msg = "Account or password is not currect.";
                    //    return null;
                    //}

                    if (!user.IsEnabled_Bool)
                    {
                        msg = "Account has locked.";
                        return null;
                    }

                    msg = string.Empty;
                    return new UserAccountModel()
                    {
                        ID = user.UserID,
                        EmpID = user.EmpID,
                        EMail = user.EMail,
                        FirstNameCH = user.FirstNameCH,
                        LastNameCH = user.LastNameCH,
                        FirstNameEN = user.FirstNameEN,
                        LastNameEN = user.LastNameEN,
                        UnitCode = user.UnitCode,
                        UnitName = user.UnitName,
                        LeaderID = user.LeaderID,
                        IsEnable = user.IsEnabled_Bool,
                    };
                }
            }
            catch (Exception ex)
            {
                _logger.WriteError(ex);
                msg = "Error happened, please check log.";
                return null;
            }
        }
    }
}
