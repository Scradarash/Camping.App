using Camping.Core.Interfaces.Repositories;
using Camping.Core.Models;
using System.Collections.Generic;

namespace Camping.Core.Data.Repositories
{
    public class InMemoryStaanplaatsRepository : IStaanplaatsRepository
    {
        public IEnumerable<Staanplaats> GetAll()
        {
            return new List<Staanplaats>
            {
                new Staanplaats
                {
                    id = 1,
                    Name = "Groepsveld",
                    Description = "Een groot en gezellig veld, perfect voor groepen en families die dicht bij elkaar willen staan. Dicht bij de sanitaire voorzieningen.",
                    XPosition = 0.08,
                    YPosition = 0.13,
                    Width = 0.232,
                    Height = 0.213
                },
                new Staanplaats
                {
                    id = 2,
                    Name = "Trekkersveld",
                    Description = "Speciaal ingericht voor wandelaars en fietsers met kleine tentjes. Auto's zijn hier niet toegestaan.",
                    XPosition = 0.11,
                    YPosition = 0.4457,
                    Width = 0.195,
                    Height = 0.181
                },
                new Staanplaats
                {
                    id = 3,
                    Name = "Chaletveld",
                    Description = "Luxe veld voorzien van verharde paden en eigen stroomaansluitingen, uitsluitend voor chalets.",
                    XPosition = 0.11,
                    YPosition = 0.669,
                    Width = 0.195,
                    Height = 0.181
                },
                new Staanplaats
                {
                    id = 4,
                    Name = "Staatseveld",
                    Description = "Een rustig gelegen veld aan de rand van het bos. Ideaal voor rustzoekers.",
                    XPosition = 0.5677,
                    YPosition = 0.576,
                    Width = 0.195,
                    Height = 0.1952
                },
                new Staanplaats
                {
                    id = 5,
                    Name = "Oranjeveld",
                    Description = "Het zonnigste veld van de camping, centraal gelegen nabij de speeltuin.",
                    XPosition = 0.37,
                    YPosition = 0.59,
                    Width = 0.145,
                    Height = 0.254
                }
            };
        }
    }
}
