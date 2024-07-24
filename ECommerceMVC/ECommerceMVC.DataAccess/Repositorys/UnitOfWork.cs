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

        private INewCategoriesRepositorie? _newCategoriesRepositorie;

        private IInvoiceDatailsRepositorie? _invoiceDatailsRepositorie;

        private IInvoiceRepositorie? _invoiceRepositorie;

        private IOrderDetailsRepositorie? _orderDetailsRepositorie;

        private IOrderRepositorie? _orderRepositorie;
        public UnitOfWork(ECommerceContext context)
        {
            _context = context;
        }


        public IProductRepositorie ProductRepositorie => _productRepositorie ??= new ProductRepositorie(_context);
        public INewCategoriesRepositorie NewCategoriesRepositorie => _newCategoriesRepositorie ??= new NewCategoriesRepositorie(_context);
        public IInvoiceDatailsRepositorie InvoiceDatailsRepositorie => _invoiceDatailsRepositorie ??= new InvoiceDatailsRepositorie(_context);
        public IInvoiceRepositorie InvoiceRepositorie => _invoiceRepositorie ??= new InvoiceRepositorie(_context);
        public IOrderDetailsRepositorie OrderDetailsRepositorie => _orderDetailsRepositorie ??= new OrderDetailsRepositorie(_context);
        public IOrderRepositorie OrderRepositorie => _orderRepositorie ??= new OrderRepositorie(_context);

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
