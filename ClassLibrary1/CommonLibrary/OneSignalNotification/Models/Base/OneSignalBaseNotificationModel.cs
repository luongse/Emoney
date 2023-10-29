using Newtonsoft.Json;

namespace OneSignalNotification.Models.Base
{
    public class OneSignalBaseNotificationModel
    {
        [JsonProperty("app_id")]
        public string AppId { get; set; }

        [JsonProperty("contents")]
        public object Contents { get; set; }

        [JsonProperty("headings")]
        public object Headings { get; set; }

        [JsonProperty("data")]
        public object Data { get; set; }
    }
}