using ItlaNetwork.Core.Application.Enums;
using ItlaNetwork.Core.Application.Interfaces.Services;
using ItlaNetwork.Core.Application.ViewModels.Reaction;
using ItlaNetwork.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace ItlaNetwork.Controllers
{
    [Authorize]
    public class ReactionController : Controller
    {
        private readonly IReactionService _reactionService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ReactionController(IReactionService reactionService, IHttpContextAccessor httpContextAccessor)
        {
            _reactionService = reactionService;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpPost]
        public async Task<IActionResult> ToggleReaction(SaveReactionViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            string userId = _httpContextAccessor.HttpContext.User.GetId();
            await _reactionService.ToggleReactionAsync(vm, userId);

            // Después de procesar la reacción, obtenemos los nuevos totales.
            var reactions = await _reactionService.GetReactionsByPostIdAsync(vm.PostId);

            // Creamos un objeto anónimo con toda la información que el frontend necesita.
            var response = new
            {
                likeCount = reactions.Count(r => r.ReactionType == (int)ReactionType.Like),
                dislikeCount = reactions.Count(r => r.ReactionType == (int)ReactionType.Dislike),
                // Devolvemos el tipo de reacción del usuario actual como un string para que JS lo entienda fácil.
                currentUserReaction = reactions.FirstOrDefault(r => r.UserId == userId)?.ReactionType.ToString()
            };

            // Devolvemos los datos en formato JSON.
            return Json(response);
        }
    }
}