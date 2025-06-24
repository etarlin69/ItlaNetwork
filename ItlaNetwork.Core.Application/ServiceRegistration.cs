using ItlaNetwork.Core.Application.Interfaces.Services;
using ItlaNetwork.Core.Application.Services;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace ItlaNetwork.Core.Application
{
    public static class ServiceRegistration
    {
        public static void AddApplicationLayer(this IServiceCollection services)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());

            #region Services
            // Ya teníamos registrado el servicio de cuentas.
            services.AddTransient<IAccountService, AccountService>();

            // --- LÍNEA QUE FALTA ---
            // Aquí registramos el servicio de publicaciones.
            services.AddTransient<IPostService, PostService>();
            services.AddTransient<ICommentService, CommentService>();
            services.AddTransient<IReactionService, ReactionService>();
            services.AddTransient<IFriendshipService, FriendshipService>();
            #endregion
        }
    }
}