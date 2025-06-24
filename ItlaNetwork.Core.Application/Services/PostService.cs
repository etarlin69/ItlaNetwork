using AutoMapper;
using ItlaNetwork.Core.Application.Enums;
using ItlaNetwork.Core.Application.Interfaces.Repositories;
using ItlaNetwork.Core.Application.Interfaces.Services;
using ItlaNetwork.Core.Application.ViewModels.Post;
using ItlaNetwork.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ItlaNetwork.Core.Application.Services
{
    public class PostService : IPostService
    {
        private readonly IGenericRepository<Post> _postRepository;
        private readonly IReactionService _reactionService;
        private readonly IMapper _mapper;

        // SE ELIMINÓ IHttpContextAccessor DEL CONSTRUCTOR Y DE LA CLASE
        public PostService(IGenericRepository<Post> postRepository, IReactionService reactionService, IMapper mapper)
        {
            _postRepository = postRepository;
            _reactionService = reactionService;
            _mapper = mapper;
        }

        public async Task<SavePostViewModel> Add(SavePostViewModel vm, string userId)
        {
            var post = _mapper.Map<Post>(vm);
            post.UserId = userId;
            post.CreatedAt = DateTime.Now;
            post = await _postRepository.AddAsync(post);
            return _mapper.Map<SavePostViewModel>(post);
        }

        public async Task Update(SavePostViewModel vm, int id)
        {
            // La lógica de update podría necesitar el userId para validaciones de permiso en el futuro
            var post = await _postRepository.GetByIdAsync(id);
            post.Content = vm.Content;
            post.ImageUrl = vm.ImageUrl;
            await _postRepository.UpdateAsync(post);
        }

        public async Task Delete(int id)
        {
            var post = await _postRepository.GetByIdAsync(id);
            await _postRepository.DeleteAsync(post);
        }

        public async Task<SavePostViewModel> GetByIdSaveViewModel(int id)
        {
            var post = await _postRepository.GetByIdAsync(id);
            return _mapper.Map<SavePostViewModel>(post);
        }

        // MÉTODO CORREGIDO: ahora usa el userId del parámetro
        public async Task<List<PostViewModel>> GetAllViewModel(string userId)
        {
            var posts = await _postRepository.GetAllWithIncludeAsync(new List<string> { "User", "Comments.User" });
            var postViewModels = _mapper.Map<List<PostViewModel>>(posts.OrderByDescending(p => p.CreatedAt));

            foreach (var vm in postViewModels)
            {
                var reactions = await _reactionService.GetReactionsByPostIdAsync(vm.Id);
                vm.LikeCount = reactions.Count(r => r.ReactionType == (int)ReactionType.Like);
                vm.DislikeCount = reactions.Count(r => r.ReactionType == (int)ReactionType.Dislike);

                var currentUserReaction = reactions.FirstOrDefault(r => r.UserId == userId);
                vm.CurrentUserReaction = currentUserReaction != null ? (ReactionType)currentUserReaction.ReactionType : null;
            }

            return postViewModels;
        }
    }
}