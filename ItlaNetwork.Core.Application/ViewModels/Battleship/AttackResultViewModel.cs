namespace ItlaNetwork.Core.Application.ViewModels.Battleship
{
    public class AttackResultViewModel
    {
        public bool IsHit { get; set; }
        public BoardViewModel UpdatedBoard { get; set; }
        
        public string WinnerId { get; set; }
    }
}
