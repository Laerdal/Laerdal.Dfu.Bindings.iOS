namespace Laerdal.Dfu.Bindings.iOS.TestBindings;

public partial class MainPage : ContentPage
{
    int count = 0;

    public MainPage()
    {
        InitializeComponent();
    }

    private void OnCounterClicked(object sender, EventArgs e)
    {
        count++;

        if (count == 1)
            CounterBtn.Text = $"Clicked {count} time";
        else
            CounterBtn.Text = $"Clicked {count} times";

        SemanticScreenReader.Announce(CounterBtn.Text);
    }
    
    private void OnTestClicked(object? sender, EventArgs e)
    {
#if __ANDROID__
        Laerdal.Dfu.Bindings.Android.DfuServiceInitiator initiator =
            new Laerdal.Dfu.Bindings.Android.DfuServiceInitiator("deviceId");
        Console.WriteLine($"Laerdal.Dfu.Bindings.Android.BuildConfig.BuildType: {Laerdal.Dfu.Bindings.Android.BuildConfig.BuildType}");
        Console.WriteLine($"Laerdal.Dfu.Bindings.Android.BuildConfig.Debug: {Laerdal.Dfu.Bindings.Android.BuildConfig.Debug}");
        Console.WriteLine($"Laerdal.Dfu.Bindings.Android.BuildConfig.VersionCode: {Laerdal.Dfu.Bindings.Android.BuildConfig.VersionCode}");
        Console.WriteLine($"Laerdal.Dfu.Bindings.Android.BuildConfig.VersionName: {Laerdal.Dfu.Bindings.Android.BuildConfig.VersionName}");
        Console.WriteLine($"Laerdal.Dfu.Bindings.Android.BuildConfig.LibraryPackageName: {Laerdal.Dfu.Bindings.Android.BuildConfig.LibraryPackageName}");
        Console.WriteLine($"Laerdal.Dfu.Bindings.Android.DfuServiceInitiator.DefaultPrnValue: {Laerdal.Dfu.Bindings.Android.DfuServiceInitiator.DefaultPrnValue}");
        Console.WriteLine($"Laerdal.Dfu.Bindings.Android.DfuServiceInitiator.DefaultMbrSize: {Laerdal.Dfu.Bindings.Android.DfuServiceInitiator.DefaultMbrSize}");
        Console.WriteLine($"Laerdal.Dfu.Bindings.Android.DfuServiceInitiator.DefaultScanTimeout: {Laerdal.Dfu.Bindings.Android.DfuServiceInitiator.DefaultScanTimeout}");
#elif __IOS__
        
        Laerdal.Dfu.Bindings.iOS.DFUFirmware firmware = new Laerdal.Dfu.Bindings.iOS.DFUFirmware(new Foundation.NSUrl("", false), out var _);

        Laerdal.Dfu.Bindings.iOS.DFUServiceInitiator initiator =
            new Laerdal.Dfu.Bindings.iOS.DFUServiceInitiator(CoreFoundation.DispatchQueue.GetGlobalQueue(CoreFoundation
                                                                .DispatchQueuePriority.Default),
                                                             CoreFoundation.DispatchQueue.GetGlobalQueue(CoreFoundation
                                                                .DispatchQueuePriority.Default),
                                                             CoreFoundation.DispatchQueue.GetGlobalQueue(CoreFoundation
                                                                .DispatchQueuePriority.Default),
                                                             CoreFoundation.DispatchQueue.GetGlobalQueue(CoreFoundation
                                                                .DispatchQueuePriority.Default),
                                                             null);
#endif
    }
}
