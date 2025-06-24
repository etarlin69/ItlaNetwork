using ItlaNetwork.Core.Application.Interfaces.Services;
using ItlaNetwork.Infrastructure.Identity.Contexts;
using ItlaNetwork.Infrastructure.Identity.Models;
using ItlaNetwork.Infrastructure.Identity.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace ItlaNetwork.Infrastructure.Identity
{
    // Clase estática para registrar los servicios de esta capa mediante un método de extensión.
    public static class ServiceRegistration
    {
        // Método de extensión para IServiceCollection que encapsula toda la configuración de Identity.
        public static void AddIdentityInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            #region DbContext
            // Registra el DbContext específico de Identity, configurándolo para usar SQL Server.
            // Se obtiene la cadena de conexión desde appsettings.json.
            // Se especifica el ensamblado donde se guardarán las migraciones para este DbContext.
            services.AddDbContext<IdentityContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("IdentityConnection"),
                b => b.MigrationsAssembly(typeof(IdentityContext).Assembly.FullName)));
            #endregion

            #region Identity
            // Configura el sistema de Identity de ASP.NET Core.
            // Se especifica la clase de usuario (ApplicationUser) y la clase de rol (IdentityRole).
            // Se vincula con Entity Framework a través de nuestro IdentityContext.
            // AddDefaultTokenProviders() habilita los proveedores para generar tokens (ej. para resetear contraseña o confirmar email).
            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<IdentityContext>().AddDefaultTokenProviders();

            // Configura las opciones y políticas de Identity para todo el sistema.
            services.Configure<IdentityOptions>(options =>
            {
                // Opciones de Contraseña.
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireNonAlphanumeric = false; // El documento no lo especifica, se puede cambiar a true si se desea.
                options.Password.RequiredLength = 8;

                // Opciones de Bloqueo de Cuenta.
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5); // Tiempo de bloqueo.
                options.Lockout.MaxFailedAccessAttempts = 3; // Intentos fallidos antes de bloquear.
                options.Lockout.AllowedForNewUsers = true;

                // Opciones de Usuario.
                options.User.RequireUniqueEmail = true; // Asegura que cada correo electrónico sea único en el sistema.

                // Opciones de Inicio de Sesión.
                options.SignIn.RequireConfirmedEmail = true; // Requiere que el correo sea confirmado para poder iniciar sesión.
            });
            #endregion

            #region Authentication
            // Configura el comportamiento de las cookies de autenticación.
            services.ConfigureApplicationCookie(options =>
            {
                // Define la ruta a la que será redirigido un usuario no autenticado que intente acceder a un recurso protegido.
                options.LoginPath = "/Account/Login";
                // Define la ruta a la que será redirigido un usuario que no tenga los permisos necesarios.
                options.AccessDeniedPath = "/Account/AccessDenied";
                options.ExpireTimeSpan = TimeSpan.FromDays(30); // Duración de la cookie de sesión.
                options.SlidingExpiration = true; // La cookie se renueva si el usuario está activo.
            });
            #endregion

            #region Services
            // Registra los servicios creados en esta capa para que puedan ser inyectados en otras partes de la aplicación.
            // Se usa AddTransient porque los servicios de Identity (UserManager, SignInManager) ya están registrados con ese ciclo de vida.
            services.AddTransient<IAccountService, AccountService>();
            #endregion
        }
    }
}