using ItlaNetwork.Core.Application.Interfaces.Services;
using ItlaNetwork.Core.Application.ViewModels.Post;
using ItlaNetwork.Extensions; // <-- Asegúrate que este using esté para User.GetId()
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Threading.Tasks;
using System;

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
        public async Task<IActionResult> Create(SavePostViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                TempData["error"] = "El contenido de la publicación no puede estar vacío.";
                return RedirectToAction("Index", "Home");
            }

            // Manejo de la subida de la imagen
            if (vm.ImageFile != null)
            {
                vm.ImageUrl = UploadImage(vm.ImageFile);
            }

            // Obtenemos el ID del usuario actual
            string userId = _httpContextAccessor.HttpContext.User.GetId();

            // Llamamos al servicio pasando el ViewModel y el ID del usuario
            await _postService.Add(vm, userId);

            return RedirectToAction("Index", "Home");
        }

        private string UploadImage(IFormFile file)
        {
            string basePath = $"/Images/Posts/{_httpContextAccessor.HttpContext.User.GetId()}";
            string path = Path.Combine(Directory.GetCurrentDirectory(), $"wwwroot{basePath}");

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            Guid guid = Guid.NewGuid();
            FileInfo fileInfo = new(file.FileName);
            string fileName = guid + fileInfo.Extension;
            string finalPath = Path.Combine(path, fileName);

            using (var stream = new FileStream(finalPath, FileMode.Create))
            {
                file.CopyTo(stream);
            }

            return $"{basePath}/{fileName}";
        }
    }
}