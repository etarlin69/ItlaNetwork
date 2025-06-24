using System.Collections.Generic;
using ItlaNetwork.Core.Domain.Common;

namespace ItlaNetwork.Core.Domain.Entities
{
    public class Comment : AuditableBaseEntity
    {
        public string Content { get; set; }
        public string UserId { get; set; }
        public int PostId { get; set; }

        // CORREGIDO: Se cambió el nombre de la propiedad de la clave foránea.
        public int? ParentCommentId { get; set; }

        // Propiedades de navegación
        public User User { get; set; }
        public Post Post { get; set; }
        public Comment ParentComment { get; set; }

        public ICollection<Comment> Replies { get; set; } = new HashSet<Comment>();
    }
}