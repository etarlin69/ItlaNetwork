using ItlaNetwork.Core.Application.Interfaces.Repositories;
using ItlaNetwork.Core.Domain.Entities;
using ItlaNetwork.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace ItlaNetwork.Infrastructure.Persistence.Repositories
{
    public class GameRepository : GenericRepository<Game>, IGameRepository
    {
        private readonly ApplicationDbContext _dbContext;
        public GameRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Game>> GetAllByPlayerIdAsync(string playerId)
        {
            return await _dbContext.Games
                .Where(g => g.Player1Id == playerId || g.Player2Id == playerId)
                .ToListAsync();
        }
    }
}