using BI.SPA_ScoringInfo;
using Platform.Messages;
using Platform.SPAScoringInfoReminder.Util;
using System;
using System.Linq;

namespace Platform.SPAScoringInfoReminder
{
    /// <summary>SPA 評鑑計分資料填寫提醒信產生排程主程式。</summary>
    internal class Program
    {
        #region 常數與欄位
        /// <summary>排程建立資料時使用的系統帳號。</summary>
        private const string SystemUser = "System";

        /// <summary>Console 預設文字顏色。</summary>
        private readonly static ConsoleColor _defaultColor = Console.ForegroundColor;
        #endregion

        #region 進入點
        /// <summary>程式進入點，直接呼叫 SPA 評鑑計分資料填寫提醒信 Manager 產生待發信件。</summary>
        /// <param name="args">命令列參數，目前未使用。</param>
        static void Main(string[] args)
        {
            WriteMessage("SPA Scoring Info Reminder Starting.");

            try
            {
                // 初始化信件相關設定，供 Manager 寫入待發信清單時使用。
                ConfigLoader.Init();

                var manager = new SPAScoringInfoReminderMailManager();
                var result = manager.Generate(SystemUser, DateTime.Now);

                WriteResult(result);
                WriteSuccessMessage("SPA Scoring Info Reminder completed.");
            }
            catch (Exception ex)
            {
                WriteErrorMessage("SPA Scoring Info Reminder fail. Please check log. ");
                WriteErrorMessage(ex.ToString());
                Environment.ExitCode = 1;
            }
            finally
            {
                Console.ForegroundColor = _defaultColor;
            }
        }
        #endregion

        #region 私有方法
        /// <summary>輸出提醒信產生結果。</summary>
        /// <param name="result">提醒信產生結果。</param>
        private static void WriteResult(ReminderMailGenerateResult result)
        {
            WriteMessage($"ReminderType: {result.ReminderType}");
            WriteMessage($"IsSkipped: {result.IsSkipped}");
            WriteMessage($"SourceCount: {result.SourceCount}");
            WriteMessage($"MailCount: {result.MailCount}");

            if (result.Messages.Any())
                WriteMessage($"Messages: {string.Join(Environment.NewLine, result.Messages)}");
        }

        /// <summary>輸出一般訊息。</summary>
        /// <param name="msg">訊息內容。</param>
        private static void WriteMessage(string msg)
        {
            Console.WriteLine(msg);
        }

        /// <summary>輸出成功訊息。</summary>
        /// <param name="msg">訊息內容。</param>
        private static void WriteSuccessMessage(string msg)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            WriteMessage(msg);
        }

        /// <summary>輸出錯誤訊息。</summary>
        /// <param name="msg">訊息內容。</param>
        private static void WriteErrorMessage(string msg)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            WriteMessage(msg);
        }
        #endregion
    }
}
