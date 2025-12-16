using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Camping.Core.Models
{
    public class Gast
    {
        // NIet 'required' omdat Id pas kenbaar gemaakt word nadat gastgegevens in DB staan (AutoIncrement)
        public int Id {get; set;}   
        public required string Naam { get; set; }
        public required DateOnly Geboortedatum { get; set; }
        public required string Email { get; set; }
        public required string Telefoon { get; set; }

    }
}
