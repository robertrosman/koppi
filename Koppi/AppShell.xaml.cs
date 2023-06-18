namespace Koppi;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();
        Routing.RegisterRoute(nameof(Views.ClipPage), typeof(Views.ClipPage));
    }
}
