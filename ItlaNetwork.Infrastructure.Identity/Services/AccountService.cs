using AutoMapper;
using ItlaNetwork.Core.Application.DTOs.Account;
using ItlaNetwork.Core.Application.Interfaces.Services;
using ItlaNetwork.Core.Domain.Entities;
using ItlaNetwork.Infrastructure.Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ItlaNetwork.Core.Application.DTOs.Email;

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

            var user = await _userManager.FindByNameAsync(request.UserNameOrEmail);
            if (user == null)
            {
                user = await _userManager.FindByEmailAsync(request.UserNameOrEmail);
            }

            if (user == null)
            {
                response.HasError = true;
                response.Error = "Las credenciales proporcionadas son incorrectas.";
                return response;
            }

            var passwordIsValid = await _userManager.CheckPasswordAsync(user, request.Password);
            if (!passwordIsValid)
            {
                response.HasError = true;
                response.Error = "Las credenciales proporcionadas son incorrectas.";
                return response;
            }

            var isAdmin = await _userManager.IsInRoleAsync(user, "Admin");
            if (!isAdmin && !user.EmailConfirmed)
            {
                response.HasError = true;
                response.Error = "Su cuenta no ha sido activada. Por favor, revise su correo para el enlace de activación.";
                return response;
            }

            await _signInManager.SignInAsync(user, isPersistent: false);

            response.Id = user.Id;
            response.FirstName = user.FirstName;
            response.LastName = user.LastName;
            response.Email = user.Email;
            response.UserName = user.UserName;
            response.ProfilePictureUrl = user.ProfilePictureUrl;
            response.IsVerified = user.EmailConfirmed;

            return response;
        }

        public async Task<RegisterResponse> RegisterUserAsync(RegisterRequest request, string origin)
        {
            var response = new RegisterResponse();
            var userWithSameUserName = await _userManager.FindByNameAsync(request.UserName);
            if (userWithSameUserName != null)
            {
                response.HasError = true;
                response.Error = $"El nombre de usuario '{request.UserName}' ya está en uso.";
                return response;
            }

            var userWithSameEmail = await _userManager.FindByEmailAsync(request.Email);
            if (userWithSameEmail != null)
            {
                response.HasError = true;
                response.Error = $"El correo electrónico '{request.Email}' ya está registrado.";
                return response;
            }

            var user = new ApplicationUser
            {
                Email = request.Email,
                UserName = request.UserName,
                FirstName = request.FirstName,
                LastName = request.LastName,
                Phone = request.Phone
            };

            var result = await _userManager.CreateAsync(user, request.Password);
            if (!result.Succeeded)
            {
                response.HasError = true;
                response.Error = $"Ocurrió un error al registrar el usuario. Errores: {string.Join(", ", result.Errors.Select(e => e.Description))}";
                return response;
            }

            await _userManager.AddToRoleAsync(user, "Basic");

            var verificationUri = await SendVerificationEmailUri(user, origin);

            try
            {
                await _emailService.SendAsync(new EmailRequest()
                {
                    To = user.Email,
                    Subject = "Confirma tu cuenta - ItlaNetwork",
                    Body = $"<h3>¡Bienvenido a ItlaNetwork!</h3><p>Por favor, confirma tu cuenta haciendo clic en el siguiente enlace:</p><a href='{verificationUri}'>Activar mi cuenta</a>"
                });
            }
            catch (System.Exception ex)
            {
                response.HasError = true;
                response.Error = $"Ocurrió un error al enviar el correo de confirmación. Por favor, contacta a soporte. Detalles: {ex.Message}";
            }

            return response;
        }

        public async Task<string> ConfirmEmailAsync(string userId, string token)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return "No se encontró una cuenta para este usuario.";

            token = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(token));
            var result = await _userManager.ConfirmEmailAsync(user, token);
            if (result.Succeeded)
            {
                return "¡Cuenta confirmada exitosamente! Ahora puedes iniciar sesión.";
            }

            return $"Ocurrió un error al confirmar tu cuenta. Por favor, intenta de nuevo o contacta a soporte.";
        }

        public async Task<ForgotPasswordResponse> ForgotPasswordAsync(ForgotPasswordRequest request, string origin)
        {
            var response = new ForgotPasswordResponse();
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                response.HasError = true;
                response.Error = $"No existe una cuenta con el correo {request.Email}";
                return response;
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            token = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));
            var uri = new System.Uri(string.Concat(origin, $"/Account/ResetPassword?token={token}"));

            await _emailService.SendAsync(new EmailRequest()
            {
                To = user.Email,
                Subject = "Restablecer tu contraseña",
                Body = $"Para restablecer tu contraseña, visita este enlace: {uri}"
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
                response.Error = $"No existe una cuenta con el correo {request.Email}";
                return response;
            }

            request.Token = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(request.Token));
            var result = await _userManager.ResetPasswordAsync(user, request.Token, request.Password);
            if (!result.Succeeded)
            {
                response.HasError = true;
                response.Error = $"Ocurrió un error al restablecer la contraseña.";
                return response;
            }

            return response;
        }

        public async Task SignOutAsync()
        {
            await _signInManager.SignOutAsync();
        }

        public async Task<List<User>> GetAllUsersAsync()
        {
            var applicationUsers = await _userManager.Users.ToListAsync();
            return _mapper.Map<List<User>>(applicationUsers);
        }

        public async Task<List<User>> GetUsersByIdsAsync(List<string> userIds)
        {
            var applicationUsers = await _userManager.Users
                                        .Where(u => userIds.Contains(u.Id))
                                        .ToListAsync();
            return _mapper.Map<List<User>>(applicationUsers);
        }

        private async Task<string> SendVerificationEmailUri(ApplicationUser user, string origin)
        {
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            token = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));
            var uri = new System.Uri(string.Concat(origin, $"/Account/ConfirmEmail?userId={user.Id}&token={token}"));
            return uri.ToString();
        }
    }
}