using System.Collections.Generic;
using System.Threading.Tasks;

namespace xereta.Models
{
    public interface IRepository<T> where T : BaseEntity
    {
        void AddAsync(T entity);
        
        Task<T> GetAsync(string id);

        IEnumerable<T> GetAll();

        void UpdateAsync(T entity);

        void DeleteAsync(T entity);
    }

}