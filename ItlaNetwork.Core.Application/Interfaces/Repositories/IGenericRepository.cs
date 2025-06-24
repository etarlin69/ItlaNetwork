using ItlaNetwork.Core.Domain.Common;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Linq.Expressions;

namespace ItlaNetwork.Core.Application.Interfaces.Repositories
{
    // La restricción 'where T : AuditableBaseEntity' asegura que esta interfaz
    // solo se pueda usar con nuestras clases de entidad que heredan de AuditableBaseEntity.
    public interface IGenericRepository<T> where T : AuditableBaseEntity
    {
        
        Task<T> AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(T entity);
        Task<List<T>> GetAllAsync();
        Task<T> GetByIdAsync(int id);
        Task<List<T>> GetAllWithIncludeAsync(List<string> properties);
        Task<T> GetByIdWithIncludeAsync(int id, List<string> properties);
        Task<T> FindFirstOrDefaultAsync(Expression<Func<T, bool>> filter);
    }
}