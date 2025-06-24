using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ItlaNetwork.Core.Domain.Common;

namespace ItlaNetwork.Core.Domain.Entities
{
    public class Game : AuditableBaseEntity
    {

        public int GameStatus { get; set; }
        public string Player1Id { get; set; }
        public string Player2Id { get; set; }
        public string CurrentTurnPlayerId { get; set; }
        public string? WinnerId { get; set; }



        public User Player1 { get; set; }
        public User Player2 { get; set; }
        public User? CurrentTurnPlayer { get; set; }
        public User? Winner { get; set; }


        public ICollection<Ship> Ships {get; set; } = new HashSet<Ship>();
        public ICollection<Attack> Attacks {get; set; } = new HashSet<Attack>();

    }
}
