using ItlaNetwork.Core.Domain.Common;

namespace ItlaNetwork.Core.Domain.Entities
{
    
    public class Post : AuditableBaseEntity
    {
        public string Content { get; set; }
        public string? ImageUrl { get; set; }
        public string? VideoUrl { get; set; }

        
        public string UserId { get; set; }
    }
}