using System.ComponentModel.DataAnnotations;

namespace ItlaNetwork.Core.Application.ViewModels.Battleship
{
    
    public class AttackViewModel
    {
        [Required] public int GameId { get; set; }
        [Required] public int Row { get; set; }
        [Required] public int Col { get; set; }
    }
}
