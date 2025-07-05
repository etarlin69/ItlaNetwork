using ItlaNetwork.Core.Domain.Common;

namespace ItlaNetwork.Core.Domain.Entities
{
    public enum GameRequestStatus { Pending, Accepted, Rejected }

    public class GameRequest : AuditableBaseEntity
    {
        public string SenderId { get; set; }
        public string ReceiverId { get; set; }
        public GameRequestStatus Status { get; set; }
        public int? GameId { get; set; } 
    }
}
