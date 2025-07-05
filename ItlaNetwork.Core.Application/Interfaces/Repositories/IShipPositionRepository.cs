using System.Collections.Generic;
using System.Threading.Tasks;
using ItlaNetwork.Core.Domain.Entities;

namespace ItlaNetwork.Core.Application.Interfaces.Repositories
{
    public interface IShipPositionRepository : IGenericRepository<ShipPosition>
    {
        Task<List<ShipPosition>> GetAllByGameIdAsync(int gameId);
        Task<List<ShipPosition>> GetAllByShipIdAsync(int shipId);
    }
}
