using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace xereta.Models
{
    public class Repository<T> : IRepository<T> where T : BaseEntity
    {
        private readonly DbContext context;
        private DbSet<T> entities;

        public Repository(DbContext context)
        {
            this.context = context;
            entities = context.Set<T>();
        }

        public async void AddAsync(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
                await entities.AddAsync(entity);
                await context.SaveChangesAsync();
        }

        public async Task<T> GetAsync(string id)
        {
            return await entities.SingleOrDefaultAsync(e => e.Id == id);  
        }

        public IEnumerable<T> GetAll()
        {
            return entities;
        }

        public async void UpdateAsync(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }

            entities.Update(entity);
            await context.SaveChangesAsync();
        }

        public async void DeleteAsync(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            entities.Remove(entity);
            await context.SaveChangesAsync();
        } 
    }
}