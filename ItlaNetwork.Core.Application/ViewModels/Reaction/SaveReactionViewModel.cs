using System.ComponentModel.DataAnnotations;
using ItlaNetwork.Core.Domain.Enums;

namespace ItlaNetwork.Core.Application.ViewModels.Reaction
{
    
    public class SaveReactionViewModel
    {
        public int Id { get; set; }

        [Required]
        public int PostId { get; set; }

        [Required]
        public ReactionType ReactionType { get; set; }
    }
}