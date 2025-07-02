using ItlaNetwork.Core.Application.Interfaces.Repositories;
using ItlaNetwork.Core.Domain.Entities;
using ItlaNetwork.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace ItlaNetwork.Infrastructure.Persistence.Repositories
{
    public class FriendRequestRepository : GenericRepository<FriendRequest>, IFriendRequestRepository
    {
        private readonly ApplicationDbContext _dbContext;
        public FriendRequestRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<FriendRequest>> GetAllBySenderIdAsync(string senderId)
        {
            return await _dbContext.FriendRequests.Where(fr => fr.SenderId == senderId).ToListAsync();
        }

        public async Task<List<FriendRequest>> GetAllByReceiverIdAsync(string receiverId)
        {
            return await _dbContext.FriendRequests.Where(fr => fr.ReceiverId == receiverId).ToListAsync();
        }

        public async Task<FriendRequest> FindByUsersAsync(string senderId, string receiverId)
        {
            return await _dbContext.FriendRequests
                .FirstOrDefaultAsync(r => (r.SenderId == senderId && r.ReceiverId == receiverId) || (r.SenderId == receiverId && r.ReceiverId == senderId));
        }
    }
}