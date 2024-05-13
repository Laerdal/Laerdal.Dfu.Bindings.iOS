namespace Laerdal.Dfu.Bindings.iOS.TestBindings;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();

        MainPage = new AppShell();
    }
}
