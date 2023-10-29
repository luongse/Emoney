using System.Collections.Generic;
using Newtonsoft.Json;

namespace OneSignalNotification.Models.Base
{
    public class OneSignalSpecialDeviceNotificationModel : OneSignalBaseNotificationModel
    {
        [JsonProperty("include_player_ids")]
        public List<string> OneSignalPlayerIds { get; set; }
    }
}