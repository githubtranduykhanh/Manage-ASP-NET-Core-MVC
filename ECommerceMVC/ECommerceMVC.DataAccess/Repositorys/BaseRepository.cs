using ECommerceMVC.DataAccess.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceMVC.DataAccess.Repositorys
{
    public class BaseRepository<T> where T : class
    {
        protected readonly ECommerceContext _context;
        public BaseRepository(ECommerceContext context)
        {
            _context = context;
        }
        public IQueryable<T> GetAllQueryable(Expression<Func<T, bool>> expression = null,Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null)
        {
            IQueryable<T> query = _context.Set<T>();

            if (include != null)
            {
                query = include(query);
            }

            if (expression != null)
            {
                query = query.Where(expression);
            }

            return query;
        }

        public async Task<IEnumerable<T>> GetAllAsync(Expression<Func<T,bool>> expression = null)
        {
            if(expression is null)
            {
                return await _context.Set<T>().ToListAsync();
            }
            
            return await _context.Set<T>().Where(expression).ToListAsync();
        }

        public async Task<IEnumerable<T>> GetAllIncludingAsync(Expression<Func<T, bool>> expression = null, Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null)
        {
            IQueryable<T> query = _context.Set<T>();

            if (include != null)
            {
                query = include(query);
            }

            if (expression != null)
            {
                query = query.Where(expression);
            }

            return await query.ToListAsync();
        }


        public async Task<T?> GetSingleAsync(Expression<Func<T, bool>> expression = null)
        {
            return await _context.Set<T>().SingleOrDefaultAsync(expression);
        }
        public async Task CreateAsync(T entity)
        {
            await _context.Set<T>().AddAsync(entity);
        }
        public void UpdateAsync(T entity)
        {
            _context.Set<T>().Attach(entity);
            _context.Set<T>().Entry(entity).State = EntityState.Modified;
        }

        public void DeleteAsync(T entity)
        {
            _context.Set<T>().Attach(entity);
            _context.Set<T>().Entry(entity).State = EntityState.Deleted;
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
