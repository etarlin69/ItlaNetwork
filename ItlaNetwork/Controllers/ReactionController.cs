// Location: ItlaNetwork/Controllers/ReactionController.cs

using ItlaNetwork.Core.Application.Interfaces.Services;
using ItlaNetwork.Core.Application.ViewModels.Reaction;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ItlaNetwork.Controllers
{
    [Authorize] // Ensures only authenticated users can access this controller
    public class ReactionController : Controller
    {
        private readonly IReactionService _reactionService;

        public ReactionController(IReactionService reactionService)
        {
            _reactionService = reactionService;
        }

        [HttpPost]
        public async Task<IActionResult> ToggleReaction(SaveReactionViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { success = false, message = "Invalid data." });
            }

            // The service now handles getting the user's ID and returns all the data we need.
            var response = await _reactionService.ToggleReactionAsync(vm);

            if (response == null)
            {
                return Unauthorized(); // Or another appropriate error
            }

            // Return the updated counts and user reaction status as JSON.
            return Json(response);
        }
    }
}
