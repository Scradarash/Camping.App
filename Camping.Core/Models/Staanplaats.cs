namespace Camping.Core.Models
{
    public class Staanplaats
    {
        public int id { get; set; }
        public int VeldId { get; set; } // Bij welk veld hoort deze staanplaats
        public string AccommodatieType { get; set; }
        public int AantalGasten { get; set; }
        public string Status { get; set; } = "Beschikbaar";
        public bool HeeftStroom { get; set; }
        public bool HeeftWater { get; set; }
    }
}