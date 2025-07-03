// Ubicación: ItlaNetwork/Controllers/AccountController.cs

using AutoMapper;
using ItlaNetwork.Core.Application.DTOs.Account;
using ItlaNetwork.Core.Application.Interfaces.Services;
using ItlaNetwork.Core.Application.ViewModels.Account;
using ItlaNetwork.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ItlaNetwork.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;
        private readonly IMapper _mapper;

        public AccountController(IAccountService accountService, IMapper mapper)
        {
            _accountService = accountService;
            _mapper = mapper;
        }

        public IActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            return View(new LoginViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel vm)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }

            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            AuthenticationRequest request = _mapper.Map<AuthenticationRequest>(vm);
            AuthenticationResponse response = await _accountService.AuthenticateAsync(request);

            if (response.HasError)
            {
                ModelState.AddModelError("LoginError", response.Error);
                return View(vm);
            }

            HttpContext.Session.Set<AuthenticationResponse>("user", response);

            return RedirectToAction("Index", "Home");
        }

        public IActionResult Register()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            return View(new RegisterViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel vm)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }

            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            var request = _mapper.Map<RegisterRequest>(vm);
            var origin = Request.Headers["origin"];
            var response = await _accountService.RegisterUserAsync(request, origin);

            if (response.HasError)
            {
                ModelState.AddModelError("RegisterError", response.Error);
                return View(vm);
            }

            return RedirectToAction("ConfirmAccountInfo");
        }

        public async Task<IActionResult> Logout()
        {
            await _accountService.SignOutAsync();
            HttpContext.Session.Remove("user");
            return RedirectToAction("Login");
        }

        [HttpGet]
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Login");
            }

            string result = await _accountService.ConfirmEmailAsync(userId, token);
            return View("ConfirmEmail", result);
        }

        public IActionResult ConfirmAccountInfo()
        {
            return View();
        }
    }
}
