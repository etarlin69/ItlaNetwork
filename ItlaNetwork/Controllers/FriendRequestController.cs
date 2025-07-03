using ItlaNetwork.Core.Application.Interfaces.Services;
using ItlaNetwork.Core.Application.ViewModels.Friendship;
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
            var vm = new FriendRequestPageViewModel
            {
                ReceivedRequests = await _friendshipService.GetPendingFriendRequests(),
                SentRequests = await _friendshipService.GetSentFriendRequests()
            };
            return View(vm);
        }

        [HttpGet]
        public async Task<IActionResult> AddFriend(string userNameQuery)
        {
            var vm = new AddFriendViewModel
            {
                Users = await _friendshipService.GetPotentialFriends(userNameQuery),
                UserName = userNameQuery
            };
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> AddFriend(AddFriendViewModel vm)
        {
            if (string.IsNullOrEmpty(vm.SelectedUserId))
            {
                TempData["Error"] = "Debe seleccionar un usuario para enviar la solicitud.";
                return RedirectToAction("AddFriend", new { userNameQuery = vm.UserName });
            }

            await _friendshipService.SendFriendRequestAsync(vm.SelectedUserId);
            TempData["Success"] = "¡Solicitud de amistad enviada exitosamente!";
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Accept(int id)
        {
            await _friendshipService.AcceptFriendRequestAsync(id);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Reject(int id)
        {
            await _friendshipService.RejectFriendRequestAsync(id);
            return RedirectToAction("Index");
        }
    }
}