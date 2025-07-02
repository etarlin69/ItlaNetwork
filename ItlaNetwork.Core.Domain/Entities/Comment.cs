using ItlaNetwork.Core.Domain.Common;
namespace ItlaNetwork.Core.Domain.Entities
{
    public class Comment : AuditableBaseEntity
    {
        public string Content { get; set; }
        public int PostId { get; set; } // Clave Foránea
        public string UserId { get; set; } // Clave Foránea
        public int? ParentCommentId { get; set; } // Clave Foránea
    }
}