using Camping.App.ViewModels;

namespace Camping.App.Views;

public partial class ToevoegenGastView : ContentPage
{
	public ToevoegenGastView(ToevoegenGastViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}