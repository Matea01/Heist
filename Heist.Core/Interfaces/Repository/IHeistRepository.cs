using Heist.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Heist.Core.Interfaces.Repository
{

    public interface IHeistRepository
    {
        Task<bool> HeistExistsAsync(string name);
        Task AddHeistAsync(HeistEntity heist);
        Task<HeistEntity> GetHeistByIdAsync(int id);
    }
}
