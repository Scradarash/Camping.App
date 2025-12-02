using Camping.App.ViewModels;

namespace Camping.App.Views;

public partial class ReserveringsoverzichtView : ContentPage
{
	public ReserveringsoverzichtView(ReserveringsoverzichtViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}