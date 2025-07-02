using ItlaNetwork.Core.Application.ViewModels.Reaction;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ItlaNetwork.Core.Application.Interfaces.Services
{
    public interface IReactionService
    {
        // The method now returns the updated counts and user reaction status.
        Task<ReactionCountViewModel> ToggleReactionAsync(SaveReactionViewModel vm);
        Task<List<ReactionViewModel>> GetReactionsByPostIdAsync(int postId);
    }
}