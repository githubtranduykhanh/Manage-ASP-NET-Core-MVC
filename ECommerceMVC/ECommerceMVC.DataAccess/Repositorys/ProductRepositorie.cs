using ECommerceMVC.DataAccess.Data;
using ECommerceMVC.Domain.Abstract;
using ECommerceMVC.Domain.Entities;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceMVC.DataAccess.Repositorys
{
    public class ProductRepositorie : BaseRepository<DbProduct>, IProductRepositorie
    {
        
        public ProductRepositorie(ECommerceContext context) : base(context) { }

        public IQueryable<DbProduct> GetAllQueryable(Expression<Func<DbProduct, bool>> expression = null, Func<IQueryable<DbProduct>, IIncludableQueryable<DbProduct, object>> include = null)
        {
            var query = base.GetAllQueryable(expression, include);
            return query;
        }

        public async Task<IEnumerable<DbProduct>> GetAllIncludingAsync(Expression<Func<DbProduct, bool>> expression = null, Func<IQueryable<DbProduct>, IIncludableQueryable<DbProduct, object>> include = null)
        {
            return await base.GetAllIncludingAsync(expression, include);
        }
        public async Task<IEnumerable<DbProduct>> GetAllAsync(Expression<Func<DbProduct, bool>>? expression = null)
        {
            return await base.GetAllAsync(expression);
        }

        public async Task<DbProduct?> GetSingleAsync(Expression<Func<DbProduct, bool>> expression = null)
        {
            return await base.GetSingleAsync(expression);
        }

        public async Task CreateAsync(DbProduct entity)
        {
            await base.CreateAsync(entity);
        }

        public void UpdateAsync(DbProduct entity)
        {
            base.UpdateAsync(entity);
        }

        public void DeleteAsync(DbProduct entity)
        {
            base.DeleteAsync(entity);
        }

        public void SaveChangesAsync()
        {
            base.SaveChangesAsync();
        }
    }
}
