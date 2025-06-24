using ItlaNetwork.Core.Application.Interfaces.Services;
using ItlaNetwork.Core.Application.ViewModels.Home;
using ItlaNetwork.Extensions; // El controlador SÍ PUEDE usar esta extensión
using ItlaNetwork.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Threading.Tasks;

namespace ItlaNetwork.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly IPostService _postService;
        private readonly ILogger<HomeController> _logger;

        // Ya no necesita IHttpContextAccessor aquí porque usamos la propiedad User
        public HomeController(IPostService postService, ILogger<HomeController> logger)
        {
            _postService = postService;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            // 1. El controlador obtiene el ID del usuario de la sesión.
            var userId = User.GetId();

            // 2. Llama al servicio y le PASA el ID del usuario como parámetro.
            var posts = await _postService.GetAllViewModel(userId);

            var homeVm = new HomeViewModel
            {
                Posts = posts,
                NewPost = new()
            };

            return View(homeVm);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}