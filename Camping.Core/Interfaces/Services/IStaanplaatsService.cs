using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Camping.Core.Models;

namespace Camping.Core.Interfaces.Services
{
    public interface IStaanplaatsService
    {
        IEnumerable<Staanplaats> GetAll();
    }
}
