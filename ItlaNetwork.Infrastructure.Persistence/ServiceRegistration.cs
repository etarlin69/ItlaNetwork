using ItlaNetwork.Core.Application.Interfaces.Repositories;
using ItlaNetwork.Infrastructure.Persistence.Contexts;
using ItlaNetwork.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ItlaNetwork.Infrastructure.Persistence
{
    // Esta es la clase de registro para la capa de Persistencia.
    public static class ServiceRegistration
    {
        // Método de extensión para registrar todos los servicios de esta capa.
        public static void AddPersistenceInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            #region DbContext
            // Verifica si la configuración especifica usar una base de datos en memoria.
            if (configuration.GetValue<bool>("UseInMemoryDatabase"))
            {
                services.AddDbContext<ApplicationDbContext>(options => options.UseInMemoryDatabase("ApplicationDb"));
            }
            else
            {
                // Registra el ApplicationDbContext con su cadena de conexión a SQL Server.
                services.AddDbContext<ApplicationDbContext>(options =>
                    options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"),
                    m => m.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));
            }
            #endregion

            #region Repositories
            // Registra el Repositorio Genérico con su interfaz.
            // Cuando un servicio pida IGenericRepository<T>, el contenedor de DI le proporcionará una instancia de GenericRepository<T>.
            services.AddTransient(typeof(IGenericRepository<>), typeof(GenericRepository<>));

            // Registra todos los repositorios específicos con sus interfaces correspondientes.
            services.AddTransient<IPostRepository, PostRepository>();
            services.AddTransient<ICommentRepository, CommentRepository>();
            services.AddTransient<IFriendshipRepository, FriendshipRepository>();
            services.AddTransient<IFriendRequestRepository, FriendRequestRepository>();
            services.AddTransient<IReactionRepository, ReactionRepository>();
            services.AddTransient<IGameRepository, GameRepository>();
            #endregion
        }
    }
}