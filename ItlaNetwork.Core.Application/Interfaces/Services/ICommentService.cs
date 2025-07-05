using ItlaNetwork.Core.Application.ViewModels.Comment;
using ItlaNetwork.Core.Domain.Entities;

namespace ItlaNetwork.Core.Application.Interfaces.Services
{
    public interface ICommentService : IGenericService<SaveCommentViewModel, CommentViewModel, Comment>
    {
        
        new Task<CommentViewModel> Add(SaveCommentViewModel vm);
    }
}