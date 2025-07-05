using System.Collections.Generic;
using ItlaNetwork.Core.Application.ViewModels.Friendship;
using ItlaNetwork.Core.Application.ViewModels.Battleship;

namespace ItlaNetwork.Core.Application.ViewModels.Battleship
{
    public class BattleshipIndexViewModel
    {
        
        public List<FriendViewModel> Friends { get; set; }

        
        public List<GameSummaryViewModel> Games { get; set; }

        
        public List<GameRequestViewModel> IncomingRequests { get; set; }
    }
}
