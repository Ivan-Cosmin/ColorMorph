//using Android.App;
//using Android.Content;

//namespace ColorMorph.Services
//{
//    public class NotificationService
//    {
//        public static void ShowNotification(string title, string message)
//        {
//            var notificationManager = (NotificationManager)Application.Context.GetSystemService(Context.NotificationService);

//            var notification = new Notification.Builder(Application.Context)
//                .SetContentTitle(title)
//                .SetContentText(message)
//                .SetSmallIcon(Resource.Drawable.icon)
//                .Build();

//            notificationManager.Notify(1, notification);
//        }
//    }
//}
