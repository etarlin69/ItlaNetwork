using ItlaNetwork.Core.Application.Interfaces.Services;
using ItlaNetwork.Core.Domain.Settings;
using ItlaNetwork.Infrastructure.Shared.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ItlaNetwork.Infrastructure.Shared
{
    public static class ServiceRegistration
    {
        public static void AddSharedInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            // --- CORRECCIÓN CRÍTICA ---
            // Esta línea lee la sección "MailSettings" de tu appsettings.json
            // y la configura para que pueda ser inyectada en otros servicios.
            services.Configure<MailSettings>(configuration.GetSection("MailSettings"));

            // Registra el EmailService para que pueda ser inyectado donde se necesite IEmailService.
            services.AddTransient<IEmailService, EmailService>();
        }
    }
}