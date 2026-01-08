using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Camping.Core.Interfaces.Services
{
    public interface IReserveringshouderValidatieService
    {
        public (bool IsValid, string Error) ValidateNaam(string naam);
        public (bool IsValid, string Error) ValidateGeboortedatum(DateTime? geboortedatum);
        public (bool IsValid, string Error) ValidateEmailadres(string emailadres);
        public (bool IsValid, string Error) ValidateTelefoonnummer(string telefoonnummer);
    }
}
