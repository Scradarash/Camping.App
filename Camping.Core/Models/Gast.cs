namespace Camping.Core.Models
{
    public class Gast
    {
        // NIet 'required' omdat Id pas kenbaar gemaakt word nadat gastgegevens in DB staan (AutoIncrement)
        public int Id {get; set;}   
        public required string Naam { get; set; }
        public required DateOnly Geboortedatum { get; set; }

        //Niet required zodat Gast ook gebruikt kan worden voor een gast die geen reserveringshouder is.
        public string Email { get; set; }
        public string Telefoon { get; set; }

    }
}
