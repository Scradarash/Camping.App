namespace Camping.Core.Models
{
    public class Reservering
    {
        public int Id { get; set; }
        public DateTime StartDatum { get; set; }
        public DateTime EindDatum { get; set; }
        public int VeldId { get; set; }
        public int StaanplaatsId { get; set; }
        public int AccommodatieId { get; set; }
        public bool KiestStroom { get; set; }
        public bool KiestWater { get; set; }
        public decimal TotaalPrijs { get; set; }
    }
}
