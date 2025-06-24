using ItlaNetwork.Core.Domain.Common;
using System.Collections.Generic;

namespace ItlaNetwork.Core.Domain.Entities
{
    public class Post : AuditableBaseEntity
    {
        public string Content { get; set; }
        public string? ImageUrl { get; set; }
        public string? VideoUrl { get; set; }

        // Clave foránea que lo conecta con el usuario
        public string UserId { get; set; }

        // Propiedades de navegación
        public User User { get; set; }
        public ICollection<Comment> Comments { get; set; }
        public ICollection<Reaction> Reactions { get; set; }

        public Post()
        {
            Comments = new HashSet<Comment>();
            Reactions = new HashSet<Reaction>();
        }
    }
}