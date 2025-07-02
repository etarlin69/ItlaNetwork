using ItlaNetwork.Core.Domain.Common;
namespace ItlaNetwork.Core.Domain.Entities
{
    public class Friendship : AuditableBaseEntity
    {
        public string UserId { get; set; } // Clave Foránea
        public string FriendId { get; set; } // Clave Foránea
    }
}