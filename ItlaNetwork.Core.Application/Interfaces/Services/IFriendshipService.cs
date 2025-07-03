using ItlaNetwork.Core.Application.ViewModels.Friendship;
using ItlaNetwork.Core.Application.ViewModels.Post;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ItlaNetwork.Core.Application.Interfaces.Services
{
    public interface IFriendshipService
    {
        Task SendFriendRequestAsync(string receiverUserId);
        Task AcceptFriendRequestAsync(int requestId);
        Task RejectFriendRequestAsync(int requestId);
        Task DeleteFriendAsync(string friendId);
        Task<List<FriendViewModel>> GetAllFriends();
        Task<List<FriendRequestViewModel>> GetPendingFriendRequests();
        Task<List<FriendRequestViewModel>> GetSentFriendRequests();
        Task<List<FriendViewModel>> GetPotentialFriends(string userNameQuery);

        // Nuevo: posts de todos mis amigos
        Task<List<PostViewModel>> GetFriendsPostsAsync();

        // Nuevo: posts de un amigo concreto
        Task<List<PostViewModel>> GetPostsByFriendAsync(string friendUserId);
    }
}
