using ItlaNetwork.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ItlaNetwork.Infrastructure.Persistence.EntityConfig
{
    public class GameRequestConfiguration : IEntityTypeConfiguration<GameRequest>
    {
        public void Configure(EntityTypeBuilder<GameRequest> builder)
        {
            builder.ToTable("GameRequests");

            builder.HasKey(gr => gr.Id);

            builder.Property(gr => gr.SenderId)
                   .IsRequired()
                   .HasMaxLength(450); 

            builder.Property(gr => gr.ReceiverId)
                   .IsRequired()
                   .HasMaxLength(450);

            builder.Property(gr => gr.Status)
                   .HasConversion<string>()
                   .IsRequired()
                   .HasDefaultValue(GameRequestStatus.Pending);

            builder.Property(gr => gr.GameId)
                   .IsRequired(false);

            
            builder.HasIndex(gr => new { gr.ReceiverId, gr.Status })
                   .HasDatabaseName("IX_GameRequests_Receiver_Status");
        }
    }
}
