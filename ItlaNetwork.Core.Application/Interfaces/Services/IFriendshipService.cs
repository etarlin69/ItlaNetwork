using ItlaNetwork.Core.Application.ViewModels.Friendship;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ItlaNetwork.Core.Application.Interfaces.Services
{
    public interface IFriendshipService
    {
        Task AddFriendByIdAsync(string senderUserId, string receiverUserId);
        Task AcceptFriendRequestAsync(string requestingUserId, string receiverUserId);
        Task RejectFriendRequestAsync(string requestingUserId, string receiverUserId);
        Task DeleteFriendRequestAsync(int requestId);
        Task DeleteFriendAsync(int friendshipId, string userId);
        Task<List<FriendViewModel>> GetAllFriends(string userId);
        Task<List<FriendRequestViewModel>> GetAllFriendRequests(string userId);
        Task<List<FriendRequestViewModel>> GetSentFriendRequests(string userId);
        Task<List<FriendViewModel>> GetAllUsers(string userId, string? userName);
    }
}