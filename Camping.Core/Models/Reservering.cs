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
    }
}
