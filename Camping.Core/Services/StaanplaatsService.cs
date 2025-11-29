using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Camping.Core.Interfaces.Repositories;
using Camping.Core.Interfaces.Services;
using Camping.Core.Models;

namespace Camping.Core.Services
{
    public class StaanplaatsService : IStaanplaatsService
    {
        private readonly IStaanplaatsRepository _repository;

        public StaanplaatsService(IStaanplaatsRepository repository)
        {
            _repository = repository;
        }

        public IEnumerable<Staanplaats> GetAll()
        {
            return _repository.GetAll();
        }
    }
}
