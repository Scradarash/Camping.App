using Camping.App.ViewModels;
using Microsoft.UI.Xaml.Controls;

namespace Camping.App.Views;

public partial class ReserveringsoverzichtView : ContentPage
{

    public ReserveringsoverzichtView(ReserveringsoverzichtViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
     

    }
}
