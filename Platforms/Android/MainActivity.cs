using Android.App;
using Android.Content.PM;
using Android.OS;
using Microsoft.Maui.Platform;

namespace MauiActivityMemoryleak;

[Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, LaunchMode = LaunchMode.SingleTop, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
public class MainActivity : MauiAppCompatActivity
{
    protected override void OnCreate(Bundle? savedInstanceState)
    {
        base.OnCreate(savedInstanceState);

        var window = this.GetWindow();
        var serviceProvider = window.Handler.MauiContext.Services;
        var gcHelper = serviceProvider.GetRequiredService<GCHelper>();
        gcHelper.MonitorInstance(this);
    }
}
