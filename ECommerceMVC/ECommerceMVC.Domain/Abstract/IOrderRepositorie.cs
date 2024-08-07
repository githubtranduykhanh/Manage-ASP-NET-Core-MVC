using ECommerceMVC.Domain.Entities;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;


namespace ECommerceMVC.Domain.Abstract
{
    public interface IOrderRepositorie : IGenericRepository<DbOrder>
    {       
        Task<IQueryable<DbOrder>> GetOrdersWithDetailsAndImagesAsync();
        Task<DbOrder?> GetOrderWithDetailsAsync(int id);
    }
}