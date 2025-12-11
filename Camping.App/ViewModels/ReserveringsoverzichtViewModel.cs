using Camping.Core.Interfaces.Services;
using Camping.Core.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;

namespace Camping.App.ViewModels;

public partial class ReserveringsoverzichtViewModel : ObservableObject
{
    private readonly IReservatieDataService _reservatieDataService;
    private readonly IAccommodatieService _accommodatieService;
    private readonly IReserveringService _reserveringService;

    [ObservableProperty]
    private string periodDescription;

    [ObservableProperty]
    private string fieldName;

    [ObservableProperty]
    private string staanplaatsNaam;

    [ObservableProperty]
    private Accommodatie selectedAccommodatie;

    [ObservableProperty]
    private string naam;

    [ObservableProperty]
    private string naamFoutmelding;

    [ObservableProperty]
    private bool isNaamFoutZichtbaar;

    public ObservableCollection<Accommodatie> Accommodaties { get; } = new();

    public ReserveringsoverzichtViewModel(IReservatieDataService reservatieDataService, IAccommodatieService accommodatieService, IReserveringService reserveringService)
    {
        _reservatieDataService = reservatieDataService;
        _accommodatieService = accommodatieService;
        _reserveringService = reserveringService;

        LoadData();
    }

    private void LoadData()
    {
        if (_reservatieDataService.SelectedVeld == null || _reservatieDataService.StartDate == null)
        {
            return;
        }

        FieldName = _reservatieDataService.SelectedVeld.Name;
        PeriodDescription = $"{_reservatieDataService.StartDate:dd-MM-yyyy} tot {_reservatieDataService.EndDate:dd-MM-yyyy}";

        var gekozenPlek = _reservatieDataService.SelectedStaanplaats;

        if (gekozenPlek != null)
        {
            StaanplaatsNaam = $"Plaats: {gekozenPlek.id} ({gekozenPlek.AccommodatieType})";
        }
        else
        {
            StaanplaatsNaam = "Geen specifieke plaats gekozen";
        }

        Accommodaties.Clear();
        var gefilterdeLijst = _accommodatieService.GetGeschikteAccommodaties(_reservatieDataService.SelectedVeld);

        foreach (var acc in gefilterdeLijst)
        {
            if (gekozenPlek != null)
            {
                if (acc.Name.Contains(gekozenPlek.AccommodatieType, StringComparison.OrdinalIgnoreCase)
                    || gekozenPlek.AccommodatieType.Contains(acc.Name, StringComparison.OrdinalIgnoreCase))
                {
                    Accommodaties.Add(acc);
                }
            }
            else
            {
                Accommodaties.Add(acc);
            }
        }

        if (Accommodaties.Count > 0)
        {
            SelectedAccommodatie = Accommodaties[0];
        }

        Naam = _reservatieDataService.Naam;
    }

    [RelayCommand]
    private async Task BevestigAccommodatie()
    {
        if (!HasValidNaam())
        {
            return;
        }

        if (SelectedAccommodatie == null)
        {
            await Application.Current.MainPage.DisplayAlert("Fout", "Kies een accommodatie type.", "OK");
            return;
        }

        if (!_reservatieDataService.StartDate.HasValue || !_reservatieDataService.EndDate.HasValue)
        {
            await Application.Current.MainPage.DisplayAlert("Fout", "Er is nog geen periode geselecteerd.", "OK");
            return;
        }

        if (_reservatieDataService.SelectedVeld == null)
        {
            await Application.Current.MainPage.DisplayAlert("Fout", "Er is geen veld geselecteerd.", "OK");
            return;
        }

        if (_reservatieDataService.SelectedStaanplaats == null)
        {
            await Application.Current.MainPage.DisplayAlert("Fout", "Er is geen staanplaats geselecteerd.", "OK");
            return;
        }

        try
        {
            _reservatieDataService.Naam = Naam;
            _reservatieDataService.SelectedAccommodatie = SelectedAccommodatie;

            _reserveringService.MaakReservering(
                _reservatieDataService.StartDate.Value,
                _reservatieDataService.EndDate.Value,
                _reservatieDataService.SelectedVeld,
                _reservatieDataService.SelectedStaanplaats,
                SelectedAccommodatie);

            await Application.Current.MainPage.DisplayAlert(
                "Reservering voltooid",
                "De reservering is succesvol opgeslagen.",
                "OK");

            await Shell.Current.GoToAsync("//PlattegrondView");
        }
        catch (Exception ex)
        {
            await Application.Current.MainPage.DisplayAlert(
                "Fout bij opslaan",
                $"Er ging iets mis bij het opslaan van de reservering:\n{ex.Message}",
                "OK");
        }
    }

    [RelayCommand]
    private async Task GoBack()
    {
        await Shell.Current.GoToAsync("..");
    }

    private bool HasValidNaam()
    {
        if (string.IsNullOrWhiteSpace(Naam))
        {
            NaamFoutmelding = "Naam is verplicht.";
            IsNaamFoutZichtbaar = true;
            return false;
        }

        var trimmed = Naam.Trim();

        if (trimmed.Length < 2)
        {
            NaamFoutmelding = "Naam moet minimaal 2 tekens bevatten.";
            IsNaamFoutZichtbaar = true;
            return false;
        }

        var regex = new Regex(@"^[a-zA-ZÀ-ÿ\s\-']+$");

        if (!regex.IsMatch(trimmed))
        {
            NaamFoutmelding = "Naam bevat ongeldige tekens.";
            IsNaamFoutZichtbaar = true;
            return false;
        }

        NaamFoutmelding = string.Empty;
        IsNaamFoutZichtbaar = false;
        return true;
    }
}