using ItlaNetwork.Core.Application.DTOs.Account;
using ItlaNetwork.Extensions;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace ItlaNetwork.Middlewares
{
    public class ValidateUserSession
    {
        private readonly RequestDelegate _next;

        public ValidateUserSession(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var user = context.Session.Get<AuthenticationResponse>("user");

            if (context.Request.Path == "/" || context.Request.Path == "/Account/Login" || context.Request.Path == "/Account/Register")
            {
                if (user != null)
                {
                    context.Response.Redirect("/Home");
                }
            }
            else
            {
                if (user == null)
                {
                    context.Response.Redirect("/Account/Login");
                }
            }

            await _next(context);
        }
    }
}