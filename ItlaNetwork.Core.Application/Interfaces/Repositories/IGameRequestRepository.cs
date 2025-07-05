using System.Collections.Generic;
using System.Threading.Tasks;
using ItlaNetwork.Core.Application.Interfaces.Repositories;
using ItlaNetwork.Core.Domain.Entities;

public interface IGameRequestRepository : IGenericRepository<GameRequest>
{
    Task<List<GameRequest>> GetPendingForUserAsync(string userId);
    Task<GameRequest> GetByIdAsync(int requestId);
}
