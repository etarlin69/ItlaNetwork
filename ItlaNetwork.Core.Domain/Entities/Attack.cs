using ItlaNetwork.Core.Domain.Common;
namespace ItlaNetwork.Core.Domain.Entities
{
    public class Attack : AuditableBaseEntity
    {
        public int Row { get; set; }
        public int Column { get; set; }
        public bool IsHit { get; set; }
        public int GameId { get; set; } 
        public string AttackerId { get; set; } 
    }
}