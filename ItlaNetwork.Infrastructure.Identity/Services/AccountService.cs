using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using ItlaNetwork.Core.Application.DTOs.Account;
using ItlaNetwork.Core.Application.DTOs.Email;
using ItlaNetwork.Core.Application.Interfaces.Services;
using ItlaNetwork.Core.Application.ViewModels.Account;
using ItlaNetwork.Core.Domain.Entities;
using ItlaNetwork.Infrastructure.Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;

namespace ItlaNetwork.Infrastructure.Identity.Services
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IEmailService _emailService;
        private readonly IMapper _mapper;

        public AccountService(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IEmailService emailService,
            IMapper mapper)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailService = emailService;
            _mapper = mapper;
        }

        public async Task<AuthenticationResponse> AuthenticateAsync(AuthenticationRequest request)
        {
            var response = new AuthenticationResponse();

           
            var user = await _userManager.FindByNameAsync(request.UserNameOrEmail)
                       ?? await _userManager.FindByEmailAsync(request.UserNameOrEmail);
            if (user == null || !await _userManager.CheckPasswordAsync(user, request.Password))
            {
                response.HasError = true;
                response.Error = "Credenciales incorrectas.";
                return response;
            }

            
            var isAdmin = await _userManager.IsInRoleAsync(user, "Admin");
            if (!isAdmin && !user.EmailConfirmed)
            {
                response.HasError = true;
                response.Error = "Debe confirmar su correo primero.";
                return response;
            }

            await _signInManager.SignInAsync(user, isPersistent: false);

            
            response.Id = user.Id;
            response.FirstName = user.FirstName;
            response.LastName = user.LastName;
            response.UserName = user.UserName;
            response.Email = user.Email;
            response.ProfilePictureUrl = user.ProfilePictureUrl;
            response.IsVerified = user.EmailConfirmed;
            return response;
        }

        public async Task<RegisterResponse> RegisterUserAsync(RegisterRequest request, string origin)
        {
            var result = new RegisterResponse();

            if (request.Password != request.ConfirmPassword)
            {
                result.HasError = true;
                result.Error = "Las contraseñas no coinciden.";
                return result;
            }

            var user = new ApplicationUser
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Phone = request.Phone,
                UserName = request.UserName,
                Email = request.Email
            };

            var createRes = await _userManager.CreateAsync(user, request.Password);
            if (!createRes.Succeeded)
            {
                result.HasError = true;
                result.Error = string.Join("; ", createRes.Errors.Select(e => e.Description));
                return result;
            }

            
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var encodedToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));
            var callback = $"{origin}/Account/ConfirmEmail?userId={user.Id}&token={encodedToken}";
            await _emailService.SendAsync(new EmailRequest
            {
                To = user.Email,
                Subject = "Confirme su correo",
                Body = $"<p>Haga clic <a href=\"{callback}\">aquí</a> para confirmar.</p>"
            });

            return result;
        }

        public async Task<string> ConfirmEmailAsync(string userId, string token)
        {
            var user = await _userManager.FindByIdAsync(userId)
                       ?? throw new ApplicationException("Usuario no existe.");
            var decoded = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(token));
            var res = await _userManager.ConfirmEmailAsync(user, decoded);
            return res.Succeeded
                ? "Correo confirmado. Puede iniciar sesión."
                : "Error al confirmar correo.";
        }

        public async Task<ForgotPasswordResponse> ForgotPasswordAsync(ForgotPasswordRequest request, string origin)
        {
            var response = new ForgotPasswordResponse();
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null || !await _userManager.IsEmailConfirmedAsync(user))
            {
                response.HasError = true;
                response.Error = "Email inválido o no confirmado.";
                return response;
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var encodedToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));
            var link = $"{origin}/Account/ResetPassword?email={user.Email}&token={encodedToken}";
            await _emailService.SendAsync(new EmailRequest
            {
                To = user.Email,
                Subject = "Restablecer contraseña",
                Body = $"<p>Haga clic <a href=\"{link}\">aquí</a> para restablecer.</p>"
            });

            return response;
        }

        public async Task<ResetPasswordResponse> ResetPasswordAsync(ResetPasswordRequest request)
        {
            var response = new ResetPasswordResponse();
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                response.HasError = true;
                response.Error = "Usuario no encontrado.";
                return response;
            }

            var decoded = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(request.Token));
            var resetRes = await _userManager.ResetPasswordAsync(user, decoded, request.Password);
            if (!resetRes.Succeeded)
            {
                response.HasError = true;
                response.Error = string.Join("; ", resetRes.Errors.Select(e => e.Description));
            }
            return response;
        }

        public async Task SignOutAsync()
            => await _signInManager.SignOutAsync();

        public async Task<List<Core.Domain.Entities.User>> GetAllUsersAsync()
        {
            var users = await _userManager.Users.ToListAsync();
            return users.Select(u => new Core.Domain.Entities.User
            {
                Id = u.Id,
                FirstName = u.FirstName,
                LastName = u.LastName,
                UserName = u.UserName,
                Email = u.Email,
                Phone = u.Phone,
                ProfilePictureUrl = u.ProfilePictureUrl,
                IsEmailConfirmed = u.EmailConfirmed
            }).ToList();
        }

        public async Task<List<Core.Domain.Entities.User>> GetUsersByIdsAsync(List<string> userIds)
        {
            var users = await _userManager.Users
                             .Where(u => userIds.Contains(u.Id))
                             .ToListAsync();
            return users.Select(u => new Core.Domain.Entities.User
            {
                Id = u.Id,
                FirstName = u.FirstName,
                LastName = u.LastName,
                UserName = u.UserName,
                Email = u.Email,
                Phone = u.Phone,
                ProfilePictureUrl = u.ProfilePictureUrl,
                IsEmailConfirmed = u.EmailConfirmed
            }).ToList();
        }

        public async Task<Core.Domain.Entities.User> GetUserByIdAsync(string userId)
        {
            var u = await _userManager.FindByIdAsync(userId);
            if (u == null) return null;
            return new Core.Domain.Entities.User
            {
                Id = u.Id,
                FirstName = u.FirstName,
                LastName = u.LastName,
                UserName = u.UserName,
                Email = u.Email,
                Phone = u.Phone,
                ProfilePictureUrl = u.ProfilePictureUrl,
                IsEmailConfirmed = u.EmailConfirmed
            };
        }

        public async Task UpdateProfileAsync(ProfileViewModel vm)
        {
            var user = await _userManager.FindByIdAsync(vm.Id)
                       ?? throw new ApplicationException("Usuario no encontrado.");

            user.FirstName = vm.FirstName;
            user.LastName = vm.LastName;
            user.Phone = vm.Phone;

            
            if (!string.IsNullOrWhiteSpace(vm.ProfileImageUrl))
                user.ProfilePictureUrl = vm.ProfileImageUrl;

            var upd = await _userManager.UpdateAsync(user);
            if (!upd.Succeeded)
                throw new ApplicationException("Error al actualizar perfil.");
        }

        public async Task<ChangePasswordResponse> ChangePasswordAsync(string userId, ChangePasswordViewModel vm)
        {
            var response = new ChangePasswordResponse();
            var user = await _userManager.FindByIdAsync(userId)
                              ?? throw new ApplicationException("Usuario no encontrado.");

            var result = await _userManager.ChangePasswordAsync(user, vm.CurrentPassword, vm.NewPassword);
            if (!result.Succeeded)
            {
                response.HasError = true;
                response.Error = string.Join("; ", result.Errors.Select(e => e.Description));
            }
            return response;
        }
    }
}
