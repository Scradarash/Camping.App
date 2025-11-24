using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Camping.Core.Models;
using System.Collections.Generic;

namespace Camping.Core.Interfaces.Repositories;

public interface IStaanplaatsRepository
{
    IEnumerable<ClickArea> GetAllStaanplaatsen();
}
