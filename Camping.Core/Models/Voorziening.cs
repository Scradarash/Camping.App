namespace Camping.Core.Models
{
    public class Voorziening
    {
        public int Id { get; set; }
        public required string Naam { get; set; }
        public decimal Prijs { get; set; }
    }
}
