using ItlaNetwork.Core.Domain.Common;

namespace ItlaNetwork.Core.Domain.Entities
{
    public class ShipPosition : AuditableBaseEntity
    {
        public int Row { get; set; }
        public int Column { get; set; }
        public bool IsHit { get; set; } = false;

        // Foreign Key
        public int ShipId { get; set; }

        // Navigation Property
        public Ship Ship { get; set; }
    }
}