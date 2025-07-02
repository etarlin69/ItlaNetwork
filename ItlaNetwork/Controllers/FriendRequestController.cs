using ItlaNetwork.Core.Application.Interfaces.Services;
using ItlaNetwork.Core.Application.ViewModels.Friendship;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ItlaNetwork.Controllers
{
    [Authorize] // Ensures only authenticated users can access this controller
    public class FriendRequestController : Controller
    {
        private readonly IFriendshipService _friendshipService;

        public FriendRequestController(IFriendshipService friendshipService)
        {
            _friendshipService = friendshipService;
        }

        // Displays both received and sent friend requests
        public async Task<IActionResult> Index()
        {
            var vm = new FriendRequestPageViewModel
            {
                ReceivedRequests = await _friendshipService.GetPendingFriendRequests(),
                SentRequests = await _friendshipService.GetSentFriendRequests()
            };
            return View(vm);
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

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            // This action is for deleting a request that the current user has sent.
            // We can reuse the RejectFriendRequestAsync logic as it simply deletes the request.
            await _friendshipService.RejectFriendRequestAsync(id);
            return RedirectToAction("Index");
        }
    }
}
