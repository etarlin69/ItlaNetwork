using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ItlaNetwork.Core.Application.Interfaces.Repositories;
using ItlaNetwork.Core.Domain.Entities;
using ItlaNetwork.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace ItlaNetwork.Infrastructure.Persistence.Repositories
{
    public class PostRepository : GenericRepository<Post>, IPostRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public PostRepository(ApplicationDbContext dbContext)
            : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Post>> GetAllAsync()
            => await _dbContext.Posts
                 .OrderByDescending(p => p.CreatedAt)
                 .ToListAsync();

        public async Task<List<Post>> GetAllByUserIdAsync(string userId)
            => await _dbContext.Posts
                 .Where(p => p.UserId == userId)
                 .OrderByDescending(p => p.CreatedAt)
                 .ToListAsync();

        public async Task<List<Post>> GetAllByUserIdsAsync(IEnumerable<string> userIds)
            => await _dbContext.Posts
                 .Where(p => userIds.Contains(p.UserId))
                 .OrderByDescending(p => p.CreatedAt)
                 .ToListAsync();
    }
}