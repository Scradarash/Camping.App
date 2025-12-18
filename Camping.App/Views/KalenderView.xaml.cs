using Syncfusion.Maui.Calendar;
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

    // Event handler voor Annuleren
    private async void OnCancelClicked(object sender, EventArgs e)
    {
        // Sluit direct, slaat niets op
        await CloseModal();
    }

    // Event handler voor Opslaan
    private async void OnSaveClicked(object sender, EventArgs e)
    {
        if (BindingContext is not KalenderViewModel vm)
            return;

        try
        {
            // Eerst valideren we de input en proberen we op te slaan (zie ViewModel)
            string error = vm.ValidateAndSaveDates();

            // Als de error string niet leeg is, is er een validatiefout
            if (await HandleValidation(error))
                return;

            // Als de datums goed zijn, callen we ShowSuccessMessage
            await ShowSuccessMessage(vm);

            // Daarna sluiten we de modal
            await CloseModal();
        }
        catch (Exception ex)
        {
            // Overige onverwachte fouten afvangen
            await DisplayAlert("Fout", $"Er is een onverwachte fout opgetreden: {ex.Message}", "OK");
        }
    }

    private async Task<bool> HandleValidation(string error)
    {
        // Als er een error tekst is, tonen we die
        if (!string.IsNullOrEmpty(error))
        {
            await DisplayAlert("Let op", error, "OK");
            return true;
        }
        return false;
    }

    private async Task ShowSuccessMessage(KalenderViewModel vm)
    {
        await DisplayAlert(
        "Datum geselecteerd!",
        $"Aankomst: {vm.StartDate:dd-MM-yyyy}\nVertrek: {vm.EndDate:dd-MM-yyyy}",
        "OK");
    }

    private async Task CloseModal()
    {
        await Navigation.PopModalAsync();
    }
}