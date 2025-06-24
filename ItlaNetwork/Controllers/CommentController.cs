using ItlaNetwork.Core.Application.Interfaces.Services;
using ItlaNetwork.Core.Application.ViewModels.Comment;
using ItlaNetwork.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ItlaNetwork.Controllers
{
    [Authorize]
    public class CommentController : Controller
    {
        private readonly ICommentService _commentService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CommentController(ICommentService commentService, IHttpContextAccessor httpContextAccessor)
        {
            _commentService = commentService;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpPost]
        public async Task<IActionResult> Create(SaveCommentViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                // Este error ahora será atrapado por el JavaScript.
                return BadRequest();
            }

            string userId = _httpContextAccessor.HttpContext.User.GetId();

            // El servicio ahora nos devuelve el CommentViewModel listo para mostrar.
            CommentViewModel newComment = await _commentService.Add(vm, userId);

            if (newComment == null)
            {
                return BadRequest();
            }

            // Devolvemos la vista parcial con el modelo del nuevo comentario.
            return PartialView("_CommentCard", newComment);
        }
    }
}