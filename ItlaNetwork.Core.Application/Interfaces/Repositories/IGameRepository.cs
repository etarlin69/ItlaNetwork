﻿using ItlaNetwork.Core.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;
namespace ItlaNetwork.Core.Application.Interfaces.Repositories
{
    public interface IGameRepository : IGenericRepository<Game>
    {
        Task<List<Game>> GetAllByPlayerIdAsync(string playerId);
    }
}