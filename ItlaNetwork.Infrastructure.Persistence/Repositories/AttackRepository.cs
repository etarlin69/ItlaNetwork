using ItlaNetwork.Core.Application.Interfaces.Repositories;
using ItlaNetwork.Core.Domain.Entities;
using ItlaNetwork.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ItlaNetwork.Infrastructure.Persistence.Repositories
{
    public class AttackRepository : GenericRepository<Attack>, IAttackRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public AttackRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Attack>> GetAllByGameIdAsync(int gameId)
        {
            return await _dbContext.Attacks
                .Where(a => a.GameId == gameId)
                .ToListAsync();
        }
    }
}
