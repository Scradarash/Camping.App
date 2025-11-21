using Camping.App.ViewModels;

namespace Camping.App.Views
{
    public partial class PlattegrondView : ContentPage
    {
        int clicks1 = 1;
        int clicks2 = 1;
        int clicks3 = 1;

        public PlattegrondView(PlattegrondViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }

        private void Staanplaats1_Clicked(object sender, EventArgs e)
        {
            clicks1++;
            Staanplaats1.Text = $"Staanplaats {clicks1}";
            SemanticScreenReader.Announce(Staanplaats1.Text);
        }

        private void Staanplaats2_Clicked(object sender, EventArgs e)
        {
            clicks2++;
            Staanplaats2.Text = $"Staanplaats {clicks2}";
            SemanticScreenReader.Announce(Staanplaats2.Text);
        }

        private void Staanplaats3_Clicked(object sender, EventArgs e)
        {
            clicks3++;
            Staanplaats3.Text = $"Staanplaats {clicks3}";
            SemanticScreenReader.Announce(Staanplaats3.Text);
        }
        private void OnCancelButtonPointerEntered(object sender, PointerEventArgs e)
        {
             Staanplaats1.BackgroundColor = Colors.Green;
        }

        private void OnCancelButtonPointerExited(object sender, PointerEventArgs e)
        {
            Staanplaats1.BackgroundColor = Colors.DarkGreen; // Revert to original color
        }

        private void OnCancelButtonPointerEnteredST2(object sender, PointerEventArgs e)
        {
            Staanplaats2.BackgroundColor = Colors.Green;
        }

        private void OnCancelButtonPointerExitedST2(object sender, PointerEventArgs e)
        {
            Staanplaats2.BackgroundColor = Colors.DarkGreen; // Revert to original color
        }
        private void OnCancelButtonPointerEnteredST3(object sender, PointerEventArgs e)
        {
            Staanplaats3.BackgroundColor = Colors.Green;
        }

        private void OnCancelButtonPointerExitedST3(object sender, PointerEventArgs e)
        {
            Staanplaats3.BackgroundColor = Colors.DarkGreen; // Revert to original color
        }
    }
}
