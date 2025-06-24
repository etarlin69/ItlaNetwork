using ItlaNetwork.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ItlaNetwork.Infrastructure.Persistence.Configurations
{
    public class FriendshipConfiguration : IEntityTypeConfiguration<Friendship>
    {
        public void Configure(EntityTypeBuilder<Friendship> builder)
        {
            builder.ToTable("Friendships");
            builder.HasKey(f => f.Id);

            // Relación explícita para el primer usuario de la amistad
            builder.HasOne(f => f.User)
                .WithMany(u => u.Friendships) // <-- Se enlaza a la primera colección
                .HasForeignKey(f => f.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // Relación explícita para el segundo usuario (el amigo)
            builder.HasOne(f => f.Friend)
                .WithMany(u => u.FriendsOf) // <-- Se enlaza a la segunda colección
                .HasForeignKey(f => f.FriendId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}