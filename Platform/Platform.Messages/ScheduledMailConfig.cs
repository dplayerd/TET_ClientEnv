using System;
using System.Configuration;

namespace Platform.Messages
{
    /// <summary> 排程信件相關設定讀取與驗證 </summary>
    public static class ScheduledMailConfig
    {
        #region 公開方法
        /// <summary> 讀取必須大於零的整數設定，例如提醒天數 </summary>
        /// <param name="key">AppSettings 設定鍵。</param>
        /// <returns>大於零的整數設定值。</returns>
        public static int ReadPositiveInt(string key)
        {
            var value = ConfigurationManager.AppSettings[key];

            if (!int.TryParse(value, out int result) || result <= 0)
                throw new ConfigurationErrorsException($"{key} must be a positive integer.");

            return result;
        }

        /// <summary> 讀取字串設定，未設定時回傳空字串 </summary>
        /// <param name="key">AppSettings 設定鍵。</param>
        /// <returns>設定值或空字串。</returns>
        public static string ReadString(string key)
        {
            return ConfigurationManager.AppSettings[key] ?? string.Empty;
        }

        /// <summary> 驗證排程 WebAPI 呼叫 Token，避免外部未授權呼叫直接產生信件 </summary>
        /// <param name="token">呼叫端傳入的 Token。</param>
        public static void ValidateToken(string token)
        {
            var expectedToken = ReadString("ScheduledMailApiToken");

            if (string.IsNullOrWhiteSpace(expectedToken))
                throw new UnauthorizedAccessException("ScheduledMailApiToken is required.");

            if (!string.Equals(expectedToken, token, StringComparison.Ordinal))
                throw new UnauthorizedAccessException("Invalid scheduled mail token.");
        }
        #endregion
    }
}
