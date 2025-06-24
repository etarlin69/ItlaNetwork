using ItlaNetwork.Core.Domain.Common;
using ItlaNetwork.Core.Domain.Enums; // <-- Se añade el using para el enum

namespace ItlaNetwork.Core.Domain.Entities
{
    public class FriendRequest : AuditableBaseEntity
    {
        public string SenderId { get; set; }
        public string ReceiverId { get; set; }

        // La propiedad Status ahora es de tipo FriendRequestStatus
        public FriendRequestStatus Status { get; set; }

        // Navigation Properties
        public User Sender { get; set; }
        public User Receiver { get; set; }
    }
}