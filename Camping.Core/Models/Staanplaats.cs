namespace Camping.Core.Models
{
    public class Staanplaats
    {
        public int id { get; set; }
        public int VeldId { get; set; } // Bij welk veld hoort deze staanplaats

        public decimal Prijs { get; set; }          // Prijs per nacht vanuit DB (staanplaatsen.prijs)
        public int AantalGasten { get; set; }       // Max gasten vanuit DB (staanplaatsen.aantal_gasten)

        public string AccommodatieType { get; set; }
        public string Status { get; set; } = "Beschikbaar";
        public bool HeeftStroom { get; set; }
        public bool HeeftWater { get; set; }
    }
}
