using ItlaNetwork.Core.Application.Interfaces.Services;
using ItlaNetwork.Core.Application.ViewModels.Home;
using ItlaNetwork.Core.Application.ViewModels.Post;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ItlaNetwork.Controllers
{
    [Authorize]
    public class PostController : Controller
    {
        private readonly IPostService _postService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public PostController(IPostService postService, IHttpContextAccessor httpContextAccessor)
        {
            _postService = postService;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SavePostViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                var homeVm = new HomeViewModel
                {
                    Posts = await _postService.GetAllViewModel(),
                    NewPost = vm
                };
                return View("~/Views/Home/Index.cshtml", homeVm);
            }

            if (vm.ImageFile != null)
            {
                vm.ImageUrl = UploadImage(vm.ImageFile);
            }

            await _postService.Add(vm);
            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> Edit(int id)
        {
            var postVm = await _postService.GetByIdSaveViewModel(id);
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // Verificación de autorización con base en el post recuperado
            var postOwnerId = await _postService.GetPostOwnerIdById(id);
            if (postOwnerId != currentUserId)
            {
                return Unauthorized();
            }

            return View("SavePost", postVm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(SavePostViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return View("SavePost", vm);
            }

            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var postOwnerId = await _postService.GetPostOwnerIdById(vm.Id);

            if (postOwnerId != currentUserId)
            {
                return Unauthorized();
            }

            if (vm.ImageFile != null)
            {
                vm.ImageUrl = UploadImage(vm.ImageFile);
            }
            else
            {
                var existing = await _postService.GetByIdSaveViewModel(vm.Id);
                vm.ImageUrl = existing.ImageUrl;
            }

            await _postService.Update(vm);
            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> Delete(int id)
        {
            var postVm = await _postService.GetByIdSaveViewModel(id);
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var postOwnerId = await _postService.GetPostOwnerIdById(id);

            if (postOwnerId != currentUserId)
            {
                return Unauthorized();
            }

            return View("DeletePost", postVm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeletePost(int id)
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var postOwnerId = await _postService.GetPostOwnerIdById(id);

            if (postOwnerId != currentUserId)
            {
                return Unauthorized();
            }

            await _postService.Delete(id);
            return RedirectToAction("Index", "Home");
        }

        private string UploadImage(IFormFile file)
        {
            var userId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId)) return null;

            string basePath = $"/Images/Posts/{userId}";
            string path = Path.Combine(Directory.GetCurrentDirectory(), $"wwwroot{basePath}");

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            Guid guid = Guid.NewGuid();
            string fileName = guid + Path.GetExtension(file.FileName);
            string finalPath = Path.Combine(path, fileName);

            using (var stream = new FileStream(finalPath, FileMode.Create))
            {
                file.CopyTo(stream);
            }

            return $"{basePath}/{fileName}";
        }
    }
}
