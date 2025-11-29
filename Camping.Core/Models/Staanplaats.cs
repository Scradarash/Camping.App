using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Camping.Core.Models
{
    public class Staanplaats : ClickArea
    {
        public required string Name { get; set; }
        public required int id { get; set; }
    }
}
