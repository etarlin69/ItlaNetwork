using System.Collections.Generic;
using System.Threading.Tasks;
using ItlaNetwork.Core.Application.DTOs.Battleship;
using ItlaNetwork.Core.Application.ViewModels.Battleship;

namespace ItlaNetwork.Core.Application.Interfaces.Services
{
    public interface IGameService
    {
        Task<List<GameSummaryViewModel>> GetMyGamesAsync();
        Task<BoardViewModel> GetBoardAsync(int gameId);
        Task<PlaceShipResultViewModel> PlaceShipAsync(PlaceShipViewModel vm);
        Task<AttackResultViewModel> AttackAsync(AttackViewModel vm);
        Task<int> CreateGameAsync(string opponentId);
        Task PlaceFleetAsync(FleetSetupDto dto);

        
        Task SurrenderAsync(int gameId);

        
        Task<GameResultViewModel> GetResultAsync(int gameId);
    }
}
