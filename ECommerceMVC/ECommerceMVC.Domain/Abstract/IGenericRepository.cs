using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceMVC.Domain.Abstract
{
    public interface IGenericRepository<TEntity> where TEntity : class
    {
        // Asynchronous query method with filters, ordering, and includes
        Task<IQueryable<TEntity>> GetAsync(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            params Expression<Func<TEntity, object>>[] includeProperties);

        // Asynchronous method to get an entity by its ID
        Task<TEntity> GetByIDAsync(object id);

        // Asynchronous method to insert a new entity
        Task InsertAsync(TEntity entity);

        // Asynchronous method to delete an entity by its ID
        Task DeleteAsync(object id);

        // Asynchronous method to delete a specific entity
        void Delete(TEntity entityToDelete);

        // Asynchronous method to update an existing entity
        void Update(TEntity entityToUpdate);
    }
}
