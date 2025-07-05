
using System;

namespace ItlaNetwork.Core.Application.ViewModels.Battleship
{
    
    public class GameResultViewModel
    {
        
        public int GameId { get; set; }

        
        public string OpponentId { get; set; }

        
        public string OpponentName { get; set; }

        
        public string WinnerId { get; set; }

        
        public DateTime CreatedAt { get; set; }

        
        public DateTime FinishedAt { get; set; }

       
        public TimeSpan Duration => FinishedAt - CreatedAt;

        
        public int[,] AttackGrid { get; set; }

        
        public int[,] MyPlacementGrid { get; set; }

        
        public int[,] OpponentPlacementGrid { get; set; }
    }
}
