using ItlaNetwork.Core.Domain.Common;
namespace ItlaNetwork.Core.Domain.Entities
{
    public class Ship : AuditableBaseEntity
    {
        public int Size { get; set; }
        public bool IsSunk { get; set; }
        public int GameId { get; set; } 
        public string PlayerId { get; set; } 
    }
}