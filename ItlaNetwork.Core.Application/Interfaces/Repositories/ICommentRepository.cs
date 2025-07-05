using ItlaNetwork.Core.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;
namespace ItlaNetwork.Core.Application.Interfaces.Repositories
{
    public interface ICommentRepository : IGenericRepository<Comment>
    {
        Task<List<Comment>> GetAllByPostIdAsync(int postId);
        Task<List<Comment>> GetAllByPostIdListAsync(List<int> postIds); 
    }
}