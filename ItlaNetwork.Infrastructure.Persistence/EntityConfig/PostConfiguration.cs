using ItlaNetwork.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ItlaNetwork.Infrastructure.Persistence.Configurations
{
    public class PostConfiguration : IEntityTypeConfiguration<Post>
    {
        public void Configure(EntityTypeBuilder<Post> builder)
        {
            // Define el nombre de la tabla en la base de datos.
            builder.ToTable("Posts");

            // Define la clave primaria.
            builder.HasKey(p => p.Id);

            // Configura las propiedades de la entidad.
            builder.Property(p => p.Content)
                .IsRequired(); // Hace que el contenido del post sea obligatorio.

            // --- RELACIONES ---

            // Relación con User (Un Usuario puede tener muchos Posts)
            builder.HasOne(p => p.User)
                .WithMany(u => u.Posts)
                .HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.Cascade); // Si se borra un usuario, se borran sus posts.

            // Relación con Comment (Un Post puede tener muchos Comentarios)
            builder.HasMany(p => p.Comments)
                .WithOne(c => c.Post)
                .HasForeignKey(c => c.PostId)
                .OnDelete(DeleteBehavior.Cascade); // Si se borra un post, se borran sus comentarios.

            // Relación con Reaction (Un Post puede tener muchas Reacciones)
            builder.HasMany(p => p.Reactions)
                .WithOne(r => r.Post)
                .HasForeignKey(r => r.PostId)
                .OnDelete(DeleteBehavior.Cascade); // Si se borra un post, se borran sus reacciones.
        }
    }
}