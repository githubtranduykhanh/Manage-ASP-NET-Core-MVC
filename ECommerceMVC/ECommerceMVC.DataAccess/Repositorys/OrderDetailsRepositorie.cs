using ECommerceMVC.DataAccess.Data;
using ECommerceMVC.Domain.Abstract;
using ECommerceMVC.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceMVC.DataAccess.Repositorys
{
    public class OrderDetailsRepositorie : GenericRepository<DbOrderDetail>, IOrderDetailsRepositorie
    {

        private readonly ECommerceContext _context;
        public OrderDetailsRepositorie(ECommerceContext context) : base(context) {
            _context = context;
        }


        // Thêm phương thức RemoveRange
        public void RemoveRange(IEnumerable<DbOrderDetail> entities)
        {
            // Xóa các đối tượng từ DbSet
            _context.Set<DbOrderDetail>().RemoveRange(entities);
        }
    }
}
