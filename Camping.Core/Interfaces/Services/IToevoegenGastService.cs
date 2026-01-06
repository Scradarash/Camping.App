using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Camping.Core.Interfaces.Services
{
    public interface IToevoegenGastService
    {
        public (bool IsValid, string Error) ValidateLeeftijd(string leeftijd);
        public (bool IsValid, string Error) ValidateGeboortedatum(DateTime? geboortedatum);
    }
}