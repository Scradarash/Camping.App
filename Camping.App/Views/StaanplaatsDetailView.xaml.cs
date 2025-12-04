using Camping.App.ViewModels;

namespace Camping.App.Views;

public partial class StaanplaatsDetailView : ContentPage
{
    public StaanplaatsDetailView(StaanplaatsDetailViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}