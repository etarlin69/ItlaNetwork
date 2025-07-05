
namespace ItlaNetwork.Core.Application.ViewModels.Battleship
{
    public class GameSummaryViewModel
    {
        public int GameId { get; set; }
        public string OpponentId { get; set; }
        public string OpponentName { get; set; }
        public bool IsMyTurn { get; set; }
        public bool IsFinished { get; set; }
        public string WinnerId { get; set; }
    }
}
