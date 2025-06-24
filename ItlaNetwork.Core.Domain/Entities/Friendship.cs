using ItlaNetwork.Core.Domain.Common;

namespace ItlaNetwork.Core.Domain.Entities
{
    public class Friendship : AuditableBaseEntity
    {
        // 'Id' y 'CreatedAt' ahora son heredados de AuditableBaseEntity.

        public string UserId { get; set; }
        public string FriendId { get; set; }

        // Navigation Properties
        public User User { get; set; }
        public User Friend { get; set; }
    }
}