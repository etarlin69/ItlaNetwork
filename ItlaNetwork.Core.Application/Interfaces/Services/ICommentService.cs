using ItlaNetwork.Core.Application.ViewModels.Comment;
using ItlaNetwork.Core.Domain.Entities;

namespace ItlaNetwork.Core.Application.Interfaces.Services
{
    public interface ICommentService : IGenericService<SaveCommentViewModel, CommentViewModel, Comment>
    {
        // --- CORRECCIÓN ---
        // El método Add ahora devuelve un CommentViewModel para que el controlador
        // pueda renderizar la vista parcial con los datos completos del autor.
        new Task<CommentViewModel> Add(SaveCommentViewModel vm);
    }
}