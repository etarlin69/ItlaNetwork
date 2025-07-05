using ItlaNetwork.Core.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ItlaNetwork.Core.Application.Interfaces.Repositories
{
    public interface IPostRepository : IGenericRepository<Post>
    {
        
        Task<List<Post>> GetAllByUserIdsAsync(IEnumerable<string> userIds);
    }
}
