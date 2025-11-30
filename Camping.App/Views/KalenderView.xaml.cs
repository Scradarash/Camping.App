using Syncfusion.Maui.Calendar;

namespace Camping.App.Views;

public partial class KalenderView : ContentPage
{
    // Variabel om de gekozen datums bij te houden
    private DateTime? _startDatum;
    private DateTime? _eindDatum;

    public KalenderView()
    {
        InitializeComponent();
    }

    // Event gooien als de selectie verandert
    private void OnSelectionChanged(object sender, CalendarSelectionChangedEventArgs e)
    {
        // De CalendarDateRange is een bereik met een start- en einddatum, ipv losse datums selecteren
        var range = e.NewValue as CalendarDateRange;

        if (range != null)
        {
            _startDatum = range.StartDate;
            _eindDatum = range.EndDate;
        }
    }

    private async void CloseButton_Clicked(object sender, EventArgs e)
    {
        // Check of start en einddatum zijn gekozen
        if (_startDatum != null && _eindDatum != null)
        {
            // Popup tonen om de boel te testen (voor nu)
            await DisplayAlert("Datum geselecteerd!",
                $"Aankomst: {_startDatum:dd-MM-yyyy}\nVertrek: {_eindDatum:dd-MM-yyyy}",
                "OK");
        }
        else
        {
            // Wanneer 1 datum is geselecteerd, maar niet beiden, ff waarschuwing tonen
            await DisplayAlert("Rustaaaagh", "Selecteer alstublieft een aankomst en vertrekdatum.", "OK");
            return; 
        }

        await Navigation.PopModalAsync();
    }
}