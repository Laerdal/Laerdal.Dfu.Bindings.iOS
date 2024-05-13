using Foundation;

namespace Laerdal.Dfu.Bindings.iOS.TestBindings;

[Register("AppDelegate")]
public class AppDelegate : MauiUIApplicationDelegate
{
    protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();
}
