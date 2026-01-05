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
    }
}
