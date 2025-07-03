using ItlaNetwork.Core.Application.DTOs.Account;
using ItlaNetwork.Core.Application.Interfaces.Services;
using ItlaNetwork.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ItlaNetwork.Middlewares
{
    public class SyncSessionMiddleware
    {
        private readonly RequestDelegate _next;

        public SyncSessionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        // Este método se ejecuta en cada petición HTTP.
        public async Task InvokeAsync(HttpContext context, IAccountService accountService, SignInManager<ItlaNetwork.Infrastructure.Identity.Models.ApplicationUser> signInManager)
        {
            // 1. Verifica si el usuario está autenticado a través de la cookie de Identity.
            if (context.User.Identity.IsAuthenticated)
            {
                var sessionData = context.Session.Get<AuthenticationResponse>("user");

                // 2. Si el usuario está autenticado PERO no hay datos en la sesión, la recargamos.
                if (sessionData == null)
                {
                    var userId = context.User.FindFirstValue(ClaimTypes.NameIdentifier);
                    var user = await accountService.GetUserByIdAsync(userId); // Necesitarás añadir este método a IAccountService

                    if (user != null)
                    {
                        var authResponse = new AuthenticationResponse
                        {
                            Id = user.Id,
                            FirstName = user.FirstName,
                            LastName = user.LastName,
                            UserName = user.UserName,
                            Email = user.Email,
                            ProfilePictureUrl = user.ProfilePictureUrl,
                            IsVerified = user.IsEmailConfirmed
                        };
                        context.Session.Set("user", authResponse);
                    }
                    else // Si el usuario de la cookie ya no existe en la BD, cerramos la sesión.
                    {
                        await signInManager.SignOutAsync();
                        context.Session.Remove("user");
                    }
                }
            }

            await _next(context);
        }
    }
}