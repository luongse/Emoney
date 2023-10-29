using System.Collections.Generic;
using OneSignalNotification.Models.Response;

namespace OneSignalNotification.Controllers
{
    public interface IApiOneSignal
    {
        /// <summary>
        /// Send notification
        /// Link: https://documentation.onesignal.com/reference/create-notification
        /// </summary>
        /// <returns></returns>
        OneSignalPushResponseModel SendToSpecificDevices(string oneSignalAppId, List<string> devices, Dictionary<string, object> pushTitleWithLang, Dictionary<string, object> pushContentWithLang, object pushData, out bool pushResultStatus);
    }
}