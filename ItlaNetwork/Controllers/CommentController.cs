using ItlaNetwork.Core.Application.Interfaces.Services;
using ItlaNetwork.Core.Application.ViewModels.Comment;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ItlaNetwork.Controllers
{
    [Authorize]
    public class CommentController : Controller
    {
        private readonly ICommentService _commentService;

        public CommentController(ICommentService commentService)
        {
            _commentService = commentService;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SaveCommentViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("El comentario no puede estar vacío.");
            }

            var newCommentViewModel = await _commentService.Add(vm);

            if (newCommentViewModel == null)
            {
                return BadRequest("No se pudo crear el comentario.");
            }

            return PartialView("_CommentCard", newCommentViewModel);
        }
    }
}