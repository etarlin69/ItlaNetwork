using ItlaNetwork.Core.Domain.Common;

namespace ItlaNetwork.Core.Domain.Entities
{
    public class Reaction : AuditableBaseEntity
    {
        // Example: 0 for Like, 1 for Dislike
        public int ReactionType { get; set; }

        // Foreign Keys
        public string UserId { get; set; }
        public int PostId { get; set; }

        // Navigation Properties
        public User User { get; set; }
        public Post Post { get; set; }
    }
}