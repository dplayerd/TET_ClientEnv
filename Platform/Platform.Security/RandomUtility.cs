using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Platform.Security
{
    /// <summary> 亂數產生器 </summary>
    public class RandomUtility
    {
        private static RNGCryptoServiceProvider _rng = new RNGCryptoServiceProvider();

        /// <summary> 產生亂數 </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static int Next(int min, int max)
        {
            int MaxRange = max - min;
            int newNumber = Next(MaxRange);
            newNumber += min;
            return newNumber;
        }

        /// <summary>
        /// 產生一個大於等於零且小於等於max的整數亂數
        /// </summary>
        /// <param name="max">最大值</param>
        public static int Next(int max)
        {
            var bytes = GenerateBytes(4);
            int value = BitConverter.ToInt32(bytes, 0);
            value = value % (max + 1);
            value = Math.Abs(value);
            return value;
        }

        /// <summary> 產生指定長度的位元組亂數 </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        public static byte[] GenerateBytes(int length)
        {
            byte[] randBytes = new byte[length];
            _rng.GetBytes(randBytes);
            return randBytes;
        }
    }
}
