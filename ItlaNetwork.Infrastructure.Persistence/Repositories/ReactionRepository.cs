using ItlaNetwork.Core.Application.Interfaces.Repositories;
using ItlaNetwork.Core.Domain.Entities;
using ItlaNetwork.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace ItlaNetwork.Infrastructure.Persistence.Repositories
{
    public class ReactionRepository : GenericRepository<Reaction>, IReactionRepository
    {
        private readonly ApplicationDbContext _dbContext;
        public ReactionRepository(ApplicationDbContext dbContext) : base(dbContext) { _dbContext = dbContext; }
        public async Task<Reaction> GetByPostAndUserIdAsync(int postId, string userId) => await _dbContext.Reactions.FirstOrDefaultAsync(r => r.PostId == postId && r.UserId == userId);
        public async Task<List<Reaction>> GetAllByPostIdAsync(int postId) => await _dbContext.Reactions.Where(r => r.PostId == postId).ToListAsync();
        public async Task<List<Reaction>> GetAllByPostIdListAsync(List<int> postIds) => await _dbContext.Reactions.Where(r => postIds.Contains(r.PostId)).ToListAsync();
    }
}