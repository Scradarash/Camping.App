using Camping.App.ViewModels;
using Microsoft.Maui.Layouts;
using System.Windows.Input;

namespace Camping.App.Views;

public partial class PlattegrondView : ContentPage
{
    private readonly PlattegrondViewModel _viewModel;

    // De afmetingen van de afbeelding (nodig voor de aspect ratio berekening)
    // Geprobeert om de afmetingen direct van de .png te krijgen, maar dat lukt nog niet
    private const double OriginalMapWidth = 1920;
    private const double OriginalMapHeight = 1080;

    // De dikte van de rand. Deze moet gelijk staan aan die in de XAML (StrokeThickness)
    private const double BorderThickness = 10;

    public PlattegrondView(PlattegrondViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = _viewModel;

        // We plaatsen de knoppen pas als de grootte van de pagina verandert (bijv. bij opstarten of bij het kleiner/groter maken van het schgerm)
        // Dit was eerst bij de afbeelding zelf
        this.SizeChanged += (s, e) => UpdateButtons();
    }

    private void UpdateButtons()
    {
        // We halen de huidige grootte van de container (de pagina zelf)
        double pageWidth = this.Width;
        double pageHeight = this.Height;

        // Als de breedte of de hoogte 0 is (app start net op), doen we niks
        if (pageWidth <= 0 || pageHeight <= 0) return;

        // Berekenen van de afmetingen
        Size renderSize = CalculateRenderSize(pageWidth, pageHeight);
        double renderWidth = renderSize.Width;
        double renderHeight = renderSize.Height;

        // MapFrame is de border rondom de kaart, ding wordt zo genoemd in de XAML
        // We geven hem hier de berekende afmetingen, en dat bepaald dus ook de grootte van de camping plattegrond
        MapFrame.WidthRequest = renderWidth;
        MapFrame.HeightRequest = renderHeight;

        // De border wordt natuurlijk binnen de renderwidth/height getekend
        // Dus de daadwerkelijk bruikbare ruimte de renderruimte min de border aan alle kanten
        double innerWidth = renderWidth - (2 * BorderThickness);
        double innerHeight = renderHeight - (2 * BorderThickness);

        // scaleFactor berekenen door de innerWidth te delen door de originele kaartbreedte
        double scaleFactor = innerWidth / OriginalMapWidth;

        // 30 als basis font size nemen, en die vermenigvuldigen met de scaleFactor
        double scaledFontSize = 30 * scaleFactor;

        // Overlay leegmaken voor de nieuwe knoppen geplaatst worden
        ButtonOverlayLayout.Children.Clear();

        AddVeldButtons(innerWidth, innerHeight, scaledFontSize);
        AddKalenderButton(innerWidth, innerHeight, scaledFontSize);
    }

    private Size CalculateRenderSize(double pageWidth, double pageHeight)
    {
        // Bereken de ratio van de originele kaart (in dit gevaal 1920 / 1080 = 1.777...)
        double mapRatio = OriginalMapWidth / OriginalMapHeight;

        // Een kleine marge rondom de kaart zodat hij mooi op tafel ligt
        double margin = 40;
        // Bereken beschikbare ruimte door de marge van de page width/height af te halen
        double availableWidth = pageWidth - margin;
        double availableHeight = pageHeight - margin;

        // Dan de daadwerkelijke beschikbare ratio door de beschikbare breedte door de beschikbare hoogte te delen
        double availableRatio = availableWidth / availableHeight;

        double renderWidth;
        double renderHeight;

        // Als de beschikbare ratio groter is dan de kaart ratio (bijv 1.8, betekent dit dat het scherm breder is dan de kaart
        if (availableRatio > mapRatio)
        {
            // Dus de renderheight wordt bepaald door de beschikbare hoogte
            renderHeight = availableHeight;
            // En de renderwidth wordt berekend op basis van de map ratio
            renderWidth = renderHeight * mapRatio;
        }
        // Als de beschikbare ratio kleiner is dan de kaart ratio (bijv 1.6), is het scherm hoger dan de kaart
        else
        {
            // Dus de renderwidth wordt bepaald door de beschikbare breedte
            renderWidth = availableWidth;
            // En de renderheight wordt berekend op basis van de map ratio
            renderHeight = renderWidth / mapRatio;
        }

        return new Size(renderWidth, renderHeight);
    }

    private void AddVeldButtons(double innerWidth, double innerHeight, double fontSize)
    {
        // De velden uit het ViewModel genereren
        foreach (var veld in _viewModel.Velden)
        {
            var btn = CreateMapButton(
                text: veld.Name,
                styleKey: "MapButtonStyle",
                command: _viewModel.SelectVeldCommand,
                fontSize: fontSize,
                commandParameter: veld
            );

            // De positie wordt nu berekend op basis van de innerWidth/Height ipv de imagesize waar het eerst op gebaseerd was
            double x = veld.XPosition * innerWidth;
            double y = veld.YPosition * innerHeight;
            double w = veld.Width * innerWidth;
            double h = veld.Height * innerHeight;

            AbsoluteLayout.SetLayoutFlags(btn, AbsoluteLayoutFlags.None);
            AbsoluteLayout.SetLayoutBounds(btn, new Rect(x, y, w, h));

            ButtonOverlayLayout.Children.Add(btn);
        }
    }

    private void AddKalenderButton(double innerWidth, double innerHeight, double fontSize)
    {
        // Kalender Knop
        var kalenderBtn = CreateMapButton(
            text: "Kalender",
            styleKey: "ConfirmButtonStyle",
            command: _viewModel.OpenKalenderCommand,
            fontSize: fontSize
        );

        // Positie verplaatst naar rechtsonder (waar voorheen de afsluitknop stond)
        double kalenderX = 0.852 * innerWidth;
        double kalenderY = 0.885 * innerHeight;
        double kalenderW = 0.098 * innerWidth;
        double kalenderH = 0.08 * innerHeight;

        AbsoluteLayout.SetLayoutFlags(kalenderBtn, AbsoluteLayoutFlags.None);
        AbsoluteLayout.SetLayoutBounds(kalenderBtn, new Rect(kalenderX, kalenderY, kalenderW, kalenderH));

        ButtonOverlayLayout.Children.Add(kalenderBtn);
    }

    private Button CreateMapButton(string text, string styleKey, ICommand command, double fontSize, object? commandParameter = null)
    {
        return new Button
        {
            Text = text,
            Style = (Style)Application.Current.Resources[styleKey],
            Command = command,
            CommandParameter = commandParameter,
            FontSize = fontSize,
            // Dus MAUI limiteert knoppen tot een minimum van 44px normaal gesproken
            // Daarom moet dit eerst op 0 gezet worden, anders scalen ze niet helemaal down op kleine schermen
            MinimumHeightRequest = 0,
            MinimumWidthRequest = 0,
            Padding = 0
        };
    }
}