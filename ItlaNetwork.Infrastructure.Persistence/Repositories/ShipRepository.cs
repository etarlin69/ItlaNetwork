using ItlaNetwork.Core.Application.Interfaces.Repositories;
using ItlaNetwork.Core.Domain.Entities;
using ItlaNetwork.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ItlaNetwork.Infrastructure.Persistence.Repositories
{
    public class ShipRepository : GenericRepository<Ship>, IShipRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public ShipRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Ship>> GetAllByGameIdAsync(int gameId)
        {
            return await _dbContext.Ships
                .Where(s => s.GameId == gameId)
                .ToListAsync();
        }
    }
}
