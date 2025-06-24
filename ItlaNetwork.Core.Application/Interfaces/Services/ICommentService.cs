using ItlaNetwork.Core.Application.ViewModels.Comment;
using System.Threading.Tasks;

namespace ItlaNetwork.Core.Application.Interfaces.Services
{
    public interface ICommentService
    {
        // Definimos un método para añadir un comentario, que necesitará
        // el ViewModel y el ID del usuario que está comentando.
        Task<CommentViewModel> Add(SaveCommentViewModel vm, string userId);
    }
}