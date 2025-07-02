using ItlaNetwork.Core.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ItlaNetwork.Controllers
{
    [Authorize] // Ensures only authenticated users can access this controller
    public class FriendController : Controller
    {
        private readonly IFriendshipService _friendshipService;

        public FriendController(IFriendshipService friendshipService)
        {
            _friendshipService = friendshipService;
        }

        // Displays the list of current friends
        public async Task<IActionResult> Index()
        {
            // The service now gets the user ID internally.
            var friends = await _friendshipService.GetAllFriends();
            return View(friends);
        }

        // Displays the view to find and add new friends
        public async Task<IActionResult> Add(string userNameQuery)
        {
            var potentialFriends = await _friendshipService.GetPotentialFriends(userNameQuery);
            ViewBag.UserNameQuery = userNameQuery; // To keep the search term in the input box
            return View("AddFriend", potentialFriends);
        }

        [HttpPost]
        public async Task<IActionResult> SendRequest(string receiverUserId)
        {
            // The service handles getting the sender's ID internally.
            await _friendshipService.SendFriendRequestAsync(receiverUserId);
            return RedirectToAction("Add");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string friendId)
        {
            // The service handles getting the current user's ID to find the correct friendship to delete.
            await _friendshipService.DeleteFriendAsync(friendId);
            return RedirectToAction("Index");
        }
    }
}
