using ECommerceMVC.Domain.Entities;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;


namespace ECommerceMVC.Domain.Abstract
{
    public interface IOrderDetailsRepositorie : IGenericRepository<DbOrderDetail>
    {
        void RemoveRange(IEnumerable<DbOrderDetail> entities);
    }
}