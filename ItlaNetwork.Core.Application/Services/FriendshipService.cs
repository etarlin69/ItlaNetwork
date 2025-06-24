using AutoMapper;
using ItlaNetwork.Core.Application.Interfaces.Repositories;
using ItlaNetwork.Core.Application.Interfaces.Services;
using ItlaNetwork.Core.Application.ViewModels.Friendship;
using ItlaNetwork.Core.Domain.Entities;
using ItlaNetwork.Core.Domain.Enums;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ItlaNetwork.Core.Application.Services
{
    public class FriendshipService : IFriendshipService
    {
        private readonly IGenericRepository<Friendship> _friendshipRepository;
        private readonly IGenericRepository<FriendRequest> _friendRequestRepository;
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;

        public FriendshipService(IGenericRepository<Friendship> fr, IGenericRepository<FriendRequest> frq, UserManager<User> um, IMapper mapper)
        {
            _friendshipRepository = fr;
            _friendRequestRepository = frq;
            _userManager = um;
            _mapper = mapper;
        }

        // --- MÉTODO ACCEPTFRIENDREQUESTASYNC CORREGIDO ---
        public async Task AcceptFriendRequestAsync(string requestingUserId, string receiverUserId)
        {
            // Buscamos directamente en la BD con el nuevo método
            var request = await _friendRequestRepository.FindFirstOrDefaultAsync(r =>
                r.SenderId == requestingUserId &&
                r.ReceiverId == receiverUserId &&
                r.Status == FriendRequestStatus.Pending);

            if (request == null) return; // Si no se encuentra, no hacer nada

            // 1. Crear la amistad en ambas direcciones
            Friendship friendship1 = new() { UserId = request.SenderId, FriendId = request.ReceiverId };
            Friendship friendship2 = new() { UserId = request.ReceiverId, FriendId = request.SenderId };
            await _friendshipRepository.AddAsync(friendship1);
            await _friendshipRepository.AddAsync(friendship2);

            // 2. Eliminar la solicitud ya procesada
            await _friendRequestRepository.DeleteAsync(request);
        }

        // --- MÉTODO REJECTFRIENDREQUESTASYNC CORREGIDO ---
        public async Task RejectFriendRequestAsync(string requestingUserId, string receiverUserId)
        {
            var request = await _friendRequestRepository.FindFirstOrDefaultAsync(r =>
                r.SenderId == requestingUserId &&
                r.ReceiverId == receiverUserId &&
                r.Status == FriendRequestStatus.Pending);

            if (request == null) return;

            // Simplemente se elimina la solicitud
            await _friendRequestRepository.DeleteAsync(request);
        }

        // El resto de los métodos permanecen igual...
        public async Task AddFriendByIdAsync(string senderUserId, string receiverUserId)
        {
            if (senderUserId == receiverUserId) return;
            FriendRequest request = new() { SenderId = senderUserId, ReceiverId = receiverUserId, Status = FriendRequestStatus.Pending };
            await _friendRequestRepository.AddAsync(request);
        }

        public async Task<List<FriendViewModel>> GetAllFriends(string userId)
        {
            var friendships = await _friendshipRepository.GetAllWithIncludeAsync(new List<string> { "Friend" });
            return _mapper.Map<List<FriendViewModel>>(friendships.Where(f => f.UserId == userId));
        }

        public async Task<List<FriendRequestViewModel>> GetAllFriendRequests(string userId)
        {
            var requests = await _friendRequestRepository.GetAllWithIncludeAsync(new List<string> { "Sender" });
            return _mapper.Map<List<FriendRequestViewModel>>(requests.Where(r => r.ReceiverId == userId && r.Status == FriendRequestStatus.Pending));
        }

        public async Task<List<FriendViewModel>> GetAllUsers(string userId, string? userName)
        {
            var allUsers = _userManager.Users.ToList();
            var friendships = await _friendshipRepository.GetAllAsync();
            var friendIds = friendships.Where(f => f.UserId == userId).Select(f => f.FriendId).ToList();
            var requests = await _friendRequestRepository.GetAllAsync();
            var pendingRequestUserIds = requests.Where(r => r.Status == FriendRequestStatus.Pending && (r.SenderId == userId || r.ReceiverId == userId)).Select(r => r.SenderId == userId ? r.ReceiverId : r.SenderId).ToList();
            var potentialFriends = allUsers.Where(u => u.Id != userId && !friendIds.Contains(u.Id) && !pendingRequestUserIds.Contains(u.Id));
            if (!string.IsNullOrEmpty(userName)) { potentialFriends = potentialFriends.Where(u => u.UserName.ToLower().Contains(userName.ToLower())); }
            return potentialFriends.Select(u => new FriendViewModel { UserId = u.Id, FirstName = u.FirstName, LastName = u.LastName, UserName = u.UserName, ProfilePictureUrl = u.ProfilePictureUrl }).ToList();
        }

        public async Task DeleteFriendAsync(int friendshipId, string userId)
        {
            var friendship1 = await _friendshipRepository.GetByIdAsync(friendshipId);
            if (friendship1 == null) return;
            var friendships = await _friendshipRepository.GetAllAsync();
            var friendship2 = friendships.FirstOrDefault(f => f.UserId == friendship1.FriendId && f.FriendId == userId);
            await _friendshipRepository.DeleteAsync(friendship1);
            if (friendship2 != null) { await _friendshipRepository.DeleteAsync(friendship2); }
        }

        public async Task<List<FriendRequestViewModel>> GetSentFriendRequests(string userId)
        {
            var requests = await _friendRequestRepository.GetAllWithIncludeAsync(new List<string> { "Receiver" });
            return requests.Where(r => r.SenderId == userId).Select(r => new FriendRequestViewModel { Id = r.Id, RequestingUserName = r.Receiver.UserName, RequestingUserProfilePictureUrl = r.Receiver.ProfilePictureUrl, Status = r.Status.ToString(), Created = r.CreatedAt }).ToList();
        }

        public async Task DeleteFriendRequestAsync(int requestId)
        {
            var request = await _friendRequestRepository.GetByIdAsync(requestId);
            if (request != null) { await _friendRequestRepository.DeleteAsync(request); }
        }
    }
}