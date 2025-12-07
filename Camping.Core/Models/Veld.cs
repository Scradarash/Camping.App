namespace Camping.Core.Models
{
    public class Veld : ClickArea
    {
        public required string Name { get; set; }
        public required int id { get; set; }
        public string Description { get; set; } = "Geen beschrijving beschikbaar.";
        public string ImageName { get; set; } = "dotnet_bot.png"; // Weet niet zeker of t nodig is om een placeholder te hebben, maar je weet maar nooit
        public List<Staanplaats> Staanplaatsen { get; set; } = new List<Staanplaats>();
    }
}