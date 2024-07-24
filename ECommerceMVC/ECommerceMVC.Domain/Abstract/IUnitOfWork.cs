using ECommerceMVC.Domain.Abstract;
using Microsoft.EntityFrameworkCore;

namespace ECommerceMVC.Domain.Abstract
{
    public interface IUnitOfWork
    {
        IProductRepositorie ProductRepositorie { get; }
        INewCategoriesRepositorie? NewCategoriesRepositorie { get; }



        public IInvoiceDatailsRepositorie InvoiceDatailsRepositorie { get; }

        public IInvoiceRepositorie InvoiceRepositorie { get; }
        public IOrderDetailsRepositorie OrderDetailsRepositorie { get; }
        public IOrderRepositorie OrderRepositorie { get; }

        Task SaveChangesAsync();
    }
}