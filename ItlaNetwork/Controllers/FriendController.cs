using ItlaNetwork.Core.Application.Interfaces.Services;
using ItlaNetwork.Extensions;
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

        public async Task<IActionResult> Index()
        {
            var userId = User.GetId();
            var friends = await _friendshipService.GetAllFriends(userId);
            return View(friends);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var userId = User.GetId();
            await _friendshipService.DeleteFriendAsync(id, userId);
            return RedirectToAction("Index");
        }
    }
}