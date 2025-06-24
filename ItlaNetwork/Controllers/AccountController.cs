using ItlaNetwork.Core.Application.Interfaces.Services;
using ItlaNetwork.Core.Application.ViewModels.Account;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace WebApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        // Muestra la vista de Login
        public IActionResult Login()
        {
            // Si el usuario ya está autenticado, lo redirige al Home.
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            return View(new LoginViewModel());
        }

        // Procesa la petición de Login
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

            var response = await _accountService.AuthenticateAsync(vm);
            if (response.HasError)
            {
                vm.HasError = response.HasError;
                vm.Error = response.Error;
                return View(vm);
            }

            // Si el login es exitoso, redirige al Home.
            // El servicio ya se encargó de crear la cookie de sesión.
            return RedirectToAction("Index", "Home");
        }

        // Muestra la vista de Registro
        public IActionResult Register()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            return View(new RegisterViewModel());
        }

        // Procesa la petición de Registro
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

            var response = await _accountService.RegisterBasicUserAsync(vm);
            if (response.HasError)
            {
                vm.HasError = response.HasError;
                vm.Error = response.Error;
                return View(vm);
            }

            // Si el registro es exitoso, redirige al Login para que el usuario inicie sesión.
            return RedirectToAction("Login");
        }

        // Procesa el Logout
        public async Task<IActionResult> Logout()
        {
            await _accountService.SignOutAsync();
            return RedirectToAction("Login");
        }

    }
}