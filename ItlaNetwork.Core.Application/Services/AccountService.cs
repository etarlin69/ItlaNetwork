using AutoMapper;
using ItlaNetwork.Core.Application.DTOs.Account;
using ItlaNetwork.Core.Application.Interfaces.Services;
using ItlaNetwork.Core.Application.ViewModels.Account;
using ItlaNetwork.Core.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ItlaNetwork.Core.Application.Services
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IMapper _mapper;

        public AccountService(UserManager<User> userManager, SignInManager<User> signInManager, IMapper mapper)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _mapper = mapper;
        }

        public async Task<AuthenticationResponse> AuthenticateAsync(LoginViewModel vm)
        {
            AuthenticationResponse response = new();
            var user = await _userManager.FindByNameAsync(vm.UserName);
            if (user == null)
            {
                response.HasError = true;
                response.Error = $"No hay cuentas registradas con el nombre de usuario: {vm.UserName}";
                return response;
            }

            var result = await _signInManager.PasswordSignInAsync(user.UserName, vm.Password, false, lockoutOnFailure: false);
            if (!result.Succeeded)
            {
                response.HasError = true;
                response.Error = $"Las credenciales ingresadas para: {vm.UserName} son incorrectas";
                return response;
            }

            response.Id = user.Id;
            response.Email = user.Email;
            response.UserName = user.UserName;
            var rolesList = await _userManager.GetRolesAsync(user).ConfigureAwait(false);
            response.Roles = rolesList.ToList();
            response.IsVerified = user.EmailConfirmed;

            return response;
        }

        public async Task SignOutAsync()
        {
            await _signInManager.SignOutAsync();
        }

        public async Task<RegisterResponse> RegisterBasicUserAsync(RegisterViewModel vm)
        {
            RegisterResponse response = new() { HasError = false };
            var userWithSameUserName = await _userManager.FindByNameAsync(vm.UserName);
            if (userWithSameUserName != null)
            {
                response.HasError = true;
                response.Error = $"El nombre de usuario: '{vm.UserName}' ya esta en uso.";
                return response;
            }

            var userWithSameEmail = await _userManager.FindByEmailAsync(vm.Email);
            if (userWithSameEmail != null)
            {
                response.HasError = true;
                response.Error = $"La direccion de correo electronico: '{vm.Email}' ya esta registrada.";
                return response;
            }

            var user = _mapper.Map<User>(vm);
            var result = await _userManager.CreateAsync(user, vm.Password);
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "Basic");
            }
            else
            {
                // ===== SECCIÓN MODIFICADA =====
                response.HasError = true;
                // Se unen todos los errores de validación de Identity en un solo string.
                response.Error = string.Join(", ", result.Errors.Select(e => e.Description));
                // ============================
            }

            return response;
        }
    }
}