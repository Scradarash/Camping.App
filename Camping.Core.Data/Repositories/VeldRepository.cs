using Camping.Core.Interfaces.Repositories;
using Camping.Core.Models;
using System.Collections.Generic;

namespace Camping.Core.Data.Repositories;

public class VeldRepository : IVeldRepository
{
    // 1. De lijst is nu een private, read-only veld van de klasse.
    // Dit garandeert dat de data slechts één keer wordt geïnstantieerd.
    private readonly List<Veld> velden;

    // 2. Initialiseer de lijst in de constructor.
    public VeldRepository()
    {
        velden = new List<Veld>
        {
            new Veld
            {
                id = 1,
                Name = "Groepsveld",
                ImageName = "groepsveld.png",
                Description = "Een groot en gezellig veld, perfect voor groepen en families die dicht bij elkaar willen staan. Dicht bij de sanitaire voorzieningen.",
                XPosition = 0.08,
                YPosition = 0.13,
                Width = 0.232,
                Height = 0.213
            },
            new Veld
            {
                id = 2,
                Name = "Trekkersveld",
                ImageName = "trekkersveld.png",
                Description = "Speciaal ingericht voor wandelaars en fietsers met kleine tentjes. Auto's zijn hier niet toegestaan.",
                XPosition = 0.11,
                YPosition = 0.4457,
                Width = 0.195,
                Height = 0.181
            },
            new Veld
            {
                id = 3,
                Name = "Winterveld",
                ImageName = "winterveld.png",
                Description = "Mooi veld dat geschikt is voor winterkamperen.",
                XPosition = 0.11,
                YPosition = 0.669,
                Width = 0.195,
                Height = 0.181
            },
            new Veld
            {
                id = 4,
                Name = "Staatseveld",
                ImageName = "staatseveld.png",
                Description = "Een rustig gelegen veld aan de rand van het bos. Ideaal voor rustzoekers.",
                XPosition = 0.5677,
                YPosition = 0.576,
                Width = 0.195,
                Height = 0.1952
            },
            new Veld
            {
                id = 5,
                Name = "Oranjeveld",
                ImageName = "oranjeveld.png",
                Description = "Het zonnigste veld van de camping, centraal gelegen nabij de speeltuin.",
                XPosition = 0.37,
                YPosition = 0.59,
                Width = 0.145,
                Height = 0.254
            }
        };
    }

    public IEnumerable<Veld> GetAll()
    {
        return velden;
    }
}