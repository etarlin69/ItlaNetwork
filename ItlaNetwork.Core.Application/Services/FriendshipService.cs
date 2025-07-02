using AutoMapper;
using ItlaNetwork.Core.Application.Interfaces.Repositories;
using ItlaNetwork.Core.Application.Interfaces.Services;
using ItlaNetwork.Core.Application.ViewModels.Friendship;
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
        private readonly IAccountService _accountService; // Para obtener los datos de todos los usuarios
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public FriendshipService(
            IFriendshipRepository friendshipRepository,
            IFriendRequestRepository friendRequestRepository,
            IAccountService accountService,
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor)
        {
            _friendshipRepository = friendshipRepository;
            _friendRequestRepository = friendRequestRepository;
            _accountService = accountService;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task SendFriendRequestAsync(string receiverUserId)
        {
            var senderUserId = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(senderUserId) || senderUserId == receiverUserId) return;

            // Opcional: Verificar si ya existe una solicitud
            var existingRequest = await _friendRequestRepository.FindByUsersAsync(senderUserId, receiverUserId);
            if (existingRequest != null) return;

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

            var friendship1 = new Friendship { UserId = request.SenderId, FriendId = request.ReceiverId };
            var friendship2 = new Friendship { UserId = request.ReceiverId, FriendId = request.SenderId };

            await _friendshipRepository.AddAsync(friendship1);
            await _friendshipRepository.AddAsync(friendship2);
            await _friendRequestRepository.DeleteAsync(request);
        }

        public async Task RejectFriendRequestAsync(int requestId)
        {
            var request = await _friendRequestRepository.GetByIdAsync(requestId);
            if (request == null || request.Status != FriendRequestStatus.Pending) return;

            await _friendRequestRepository.DeleteAsync(request);
        }

        public async Task DeleteFriendAsync(string friendId)
        {
            var currentUserId = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(currentUserId)) return;

            var friendship1 = await _friendshipRepository.GetByUsersAsync(currentUserId, friendId);
            var friendship2 = await _friendshipRepository.GetByUsersAsync(friendId, currentUserId);

            if (friendship1 != null) await _friendshipRepository.DeleteAsync(friendship1);
            if (friendship2 != null) await _friendshipRepository.DeleteAsync(friendship2);
        }

        public async Task<List<FriendViewModel>> GetAllFriends()
        {
            var currentUserId = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(currentUserId)) return new List<FriendViewModel>();

            var friendships = await _friendshipRepository.GetAllByUserIdAsync(currentUserId);
            var friendIds = friendships.Select(f => f.UserId == currentUserId ? f.FriendId : f.UserId).Distinct().ToList();

            if (!friendIds.Any()) return new List<FriendViewModel>();

            var friendsData = await _accountService.GetUsersByIdsAsync(friendIds);

            return friendsData.Select(user => _mapper.Map<FriendViewModel>(user)).ToList();
        }

        public async Task<List<FriendRequestViewModel>> GetPendingFriendRequests()
        {
            var currentUserId = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(currentUserId)) return new List<FriendRequestViewModel>();

            var requests = await _friendRequestRepository.GetAllByReceiverIdAsync(currentUserId);
            if (!requests.Any()) return new List<FriendRequestViewModel>();

            var senderIds = requests.Select(r => r.SenderId).ToList();
            var sendersData = await _accountService.GetUsersByIdsAsync(senderIds);

            return requests.Select(req => {
                var sender = sendersData.FirstOrDefault(u => u.Id == req.SenderId);
                return new FriendRequestViewModel
                {
                    Id = req.Id,
                    RequestingUserId = sender?.Id,
                    RequestingUserName = sender?.UserName,
                    RequestingUserProfilePictureUrl = sender?.ProfilePictureUrl,
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

            var receiverIds = requests.Select(r => r.ReceiverId).ToList();
            var receiversData = await _accountService.GetUsersByIdsAsync(receiverIds);

            return requests.Select(req => {
                var receiver = receiversData.FirstOrDefault(u => u.Id == req.ReceiverId);
                return new FriendRequestViewModel
                {
                    Id = req.Id,
                    RequestingUserId = receiver?.Id, // Este es ahora el receptor
                    RequestingUserName = receiver?.UserName,
                    RequestingUserProfilePictureUrl = receiver?.ProfilePictureUrl,
                    Status = req.Status.ToString(),
                    Created = req.CreatedAt
                };
            }).ToList();
        }

        public async Task<List<FriendViewModel>> GetPotentialFriends(string userNameQuery)
        {
            var currentUserId = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(currentUserId)) return new List<FriendViewModel>();

            // 1. Obtener todos los usuarios
            var allUsers = await _accountService.GetAllUsersAsync();

            // 2. Obtener todas las amistades actuales
            var friendships = await _friendshipRepository.GetAllByUserIdAsync(currentUserId);
            var friendIds = new HashSet<string>(friendships.Select(f => f.UserId == currentUserId ? f.FriendId : f.UserId));

            // 3. Obtener todas las solicitudes pendientes (enviadas o recibidas)
            var sentRequests = await _friendRequestRepository.GetAllBySenderIdAsync(currentUserId);
            var receivedRequests = await _friendRequestRepository.GetAllByReceiverIdAsync(currentUserId);
            var pendingUserIds = new HashSet<string>(sentRequests.Select(r => r.ReceiverId).Union(receivedRequests.Select(r => r.SenderId)));

            // 4. Filtrar usuarios
            var potentialFriends = allUsers.Where(u =>
                u.Id != currentUserId &&
                !friendIds.Contains(u.Id) &&
                !pendingUserIds.Contains(u.Id)
            );

            // 5. Aplicar el filtro de búsqueda si se proporcionó
            if (!string.IsNullOrEmpty(userNameQuery))
            {
                potentialFriends = potentialFriends.Where(u => u.UserName.ToLower().Contains(userNameQuery.ToLower()));
            }

            return potentialFriends.Select(u => _mapper.Map<FriendViewModel>(u)).ToList();
        }
    }
}