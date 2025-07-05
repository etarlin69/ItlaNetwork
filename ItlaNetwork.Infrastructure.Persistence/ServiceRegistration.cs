using ItlaNetwork.Core.Application.Interfaces.Repositories;
using ItlaNetwork.Infrastructure.Persistence.Contexts;
using ItlaNetwork.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ItlaNetwork.Infrastructure.Persistence
{
    
    public static class ServiceRegistration
    {
        
        public static void AddPersistenceInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            #region DbContext
            if (configuration.GetValue<bool>("UseInMemoryDatabase"))
            {
                services.AddDbContext<ApplicationDbContext>(options =>
                    options.UseInMemoryDatabase("ApplicationDb"));
            }
            else
            {
                services.AddDbContext<ApplicationDbContext>(options =>
                    options.UseSqlServer(
                        configuration.GetConnectionString("DefaultConnection"),
                        sql => sql.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));
            }
            #endregion

            #region Repositories
            // Genérico
            services.AddTransient(typeof(IGenericRepository<>), typeof(GenericRepository<>));

            // Específicos
            services.AddTransient<IPostRepository, PostRepository>();
            services.AddTransient<ICommentRepository, CommentRepository>();
            services.AddTransient<IFriendshipRepository, FriendshipRepository>();
            services.AddTransient<IFriendRequestRepository, FriendRequestRepository>();
            services.AddTransient<IReactionRepository, ReactionRepository>();
            services.AddTransient<IGameRepository, GameRepository>();

            // Nuevos para Battleship
            services.AddTransient<IShipRepository, ShipRepository>();
            services.AddTransient<IShipPositionRepository, ShipPositionRepository>();
            services.AddTransient<IAttackRepository, AttackRepository>();
            services.AddTransient<IGameRequestRepository, GameRequestRepository>();

            #endregion
        }
    }
}
