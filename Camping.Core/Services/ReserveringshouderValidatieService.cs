using Camping.Core.Interfaces.Services;
using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace Camping.Core.Services
{
    public class ReserveringshouderValidatieService : IReserveringshouderValidatieService
    {
        // UC5.1 – Validatie naam || UC6.2 Valideren naam gast
        public (bool IsValid, string Error) ValidateNaam(string naam)
        {
            if (string.IsNullOrWhiteSpace(naam))
                return (false, "Naam is verplicht.");

            var trimmed = naam.Trim();

            if (trimmed.Length < 2)
                return (false, "Naam moet minimaal 2 tekens bevatten.");

            if (trimmed.Length > 25)
                return (false, "Naam mag maximaal 25 tekens bevatten.");

            var regex = new Regex(@"^[a-zA-ZÀ-ÿ\s\-']+$");
            if (!regex.IsMatch(trimmed))
                return (false, "Naam bevat ongeldige tekens.");

            return (true, string.Empty);
        }

        // UC5.2 – Validatie geboortedatum
        public (bool IsValid, string Error) ValidateGeboortedatum(DateTime? geboortedatum)
        {
            if (geboortedatum == null)
                return (false, "Geboortedatum is verplicht.");

            var vandaag = DateTime.Today;
            var leeftijd = vandaag.Year - geboortedatum.Value.Year;
            if (geboortedatum.Value.Date > vandaag.AddYears(-leeftijd)) leeftijd--;

            if (leeftijd < 18)
                return (false, "De hoofdboeker moet minimaal 18 jaar zijn.");

            if (leeftijd > 120)
                return (false, "Leeftijd boven 120 jaar is niet toegestaan.");

            return (true, string.Empty);
        }

        // UC5.3 – Validatie emailadres
        public (bool IsValid, string Error) ValidateEmailadres(string emailadres)
        {
            if (string.IsNullOrWhiteSpace(emailadres))
                return (false, "E-mailadres is verplicht.");

            var pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            if (!Regex.IsMatch(emailadres, pattern, RegexOptions.IgnoreCase))
                return (false, "E-mailadres structuur klopt niet.");

            if (Regex.IsMatch(emailadres, @"[^a-zA-Z0-9@\.\-_]", RegexOptions.IgnoreCase))
                return (false, "E-mailadres bevat niet toegestane karakters.");

            return (true, string.Empty);
        }

        // UC5.4 – Validatie telefoonnummer
        public (bool IsValid, string Error) ValidateTelefoonnummer(string telefoonnummer)
        {
            if (string.IsNullOrWhiteSpace(telefoonnummer))
                return (false, "Telefoonnummer is verplicht.");

            var digits = new string(telefoonnummer.Where(char.IsDigit).ToArray());

            if (digits.Length < 8 || digits.Length > 15)
                return (false, "Telefoonnummer moet tussen 8 en 15 cijfers bevatten.");

            if (!Regex.IsMatch(telefoonnummer, "^[0-9 +]+$"))
                return (false, "Alleen cijfers, spaties en '+' zijn toegestaan.");

            return (true, string.Empty);
        }
    }
}