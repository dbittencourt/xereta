using System.Collections.Generic;
using System.Threading.Tasks;

namespace xereta.Core.Models
{
    public interface IRepository<T> where T : BaseEntity
    {
        Task AddAsync(T entity);
        
        Task AddRangeAsync(IEnumerable<T> entities);

        Task<T> GetAsync(string id);

        Task<IEnumerable<T>> GetAllAsync();

        Task<IEnumerable<T>> GetAllWithIdAsync(string id);

        Task UpdateAsync(T entity);

        Task DeleteAsync(T entity);
    }

}