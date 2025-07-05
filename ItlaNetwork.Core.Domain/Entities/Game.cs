// ItlaNetwork.Core.Domain/Entities/Game.cs
using ItlaNetwork.Core.Domain.Common;
using System;

namespace ItlaNetwork.Core.Domain.Entities
{
    
    public class Game : AuditableBaseEntity
    {
        public string Player1Id { get; set; }
        public string Player2Id { get; set; }
        public string CurrentTurnPlayerId { get; set; }

        
        public string WinnerId { get; set; }

        
        public DateTime CreatedAt { get; set; }

        
        public DateTime? FinishedAt { get; set; }
    }
}
