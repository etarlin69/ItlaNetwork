using ItlaNetwork.Core.Domain.Common;

namespace ItlaNetwork.Core.Domain.Entities
{
    // Esta es la versión purista y desacoplada de la entidad Post.
    public class Post : AuditableBaseEntity
    {
        public string Content { get; set; }
        public string? ImageUrl { get; set; }
        public string? VideoUrl { get; set; }

        // Se mantiene la clave foránea, pero se eliminan las propiedades de navegación.
        public string UserId { get; set; }
    }
}