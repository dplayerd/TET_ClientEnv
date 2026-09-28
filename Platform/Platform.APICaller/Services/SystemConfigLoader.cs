using Newtonsoft.Json;
using Platform.APICaller.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;

namespace Platform.APICaller.Services
{
    /// <summary>讀取 API 呼叫工具的系統設定。</summary>
    public class SystemConfigLoader
    {
        /// <summary>設定檔名稱。</summary>
        private const string ConfigFileName = "SystemConfig.Json";

        /// <summary>讀取 SystemConfig.Json 中的 JsonConfig API 設定陣列。</summary>
        /// <returns>API 呼叫設定清單。</returns>
        public List<ApiCallConfig> LoadApiCallConfigs()
        {
            var configPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ConfigFileName);

            if (!File.Exists(configPath))
                throw new ConfigurationErrorsException($"{ConfigFileName} is required.");

            var json = File.ReadAllText(configPath);
            var config = JsonConvert.DeserializeObject<SystemConfig>(json);

            return config?.JsonConfig ?? new List<ApiCallConfig>();
        }
    }
}
