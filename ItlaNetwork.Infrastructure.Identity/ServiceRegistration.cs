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
    // Static class to register the services of this layer using an extension method.
    public static class ServiceRegistration
    {
        // Extension method for IServiceCollection that encapsulates all Identity configuration.
        public static void AddIdentityInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            #region DbContext
            // Registers the Identity-specific DbContext, configuring it to use SQL Server.
            // The connection string is obtained from appsettings.json.
            // The assembly where migrations for this DbContext will be stored is specified.
            services.AddDbContext<IdentityContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("IdentityConnection"),
                b => b.MigrationsAssembly(typeof(IdentityContext).Assembly.FullName)));
            #endregion

            #region Identity
            // Configures the ASP.NET Core Identity system.
            // The user class (ApplicationUser) and role class (IdentityRole) are specified.
            // It is linked with Entity Framework through our IdentityContext.
            // AddDefaultTokenProviders() enables providers to generate tokens (e.g., for password reset or email confirmation).
            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<IdentityContext>().AddDefaultTokenProviders();

            // Configures the Identity options and policies for the entire system.
            services.Configure<IdentityOptions>(options =>
            {
                // Password Options.
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireNonAlphanumeric = false; // The document doesn't specify, can be changed to true if desired.
                options.Password.RequiredLength = 8;

                // Account Lockout Options.
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5); // Lockout duration.
                options.Lockout.MaxFailedAccessAttempts = 3; // Failed attempts before lockout.
                options.Lockout.AllowedForNewUsers = true;

                // User Options.
                options.User.RequireUniqueEmail = true; // Ensures that each email is unique in the system.

                // Sign-In Options.
                options.SignIn.RequireConfirmedEmail = true; // Requires email to be confirmed to be able to sign in.
            });
            #endregion

            #region Authentication
            // Configures the behavior of the authentication cookies.
            services.ConfigureApplicationCookie(options =>
            {
                // Defines the path to which an unauthenticated user trying to access a protected resource will be redirected.
                options.LoginPath = "/Account/Login";
                // Defines the path to which a user who does not have the necessary permissions will be redirected.
                options.AccessDeniedPath = "/Account/AccessDenied";
                options.ExpireTimeSpan = TimeSpan.FromDays(30); // Session cookie duration.
                options.SlidingExpiration = true; // The cookie is renewed if the user is active.
            });
            #endregion

            #region Services
            // Registers the services created in this layer so they can be injected into other parts of the application.
            // AddTransient is used because Identity services (UserManager, SignInManager) are already registered with this lifecycle.
            services.AddTransient<IAccountService, AccountService>();
            #endregion
        }
    }
}