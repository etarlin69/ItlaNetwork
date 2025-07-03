using ItlaNetwork.Core.Application.ViewModels.Post;
using ItlaNetwork.Core.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ItlaNetwork.Core.Application.Interfaces.Services
{
    /// <summary>
    /// Interfaz para el servicio de publicaciones.
    /// Hereda las operaciones CRUD básicas de IGenericService.
    /// </summary>
    public interface IPostService : IGenericService<SavePostViewModel, PostViewModel, Post>
    {
        // Esta interfaz hereda correctamente los siguientes métodos de IGenericService:
        // - Task<SavePostViewModel> Add(SavePostViewModel vm)
        // - Task Update(SavePostViewModel vm)
        // - Task Delete(int id)
        // - Task<SavePostViewModel> GetByIdSaveViewModel(int id)
        // - Task<List<PostViewModel>> GetAllViewModel()

        /// <summary>
        /// Devuelve el UserId del autor de un post específico.
        /// Usado para comprobaciones de autorización.
        /// </summary>
        /// <param name="id">Id del post</param>
        /// <returns>UserId del autor o null si no existe</returns>
        Task<string> GetPostOwnerIdById(int id);
    }
}
