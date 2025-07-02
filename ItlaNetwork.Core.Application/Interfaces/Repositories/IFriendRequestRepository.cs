using ItlaNetwork.Core.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;
namespace ItlaNetwork.Core.Application.Interfaces.Repositories
{
    public interface IFriendRequestRepository : IGenericRepository<FriendRequest>
    {
        Task<List<FriendRequest>> GetAllBySenderIdAsync(string senderId);
        Task<List<FriendRequest>> GetAllByReceiverIdAsync(string receiverId);
        Task<FriendRequest> FindByUsersAsync(string senderId, string receiverId);
    }
}