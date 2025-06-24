using ItlaNetwork.Core.Application.Enums;
using System.ComponentModel.DataAnnotations;

namespace ItlaNetwork.Core.Application.ViewModels.Reaction
{
    public class SaveReactionViewModel
    {
        [Required]
        public int PostId { get; set; }

        [Required]
        public ReactionType ReactionType { get; set; }
    }
}