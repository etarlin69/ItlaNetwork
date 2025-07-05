namespace ItlaNetwork.Core.Application.ViewModels.Battleship
{
    public class PlaceShipResultViewModel
    {
        
        public bool Success { get; set; }
        public string Message { get; set; }

        public BoardViewModel UpdatedBoard { get; set; }
    }
}
