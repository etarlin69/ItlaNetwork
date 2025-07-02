using AutoMapper;
using ItlaNetwork.Core.Application.Interfaces.Services;
using ItlaNetwork.Core.Application.ViewModels.Comment;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ItlaNetwork.Controllers
{
    [Authorize]
    public class CommentController : Controller
    {
        private readonly ICommentService _commentService;
        private readonly IPostService _postService; // Necesario para la comprobación de autorización
        private readonly IMapper _mapper;

        public CommentController(ICommentService commentService, IPostService postService, IMapper mapper)
        {
            _commentService = commentService;
            _postService = postService;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<IActionResult> Create(SaveCommentViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Index", "Home");
            }

            var newComment = await _commentService.Add(vm);

            if (newComment == null)
            {
                return RedirectToAction("Index", "Home");
            }

            return PartialView("_CommentCard", newComment);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var commentVm = await _commentService.GetByIdSaveViewModel(id);
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // Esta comprobación ahora funciona porque SavePostViewModel tiene UserId.
            var post = await _postService.GetByIdSaveViewModel(commentVm.PostId);
            if (post.UserId != currentUserId)
            {
                return Unauthorized();
            }

            return View("SaveComment", commentVm);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(SaveCommentViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return View("SaveComment", vm);
            }

            var commentToEdit = await _commentService.GetByIdSaveViewModel(vm.Id);
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var post = await _postService.GetByIdSaveViewModel(commentToEdit.PostId);
            if (post.UserId != currentUserId)
            {
                return Unauthorized();
            }

            await _commentService.Update(vm);
            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> Delete(int id)
        {
            var commentVm = await _commentService.GetByIdSaveViewModel(id);
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var post = await _postService.GetByIdSaveViewModel(commentVm.PostId);
            if (post.UserId != currentUserId)
            {
                return Unauthorized();
            }

            return View("DeleteComment", commentVm);
        }

        [HttpPost]
        public async Task<IActionResult> DeletePost(int id)
        {
            var commentToDelete = await _commentService.GetByIdSaveViewModel(id);
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var post = await _postService.GetByIdSaveViewModel(commentToDelete.PostId);
            if (post.UserId != currentUserId)
            {
                return Unauthorized();
            }

            await _commentService.Delete(id);
            return RedirectToAction("Index", "Home");
        }
    }
}