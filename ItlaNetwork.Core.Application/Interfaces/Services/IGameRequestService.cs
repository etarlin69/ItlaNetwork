using System.Collections.Generic;
using System.Threading.Tasks;
using ItlaNetwork.Core.Application.ViewModels.Battleship;

namespace ItlaNetwork.Core.Application.Interfaces.Services
{
    
    public interface IGameRequestService
    {
       
        Task SendRequestAsync(string receiverId);

        
        Task<List<GameRequestViewModel>> GetMyIncomingRequestsAsync();

        
        Task AcceptAsync(int requestId);

        
        Task RejectAsync(int requestId);
    }
}
