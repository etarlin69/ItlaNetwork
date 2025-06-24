using System.Collections.Generic;
using System.Threading.Tasks;

namespace ItlaNetwork.Core.Application.Interfaces.Services
{
    public interface IGenericService<SaveViewModel, ViewModel, Entity>
        where SaveViewModel : class
        where ViewModel : class
        where Entity : class
    {
        // El método Add ya fue eliminado correctamente.
        Task Update(SaveViewModel vm, int id);
        Task Delete(int id);
        Task<SaveViewModel> GetByIdSaveViewModel(int id);

        // Se ELIMINA el método GetAllViewModel de la interfaz genérica.
        // Task<List<ViewModel>> GetAllViewModel(); 
    }
}