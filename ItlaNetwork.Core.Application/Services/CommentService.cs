using AutoMapper;
using ItlaNetwork.Core.Application.Interfaces.Repositories;
using ItlaNetwork.Core.Application.Interfaces.Services;
using ItlaNetwork.Core.Application.ViewModels.Comment;
using ItlaNetwork.Core.Domain.Entities;
using System;
using System.Threading.Tasks;

namespace ItlaNetwork.Core.Application.Services
{
    public class CommentService : ICommentService
    {
        private readonly IGenericRepository<Comment> _commentRepository;
        private readonly IMapper _mapper;

        public CommentService(IGenericRepository<Comment> commentRepository, IMapper mapper)
        {
            _commentRepository = commentRepository;
            _mapper = mapper;
        }

        public async Task<CommentViewModel> Add(SaveCommentViewModel vm, string userId)
        {
            var comment = _mapper.Map<Comment>(vm);
            comment.UserId = userId;
            comment.CreatedAt = DateTime.Now;

            comment = await _commentRepository.AddAsync(comment);

            // Volvemos a buscar el comentario, pero esta vez incluyendo los datos del autor (User)
            var commentWithUser = await _commentRepository.GetByIdWithIncludeAsync(comment.Id, new List<string> { "User" });

            // Mapeamos la entidad completa al ViewModel de vista y lo devolvemos
            return _mapper.Map<CommentViewModel>(commentWithUser);
        }
    }
}