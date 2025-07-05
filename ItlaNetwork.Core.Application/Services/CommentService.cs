using AutoMapper;
using ItlaNetwork.Core.Application.Interfaces.Repositories;
using ItlaNetwork.Core.Application.Interfaces.Services;
using ItlaNetwork.Core.Application.ViewModels.Comment;
using ItlaNetwork.Core.Domain.Entities;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ItlaNetwork.Core.Application.Services
{
    public class CommentService : ICommentService
    {
        private readonly ICommentRepository _commentRepository;
        private readonly IAccountService _accountService; 
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CommentService(ICommentRepository commentRepository, IAccountService accountService, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _commentRepository = commentRepository;
            _accountService = accountService;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        
        public new async Task<CommentViewModel> Add(SaveCommentViewModel vm)
        {
            var currentUserId = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(currentUserId)) return null;

            var comment = _mapper.Map<Comment>(vm);
            comment.UserId = currentUserId;

            comment = await _commentRepository.AddAsync(comment);

            
            var author = await _accountService.GetUserByIdAsync(currentUserId);
            var commentViewModel = _mapper.Map<CommentViewModel>(comment);

            if (author != null)
            {
                commentViewModel.AuthorFullName = $"{author.FirstName} {author.LastName}";
                commentViewModel.AuthorProfilePictureUrl = author.ProfilePictureUrl;
            }

            return commentViewModel;
        }

        
        async Task<SaveCommentViewModel> IGenericService<SaveCommentViewModel, CommentViewModel, Comment>.Add(SaveCommentViewModel vm)
        {
            var comment = _mapper.Map<Comment>(vm);
            var currentUserId = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            comment.UserId = currentUserId;
            comment = await _commentRepository.AddAsync(comment);
            return _mapper.Map<SaveCommentViewModel>(comment);
        }

        public async Task Update(SaveCommentViewModel vm)
        {
            var comment = _mapper.Map<Comment>(vm);
            var currentUserId = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            comment.UserId = currentUserId; 
            await _commentRepository.UpdateAsync(comment);
        }

        public async Task Delete(int id)
        {
            var comment = await _commentRepository.GetByIdAsync(id);
            await _commentRepository.DeleteAsync(comment);
        }

        public async Task<SaveCommentViewModel> GetByIdSaveViewModel(int id)
        {
            var comment = await _commentRepository.GetByIdAsync(id);
            return _mapper.Map<SaveCommentViewModel>(comment);
        }

        public async Task<List<CommentViewModel>> GetAllViewModel()
        {
            var comments = await _commentRepository.GetAllAsync();
            if (!comments.Any()) return new List<CommentViewModel>();

            var commentViewModels = _mapper.Map<List<CommentViewModel>>(comments);

            var userIds = comments.Select(c => c.UserId).Distinct().ToList();
            var users = await _accountService.GetUsersByIdsAsync(userIds);

            foreach (var commentVm in commentViewModels)
            {
                var author = users.FirstOrDefault(u => u.Id == commentVm.UserId);
                if (author != null)
                {
                    commentVm.AuthorFullName = $"{author.FirstName} {author.LastName}";
                    commentVm.AuthorProfilePictureUrl = author.ProfilePictureUrl;
                }
            }

            return commentViewModels;
        }
    }
}