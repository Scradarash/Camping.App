using Camping.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Camping.Core.Interfaces.Services
{
    public interface IToevoegenGastService
    {
        public (bool IsValid, string Error) ValidateGeboortedatum(DateTime? geboortedatum);
        public Gast maakGast(string naam, DateTime invoerleeftijd);
        public bool ValidateMaxGuests(int maxGasten, int hoeveelheidGasten);
    }
}