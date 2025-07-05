using ItlaNetwork.Core.Application.Interfaces.Repositories;
using ItlaNetwork.Core.Domain.Entities;
using ItlaNetwork.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ItlaNetwork.Infrastructure.Persistence.Repositories
{
    public class ShipPositionRepository : GenericRepository<ShipPosition>, IShipPositionRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public ShipPositionRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<ShipPosition>> GetAllByGameIdAsync(int gameId)
        {
            
            return await _dbContext.ShipPositions
                .Where(pos => _dbContext.Ships
                    .Where(s => s.GameId == gameId)
                    .Select(s => s.Id)
                    .Contains(pos.ShipId))
                .ToListAsync();
        }

        public async Task<List<ShipPosition>> GetAllByShipIdAsync(int shipId)
        {
            return await _dbContext.ShipPositions
                .Where(pos => pos.ShipId == shipId)
                .ToListAsync();
        }
    }
}
