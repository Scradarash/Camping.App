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

            if (vm.TrySaveDates())
            {
                await DisplayAlert(
                    "Datum geselecteerd!",
                    $"Aankomst: {vm.StartDatum:dd-MM-yyyy}\nVertrek: {vm.EndDatum:dd-MM-yyyy}",
                    "OK");

                await Navigation.PopModalAsync();
            }
            else
            {
                await DisplayAlert(
                    "Rustaaaagh",
                    "Selecteer alstublieft een aankomst en vertrekdatum.",
                    "OK");
            }
    }
}