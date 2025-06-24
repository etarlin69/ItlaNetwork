using ItlaNetwork.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ItlaNetwork.Infrastructure.Persistence.Configurations
{
    public class AttackConfiguration : IEntityTypeConfiguration<Attack>
    {
        public void Configure(EntityTypeBuilder<Attack> builder)
        {
            builder.ToTable("Attacks");
            builder.HasKey(a => a.Id);

            builder.HasOne(a => a.Game)
                .WithMany(g => g.Attacks)
                .HasForeignKey(a => a.GameId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(a => a.Attacker)
                .WithMany()
                .HasForeignKey(a => a.AttackerId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}