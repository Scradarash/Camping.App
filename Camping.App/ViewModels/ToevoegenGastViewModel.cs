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

    private bool _naamAkkoord;
    private bool _leeftijdAkkoord;

    [ObservableProperty]
    private bool _toevoegenGastKnopEnabled;


    public ToevoegenGastViewModel(ReserveringshouderValidatieService validatieService, ToevoegenGastService toevoegenGastService)
    {
        _validatieService = validatieService;
        _toevoegenGastService = toevoegenGastService;
        _toevoegenGastKnopEnabled = false;
        _naamAkkoord = false;
        _leeftijdAkkoord = false;
    }

    [RelayCommand]
    private async Task GoBack()
    {
        await Shell.Current.GoToAsync("..");
    }

    [RelayCommand]
    private async Task ToevoegenGast()
    {
        //Hier komt de functie om de query aan te roepen
        await Shell.Current.GoToAsync("..");
    }



    // Deze methode wordt automatisch aangeroepen bij elke verandering
    partial void OnInvoerNaamChanged(string value)
    {
        HasValidNaam(value);
    }
    partial void OnInvoerLeeftijdChanged(string value)
    {
        HasValidLeeftijd(value);
    }

    private bool HasValidNaam(String naam)
    {
        var result = _validatieService.ValidateNaam(naam);

        if (!result.IsValid)
        {
            SetNameValidNegative(result);
            CheckValidityInputs();
            return false;
        }
        SetNameValidPositive();
        CheckValidityInputs();
        return true;
    }

    private void SetNameValidPositive()
    {
        ZichtbaarheidMeldingNaam = false;
        FoutMeldingNaam = string.Empty;
        _naamAkkoord = true;
    }

    private void SetNameValidNegative((bool IsValid, string Error) result)
    {
        FoutMeldingNaam = result.Error;
        ZichtbaarheidMeldingNaam = true;
        _naamAkkoord = false;
    }

    private bool HasValidLeeftijd(String leeftijd)
    {
        var result = _toevoegenGastService.ValidateLeeftijd(leeftijd);

        if (!result.IsValid)
        {
            SetValidLeeftijdNegative(result);
            CheckValidityInputs();
            return false;
        }
        SetValidLeeftijdPositive();
        CheckValidityInputs();
        return true;
    }

    private void SetValidLeeftijdPositive()
    {
        ZichtbaarheidMeldingLeeftijd = false;
        FoutMeldingLeeftijd = string.Empty;
        _leeftijdAkkoord = true;
    }

    private void SetValidLeeftijdNegative((bool IsValid, string Error) result)
    {
        FoutMeldingLeeftijd = result.Error;
        ZichtbaarheidMeldingLeeftijd = true;
        _leeftijdAkkoord = false;
    }

    private bool CheckValidityInputs()
    {
        ToevoegenGastKnopEnabled = _leeftijdAkkoord && _naamAkkoord;
        if (ToevoegenGastKnopEnabled)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}