using ECommerceMVC.DataAccess.Data;
using ECommerceMVC.Domain.Abstract;
using ECommerceMVC.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceMVC.DataAccess.Repositorys
{
    public class OrderRepositorie : GenericRepository<DbOrder>, IOrderRepositorie
    {
        public OrderRepositorie(ECommerceContext context) : base(context) { }


        public async Task<DbOrder?> GetOrderWithDetailsAsync(int id)
        {
            var order = await base.GetAsync(
                filter: o => o.Id == id,             
                includeProperties: new Expression<Func<DbOrder, object>>[]
                {
                    o => o.DbOrderDetails
                }
            );

            return order.FirstOrDefault(); // Lấy bản ghi đầu tiên từ kết quả (vì filter chỉ có một bản ghi)
        }
    }
}
