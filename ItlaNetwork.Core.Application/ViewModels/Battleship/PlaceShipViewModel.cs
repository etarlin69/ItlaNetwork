namespace ItlaNetwork.Core.Application.ViewModels.Battleship
{
    public class PlaceShipViewModel
    {
        public int GameId { get; set; }
        public int Row { get; set; }
        public int Col { get; set; }
        public int Size { get; set; }
        public bool Horizontal { get; set; }
    }
}