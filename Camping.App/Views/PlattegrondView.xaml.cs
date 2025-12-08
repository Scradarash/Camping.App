using Camping.App.ViewModels;
using Microsoft.Maui.Layouts;

namespace Camping.App.Views;

public partial class PlattegrondView : ContentPage
{
    private readonly PlattegrondViewModel _viewModel;

    public PlattegrondView(PlattegrondViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = _viewModel;

        // Wacht tot het plaatje van grootte verandert (bij opstarten of draaien)
        // Dit doen we zodat we de knoppen altijd op de juiste plek kunnen zetten
        CampingPlattegrond.SizeChanged += (s, e) => UpdateButtons();
    }

    private void UpdateButtons()
    {
        // De overlay layout op de zelfde grootte zetten als de plattegrond afbeelding
        // Hierdoor komen de knoppen altijd op de juiste plek te staan
        ButtonOverlayLayout.WidthRequest = CampingPlattegrond.Width;
        ButtonOverlayLayout.HeightRequest = CampingPlattegrond.Height;

        ButtonOverlayLayout.Children.Clear();

        double imgWidth = CampingPlattegrond.Width;
        double imgHeight = CampingPlattegrond.Height;

        // Als de breedte of de hoogte 0 is (app start net op), doen we niks
        if (imgWidth <= 0 || imgHeight <= 0) return;

        // De velden uit het ViewModel genereren
        foreach (var veld in _viewModel.Velden)
        {
            var btn = new Button
            {
                Text = veld.Name,
                Style = (Style)Application.Current.Resources["MapButtonStyle"],
                Command = _viewModel.SelectVeldCommand,
                CommandParameter = veld
            };

            double x = veld.XPosition * imgWidth;
            double y = veld.YPosition * imgHeight;
            double w = veld.Width * imgWidth;
            double h = veld.Height * imgHeight;

            AbsoluteLayout.SetLayoutFlags(btn, AbsoluteLayoutFlags.None);
            AbsoluteLayout.SetLayoutBounds(btn, new Rect(x, y, w, h));

            ButtonOverlayLayout.Children.Add(btn);
        }

        // Kalender Knop
        var kalenderBtn = new Button
        {
            Text = "Kalender",
            Style = (Style)Resources["KalenderButtonStyle"],
            Command = _viewModel.OpenKalenderCommand
        };

        double kalenderX = 0.4 * imgWidth;
        double kalenderY = 0.05 * imgHeight;
        double kalenderW = 0.08 * imgWidth;
        double kalenderH = 0.06 * imgHeight;

        AbsoluteLayout.SetLayoutFlags(kalenderBtn, AbsoluteLayoutFlags.None);
        AbsoluteLayout.SetLayoutBounds(kalenderBtn, new Rect(kalenderX, kalenderY, kalenderW, kalenderH));
        ButtonOverlayLayout.Children.Add(kalenderBtn);

        // Afsluiten Knop
        var exitBtn = new Button
        {
            Text = "Afsluiten",
            // Stijl ophalen uit de App.xaml resources, daar staat namelijk alle styling voor knoppen die niet per view verschillen.
            Style = (Style)Application.Current.Resources["ExitButtonStyle"],
            Command = _viewModel.ExitAppCommand
        };

        double exitX = 0.8632 * imgWidth;
        double exitY = 0.89 * imgHeight;
        double exitW = 0.08 * imgWidth;
        double exitH = 0.06 * imgHeight;

        AbsoluteLayout.SetLayoutFlags(exitBtn, AbsoluteLayoutFlags.None);
        AbsoluteLayout.SetLayoutBounds(exitBtn, new Rect(exitX, exitY, exitW, exitH));
        ButtonOverlayLayout.Children.Add(exitBtn);
    }
}