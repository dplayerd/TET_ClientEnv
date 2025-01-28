using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Platform.Security
{
    public class HashUtility 
    {
        /// <summary> 進行雜湊 </summary>
        /// <param name="sourceText">原文字</param>
        /// <param name="hashKey">金鑰</param>
        /// <returns></returns>
        public static byte[] GetHashValue(string sourceText, string hashKey)
        {
            byte[] hashKeyBytes = ByteUtility.StringToBytes(hashKey);
            byte[] hashed = HashUtility.GetHashValue(sourceText, hashKeyBytes);
            return hashed;
        }

        /// <summary> 進行雜湊 </summary>
        /// <param name="sourceText">原文字</param>
        /// <param name="hashKey">金鑰</param>
        /// <returns></returns>
        public static byte[] GetHashValue(string sourceText, byte[] hashKey)
        {
            HMACSHA512 hmacsha512 = new HMACSHA512(hashKey);
            byte[] sourceTextBytes = ByteUtility.StringToBytes(sourceText);
            byte[] hashed = hmacsha512.ComputeHash(sourceTextBytes);
            return hashed;
        }
    }
}
