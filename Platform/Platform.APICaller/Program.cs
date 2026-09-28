using Platform.APICaller.Services;
using Platform.APICaller.Util;
using System;

namespace Platform.APICaller
{
    /// <summary>API 呼叫工具主程式。</summary>
    internal class Program
    {
        /// <summary>程式進入點，負責串接讀取設定及執行 API 呼叫流程。</summary>
        /// <param name="args">命令列參數，目前未使用。</param>
        static void Main(string[] args)
        {
            ConsoleMessage.Write("API Caller Starting.");

            try
            {
                // 從 SystemConfig.Json 讀取要呼叫的 API 清單。
                var configLoader = new SystemConfigLoader();
                var configs = configLoader.LoadApiCallConfigs();
                ConsoleMessage.Write($"API config readed. Total rows: {configs.Count}. ");

                // 逐一呼叫 API，任一筆失敗時以非 0 exit code 回報給排程。
                var caller = new ApiCallExecutor();
                var success = caller.ExecuteAsync(configs).GetAwaiter().GetResult();

                ConsoleMessage.ResetColor();
                ConsoleMessage.Write("API Caller completed.");

                if (!success)
                    Environment.ExitCode = 1;
            }
            catch (Exception ex)
            {
                ConsoleMessage.WriteError("API Caller fail. Please check log. ");
                ConsoleMessage.WriteError(ex.ToString());
                Environment.ExitCode = 1;
            }
            finally
            {
                ConsoleMessage.ResetColor();
            }
        }
    }
}
