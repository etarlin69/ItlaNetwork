
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using ItlaNetwork.Core.Application.DTOs.Battleship;
using ItlaNetwork.Core.Application.Interfaces.Repositories;
using ItlaNetwork.Core.Application.Interfaces.Services;
using ItlaNetwork.Core.Application.ViewModels.Battleship;
using ItlaNetwork.Core.Domain.Entities;
using Microsoft.AspNetCore.Http;

namespace ItlaNetwork.Core.Application.Services
{
    public class GameService : IGameService
    {
        private readonly IGameRepository _gameRepo;
        private readonly IShipRepository _shipRepo;
        private readonly IShipPositionRepository _posRepo;
        private readonly IAttackRepository _attackRepo;
        private readonly IAccountService _accountService;
        private readonly IHttpContextAccessor _http;

        public GameService(
            IGameRepository gameRepo,
            IShipRepository shipRepo,
            IShipPositionRepository posRepo,
            IAttackRepository attackRepo,
            IAccountService accountService,
            IHttpContextAccessor http)
        {
            _gameRepo = gameRepo;
            _shipRepo = shipRepo;
            _posRepo = posRepo;
            _attackRepo = attackRepo;
            _accountService = accountService;
            _http = http;
        }

        private string CurrentUserId() =>
            _http.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

        public async Task<int> CreateGameAsync(string opponentId)
        {
            var me = CurrentUserId();
            var game = new Game
            {
                Player1Id = me,
                Player2Id = opponentId,
                CurrentTurnPlayerId = me,
                WinnerId = string.Empty   
            };
            var created = await _gameRepo.AddAsync(game);
            return created.Id;
        }

        public async Task<List<GameSummaryViewModel>> GetMyGamesAsync()
        {
            var me = CurrentUserId();
            var games = await _gameRepo.GetAllByPlayerIdAsync(me);

            var oppIds = games
                .Select(g => g.Player1Id == me ? g.Player2Id : g.Player1Id)
                .Distinct()
                .ToList();

            var users = await _accountService.GetUsersByIdsAsync(oppIds);

            return games.Select(g =>
            {
                var oppId = g.Player1Id == me ? g.Player2Id : g.Player1Id;
                var opp = users.First(u => u.Id == oppId);
                var finished = !string.IsNullOrEmpty(g.WinnerId);

                return new GameSummaryViewModel
                {
                    GameId = g.Id,
                    OpponentId = oppId,
                    OpponentName = $"{opp.FirstName} {opp.LastName}",
                    IsMyTurn = g.CurrentTurnPlayerId == me && !finished,
                    IsFinished = finished,
                    WinnerId = finished ? g.WinnerId : null
                };
            }).ToList();
        }

        public async Task<BoardViewModel> GetBoardAsync(int gameId)
        {
            var me = CurrentUserId();
            var game = await _gameRepo.GetByIdAsync(gameId);
            if (game == null) return null;

            var allShips = await _shipRepo.GetAllByGameIdAsync(gameId);
            var myShips = allShips.Where(s => s.PlayerId == me).ToList();
            var opShips = allShips.Where(s => s.PlayerId != me).ToList();
            bool myReady = myShips.Count == 5;
            bool opReady = opShips.Count == 5;

            var grid = new int[12, 12];
            foreach (var s in myShips)
                foreach (var p in await _posRepo.GetAllByShipIdAsync(s.Id))
                    grid[p.Row, p.Column] = 1;
            foreach (var a in await _attackRepo.GetAllByGameIdAsync(gameId))
                grid[a.Row, a.Column] = a.IsHit ? 2 : 3;

            return new BoardViewModel
            {
                GameId = game.Id,
                Grid = grid,
                MyFleetPlaced = myReady,
                OpponentFleetPlaced = opReady,
                IsMyTurn = game.CurrentTurnPlayerId == me && string.IsNullOrEmpty(game.WinnerId),
                WinnerId = string.IsNullOrEmpty(game.WinnerId) ? null : game.WinnerId
            };
        }

        public async Task<PlaceShipResultViewModel> PlaceShipAsync(PlaceShipViewModel vm)
        {
            var me = CurrentUserId();
            var occupied = new HashSet<(int r, int c)>(
                (await _posRepo.GetAllByGameIdAsync(vm.GameId))
                .Select(p => (p.Row, p.Column))
            );

            var coords = new List<(int r, int c)>();
            for (int i = 0; i < vm.Size; i++)
            {
                var r = vm.Row + (vm.Horizontal ? 0 : i);
                var c = vm.Col + (vm.Horizontal ? i : 0);
                if (r < 0 || r >= 12 || c < 0 || c >= 12)
                    return new PlaceShipResultViewModel { Success = false, Message = "Fuera de rango" };
                if (occupied.Contains((r, c)))
                    return new PlaceShipResultViewModel { Success = false, Message = "Solapamiento" };
                coords.Add((r, c));
            }

            var ship = new Ship
            {
                GameId = vm.GameId,
                PlayerId = me,
                Size = vm.Size,
                IsSunk = false
            };
            var created = await _shipRepo.AddAsync(ship);
            foreach (var (r, c) in coords)
                await _posRepo.AddAsync(new ShipPosition { ShipId = created.Id, Row = r, Column = c });

            var board = await GetBoardAsync(vm.GameId);
            return new PlaceShipResultViewModel
            {
                Success = true,
                Message = "Barco colocado",
                UpdatedBoard = board
            };
        }

