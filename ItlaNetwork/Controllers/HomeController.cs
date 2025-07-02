using ItlaNetwork.Core.Application.DTOs.Account; // Necesario para AuthenticationResponse
using ItlaNetwork.Core.Application.Interfaces.Services;
using ItlaNetwork.Core.Application.ViewModels.Home;
using ItlaNetwork.Extensions; // Necesario para la extensi�n de sesi�n
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http; // Necesario para HttpContext.Session
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ItlaNetwork.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly IPostService _postService;

        public HomeController(IPostService postService)
        {
            _postService = postService;
        }

        public async Task<IActionResult> Index()
        {
            // --- L�GICA ACTUALIZADA ---
            // 1. Obtener el usuario de la sesi�n.
            var user = HttpContext.Session.Get<AuthenticationResponse>("user");

            // 2. Obtener los posts.
            var posts = await _postService.GetAllViewModel();

            // 3. Crear el HomeViewModel y pasarle los datos necesarios.
            var homeVm = new HomeViewModel
            {
                Posts = posts,
                NewPost = new(),
                CurrentUserName = user?.UserName ?? "Usuario" // Pasamos el nombre de usuario
            };

            return View(homeVm);
        }
    }
}