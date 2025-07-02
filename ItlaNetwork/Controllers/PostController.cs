using System;
using System.IO;
using System.Security.Claims;
using System.Threading.Tasks;
using ItlaNetwork.Core.Application.Interfaces.Services;
using ItlaNetwork.Core.Application.ViewModels.Home;
using ItlaNetwork.Core.Application.ViewModels.Post;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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

        // POST: /Post/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SavePostViewModel vm)
        {
            // Si la validación del modelo falla, volvemos a la vista Home con los errores
            if (!ModelState.IsValid)
            {
                var homeVm = new HomeViewModel
                {
                    Posts = await _postService.GetAllViewModel(),
                    NewPost = vm
                };
                return View("~/Views/Home/Index.cshtml", homeVm);
            }

            try
            {
                // Si hay fichero de imagen, lo subimos y guardamos la URL en el VM
                if (vm.ImageFile != null)
                {
                    vm.ImageUrl = UploadImage(vm.ImageFile);
                }

                // Agregamos el nuevo post
                await _postService.Add(vm);

                // En caso de éxito, redirigimos al listado
                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                // Capturamos la excepción y la añadimos al ModelState
                ModelState.AddModelError(string.Empty, ex.Message);

                // Reenviamos a la vista Home con el VM para mostrar el error
                var homeVm = new HomeViewModel
                {
                    Posts = await _postService.GetAllViewModel(),
                    NewPost = vm
                };
                return View("~/Views/Home/Index.cshtml", homeVm);
            }
        }

        // GET: /Post/Edit/{id}
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var vm = await _postService.GetByIdSaveViewModel(id);
            if (vm == null)
                return NotFound();

            return View(vm);
        }

        // POST: /Post/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(SavePostViewModel vm)
        {
            if (!ModelState.IsValid)
                return View(vm);

            try
            {
                if (vm.ImageFile != null)
                {
                    vm.ImageUrl = UploadImage(vm.ImageFile);
                }

                await _postService.Update(vm);
                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(vm);
            }
        }

        // POST: /Post/Delete/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _postService.Delete(id);
            }
            catch (Exception ex)
            {
                // Guardamos el error en TempData para mostrarlo en Home (o donde prefieras)
                TempData["ErrorMessage"] = ex.Message;
            }

            return RedirectToAction("Index", "Home");
        }

        /// <summary>
        /// Sube la imagen al servidor (wwwroot) organizándola por usuario.
        /// </summary>
        private string UploadImage(IFormFile file)
        {
            var userId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            string basePath = $"/Images/Posts/{userId}";
            string path = Path.Combine(Directory.GetCurrentDirectory(), $"wwwroot{basePath}");

            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            var guid = Guid.NewGuid();
            var fileInfo = new FileInfo(file.FileName);
            var fileName = guid + fileInfo.Extension;
            var finalPath = Path.Combine(path, fileName);

            using (var stream = new FileStream(finalPath, FileMode.Create))
            {
                file.CopyTo(stream);
            }

            return $"{basePath}/{fileName}";
        }
    }
}
