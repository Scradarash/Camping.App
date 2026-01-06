using Camping.Core.Interfaces.Services;
using Camping.Core.Models;
using Camping.Core.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.ApplicationModel.Communication;

namespace Camping.App.ViewModels;



public partial class ToevoegenGastViewModel : ObservableObject
{
    private readonly IReserveringshouderValidatieService _validatieService;
    private readonly IToevoegenGastService _toevoegenGastService;
    private readonly IReservatieDataService _reservatieDataService;

    [ObservableProperty]
    private string _invoerNaam;

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

    [ObservableProperty]
    private DateTime _invoerLeeftijd = DateTime.Today;
    public ToevoegenGastViewModel(IReserveringshouderValidatieService validatieService, IToevoegenGastService toevoegenGastService, IReservatieDataService reservatieDataService)
    {
        _validatieService = validatieService;
        _toevoegenGastService = toevoegenGastService;
        _toevoegenGastKnopEnabled = false;
        _naamAkkoord = false;
        _leeftijdAkkoord = false;
        _reservatieDataService = reservatieDataService;
    }

    [RelayCommand]
    private async Task GoBack()
    {
        await Shell.Current.GoToAsync("..");
    }

    [RelayCommand]
    private async Task ToevoegenGast()
    {
        gastOpGastenlijst(maakGast());
        await Shell.Current.GoToAsync("..");
    }

    [RelayCommand]
    public void gastOpGastenlijst(Gast gast)
    {
        _reservatieDataService.GastenLijst.Add(gast);     
    }

    private Gast maakGast()
    {
        var nieuweGast = new Gast
        {
            Naam = _invoerNaam,
            Geboortedatum = DateOnly.FromDateTime(_invoerLeeftijd)
        };
        return nieuweGast;
    }
    

    // Deze methode wordt automatisch aangeroepen bij elke verandering
    partial void OnInvoerNaamChanged(string value)
    {
        HasValidNaam(value);
    }
    partial void OnInvoerLeeftijdChanged(DateTime value)
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

    private bool HasValidLeeftijd(DateTime leeftijd)
    {
        var result = _toevoegenGastService.ValidateGeboortedatum(leeftijd);

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