using ItlaNetwork.Core.Application.Interfaces.Repositories;
using ItlaNetwork.Core.Application.Interfaces.Services;
using ItlaNetwork.Core.Application.ViewModels.Battleship;
using ItlaNetwork.Core.Domain.Entities;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

public class GameRequestService : IGameRequestService
{
    private readonly IGameRequestRepository _repo;
    private readonly IGameService _gameService;
    private readonly IHttpContextAccessor _http;

    public GameRequestService(
        IGameRequestRepository repo,
        IGameService gameService,
        IHttpContextAccessor http)
    {
        _repo = repo;
        _gameService = gameService;
        _http = http;
    }

    private string CurrentUserId() =>
        _http.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

    public async Task SendRequestAsync(string receiverId)
    {
        var sender = CurrentUserId();
        if (sender == receiverId) return;

        var req = new GameRequest
        {
            SenderId = sender,
            ReceiverId = receiverId,
            Status = GameRequestStatus.Pending
        };
        await _repo.AddAsync(req);
    }

    public async Task<List<GameRequestViewModel>> GetMyIncomingRequestsAsync()
    {
        var me = CurrentUserId();
        var list = await _repo.GetPendingForUserAsync(me);
        return list.Select(r => new GameRequestViewModel
        {
            RequestId = r.Id,
            SenderId = r.SenderId
        }).ToList();
    }

    public async Task AcceptAsync(int requestId)
    {
        var req = await _repo.GetByIdAsync(requestId);
        if (req == null || req.Status != GameRequestStatus.Pending) return;

        // crear partida
        var gameId = await _gameService.CreateGameAsync(req.SenderId);

        req.Status = GameRequestStatus.Accepted;
        req.GameId = gameId;
        await _repo.UpdateAsync(req);
    }

    public async Task RejectAsync(int requestId)
    {
        var req = await _repo.GetByIdAsync(requestId);
        if (req == null) return;
        req.Status = GameRequestStatus.Rejected;
        await _repo.UpdateAsync(req);
    }
}
