using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Camping.Core.Models
{
    public class Veld : ClickArea
    {
        public required string Name { get; set; }
        public required int id { get; set; }

        //Beschrijving veld
        public string Description { get; set; } = "Geen beschrijving beschikbaar.";
    }
}
