using Camping.App.Views;
using Camping.Core.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Camping.App.ViewModels;



public partial class ToevoegenGastViewModel : ObservableObject
{


    private readonly ReserveringshouderValidatieService _validatieService;
    private readonly ToevoegenGastService _toevoegenGastService;

    [ObservableProperty]
    private string _invoerNaam;

    [ObservableProperty]
    private string _invoerLeeftijd;

    [ObservableProperty]
    private string _foutMeldingNaam;

    [ObservableProperty]
    private bool _zichtbaarheidMeldingNaam;

    [ObservableProperty]
    private string _foutMeldingLeeftijd;

    [ObservableProperty]
    private bool _zichtbaarheidMeldingLeeftijd;

   
    public ToevoegenGastViewModel(ReserveringshouderValidatieService validatieService, ToevoegenGastService toevoegenGastService)
    {
        _validatieService = validatieService;
        _toevoegenGastService = toevoegenGastService;
    }

    [RelayCommand]
    private async Task GoBack()
    {
        await Shell.Current.GoToAsync("..");
    }

    [RelayCommand]
    private async Task ToevoegenGast()
    {

        //Hier komen functies voor het checken van de status van de inputvelden
        await Shell.Current.GoToAsync("..");
    }

    

    // Deze methode wordt automatisch aangeroepen bij elke verandering
    partial void OnInvoerNaamChanged(string value)
    {
        // Jouw 'onchange' logica hier
        System.Diagnostics.Debug.WriteLine($"Nieuwe waarde naam: {value}");
        HasValidNaam(value);
    }
    partial void OnInvoerLeeftijdChanged(string value)
    {
        // Jouw 'onchange' logica hier
        System.Diagnostics.Debug.WriteLine($"Nieuwe waarde leeftijd: {value}");
        HasValidLeeftijd(value);
    }

   private bool HasValidNaam(String naam)
    {
        var result = _validatieService.ValidateNaam(naam);

        if (!result.IsValid)
        {
            FoutMeldingNaam = result.Error;
            ZichtbaarheidMeldingNaam = true;
            //Hier functie om knop inactief te maken
            return false;
        }
        ZichtbaarheidMeldingNaam = false;
        FoutMeldingNaam = string.Empty;
        return true;
    }

    private bool HasValidLeeftijd(String leeftijd)
    {
        var result = _toevoegenGastService.ValidateLeeftijd(leeftijd);

        if (!result.IsValid)
        {
            FoutMeldingLeeftijd = result.Error;
            ZichtbaarheidMeldingLeeftijd = true;
            //Hier functie om knop inactief te maken
            return false;
        }
        ZichtbaarheidMeldingLeeftijd = false;
        FoutMeldingLeeftijd = string.Empty;
        return true;
    }

}