        public async Task<AttackResultViewModel> AttackAsync(AttackViewModel vm)
        {
            var me = CurrentUserId();
            var game = await _gameRepo.GetByIdAsync(vm.GameId);
            if (game == null) return null;

            var opponent = game.Player1Id == me ? game.Player2Id : game.Player1Id;

            // registrar ataque
            var atk = new Attack
            {
                GameId = vm.GameId,
                AttackerId = me,
                Row = vm.Row,
                Column = vm.Col
            };
            await _attackRepo.AddAsync(atk);

            // determinar si impacto
            var oppShipIds = (await _shipRepo.GetAllByGameIdAsync(vm.GameId))
                             .Where(s => s.PlayerId != me)
                             .Select(s => s.Id)
                             .ToHashSet();

            var oppPos = (await _posRepo.GetAllByGameIdAsync(vm.GameId))
                         .Where(p => oppShipIds.Contains(p.ShipId))
                         .ToList();

            bool isHit = oppPos.Any(p => p.Row == vm.Row && p.Column == vm.Col);
            atk.IsHit = isHit;

            if (isHit)
            {
                var hitShipId = oppPos.First(p => p.Row == vm.Row && p.Column == vm.Col).ShipId;
                var allPos = await _posRepo.GetAllByShipIdAsync(hitShipId);
                var allAtt = await _attackRepo.GetAllByGameIdAsync(vm.GameId);

                if (allPos.All(pp => allAtt.Any(a => a.Row == pp.Row && a.Column == pp.Column && a.IsHit)))
                {
                    var ship = await _shipRepo.GetByIdAsync(hitShipId);
                    ship.IsSunk = true;
                    await _shipRepo.UpdateAsync(ship);
                }
            }

            // verificar fin de partida
            bool gameFinished = false;
            if (isHit)
            {
                var remainingOpp = (await _shipRepo.GetAllByGameIdAsync(vm.GameId))
                                   .Where(s => s.PlayerId != me)
                                   .Count(s => !s.IsSunk);
                if (remainingOpp == 0)
                {
                    game.WinnerId = me;
                    game.FinishedAt = DateTime.UtcNow;
                    gameFinished = true;
                }
            }

            
            if (!isHit && !gameFinished)
            {
                game.CurrentTurnPlayerId = opponent;
            }

            await _gameRepo.UpdateAsync(game);

            var board = await GetBoardAsync(vm.GameId);
            return new AttackResultViewModel
            {
                IsHit = isHit,
                UpdatedBoard = board,
                WinnerId = board.WinnerId
            };
        }

        public async Task PlaceFleetAsync(FleetSetupDto dto)
        {
            var me = dto.PlayerId;
            var old = (await _shipRepo.GetAllByGameIdAsync(dto.GameId))
                      .Where(s => s.PlayerId == me)
                      .ToList();

            foreach (var s in old)
            {
                var poses = await _posRepo.GetAllByShipIdAsync(s.Id);
                foreach (var p in poses)
                    await _posRepo.DeleteAsync(p);
                await _shipRepo.DeleteAsync(s);
            }

            foreach (var fs in dto.Ships)
            {
                var ship = new Ship
                {
                    GameId = dto.GameId,
                    PlayerId = me,
                    Size = fs.Size,
                    IsSunk = false
                };
                var created = await _shipRepo.AddAsync(ship);
                foreach (var c in fs.Coords)
                    await _posRepo.AddAsync(new ShipPosition { ShipId = created.Id, Row = c.Row, Column = c.Col });
            }
        }

        public async Task SurrenderAsync(int gameId)
        {
            var me = CurrentUserId();
            var game = await _gameRepo.GetByIdAsync(gameId);
            if (game == null || game.WinnerId != null) return;

            var opponent = game.Player1Id == me ? game.Player2Id : game.Player1Id;
            game.WinnerId = opponent;
            game.FinishedAt = DateTime.UtcNow;
            await _gameRepo.UpdateAsync(game);
        }

        public async Task<GameResultViewModel> GetResultAsync(int gameId)
        {
            throw new NotImplementedException();
        }
    }
}
