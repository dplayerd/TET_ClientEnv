using Platform.APICaller.Models;
using Platform.APICaller.Util;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Platform.APICaller.Services
{
    /// <summary>依設定逐一執行 API 呼叫。</summary>
    public class ApiCallExecutor
    {
        /// <summary>執行多筆 API 呼叫。</summary>
        /// <param name="configs">API 呼叫設定清單。</param>
        /// <returns>全部呼叫成功時回傳 true；任一呼叫失敗時回傳 false。</returns>
        public async Task<bool> ExecuteAsync(List<ApiCallConfig> configs)
        {
            var success = true;

            using (var client = new HttpClient())
            {
                foreach (var config in configs)
                {
                    success &= await CallAsync(client, config);
                }
            }

            return success;
        }

        /// <summary>執行單一 API 呼叫。</summary>
        /// <param name="client">共用的 HTTP Client。</param>
        /// <param name="config">API 呼叫設定。</param>
        /// <returns>HTTP 狀態碼為成功時回傳 true；例外或非成功狀態碼回傳 false。</returns>
        private static async Task<bool> CallAsync(HttpClient client, ApiCallConfig config)
        {
            var name = config?.Name ?? "(null)";

            try
            {
                Validate(config);
                name = config.Name;

                var method = config.Method.Trim().ToUpperInvariant();
                ConsoleMessage.ResetColor();
                ConsoleMessage.Write($"Calling API[{config.Name}], method is {method}, url is {config.Url}. ");

                var response = await SendAsync(client, config, method);
                var responseContent = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    ConsoleMessage.WriteSuccess($"API[{config.Name}] call success. StatusCode: {(int)response.StatusCode} {response.ReasonPhrase}. ");
                    WriteResponseContent(responseContent);
                    return true;
                }

                ConsoleMessage.WriteError($"API[{config.Name}] call fail. StatusCode: {(int)response.StatusCode} {response.ReasonPhrase}. ");
                WriteResponseContent(responseContent);
                return false;
            }
            catch (Exception ex)
            {
                ConsoleMessage.WriteError($"API[{name}] call fail. Please check log. ");
                ConsoleMessage.WriteError(ex.ToString());
                return false;
            }
        }

        /// <summary>依 HTTP 方法送出要求。</summary>
        /// <param name="client">共用的 HTTP Client。</param>
        /// <param name="config">API 呼叫設定。</param>
        /// <param name="method">已標準化為大寫的 HTTP 方法。</param>
        /// <returns>API 回應。</returns>
        private static Task<HttpResponseMessage> SendAsync(HttpClient client, ApiCallConfig config, string method)
        {
            if (method == "POST")
            {
                var body = config.Body ?? string.Empty;
                return client.PostAsync(config.Url, new StringContent(body, Encoding.UTF8, config.ContentType));
            }

            if (method == "GET")
                return client.GetAsync(config.Url);

            throw new NotSupportedException($"HTTP method is not supported: {config.Method}");
        }

        /// <summary>補齊預設值並驗證必要設定。</summary>
        /// <param name="config">API 呼叫設定。</param>
        private static void Validate(ApiCallConfig config)
        {
            if (config == null)
                throw new ConfigurationErrorsException("API config cannot be null.");

            if (string.IsNullOrWhiteSpace(config.Name))
                config.Name = config.Url;

            if (string.IsNullOrWhiteSpace(config.Url))
                throw new ConfigurationErrorsException("API Url is required.");

            if (string.IsNullOrWhiteSpace(config.Method))
                config.Method = "GET";

            if (string.IsNullOrWhiteSpace(config.ContentType))
                config.ContentType = "application/json";
        }

        /// <summary>輸出 API 回應內容。</summary>
        /// <param name="responseContent">API 回應內容。</param>
        private static void WriteResponseContent(string responseContent)
        {
            if (!string.IsNullOrWhiteSpace(responseContent))
                ConsoleMessage.Write(responseContent);
        }
    }
}
