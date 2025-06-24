using ItlaNetwork.Core.Domain.Common;
using System.Collections.Generic;

namespace ItlaNetwork.Core.Domain.Entities
{
    public class Ship : AuditableBaseEntity
    {
        // Example: 5 for Carrier, 4 for Battleship, etc.
        public int ShipType { get; set; }
        public bool IsSunk { get; set; } = false;

        // Foreign Keys
        public int GameId { get; set; }
        public string PlayerId { get; set; }

        // Navigation Properties
        public Game Game { get; set; }
        public User Player { get; set; }
        public ICollection<ShipPosition> Positions { get; set; } = new HashSet<ShipPosition>();
    }
}