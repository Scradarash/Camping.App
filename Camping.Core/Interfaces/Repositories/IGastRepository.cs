using Camping.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Camping.Core.Interfaces.Repositories
{
    public interface IGastRepository
    {
        Task<IEnumerable<Gast>> GetAllAsync();
    }
}
