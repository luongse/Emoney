using Newtonsoft.Json;

namespace OneSignalNotification.Models.Base
{
    public class OneSignalBaseNotificationResultModel
    {
        [JsonProperty("id")]
        public string NotificationId { get; set; }

        [JsonProperty("recipients")]
        public int Recipients { get; set; }

        [JsonProperty("external_id")]
        public string ExternalId { get; set; }

        [JsonProperty("errors")]
        public object Errors { get; set; }
    }
}
