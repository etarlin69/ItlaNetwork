using ItlaNetwork.Core.Application.Interfaces.Services;
using ItlaNetwork.Core.Application.ViewModels.Friendship;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ItlaNetwork.Controllers
{
    [Authorize]
    public class FriendController : Controller
    {
        private readonly IFriendshipService _friendshipService;

        public FriendController(IFriendshipService friendshipService)
        {
            _friendshipService = friendshipService;
        }

        
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var vm = new FriendFeedViewModel
            {
                Friends = await _friendshipService.GetAllFriends(),
                Posts = await _friendshipService.GetFriendsPostsAsync()
            };
            return View(vm);
        }

        
        [HttpGet]
        public async Task<IActionResult> ByUser(string userId)
        {
            var vm = new FriendFeedViewModel
            {
                Friends = await _friendshipService.GetAllFriends(),
                Posts = await _friendshipService.GetPostsByFriendAsync(userId)
            };
            return View("Index", vm);
        }

        
        [HttpGet]
        public async Task<IActionResult> Add(string userNameQuery)
        {
            var users = await _friendshipService.GetPotentialFriends(userNameQuery);
            var vm = new AddFriendViewModel
            {
                Users = users,
                UserName = userNameQuery
            };
            
            return View("~/Views/FriendRequest/AddFriend.cshtml", vm);
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SendRequest(AddFriendViewModel vm)
        {
            if (string.IsNullOrEmpty(vm.SelectedUserId))
            {
                TempData["Error"] = "Debe seleccionar un usuario para enviar la solicitud.";
                
                return RedirectToAction(nameof(Add), new { userNameQuery = vm.UserName });
            }

            await _friendshipService.SendFriendRequestAsync(vm.SelectedUserId);
            TempData["Success"] = "¡Solicitud de amistad enviada exitosamente!";
            return RedirectToAction(nameof(Index));
        }

       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(string friendId)
        {
            if (!string.IsNullOrEmpty(friendId))
                await _friendshipService.DeleteFriendAsync(friendId);

            return RedirectToAction(nameof(Index));
        }
    }
}
