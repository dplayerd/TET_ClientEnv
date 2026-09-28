namespace Platform.APICaller.Models
{
    /// <summary>單一 API 呼叫設定。</summary>
    public class ApiCallConfig
    {
        /// <summary>API 名稱，用於輸出執行訊息。</summary>
        public string Name { get; set; }

        /// <summary>API 完整網址。</summary>
        public string Url { get; set; }

        /// <summary>HTTP 方法，目前支援 GET 與 POST。</summary>
        public string Method { get; set; }

        /// <summary>POST 固定送出的內容；GET 時不使用。</summary>
        public string Body { get; set; }

        /// <summary>POST 內容類型，未設定時預設為 application/json。</summary>
        public string ContentType { get; set; }
    }
}
