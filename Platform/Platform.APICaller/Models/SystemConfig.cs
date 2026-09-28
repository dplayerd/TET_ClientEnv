using System.Collections.Generic;

namespace Platform.APICaller.Models
{
    /// <summary>SystemConfig.Json 對應的根設定物件。</summary>
    public class SystemConfig
    {
        /// <summary>要依序呼叫的 API 設定清單。</summary>
        public List<ApiCallConfig> JsonConfig { get; set; }
    }
}
