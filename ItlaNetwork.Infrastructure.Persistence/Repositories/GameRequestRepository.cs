using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ItlaNetwork.Core.Application.Interfaces.Repositories;
using ItlaNetwork.Core.Domain.Entities;
using ItlaNetwork.Infrastructure.Persistence.Contexts;
using ItlaNetwork.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;

public class GameRequestRepository : GenericRepository<GameRequest>, IGameRequestRepository
{
    private readonly ApplicationDbContext _ctx;
    public GameRequestRepository(ApplicationDbContext ctx) : base(ctx) { _ctx = ctx; }

    public Task<List<GameRequest>> GetPendingForUserAsync(string userId)
        => _ctx.Set<GameRequest>()
               .Where(r => r.ReceiverId == userId && r.Status == GameRequestStatus.Pending)
               .ToListAsync();

    public Task<GameRequest> GetByIdAsync(int requestId)
        => _ctx.Set<GameRequest>().FirstOrDefaultAsync(r => r.Id == requestId);
}
