using ECommerceMVC.DataAccess.Data;
using ECommerceMVC.Domain.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceMVC.DataAccess.Repositorys
{
    public class UnitOfWork : IUnitOfWork , IDisposable
    {
        protected readonly ECommerceContext _context;

        private IProductRepositorie? _productRepositorie;
        public UnitOfWork(ECommerceContext context)
        {
            _context = context;
        }


        public IProductRepositorie ProductRepositorie => _productRepositorie ??= new ProductRepositorie(_context);

       

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
            
        }

        public void Dispose()
        {
           if(_context != null ) _context.Dispose();
        }

    }
}
