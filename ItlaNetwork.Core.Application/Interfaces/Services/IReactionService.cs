using ItlaNetwork.Core.Application.ViewModels.Reaction;
using ItlaNetwork.Core.Domain.Entities;
using System.Threading.Tasks;

namespace ItlaNetwork.Core.Application.Interfaces.Services
{
    public interface IReactionService
    {
        Task ToggleReactionAsync(SaveReactionViewModel vm, string userId);
        Task<List<Reaction>> GetReactionsByPostIdAsync(int postId);
    }
}