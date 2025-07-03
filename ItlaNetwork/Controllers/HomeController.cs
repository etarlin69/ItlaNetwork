using ItlaNetwork.Core.Application.Interfaces.Services;
using ItlaNetwork.Core.Application.ViewModels.Home;
using Microsoft.AspNetCore.Authorization;
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
            var posts = await _postService.GetAllViewModel();
            var homeVm = new HomeViewModel
            {
                Posts = posts,
                NewPost = new()
            };
            return View(homeVm);
        }
    }
}