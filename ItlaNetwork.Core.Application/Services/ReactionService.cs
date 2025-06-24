using AutoMapper;
using ItlaNetwork.Core.Application.Interfaces.Repositories;
using ItlaNetwork.Core.Application.Interfaces.Services;
using ItlaNetwork.Core.Application.ViewModels.Reaction;
using ItlaNetwork.Core.Domain.Entities;
using System.Linq;
using System.Threading.Tasks;

namespace ItlaNetwork.Core.Application.Services
{
    public class ReactionService : IReactionService
    {
        private readonly IGenericRepository<Reaction> _reactionRepository;
        private readonly IMapper _mapper;

        public ReactionService(IGenericRepository<Reaction> reactionRepository, IMapper mapper)
        {
            _reactionRepository = reactionRepository;
            _mapper = mapper;
        }

        public async Task ToggleReactionAsync(SaveReactionViewModel vm, string userId)
        {
            // Busca si ya existe una reacción del mismo usuario para el mismo post
            var reactions = await _reactionRepository.GetAllAsync();
            var existingReaction = reactions.FirstOrDefault(r => r.UserId == userId && r.PostId == vm.PostId);

            if (existingReaction != null)
            {
                // Si el usuario vuelve a hacer clic en la misma reacción (ej. "Me gusta" otra vez), se elimina.
                if (existingReaction.ReactionType == (int)vm.ReactionType)
                {
                    await _reactionRepository.DeleteAsync(existingReaction);
                }
                else // Si hace clic en la otra reacción (ej. de "Me gusta" a "No me gusta"), se actualiza.
                {
                    existingReaction.ReactionType = (int)vm.ReactionType;
                    await _reactionRepository.UpdateAsync(existingReaction);
                }
            }
            else // Si no existe ninguna reacción previa, se crea una nueva.
            {
                var reaction = _mapper.Map<Reaction>(vm);
                reaction.UserId = userId;
                await _reactionRepository.AddAsync(reaction);
            }
        }

        public async Task<List<Reaction>> GetReactionsByPostIdAsync(int postId)
        {
            var allReactions = await _reactionRepository.GetAllAsync();
            return allReactions.Where(r => r.PostId == postId).ToList();
        }
    }
}