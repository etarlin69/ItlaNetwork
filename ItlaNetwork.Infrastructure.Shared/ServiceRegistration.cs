using ItlaNetwork.Core.Application.Interfaces.Services;
using ItlaNetwork.Infrastructure.Shared.Services; // <-- Añade este using
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ItlaNetwork.Infrastructure.Shared
{
    public static class ServiceRegistration
    {
        public static void AddSharedInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            #region Services
            // Registra el EmailService
            services.AddTransient<IEmailService, EmailService>();
            #endregion
        }
    }
}