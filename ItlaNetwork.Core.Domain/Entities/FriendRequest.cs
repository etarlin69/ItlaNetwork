using ItlaNetwork.Core.Domain.Common;
using ItlaNetwork.Core.Domain.Enums;
namespace ItlaNetwork.Core.Domain.Entities
{
    public class FriendRequest : AuditableBaseEntity
    {
        public FriendRequestStatus Status { get; set; }
        public string SenderId { get; set; } // Clave Foránea
        public string ReceiverId { get; set; } // Clave Foránea
    }
}