using Camping.Core.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Camping.Core.Services
{
    public class ToevoegenGastService : IToevoegenGastService
    {
        public (bool IsValid, string Error) ValidateLeeftijd(string leeftijd)
        {
            if (string.IsNullOrWhiteSpace(leeftijd))
                return (false, "Leeftijd is verplicht.");

            if (!Regex.IsMatch(leeftijd, "^[0-9]+$"))
                return (false, "Alleen gehele getallen zijn toegestaan");

            var leeftijdInCijfer = int.Parse(leeftijd);

            if (leeftijdInCijfer > 120)
            {
                return (false, "Het is erg onwaarschijnlijk dat je zo oud bent!");
            }

            return (true, string.Empty);
        }

        public (bool IsValid, string Error) ValidateGeboortedatum(DateTime? geboortedatum)
        {
            if (geboortedatum == null)
                return (false, "Geboortedatum is verplicht.");

            var vandaag = DateTime.Today;
            var leeftijd = vandaag.Year - geboortedatum.Value.Year;
            if (geboortedatum.Value.Date > vandaag.AddYears(-leeftijd)) leeftijd--;

            if (leeftijd < 0)
                return (false, "De gast kan niet onder de 0 zijn.");

            if (leeftijd > 120)
                return (false, "Het is erg onwaarschijnlijk dat de gast zo oud is.");

            return (true, string.Empty);
        }
    }
}
