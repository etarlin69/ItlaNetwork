using ItlaNetwork.Core.Application.Interfaces.Repositories;
using ItlaNetwork.Core.Domain.Entities;
using ItlaNetwork.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace ItlaNetwork.Infrastructure.Persistence.Repositories
{
    public class CommentRepository : GenericRepository<Comment>, ICommentRepository
    {
        private readonly ApplicationDbContext _dbContext;
        public CommentRepository(ApplicationDbContext dbContext) : base(dbContext) { _dbContext = dbContext; }
        public async Task<List<Comment>> GetAllByPostIdAsync(int postId) => await _dbContext.Comments.Where(c => c.PostId == postId).ToListAsync();
        public async Task<List<Comment>> GetAllByPostIdListAsync(List<int> postIds) => await _dbContext.Comments.Where(c => postIds.Contains(c.PostId)).ToListAsync();
    }
}