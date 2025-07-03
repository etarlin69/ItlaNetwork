using AutoMapper;
using ItlaNetwork.Core.Application.Interfaces.Repositories;
using ItlaNetwork.Core.Application.Interfaces.Services;
using ItlaNetwork.Core.Application.ViewModels.Post;
using ItlaNetwork.Core.Application.ViewModels.Comment;
using ItlaNetwork.Core.Domain.Entities;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ItlaNetwork.Core.Application.Services
{
    public class PostService : IPostService
    {
        private readonly IPostRepository _postRepository;
        private readonly ICommentRepository _commentRepository;
        private readonly IReactionRepository _reactionRepository;
        private readonly IAccountService _accountService;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public PostService(
            IPostRepository postRepository,
            ICommentRepository commentRepository,
            IReactionRepository reactionRepository,
            IAccountService accountService,
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor)
        {
            _postRepository = postRepository;
            _commentRepository = commentRepository;
            _reactionRepository = reactionRepository;
            _accountService = accountService;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<SavePostViewModel> Add(SavePostViewModel vm)
        {
            var currentUserId = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(currentUserId)) return null;

            // Evitar que Content sea null al guardar en BD
            if (string.IsNullOrWhiteSpace(vm.Content))
            {
                vm.Content = "";
            }

            var post = _mapper.Map<Post>(vm);
            post.UserId = currentUserId;

            post = await _postRepository.AddAsync(post);
            return _mapper.Map<SavePostViewModel>(post);
        }

        public async Task Update(SavePostViewModel vm)
        {
            var existingPost = await _postRepository.GetByIdAsync(vm.Id);
            if (existingPost == null) return;

            var updatedPost = _mapper.Map<Post>(vm);
            updatedPost.UserId = existingPost.UserId; // preservar autor original

            // Asegurar que Content no sea null
            if (string.IsNullOrWhiteSpace(updatedPost.Content))
            {
                updatedPost.Content = "";
            }

            await _postRepository.UpdateAsync(updatedPost);
        }

        public async Task Delete(int id)
        {
            var post = await _postRepository.GetByIdAsync(id);
            if (post != null)
                await _postRepository.DeleteAsync(post);
        }

        public async Task<SavePostViewModel> GetByIdSaveViewModel(int id)
        {
            var post = await _postRepository.GetByIdAsync(id);
            return _mapper.Map<SavePostViewModel>(post);
        }

        public async Task<string> GetPostOwnerIdById(int id)
        {
            var post = await _postRepository.GetByIdAsync(id);
            return post?.UserId;
        }

        public async Task<List<PostViewModel>> GetAllViewModel()
        {
            var currentUserId = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            var postList = await _postRepository.GetAllAsync();

            if (postList == null || !postList.Any())
                return new List<PostViewModel>();

            var postViewModels = _mapper.Map<List<PostViewModel>>(postList);
            var postIds = postList.Select(p => p.Id).ToList();

            var allComments = await _commentRepository.GetAllByPostIdListAsync(postIds);
            var allReactions = await _reactionRepository.GetAllByPostIdListAsync(postIds);
            var allUserIds = postList.Select(p => p.UserId)
                                     .Union(allComments.Select(c => c.UserId))
                                     .Distinct().ToList();
            var users = await _accountService.GetUsersByIdsAsync(allUserIds);

            foreach (var postVm in postViewModels)
            {
                var author = users.FirstOrDefault(u => u.Id == postVm.UserId);
                if (author != null)
                {
                    postVm.AuthorFullName = $"{author.FirstName} {author.LastName}";
                    postVm.AuthorUserName = author.UserName;
                    postVm.AuthorProfilePictureUrl = author.ProfilePictureUrl;
                }

                var postComments = allComments.Where(c => c.PostId == postVm.Id).ToList();
                postVm.Comments = postComments.Select(comment =>
                {
                    var commentVm = _mapper.Map<CommentViewModel>(comment);
                    var commentAuthor = users.FirstOrDefault(u => u.Id == comment.UserId);
                    if (commentAuthor != null)
                    {
                        commentVm.AuthorFullName = $"{commentAuthor.FirstName} {commentAuthor.LastName}";
                        commentVm.AuthorProfilePictureUrl = commentAuthor.ProfilePictureUrl;
                    }
                    return commentVm;
                }).ToList();

                var postReactions = allReactions.Where(r => r.PostId == postVm.Id).ToList();
                postVm.LikeCount = postReactions.Count(r => r.ReactionType == Core.Domain.Enums.ReactionType.Like);
                postVm.DislikeCount = postReactions.Count(r => r.ReactionType == Core.Domain.Enums.ReactionType.Dislike);
                var currentUserReaction = postReactions.FirstOrDefault(r => r.UserId == currentUserId);
                postVm.CurrentUserReaction = currentUserReaction?.ReactionType;
            }

            return postViewModels.OrderByDescending(p => p.CreatedAt).ToList();
        }
    }
}
