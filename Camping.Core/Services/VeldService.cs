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
    public class VeldService : IVeldService
    {
        private readonly IVeldRepository _repository;

        public VeldService(IVeldRepository repository)
        {
            _repository = repository;
        }

        public IEnumerable<Veld> GetAll()
        {
            return _repository.GetAll();
        }
    }
}
