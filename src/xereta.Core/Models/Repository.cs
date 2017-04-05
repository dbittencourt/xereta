using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using xereta.Core.Data;

namespace xereta.Core.Models
{
    public class Repository<T> : IRepository<T> where T : BaseEntity
    {
        private readonly ApplicationDbContext context;
        private DbSet<T> entities;

        public Repository(ApplicationDbContext context)
        {
            this.context = context;
            entities = context.Set<T>();
        }

        public async Task AddAsync(T entity)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");

                await entities.AddAsync(entity);
                await context.SaveChangesAsync();
        }

        public async Task AddRangeAsync(IEnumerable<T> newEntities)
        {
            try
            {
                if (newEntities == null)
                throw new ArgumentException("entity");
                    
                await entities.AddRangeAsync(newEntities);
                await context.SaveChangesAsync();
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public async Task<T> GetAsync(string id)
        {
            return await entities.AsNoTracking().SingleOrDefaultAsync(e => e.Id == id);  
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await Task.FromResult(entities.AsNoTracking().ToListAsync() as IEnumerable<T>);
        }

        public async Task<IEnumerable<T>> GetAllWithIdAsync(string id)
        {
            return (await entities.AsNoTracking().ToListAsync()).FindAll(entity => entity.Id.Equals(id));
        }

        public async Task UpdateAsync(T entity)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");

            entities.Update(entity);
            await context.SaveChangesAsync();
        }

        public async Task DeleteAsync(T entity)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");
            
            entities.Remove(entity);
            await context.SaveChangesAsync();
        } 
    }
}