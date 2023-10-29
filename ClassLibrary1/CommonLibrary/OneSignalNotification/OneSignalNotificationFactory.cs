using OneSignalNotification.Controllers;

namespace OneSignalNotification
{
    public class OneSignalNotificationFactory
    {
        public static IApiOneSignal ApiOneSignal { get { return new ApiOneSignal(); } }
    }
}