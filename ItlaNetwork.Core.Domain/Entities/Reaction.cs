using ItlaNetwork.Core.Domain.Common;
using ItlaNetwork.Core.Domain.Enums;
namespace ItlaNetwork.Core.Domain.Entities
{
    public class Reaction : AuditableBaseEntity
    {
        public ReactionType ReactionType { get; set; }
        public int PostId { get; set; } 
        public string UserId { get; set; }
    }
}