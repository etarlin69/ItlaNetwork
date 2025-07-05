using AutoMapper;
using ItlaNetwork.Core.Application.Interfaces.Repositories;
using ItlaNetwork.Core.Application.Interfaces.Services;
using ItlaNetwork.Core.Application.ViewModels.Friendship;
using ItlaNetwork.Core.Application.ViewModels.Post;
using ItlaNetwork.Core.Domain.Entities;
using ItlaNetwork.Core.Domain.Enums;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ItlaNetwork.Core.Application.Services
{
    public class FriendshipService : IFriendshipService
    {
        private readonly IFriendshipRepository _friendshipRepository;
        private readonly IFriendRequestRepository _friendRequestRepository;
        private readonly IAccountService _accountService;
        private readonly IPostService _postService;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public FriendshipService(
            IFriendshipRepository friendshipRepository,
            IFriendRequestRepository friendRequestRepository,
            IAccountService accountService,
            IPostService postService,
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor)
        {
            _friendshipRepository = friendshipRepository;
            _friendRequestRepository = friendRequestRepository;
            _accountService = accountService;
            _postService = postService;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task SendFriendRequestAsync(string receiverUserId)
        {
            var senderUserId = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(senderUserId) || senderUserId == receiverUserId)
                return;

            var existing = await _friendRequestRepository.FindByUsersAsync(senderUserId, receiverUserId);
            if (existing != null) return;

            var request = new FriendRequest
            {
                SenderId = senderUserId,
                ReceiverId = receiverUserId,
                Status = FriendRequestStatus.Pending
            };

            await _friendRequestRepository.AddAsync(request);
        }

        public async Task AcceptFriendRequestAsync(int requestId)
        {
            var request = await _friendRequestRepository.GetByIdAsync(requestId);
            if (request == null || request.Status != FriendRequestStatus.Pending) return;

            await _friendshipRepository.AddAsync(new Friendship { UserId = request.SenderId, FriendId = request.ReceiverId });
            await _friendshipRepository.AddAsync(new Friendship { UserId = request.ReceiverId, FriendId = request.SenderId });
            await _friendRequestRepository.DeleteAsync(request);
        }

        public async Task RejectFriendRequestAsync(int requestId)
        {
            var request = await _friendRequestRepository.GetByIdAsync(requestId);
            if (request == null) return;
            await _friendRequestRepository.DeleteAsync(request);
        }

        public async Task DeleteFriendAsync(string friendId)
        {
            var currentUserId = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(currentUserId)) return;

            var f1 = await _friendshipRepository.GetByUsersAsync(currentUserId, friendId);
            var f2 = await _friendshipRepository.GetByUsersAsync(friendId, currentUserId);

            if (f1 != null) await _friendshipRepository.DeleteAsync(f1);
            if (f2 != null) await _friendshipRepository.DeleteAsync(f2);
        }

        public async Task<List<FriendViewModel>> GetAllFriends()
        {
            var currentUserId = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(currentUserId)) return new List<FriendViewModel>();

            var friendships = await _friendshipRepository.GetAllByUserIdAsync(currentUserId);
            var friendIds = friendships
                .Select(f => f.UserId == currentUserId ? f.FriendId : f.UserId)
                .Distinct()
                .ToList();

            if (!friendIds.Any()) return new List<FriendViewModel>();

            var users = await _accountService.GetUsersByIdsAsync(friendIds);
            return _mapper.Map<List<FriendViewModel>>(users);
        }

        public async Task<List<FriendRequestViewModel>> GetPendingFriendRequests()
        {
            var currentUserId = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(currentUserId)) return new List<FriendRequestViewModel>();

            var requests = await _friendRequestRepository.GetAllByReceiverIdAsync(currentUserId);
            if (!requests.Any()) return new List<FriendRequestViewModel>();

            var senderIds = requests.Select(r => r.SenderId).Distinct().ToList();
            var senders = await _accountService.GetUsersByIdsAsync(senderIds);

            return requests.Select(req =>
            {
                var usr = senders.FirstOrDefault(u => u.Id == req.SenderId);
                return new FriendRequestViewModel
                {
                    Id = req.Id,
                    RequestingUserId = usr?.Id,
                    RequestingUserName = usr?.UserName,
                    RequestingUserProfilePictureUrl = usr?.ProfilePictureUrl,
                    Created = req.CreatedAt
                };
            }).ToList();
        }

        public async Task<List<FriendRequestViewModel>> GetSentFriendRequests()
        {
            var currentUserId = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(currentUserId)) return new List<FriendRequestViewModel>();

            var requests = await _friendRequestRepository.GetAllBySenderIdAsync(currentUserId);
            if (!requests.Any()) return new List<FriendRequestViewModel>();

            var receiverIds = requests.Select(r => r.ReceiverId).Distinct().ToList();
            var receivers = await _accountService.GetUsersByIdsAsync(receiverIds);

            return requests.Select(req =>
            {
                var usr = receivers.FirstOrDefault(u => u.Id == req.ReceiverId);
                return new FriendRequestViewModel
                {
                    Id = req.Id,
                    RequestingUserId = usr?.Id,
                    RequestingUserName = usr?.UserName,
                    RequestingUserProfilePictureUrl = usr?.ProfilePictureUrl,
                    Status = req.Status.ToString(),
                    Created = req.CreatedAt
                };
            }).ToList();
        }

        public async Task<List<FriendViewModel>> GetPotentialFriends(string userNameQuery)
        {
            var currentUserId = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(currentUserId)) return new List<FriendViewModel>();

            var allUsers = await _accountService.GetAllUsersAsync();
            var friendships = await _friendshipRepository.GetAllByUserIdAsync(currentUserId);
            var friendIds = new HashSet<string>(friendships.Select(f => f.UserId == currentUserId ? f.FriendId : f.UserId));
            var sentRequests = await _friendRequestRepository.GetAllBySenderIdAsync(currentUserId);
            var recRequests = await _friendRequestRepository.GetAllByReceiverIdAsync(currentUserId);
            var pendingUserIds = new HashSet<string>(sentRequests.Select(r => r.ReceiverId)
                                                               .Union(recRequests.Select(r => r.SenderId)));

            var potential = allUsers.Where(u =>
                u.Id != currentUserId
                && !friendIds.Contains(u.Id)
                && !pendingUserIds.Contains(u.Id));

            if (!string.IsNullOrEmpty(userNameQuery))
                potential = potential.Where(u => u.UserName.ToLower().Contains(userNameQuery.ToLower()));

            return _mapper.Map<List<FriendViewModel>>(potential);
        }

        public async Task<List<PostViewModel>> GetFriendsPostsAsync()
        {
            
            var allPosts = await _postService.GetAllViewModel();

            
            var currentUserId = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            var friendships = await _friendshipRepository.GetAllByUserIdAsync(currentUserId);
            var friendIds = friendships
                .Select(f => f.UserId == currentUserId ? f.FriendId : f.UserId)
                .Distinct()
                .ToList();

            var friendPosts = allPosts
                .Where(p => friendIds.Contains(p.UserId))
                .OrderByDescending(p => p.CreatedAt)
                .ToList();

            return friendPosts;
        }

        public async Task<List<PostViewModel>> GetPostsByFriendAsync(string friendUserId)
        {
            
            var allPosts = await _postService.GetAllViewModel();

            
            var friendPosts = allPosts
                .Where(p => p.UserId == friendUserId)
                .OrderByDescending(p => p.CreatedAt)
                .ToList();

            return friendPosts;
        }
    }
}
