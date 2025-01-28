using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Platform.ORM;
using Platform.Security;

namespace Platform.Auth.InternalUtility
{
    /// <summary> 密碼檢查器 </summary>
    internal class PWDUtility
    {
        /// <summary> 檢查密碼是否一致
        /// <para> 如果金鑰沒寫入，直接透過文字比對 </para>
        /// </summary>
        /// <param name="inputPWD"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        internal static bool ValidatePWD(string inputPWD, SystemUser user)
        {
            if (string.IsNullOrWhiteSpace(user.HashKey))
            {
                return string.Compare(inputPWD, user.Password) == 0;
            }
            else
            {
                byte[] hashKeyBytes = ByteUtility.Base64StringToBytes(user.HashKey);
                byte[] dbPasswordBytes = ByteUtility.Base64StringToBytes(user.Password);
                byte[] inpPasswordBytes = HashUtility.GetHashValue(inputPWD, hashKeyBytes);

                return dbPasswordBytes.SequenceEqual(inpPasswordBytes);
            }
        }

        /// <summary> 產生密碼 </summary>
        /// <param name="inputPWD"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        internal static string CalculatePWD(string inputPWD, SystemUser user)
        {
            byte[] hashKeyBytes = ByteUtility.Base64StringToBytes(user.HashKey);
            byte[] inpPasswordBytes = HashUtility.GetHashValue(inputPWD, hashKeyBytes);
            string pwdText = ByteUtility.BytesToBase64String(inpPasswordBytes);

            return pwdText;
        }
    }
}
