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
                // Llama al siguiente middleware en la cadena.
                // Si no hay excepciones, la petición sigue su curso normal.
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                // Si ocurre una excepción, la atrapamos aquí.
                _logger.LogError(ex, "Ha ocurrido una excepción no manejada.");

                // Usamos TempData para pasar el mensaje de error a la siguiente petición (el redirect).
                // Es ideal para mensajes que solo deben mostrarse una vez.
                var controller = httpContext.Request.RouteValues["controller"]?.ToString();
                var action = httpContext.Request.RouteValues["action"]?.ToString();

                // Redirigimos al Home. Podrías redirigir a una página de error específica si lo prefieres.
                httpContext.Response.Redirect("/Home");
            }
        }
    }
}