﻿using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using ItlaNetwork.Core.Application.Interfaces.Repositories;
using ItlaNetwork.Core.Application.Interfaces.Services;
using ItlaNetwork.Core.Application.ViewModels.Reaction;
using ItlaNetwork.Core.Domain.Entities;
using ItlaNetwork.Core.Domain.Enums;
using Microsoft.AspNetCore.Http;

namespace ItlaNetwork.Core.Application.Services
{
    public class ReactionService : IReactionService
    {
        private readonly IReactionRepository _reactionRepository;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ReactionService(IReactionRepository reactionRepository, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _reactionRepository = reactionRepository;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<ReactionCountViewModel> ToggleReactionAsync(SaveReactionViewModel vm)
        {
            var currentUserId = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(currentUserId)) return null;

            var existingReaction = await _reactionRepository.GetByPostAndUserIdAsync(vm.PostId, currentUserId);

            if (existingReaction != null)
            {
                if (existingReaction.ReactionType == vm.ReactionType)
                {
                    await _reactionRepository.DeleteAsync(existingReaction);
                }
                else
                {
                    existingReaction.ReactionType = vm.ReactionType;
                    await _reactionRepository.UpdateAsync(existingReaction);
                }
            }
            else
            {
                var reaction = _mapper.Map<Reaction>(vm);
                reaction.UserId = currentUserId;
                await _reactionRepository.AddAsync(reaction);
            }

            return await GetReactionCountsForPost(vm.PostId);
        }

        private async Task<ReactionCountViewModel> GetReactionCountsForPost(int postId)
        {
            var currentUserId = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            var reactions = await _reactionRepository.GetAllByPostIdAsync(postId);
            var currentUserReaction = reactions.FirstOrDefault(r => r.UserId == currentUserId);

            return new ReactionCountViewModel
            {
                LikeCount = reactions.Count(r => r.ReactionType == ReactionType.Like),
                DislikeCount = reactions.Count(r => r.ReactionType == ReactionType.Dislike),
                CurrentUserReaction = currentUserReaction?.ReactionType.ToString()
            };
        }
    }
}
