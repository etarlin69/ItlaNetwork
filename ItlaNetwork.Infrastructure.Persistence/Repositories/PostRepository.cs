using ItlaNetwork.Core.Application.Interfaces.Repositories;
using ItlaNetwork.Core.Domain.Entities;
using ItlaNetwork.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ItlaNetwork.Infrastructure.Persistence.Repositories
{
    public class PostRepository : GenericRepository<Post>, IPostRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public PostRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// Obtiene todos los posts de un solo usuario.
        /// </summary>
        public async Task<List<Post>> GetAllByUserIdAsync(string userId)
        {
            return await _dbContext.Posts
                .Where(p => p.UserId == userId)
                .ToListAsync();
        }

        /// <summary>
        /// Obtiene todos los posts de varios usuarios a la vez.
        /// </summary>
        public async Task<List<Post>> GetAllByUserIdsAsync(IEnumerable<string> userIds)
        {
            return await _dbContext.Posts
                .Where(p => userIds.Contains(p.UserId))
                .ToListAsync();
        }
    }
}
