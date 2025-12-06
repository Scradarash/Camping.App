namespace Camping.Core.Models
{
    public class KampeerPlek
    {
        public int Nummer { get; set; }
        public string AccommodatieType { get; set; } // Bijv. "Tent"
        public string Status { get; set; } = "Beschikbaar"; // Voor later

        //Voorzieningen per plek
        public bool HasElectricity { get; set; }
        public bool HasWater { get; set; }
    }
}