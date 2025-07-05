using ItlaNetwork.Core.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;
namespace ItlaNetwork.Core.Application.Interfaces.Repositories
{
    public interface IReactionRepository : IGenericRepository<Reaction>
    {
        Task<Reaction> GetByPostAndUserIdAsync(int postId, string userId);
        Task<List<Reaction>> GetAllByPostIdAsync(int postId);
        Task<List<Reaction>> GetAllByPostIdListAsync(List<int> postIds); 
    }
}