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
            // Registers AutoMapper and finds all profiles in this assembly.
            services.AddAutoMapper(Assembly.GetExecutingAssembly());

            #region Application Services
            // This file is responsible for registering services that are implemented
            // within the Core.Application layer itself.

            // The line for IAccountService has been removed because its implementation,
            // AccountService, now resides in the Infrastructure.Identity layer.
            // That registration is correctly handled in the ServiceRegistration of that project.

            services.AddTransient<IPostService, PostService>();
            services.AddTransient<ICommentService, CommentService>();
            services.AddTransient<IReactionService, ReactionService>();
            services.AddTransient<IFriendshipService, FriendshipService>();
            // Add other application-layer services here.
            #endregion
        }
    }
}
