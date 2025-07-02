using ItlaNetwork.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ItlaNetwork.Infrastructure.Persistence.Configurations
{
    public class ShipPositionConfiguration : IEntityTypeConfiguration<ShipPosition>
    {
        public void Configure(EntityTypeBuilder<ShipPosition> builder)
        {
            builder.ToTable("ShipPositions");
            builder.HasKey(sp => sp.Id);

        }
    }
}