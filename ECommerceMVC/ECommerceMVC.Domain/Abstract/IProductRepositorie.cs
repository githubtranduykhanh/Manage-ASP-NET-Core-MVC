using ECommerceMVC.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceMVC.Domain.Abstract
{
    public interface IProductRepositorie 
    {
        Task<IEnumerable<DbProduct>> GetAllAsync(Expression<Func<DbProduct, bool>> expression = null);
        Task<DbProduct?> GetSingleAsync(Expression<Func<DbProduct, bool>> expression = null);
        Task CreateAsync(DbProduct entity);
        void UpdateAsync(DbProduct entity);
        void DeleteAsync(DbProduct entity);

        Task SaveChangesAsync();
    }
}
