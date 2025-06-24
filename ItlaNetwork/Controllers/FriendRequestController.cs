using ItlaNetwork.Core.Application.Interfaces.Services;
using ItlaNetwork.Core.Application.ViewModels.Friendship;
using ItlaNetwork.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ItlaNetwork.Controllers
{
    [Authorize]
    public class FriendRequestController : Controller
    {
        private readonly IFriendshipService _friendshipService;

        public FriendRequestController(IFriendshipService friendshipService)
        {
            _friendshipService = friendshipService;
        }

        public async Task<IActionResult> Index()
        {
            var userId = User.GetId();
            var vm = new FriendRequestPageViewModel
            {
                ReceivedRequests = await _friendshipService.GetAllFriendRequests(userId),
                SentRequests = await _friendshipService.GetSentFriendRequests(userId)
            };
            return View(vm);
        }

        public async Task<IActionResult> AddFriend(string? userName)
        {
            var userId = User.GetId();
            var vm = new AddFriendViewModel
            {
                Users = await _friendshipService.GetAllUsers(userId, userName),
                UserName = userName
            };
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> AddFriend(AddFriendViewModel vm)
        {
            if (string.IsNullOrEmpty(vm.SelectedUserId))
            {
                TempData["error"] = "Debe seleccionar un usuario para enviar la solicitud.";
                return RedirectToAction("AddFriend", new { userName = vm.UserName });
            }

            var senderUserId = User.GetId();
            await _friendshipService.AddFriendByIdAsync(senderUserId, vm.SelectedUserId);

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Accept(string requestingUserId)
        {
            var receiverUserId = User.GetId();
            await _friendshipService.AcceptFriendRequestAsync(requestingUserId, receiverUserId);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Reject(string requestingUserId)
        {
            var receiverUserId = User.GetId();
            await _friendshipService.RejectFriendRequestAsync(requestingUserId, receiverUserId);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            await _friendshipService.DeleteFriendRequestAsync(id);
            return RedirectToAction("Index");
        }
    }
}