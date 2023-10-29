using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using OneSignalNotification.Helper;
using OneSignalNotification.Models.Base;
using OneSignalNotification.Models.Configs;
using OneSignalNotification.Models.Response;

namespace OneSignalNotification.Controllers
{
    public class ApiOneSignal : IApiOneSignal
    {
        /// <summary>
        /// Send notification
        /// Link: https://documentation.onesignal.com/reference/create-notification
        /// </summary>
        /// <returns></returns>
        public OneSignalPushResponseModel SendToSpecificDevices(string oneSignalAppId, List<string> devices, Dictionary<string, object> pushTitleWithLang, Dictionary<string, object> pushContentWithLang, object pushData, out bool pushResultStatus)
        {
            pushResultStatus = false;
            if (string.IsNullOrEmpty(oneSignalAppId) || devices == null || !devices.Any())
                return null;

            var pushModel = new OneSignalSpecialDeviceNotificationModel
            {
                AppId = oneSignalAppId,
                OneSignalPlayerIds = devices,
                Contents = pushContentWithLang,
                Data = pushData,
                Headings = pushTitleWithLang
            };

            var httpStatusCode = 200;
            var pushResult = OneSignalHelper.SendPushToServer(Configuration.OneSignalConfigs.CreateNotificationUrl, pushModel, ref httpStatusCode);
            if (httpStatusCode != 200 || string.IsNullOrEmpty(pushResult))
                return null;

            var pushResultObj = JsonConvert.DeserializeObject<OneSignalBaseNotificationResultModel>(pushResult);
            if (pushResultObj == null || pushResultObj.Errors != null || string.IsNullOrEmpty(pushResultObj.NotificationId)) return null;

            pushResultStatus = true;
            return new OneSignalPushResponseModel
            {
                NotificationId = pushResultObj.NotificationId,
                Recipients = pushResultObj.Recipients
            };
        }
    }
}