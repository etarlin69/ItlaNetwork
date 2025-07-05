using System.Security.Claims;
using System.Threading.Tasks;
using ItlaNetwork.Core.Application.DTOs.Account;
using ItlaNetwork.Core.Application.Interfaces.Services;
using ItlaNetwork.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace ItlaNetwork.Middlewares
{
    public class SyncSessionMiddleware
    {
        private readonly RequestDelegate _next;

        public SyncSessionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(
            HttpContext context,
            IAccountService accountService,
            SignInManager<Infrastructure.Identity.Models.ApplicationUser> signInManager)
        {
            if (context.User.Identity?.IsAuthenticated == true)
            {
                
                var sessionData = context.Session.Get<AuthenticationResponse>("user");

                
                var userId = context.User.FindFirstValue(ClaimTypes.NameIdentifier);

                
                var user = await accountService.GetUserByIdAsync(userId);

                if (user == null)
                {
                    
                    await signInManager.SignOutAsync();
                    context.Session.Remove("user");
                }
                else
                {
                    
                    if (sessionData == null
                        || sessionData.ProfilePictureUrl != user.ProfilePictureUrl
                        || sessionData.FirstName != user.FirstName
                        || sessionData.LastName != user.LastName)
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
                }
            }

            
            await _next(context);
        }
    }
}