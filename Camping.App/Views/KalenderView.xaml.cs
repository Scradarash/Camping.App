using Syncfusion.Maui.Calendar;
using Camping.Core.Interfaces.Services;
using Camping.App.ViewModels;

namespace Camping.App.Views;

public partial class KalenderView : ContentPage
{

    public KalenderView(KalenderViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }


    // Event gooien als de selectie verandert
    private void OnSelectionChanged(object sender, CalendarSelectionChangedEventArgs e)
    {
        if (BindingContext is KalenderViewModel vm &&
            e.NewValue is CalendarDateRange range)
        {
            vm.UpdateRange(range);
        }
    }
    private async void CloseButton_Clicked(object sender, EventArgs e)
    {
        if (BindingContext is not KalenderViewModel vm)
            return;

        // Check hier uitvoeren, en error tekst terugkrijgen indien nodig
        string error = vm.OpslaanDatum();

        // Als er een error tekst is, zoals het selecteren van een verkeerd jaar, toon die dan (nu is dat niet mogelijk door de MinDate/MaxDate restricties), maar je weet maar nooit
        if (!string.IsNullOrEmpty(error))
        {
            await DisplayAlert("Let op", error, "OK");
            return;
        }

        // Als er geen error is, zijn er twee opties:
        // Er is daadwerkelijk een datum gekozen (vm.StartDatum is niet null) -> Toon bevestiging van gekozen periode
        // Als er geen datum gekozen is (vm.StartDatum is null) -> dan sluiten we gwn het scherm zonder melding

        if (vm.StartDatum != null && vm.EndDatum != null)
        {
            await DisplayAlert(
                "Datum geselecteerd!",
                $"Aankomst: {vm.StartDatum:dd-MM-yyyy}\nVertrek: {vm.EndDatum:dd-MM-yyyy}",
                "OK");
        }

        // In alle gevallen zonder error modal sluiten
        // Dit dus zodat de gebruiker niet geforceerd wordt een datum te kiezen, maar wel de kalender kan bekijken
        await Navigation.PopModalAsync();
    }
}