namespace Camping.Core.Models
{
    public class PrijsInfo
    {
        public string Omschrijving { get; set; } = string.Empty;
        public decimal Bedrag { get; set; }
        public string BedragTekst => $"€ {Bedrag:F2}";
    }
}
