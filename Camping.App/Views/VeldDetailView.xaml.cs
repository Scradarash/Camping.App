using Camping.App.ViewModels;
using Microsoft.Maui.Layouts;

namespace Camping.App.Views;

public partial class VeldDetailView : ContentPage
{
    private readonly VeldDetailViewModel _viewModel;

    public VeldDetailView(VeldDetailViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = _viewModel;

        // Net als met de knoppen op de plattegrond, wachten tot de afbeelding van grootte verandert
        VeldAfbeelding.SizeChanged += (s, e) => TekenGrid();
        // Als de lijst met staanplaatsen verandert, opnieuw tekenen
        _viewModel.Staanplaatsen.CollectionChanged += (s, e) => TekenGrid();
    }

    // Dit is een grote methode die het grid met knoppen tekent
    // Is het SRP? Ik denk t wel want het tekent alleen het grid en plaatst knoppen binnen die grid
    // Het is wel een beest van een methode, maar als je het doorloopt is het vrij logisch wat er gebeurt
    // Helemaal met mijn enge comments
    private void TekenGrid()
    {
        // Afmeting van de afbeelding
        double w = VeldAfbeelding.Width;
        double h = VeldAfbeelding.Height;
        var plekken = _viewModel.Staanplaatsen;

        // Als er geen plekken zijn of de afmeting is 0, niks doen
        if (w <= 0 || h <= 0 || plekken == null || plekken.Count == 0) return;

        // Overlay clearen en op de juiste grootte zetten
        PlekkenOverlay.Children.Clear();
        PlekkenOverlay.WidthRequest = w;
        PlekkenOverlay.HeightRequest = h;

        // Aantal plekken tellen
        int aantal = plekken.Count;

        // Bepaal hoeveel kolommen we nodig hebben.
        // We nemen de wortel van het totaal om een zo vierkant mogelijk rooster te krijgen.
        // We ronden naar boven af (Ceiling) zodat we breed genoeg zijn.
        // Dit wordt dus het aantal kolommen.
        int kolommen = (int)Math.Ceiling(Math.Sqrt(aantal));
        // Bepaal hoeveel rijen we nodig hebben op basis van die kolommen.
        // We delen het totaal aantal staanplaatsen door de breedte en ronden weer naar boven af.
        int rijen = (int)Math.Ceiling((double)aantal / kolommen);

        // Hoe groot 1 cel (dus 1 staanplaats knop) is
        // We doen dus de totale breedte delen door het aantal kolommen en totale hoogte door het aantal rijen
        double celBreedte = w / kolommen;
        double celHoogte = h / rijen;

        // Kleine marge rondom elke knop zodat ze niet tegen elkaar aan zitten
        double marge = 2;

        // Loop door alle plekken en plaats ze in het grid
        for (int i = 0; i < aantal; i++)
        {
            var plek = plekken[i];
            // Bepaal Rij en Kolom van deze plek
            int rij = i / kolommen;
            int kol = i % kolommen;

            // Positie en Grootte berekenen
            // De X en Y zijn startpunt van de cel + de marge
            double x = (kol * celBreedte) + marge;
            double y = (rij * celHoogte) + marge;

            // De breedte en hoogte zijn de celgrootte - 2x de marge zodat ze netjes binnen de cel passen en een beetje ruimte tussen de staanplaatsen hebben
            double knopBreedte = (celBreedte - (2 * marge));
            double knopHoogte = (celHoogte - (2 * marge));

            var btn = new Button
            {
                // Voorlopig tonen we gewoon het ID van de plek
                Text = plek.id.ToString(),
                Style = (Style)Application.Current.Resources["MapButtonStyle"],
                Padding = 0,
                CornerRadius = 8,
                Command = _viewModel.KiesPlekCommand,
                CommandParameter = plek
            };

            AbsoluteLayout.SetLayoutFlags(btn, AbsoluteLayoutFlags.None);
            // Hier gebruiken we de los berekende breedte en hoogte
            AbsoluteLayout.SetLayoutBounds(btn, new Rect(x, y, knopBreedte, knopHoogte));

            // En dan wordt de knop eindelijk toegevoegd aan de overlay
            PlekkenOverlay.Children.Add(btn);
        }
    }
}