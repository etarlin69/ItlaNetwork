using System;
using System.IO;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using ItlaNetwork.Core.Application.DTOs.Account;
using ItlaNetwork.Core.Application.Interfaces.Services;
using ItlaNetwork.Core.Application.ViewModels.Account;
using ItlaNetwork.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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

        [HttpGet]
        public IActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
                return RedirectToAction("Index", "Home");
            return View(new LoginViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel vm)
        {
            if (User.Identity.IsAuthenticated)
                return RedirectToAction("Index", "Home");
            if (!ModelState.IsValid)
                return View(vm);

            var authReq = _mapper.Map<AuthenticationRequest>(vm);
            var resp = await _accountService.AuthenticateAsync(authReq);
            if (resp.HasError)
            {
                ModelState.AddModelError("", resp.Error);
                return View(vm);
            }

            HttpContext.Session.Set("user", resp);
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult Register()
        {
            if (User.Identity.IsAuthenticated)
                return RedirectToAction("Index", "Home");
            return View(new RegisterViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel vm)
        {
            if (User.Identity.IsAuthenticated)
                return RedirectToAction("Index", "Home");
            if (!ModelState.IsValid)
                return View(vm);

            var req = _mapper.Map<RegisterRequest>(vm);
            var origin = Request.Headers["origin"].ToString();
            var res = await _accountService.RegisterUserAsync(req, origin);
            if (res.HasError)
            {
                ModelState.AddModelError("", res.Error);
                return View(vm);
            }
            return RedirectToAction("ConfirmAccountInfo");
        }

        [Authorize]
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
                return RedirectToAction("Login");

            var msg = await _accountService.ConfirmEmailAsync(userId, token);
            return View("ConfirmEmail", model: msg);
        }

        public IActionResult ConfirmAccountInfo() => View();

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Profile()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _accountService.GetUserByIdAsync(userId);

            var vm = new ProfileViewModel
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Phone = user.Phone,
                ProfileImageUrl = user.ProfilePictureUrl
            };
            return View(vm);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Profile(ProfileViewModel vm, IFormFile imageFile)
        {
            if (!ModelState.IsValid)
                return View(vm);

            
            if (imageFile != null && imageFile.Length > 0)
            {
                var webRoot = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
                var dir = Path.Combine(webRoot, "images", "profiles", vm.Id);
                Directory.CreateDirectory(dir);

                var fileName = $"{Guid.NewGuid()}{Path.GetExtension(imageFile.FileName)}";
                var fullPath = Path.Combine(dir, fileName);
                using var fs = new FileStream(fullPath, FileMode.Create);
                await imageFile.CopyToAsync(fs);

                vm.ProfileImageUrl = $"/images/profiles/{vm.Id}/{fileName}";
            }

            await _accountService.UpdateProfileAsync(vm);

            
            var auth = HttpContext.Session.Get<AuthenticationResponse>("user");
            if (!string.IsNullOrWhiteSpace(vm.ProfileImageUrl))
                auth.ProfilePictureUrl = vm.ProfileImageUrl;
            HttpContext.Session.Set("user", auth);

            TempData["Success"] = "Perfil actualizado correctamente.";
            return RedirectToAction("Profile");
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel vm)
        {
            if (!ModelState.IsValid)
                return PartialView("_ChangePasswordForm", vm);

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var resp = await _accountService.ChangePasswordAsync(userId, vm);
            if (resp.HasError)
            {
                ModelState.AddModelError("", resp.Error);
                return PartialView("_ChangePasswordForm", vm);
            }

            TempData["PwdSuccess"] = "Contraseña cambiada correctamente.";
            return RedirectToAction("Profile");
        }
    }
}
