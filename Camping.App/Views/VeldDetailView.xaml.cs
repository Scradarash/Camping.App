using Camping.App.ViewModels;

namespace Camping.App.Views;

public partial class VeldDetailView : ContentPage
{
    public VeldDetailView(VeldDetailViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}