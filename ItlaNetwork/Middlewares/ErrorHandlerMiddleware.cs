using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace WebApp.Middlewares
{
    public class ErrorHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ErrorHandlerMiddleware> _logger;

        public ErrorHandlerMiddleware(RequestDelegate next, ILogger<ErrorHandlerMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            try
            {
                
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                
                _logger.LogError(ex, "Ha ocurrido una excepción no manejada.");

                
                var controller = httpContext.Request.RouteValues["controller"]?.ToString();
                var action = httpContext.Request.RouteValues["action"]?.ToString();

                
                httpContext.Response.Redirect("/Home");
            }
        }
    }
}