using Camping.App.ViewModels;
using Camping.Core.Models;
using Microsoft.Maui.Layouts;
using System.Collections.ObjectModel;

namespace Camping.App.Views;

public partial class VeldDetailView : ContentPage
{
    private readonly VeldDetailViewModel _viewModel;

    // Houdt bij welke knop als laatste is aangeklikt voor de blauwe selectie
    private Button? _lastSelectedButton;

    public VeldDetailView(VeldDetailViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = _viewModel;

        // Net als met de knoppen op de plattegrond, wachten tot de afbeelding van grootte verandert
        VeldAfbeelding.SizeChanged += (s, e) => DrawGrid();
        // Als de lijst met staanplaatsen verandert, opnieuw tekenen
        _viewModel.Staanplaatsen.CollectionChanged += (s, e) => DrawGrid();
    }

    private void DrawGrid()
    {
        // Afmeting van de afbeelding
        double imageWidth = VeldAfbeelding.Width;
        double imageHeight = VeldAfbeelding.Height;

        ObservableCollection<Staanplaats> staanplaatsen = _viewModel.Staanplaatsen;

        // Als er geen plekken zijn, niks doen
        if (staanplaatsen.Count == 0) return;

        // Reset de selectie variabele omdat we het grid opnieuw tekenen
        _lastSelectedButton = null;

        // Overlay clearen en op de juiste grootte zetten
        PlekkenOverlay.Children.Clear();
        PlekkenOverlay.WidthRequest = imageWidth;
        PlekkenOverlay.HeightRequest = imageHeight;

        int count = staanplaatsen.Count;

        (int columns, int rows) = CalculateGridDimensions(count);

        // Hoe groot 1 cel (dus 1 staanplaats knop) is
        // We doen dus de totale breedte delen door het aantal kolommen en totale hoogte door het aantal rijen
        double cellWidth = imageWidth / columns;
        double cellHeight = imageHeight / rows;

        // Kleine marge rondom elke knop zodat ze niet tegen elkaar aan zitten
        double margin = 2;

        // Loop door alle plekken en plaats ze in het grid
        for (int i = 0; i < count; i++)
        {
            Staanplaats staanplaats = staanplaatsen[i];

            // Bepaal Rij en Kolom van deze plek
            int row = i / columns;
            int col = i % columns;

            // Button aanmaken
            Button btn = CreateStaanplaatsButton(staanplaats, cellWidth, cellHeight, margin, col, row);

            // Toevoegen aan layout
            PlekkenOverlay.Children.Add(btn);
        }
    }

    private (int cols, int rows) CalculateGridDimensions(int count)
    {
        // Bepaal hoeveel kolommen we nodig hebben.
        // We nemen de wortel van het totaal om een zo vierkant mogelijk rooster te krijgen.
        // We ronden naar boven af (Ceiling) zodat we breed genoeg zijn.
        int columns = (int)Math.Ceiling(Math.Sqrt(count));

        // Bepaal hoeveel rijen we nodig hebben op basis van die kolommen.
        // We delen het totaal aantal staanplaatsen door de breedte en ronden weer naar boven af.
        int rows = (int)Math.Ceiling((double)count / columns);

        return (columns, rows);
    }

    private Button CreateStaanplaatsButton(Staanplaats staanplaats, double cellWidth, double cellHeight, double margin, int col, int row)
    {
        // Positie en Grootte berekenen
        // De X en Y zijn startpunt van de cel + de marge
        double x = (col * cellWidth) + margin;
        double y = (row * cellHeight) + margin;

        // De breedte en hoogte zijn de celgrootte - 2x de marge zodat ze netjes binnen de cel passen en een beetje ruimte tussen de staanplaatsen hebben
        double buttonWidth = (cellWidth - (2 * margin));
        double buttonHeight = (cellHeight - (2 * margin));

        // Image source bepalen op basis van AccommodatieType
        string imageSource = GetStaanplaatsImageSource(staanplaats);

        Button btn = new Button
        {
            ImageSource = imageSource,
            Padding = 0,
            CornerRadius = 8,
            Command = _viewModel.SelectStaanplaatsCommand,
            CommandParameter = staanplaats
        };

        // Tooltip om staanplaats id te tonen
        ToolTipProperties.SetText(btn, $"Staanplaats {staanplaats.id}");

        ApplyButtonStyle(btn, staanplaats);

        // Layout instellen
        AbsoluteLayout.SetLayoutFlags(btn, AbsoluteLayoutFlags.None);
        // Hier gebruiken we de los berekende breedte en hoogte
        AbsoluteLayout.SetLayoutBounds(btn, new Rect(x, y, buttonWidth, buttonHeight));

        return btn;
    }

    private string GetStaanplaatsImageSource(Staanplaats staanplaats)
    {
        // Controleren op null voor de zekerheid
        if (string.IsNullOrWhiteSpace(staanplaats.AccommodatieType))
            return "tent.png"; // tent image als fallback

        string type = staanplaats.AccommodatieType.ToLowerInvariant();

        if (type.Contains("chalet"))
            return "chalet.png";

        // Op alle plekken waar een caravan kan staan, kan ook een camper staan, dus die in 1 check verwerken
        if (type.Contains("caravan") || type.Contains("camper"))
            return "caravan_camper.png";

        // Als er geen match is, standaard tent afbeelding gebruiken
        return "tent.png";
    }

    private void ApplyButtonStyle(Button btn, Staanplaats staanplaats)
    {
        // Als de status bezet is, andere stijl en knop uitschakelen
        if (staanplaats.Status == "Bezet")
        {
            btn.Style = (Style)Resources["OccupiedMapButtonStyle"];
            btn.IsEnabled = false;
        }
        else
        {
            btn.Style = (Style)Application.Current.Resources["MapButtonStyle"];

            // Geklikte button moet blauw worden, vorige weer groen.
            btn.Clicked += (sender, args) =>
            {
                UpdateVisualSelection((Button)sender);
            };
        }
    }

    private void UpdateVisualSelection(Button clickedButton)
    {
        // Als er al een knop geselecteerd was, maak die dan weer groen
        if (_lastSelectedButton != null)
        {
            _lastSelectedButton.Style = (Style)Application.Current.Resources["MapButtonStyle"];
        }

        // De nieuw geselecteerde knop blauw maken.
        clickedButton.Style = (Style)Resources["SelectedMapButtonStyle"];

        // Knop opslaan in _laatstGeselecteerdeKnop
        _lastSelectedButton = clickedButton;
    }
}