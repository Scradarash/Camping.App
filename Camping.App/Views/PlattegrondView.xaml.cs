using Camping.App.ViewModels;

namespace Camping.App.Views
{
    public partial class PlattegrondView : ContentPage
    {
        public PlattegrondView(PlattegrondViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }
    }
}
