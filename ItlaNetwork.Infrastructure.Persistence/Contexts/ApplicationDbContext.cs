using ItlaNetwork.Core.Domain.Common;
using ItlaNetwork.Core.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Reflection; // Necesario para Assembly.GetExecutingAssembly()
using System.Threading;
using System.Threading.Tasks;

namespace ItlaNetwork.Infrastructure.Persistence.Contexts
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        // Agrega un DbSet para cada una de tus entidades
        public DbSet<Post> Posts { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Reaction> Reactions { get; set; }
        public DbSet<Friendship> Friendships { get; set; }
        public DbSet<FriendRequest> FriendRequests { get; set; }
        public DbSet<Game> Games { get; set; }
        public DbSet<Ship> Ships { get; set; }
        public DbSet<ShipPosition> ShipPositions { get; set; }
        public DbSet<Attack> Attacks { get; set; }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            foreach (var entry in ChangeTracker.Entries<AuditableBaseEntity>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedAt = DateTime.Now;
                        // entry.Entity.CreatedBy = "DefaultUser"; // Esto se poblará con el usuario autenticado
                        break;
                    case EntityState.Modified:
                        entry.Entity.ModifiedAt = DateTime.Now;
                        // entry.Entity.ModifiedBy = "DefaultUser"; // Esto se poblará con el usuario autenticado
                        break;
                }
            }
            return base.SaveChangesAsync(cancellationToken);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Es MUY importante llamar al método base para que la configuración de Identity funcione.
            base.OnModelCreating(modelBuilder);

            // Esta única línea escaneará todo el proyecto de persistencia,
            // encontrará todas las clases que implementan IEntityTypeConfiguration
            // y aplicará sus configuraciones automáticamente.
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}