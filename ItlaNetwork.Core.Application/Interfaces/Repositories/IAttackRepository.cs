using System.Collections.Generic;
using System.Threading.Tasks;
using ItlaNetwork.Core.Domain.Entities;

namespace ItlaNetwork.Core.Application.Interfaces.Repositories
{
    public interface IAttackRepository : IGenericRepository<Attack>
    {
        Task<List<Attack>> GetAllByGameIdAsync(int gameId);
    }
}
