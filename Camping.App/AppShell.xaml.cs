using Camping.App.ViewModels;
using Camping.App.Views;

namespace Camping.App;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();
        Routing.RegisterRoute(nameof(ReserveringsoverzichtView), typeof(ReserveringsoverzichtView));
        Routing.RegisterRoute(nameof(ToevoegenGastView), typeof(ToevoegenGastView));
    }
}
