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
            
            services.Configure<MailSettings>(configuration.GetSection("MailSettings"));

            
            services.AddTransient<IEmailService, EmailService>();
        }
    }
}