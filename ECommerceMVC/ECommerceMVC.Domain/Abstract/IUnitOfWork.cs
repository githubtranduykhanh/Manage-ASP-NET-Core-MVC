using ECommerceMVC.Domain.Abstract;

namespace ECommerceMVC.Domain.Abstract
{
    public interface IUnitOfWork
    {
        IProductRepositorie ProductRepositorie { get; }

        Task SaveChangesAsync();
    }
}