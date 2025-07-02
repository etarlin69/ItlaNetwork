using ItlaNetwork.Core.Application.ViewModels.Post;
using ItlaNetwork.Core.Domain.Entities;

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

        // Puedes añadir aquí cualquier método que sea específico y exclusivo para los Posts
        // y que no esté definido en la interfaz genérica.
        // Por ahora, puede permanecer vacía.
    }
}