using Camping.App.ViewModels;

namespace Camping.App.Views
{
    public partial class PlattegrondView : ContentPage
    {
        int clicks = 1;
        public PlattegrondView(PlattegrondViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }

        private void Staanplaats1_Clicked(object sender, EventArgs e)
        {
            clicks++;
            Staanplaats1.Text = $"Staanplaats {clicks}";
            SemanticScreenReader.Announce(Staanplaats1.Text);
        }

        private void OnCancelButtonPointerEntered(object sender, PointerEventArgs e)
        {
             Staanplaats1.BackgroundColor = Colors.Green;
        }

        private void OnCancelButtonPointerExited(object sender, PointerEventArgs e)
        {
            Staanplaats1.BackgroundColor = Colors.DarkGreen; // Revert to original color
        }
    }
}
