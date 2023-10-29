namespace OneSignalNotification.Models.Response
{
    public class OneSignalPushResponseModel
    {
        /// <summary>
        /// Id Notification from OneSignal
        /// </summary>
        public string NotificationId { get; set; }

        /// <summary>
        /// Number players who received notification
        /// </summary>
        public int Recipients { get; set; }
    }
}