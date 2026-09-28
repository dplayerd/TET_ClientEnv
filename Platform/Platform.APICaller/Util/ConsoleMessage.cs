using System;

namespace Platform.APICaller.Util
{
    /// <summary>Console 訊息輸出工具。</summary>
    public static class ConsoleMessage
    {
        /// <summary>Console 預設文字顏色。</summary>
        private readonly static ConsoleColor _defaultColor = Console.ForegroundColor;

        /// <summary>輸出一般訊息。</summary>
        /// <param name="msg">要輸出的訊息。</param>
        public static void Write(string msg)
        {
            Console.WriteLine(msg);
        }

        /// <summary>以成功顏色輸出訊息。</summary>
        /// <param name="msg">要輸出的訊息。</param>
        public static void WriteSuccess(string msg)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Write(msg);
        }

        /// <summary>以錯誤顏色輸出訊息。</summary>
        /// <param name="msg">要輸出的訊息。</param>
        public static void WriteError(string msg)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Write(msg);
        }

        /// <summary>還原 Console 預設文字顏色。</summary>
        public static void ResetColor()
        {
            Console.ForegroundColor = _defaultColor;
        }
    }
}
