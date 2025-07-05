using ItlaNetwork.Core.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ItlaNetwork.Controllers
{
    [Authorize]
    public class GameRequestController : Controller
    {
        private readonly IGameRequestService _requestService;

        public GameRequestController(IGameRequestService requestService)
        {
            _requestService = requestService;
        }

        
        [HttpGet]
        public async Task<IActionResult> Send(string receiverId)
        {
            if (string.IsNullOrEmpty(receiverId))
            {
                TempData["Error"] = "Debes seleccionar un amigo a quien retar.";
                return RedirectToAction("Index", "Battleship");
            }

            await _requestService.SendRequestAsync(receiverId);
            TempData["Success"] = "Solicitud de partida enviada correctamente.";
            return RedirectToAction("Index", "Battleship");
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Accept(int id)
        {
            await _requestService.AcceptAsync(id);
            TempData["Success"] = "¡Partida iniciada!";
            return RedirectToAction("Index", "Battleship");
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Reject(int id)
        {
            await _requestService.RejectAsync(id);
            TempData["Error"] = "Solicitud rechazada.";
            return RedirectToAction("Index", "Battleship");
        }
    }
}
