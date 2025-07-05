using Microsoft.Extensions.DependencyInjection;
using AutoMapper;
using ItlaNetwork.Core.Application.Interfaces.Services;
using ItlaNetwork.Core.Application.Services;
using ItlaNetwork.Core.Application.Mappings;

namespace ItlaNetwork.Core.Application
{
    public static class ServiceRegistration
    {
        public static void AddApplicationLayer(this IServiceCollection services)
        {
            
            services.AddAutoMapper(typeof(GeneralProfile));

            
            services.AddHttpContextAccessor();

            
            services.AddTransient<IPostService, PostService>();
            services.AddTransient<ICommentService, CommentService>();
            services.AddTransient<IReactionService, ReactionService>();
            services.AddTransient<IFriendshipService, FriendshipService>();

            
            services.AddTransient<IGameService, GameService>();
            services.AddTransient<IGameRequestService, GameRequestService>();
        }
    }
}
