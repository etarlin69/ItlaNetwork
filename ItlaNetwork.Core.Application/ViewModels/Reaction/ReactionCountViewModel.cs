namespace ItlaNetwork.Core.Application.ViewModels.Reaction
{
    // This ViewModel is a dedicated response object for the ToggleReaction action.
    public class ReactionCountViewModel
    {
        public int LikeCount { get; set; }
        public int DislikeCount { get; set; }
        public string CurrentUserReaction { get; set; } // "Like", "Dislike", or null
    }
}