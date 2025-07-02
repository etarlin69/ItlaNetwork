using ItlaNetwork.Core.Domain.Common;
namespace ItlaNetwork.Core.Domain.Entities
{
    public class Game : AuditableBaseEntity
    {
        public string Player1Id { get; set; } // Clave Foránea
        public string Player2Id { get; set; } // Clave Foránea
        public string CurrentTurnPlayerId { get; set; } // Clave Foránea
        public string? WinnerId { get; set; } // Clave Foránea
    }
}