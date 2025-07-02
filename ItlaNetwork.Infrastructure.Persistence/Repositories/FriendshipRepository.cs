using ItlaNetwork.Core.Application.Interfaces.Repositories;
using ItlaNetwork.Core.Domain.Entities;
using ItlaNetwork.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace ItlaNetwork.Infrastructure.Persistence.Repositories
{
    public class FriendshipRepository : GenericRepository<Friendship>, IFriendshipRepository
    {
        private readonly ApplicationDbContext _dbContext;
        public FriendshipRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Friendship>> GetAllByUserIdAsync(string userId)
        {
            return await _dbContext.Friendships
                .Where(f => f.UserId == userId || f.FriendId == userId)
                .ToListAsync();
        }

        public async Task<Friendship> GetByUsersAsync(string userId, string friendId)
        {
            return await _dbContext.Friendships
                .FirstOrDefaultAsync(f => f.UserId == userId && f.FriendId == friendId);
        }
    }
}