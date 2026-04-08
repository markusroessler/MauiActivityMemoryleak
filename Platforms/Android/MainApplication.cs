using Android.App;
using Android.Runtime;

[assembly: UsesPermission(Android.Manifest.Permission.ForegroundService)]

namespace MauiActivityMemoryleak;

[Application]
public class MainApplication : MauiApplication
{
	public MainApplication(IntPtr handle, JniHandleOwnership ownership)
		: base(handle, ownership)
	{
	}

	protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();

	public override void OnCreate()
	{
		base.OnCreate();

		var gcHelper = IPlatformApplication.Current.Services.GetRequiredService<GCHelper>();
		gcHelper.Run();

		var intent = new Android.Content.Intent(Context, typeof(MyForegroundService));
		intent.SetAction(MyForegroundService.StartServiceIntentAction);
		Context.StartForegroundService(intent);
	}
}
