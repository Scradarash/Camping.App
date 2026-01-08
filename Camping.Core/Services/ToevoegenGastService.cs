using Camping.Core.Interfaces.Services;
using Camping.Core.Models;
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

        public Gast maakGast(string naam, DateTime invoerleeftijd)
        {
            var nieuweGast = new Gast
            {
                Naam = naam,
                Geboortedatum = DateOnly.FromDateTime(invoerleeftijd)
            };
            return nieuweGast;
        }

        public bool ValidateMaxGuests(int maxGasten, int hoeveelheidGasten)
        {
            if (hoeveelheidGasten < maxGasten)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
