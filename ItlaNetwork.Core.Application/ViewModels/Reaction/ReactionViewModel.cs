using System.ComponentModel.DataAnnotations;
using ItlaNetwork.Core.Domain.Enums;

namespace ItlaNetwork.Core.Application.ViewModels.Reaction
{

    
    public class ReactionViewModel
    {
        public int Id { get; set; }
        public int PostId { get; set; }
        public string UserId { get; set; }
        public ReactionType ReactionType { get; set; }
    }
}