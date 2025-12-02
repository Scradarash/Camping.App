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
                    Id = 1,
                    Name = "Groepsveld",
                    IsBeschikbaar = true,
                    XPosition = 0.08,
                    YPosition = 0.13,
                    Width = 0.232,
                    Height = 0.213
                },
                new Staanplaats
                {
                    Id = 2,
                    Name = "Trekkersveld",
                    IsBeschikbaar = true,
                    XPosition = 0.11,
                    YPosition = 0.4457,
                    Width = 0.195,
                    Height = 0.181
                },
                new Staanplaats
                {
                    Id = 3,
                    Name = "Chaletveld",
                    IsBeschikbaar = false,
                    XPosition = 0.11,
                    YPosition = 0.669,
                    Width = 0.195,
                    Height = 0.181
                },
                new Staanplaats
                {
                    Id = 4,
                    Name = "Staatseveld",
                    IsBeschikbaar = true,
                    XPosition = 0.5677,
                    YPosition = 0.576,
                    Width = 0.195,
                    Height = 0.1952
                },
                new Staanplaats
                {
                    Id = 5,
                    Name = "Oranjeveld",
                    IsBeschikbaar = true,
                    XPosition = 0.37,
                    YPosition = 0.59,
                    Width = 0.145,
                    Height = 0.254
                }
            };
        }
    }
}
