using System;
using System.Linq;
using System.Text;

namespace Platform.Security
{
    public class ByteUtility
    {
        /// <summary> 產生 128 bytes 隨機金鑰 </summary>
        /// <returns></returns>
        public static byte[] BuildNewHashKey()
        {
            byte[] randBytes = RandomUtility.GenerateBytes(128);
            return randBytes;
        }

        #region Hex
        /// <summary> 位元組轉為十六進位文字 </summary>
        /// <param name="bytes"> 原始位元組 </param>
        /// <param name="contactText"> 連接符號 </param>
        /// <returns></returns>
        public static string ByteToHexString(byte[] bytes, string contactText = " ")
        {
            string val = string.Join(contactText, bytes.Select(obj => obj.ToString("X2")));
            return val;
        }

        /// <summary> 十六進位文字轉為位元組 </summary>
        /// <param name="hexText"> 原始十六進位文字 </param>
        /// <param name="splitChar"> 連接符號 </param>
        /// <returns></returns>
        public static byte[] HexStringToByte(string hexText, char splitChar = ' ')
        {
            string[] hexTexts = hexText.Split(splitChar);
            var bytes = hexTexts.Select(obj => byte.Parse(obj, System.Globalization.NumberStyles.HexNumber)).ToArray();
            return bytes;
        }
        #endregion

        #region Bytes To String
        /// <summary> 將文字轉為位元組 </summary>
        /// <param name="sourceText"></param>
        /// <returns></returns>
        public static byte[] StringToBytes(string sourceText)
        {
            var result = Encoding.UTF8.GetBytes(sourceText);
            return result;
        }

        /// <summary> 將位元組轉為文字 </summary>
        /// <param name="sourceByte"></param>
        /// <returns></returns>
        public static string BytesToString(byte[] sourceByte)
        {
            var result = Encoding.UTF8.GetString(sourceByte);
            return result;
        }
        #endregion

        #region Base64
        /// <summary> 將 Base64 文字轉為位元組 </summary>
        /// <param name="sourceText"></param>
        /// <returns></returns>
        public static byte[] Base64StringToBytes(string sourceText)
        {
            var result = Convert.FromBase64String(sourceText);
            return result;
        }

        /// <summary> 將位元組轉為 Base64 文字 </summary>
        /// <param name="sourceByte"></param>
        /// <returns></returns>
        public static string BytesToBase64String(byte[] sourceByte)
        {
            var result = Convert.ToBase64String(sourceByte);
            return result;
        }
        #endregion
    }
}