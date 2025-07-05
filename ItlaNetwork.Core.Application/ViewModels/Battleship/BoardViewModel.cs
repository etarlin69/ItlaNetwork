namespace ItlaNetwork.Core.Application.ViewModels.Battleship
{
    
    public class BoardViewModel
    {
        
        public int GameId { get; set; }

        
        public int[,] Grid { get; set; }

        
        public bool MyFleetPlaced { get; set; }

        
        public bool OpponentFleetPlaced { get; set; }

        
        public bool IsMyTurn { get; set; }

        
        public string WinnerId { get; set; }
    }
}
