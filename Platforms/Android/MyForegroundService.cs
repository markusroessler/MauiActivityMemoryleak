using Android.App;
using Android.Content;
using Android.OS;
using AndroidX.Core.App;
using Microsoft.Extensions.Logging;
using System;

namespace MauiActivityMemoryleak
{
    [Service]
    public class MyForegroundService : Android.App.Service
    {
        public const string StartServiceIntentAction = "action.MyForegroundService.Start";
        private const string ChannelId = "MauiApp";
        private const int ServiceId = 3;

        public MyForegroundService()
        {
        }

        public override StartCommandResult OnStartCommand(Intent intent, StartCommandFlags flags, int startId)
        {
            CreateNotificationChannel();
            var notification = CreateNotification("MyForegroundService", "Hello World", "");
            StartForeground(ServiceId, notification);

            return StartCommandResult.Sticky;
        }

        public override IBinder OnBind(Intent intent)
        {
            return null;
        }

        private void CreateNotificationChannel()
        {
            var channel = new NotificationChannel(ChannelId, "Service", NotificationImportance.Low);
            var notificationManager = (NotificationManager)GetSystemService(NotificationService);
            notificationManager.CreateNotificationChannel(channel);
        }

        private static Notification CreateNotification(string title, string message, string subtext)
        {
            // Set up an intent so that tapping the notifications returns to this app:
            var context = Android.App.Application.Context;
            var intent = new Intent(context, typeof(MainActivity));
            intent.AddFlags(ActivityFlags.SingleTop);

            // Create a PendingIntent; we're only using one PendingIntent:
            int pendingIntentId = ServiceId;
            var pendingIntent =
                PendingIntent.GetActivity(context, pendingIntentId, intent, PendingIntentFlags.UpdateCurrent | PendingIntentFlags.Immutable);

            var builder = new NotificationCompat.Builder(context, ChannelId)
                .SetContentIntent(pendingIntent)
                .SetContentTitle(title)
                .SetContentText(message)
                .SetSubText(subtext)
                .SetShowWhen(false)
                .SetOngoing(true)
                .SetDefaults(0 | NotificationCompat.DefaultVibrate);

            // Build the notification:
            var notification = builder.Build();

            return notification;
        }
    }
}

