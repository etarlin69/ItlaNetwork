
namespace ItlaNetwork.Core.Application.DTOs.Battleship
{
    public class FleetSetupDto
    {
        public int GameId { get; set; }
        public string PlayerId { get; set; }
        public List<ShipPlacementDto> Ships { get; set; }
    }

    public class ShipPlacementDto
    {
        public string Key { get; set; }         
        public int Size { get; set; }
        public List<CoordinateDto> Coords { get; set; }
    }

    public class CoordinateDto
    {
        public int Row { get; set; }
        public int Col { get; set; }
    }
}
