using ItlaNetwork.Core.Domain.Common;
namespace ItlaNetwork.Core.Domain.Entities
{
    public class ShipPosition : AuditableBaseEntity
    {
        public int Row { get; set; }
        public int Column { get; set; }
        public int ShipId { get; set; } // Clave Foránea
    }
}