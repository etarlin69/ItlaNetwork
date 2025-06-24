using ItlaNetwork.Core.Application.ViewModels.Post;
using ItlaNetwork.Core.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ItlaNetwork.Core.Application.Interfaces.Services
{
    public interface IPostService : IGenericService<SavePostViewModel, PostViewModel, Post>
    {
        // El método 'Add' ya estaba correcto, recibiendo el userId.
        Task<SavePostViewModel> Add(SavePostViewModel vm, string userId);

        // El método 'GetAllViewModel' también debe recibir el userId para poder
        // calcular las reacciones del usuario actual.
        Task<List<PostViewModel>> GetAllViewModel(string userId);
    }
}