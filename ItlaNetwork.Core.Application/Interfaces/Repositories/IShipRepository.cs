using System.Collections.Generic;
using System.Threading.Tasks;
using ItlaNetwork.Core.Domain.Entities;

namespace ItlaNetwork.Core.Application.Interfaces.Repositories
{
    public interface IShipRepository : IGenericRepository<Ship>
    {
        Task<List<Ship>> GetAllByGameIdAsync(int gameId);
    }
}
