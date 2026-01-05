namespace Camping.Core.Models
{
    public class Accommodatie
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public decimal Prijs { get; set; }
        public string PrijsTekst => $"€ {Prijs:F2},- per nacht";
    }
}
