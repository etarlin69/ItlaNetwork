using ItlaNetwork.Core.Domain.Common;
namespace ItlaNetwork.Core.Domain.Entities
{
    public class Comment : AuditableBaseEntity
    {
        public string Content { get; set; }
        public int PostId { get; set; } 
        public string UserId { get; set; } 
        public int? ParentCommentId { get; set; } 
    }
}