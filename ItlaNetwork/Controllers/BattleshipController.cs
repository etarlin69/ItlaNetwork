using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using ItlaNetwork.Core.Application.DTOs.Battleship;
using ItlaNetwork.Core.Application.Interfaces.Services;
using ItlaNetwork.Core.Application.ViewModels.Battleship;
using ItlaNetwork.Core.Application.ViewModels.Friendship;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ItlaNetwork.Controllers
{
    [Authorize]
    public class BattleshipController : Controller
    {
        private readonly IGameService _gameService;
        private readonly IFriendshipService _friendshipService;
        private readonly IGameRequestService _requestService;

        public BattleshipController(
            IGameService gameService,
            IFriendshipService friendshipService,
            IGameRequestService requestService
        )
        {
            _gameService = gameService;
            _friendshipService = friendshipService;
            _requestService = requestService;
        }

        
        public async Task<IActionResult> Index()
        {
            var games = await _gameService.GetMyGamesAsync();
            ViewBag.Friends = await _friendshipService.GetAllFriends();
            ViewBag.IncomingRequests = await _requestService.GetMyIncomingRequestsAsync();
            return View(games);
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Surrender(int id)
        {
            await _gameService.SurrenderAsync(id);
            TempData["Info"] = "Te has rendido. La partida ha finalizado.";
            return RedirectToAction(nameof(Index));
        }

        
        public async Task<IActionResult> Board(int id)
        {
            var board = await _gameService.GetBoardAsync(id);
            if (board == null) return NotFound();
            return View("Board", board);
        }

        
        public class RespondRequestDto
        {
            public int RequestId { get; set; }
            public bool Accept { get; set; }
        }

        [HttpPost]
        [IgnoreAntiforgeryToken]
        public async Task<IActionResult> RespondRequest([FromBody] RespondRequestDto dto)
        {
            if (dto.Accept)
                await _requestService.AcceptAsync(dto.RequestId);
            else
                await _requestService.RejectAsync(dto.RequestId);

            return Ok();
        }

        
        [HttpPost]
        public async Task<IActionResult> PlaceFleet([FromBody] FleetSetupDto dto)
        {
            await _gameService.PlaceFleetAsync(dto);
            return Json(new { placed = true });
        }

        
        [HttpPost]
        public async Task<IActionResult> Attack([FromBody] AttackViewModel vm)
        {
            var result = await _gameService.AttackAsync(vm);
            return Json(new
            {
                isHit = result.IsHit,
                winnerId = result.WinnerId
            });
        }

        
        [HttpPost]
        public async Task<IActionResult> Create(string opponentId)
        {
            var gameId = await _gameService.CreateGameAsync(opponentId);
            return RedirectToAction(nameof(Board), new { id = gameId });
        }

        
        public async Task<IActionResult> Details(int id)
        {
            var resultVm = await _gameService.GetResultAsync(id);
            if (resultVm == null) return NotFound();
            return View("Details", resultVm);
        }
    }
}
