using ECommerceMVC.DataAccess.Data;
using ECommerceMVC.Domain.Abstract;
using ECommerceMVC.Domain.Entities;
using Microsoft.EntityFrameworkCore;
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

        public async Task<IQueryable<DbOrder>> GetOrdersWithDetailsAndImagesAsync()
        {
            return dbSet
                .Include(o => o.DbOrderDetails)
                    .ThenInclude(od => od.IdProductNavigation)
                        .ThenInclude(p => p.DbProductImages)
                            .ThenInclude(pi => pi.IdImageNavigation); // Đảm bảo bao gồm DbImage
        }


        public async Task<IQueryable<DbOrder>> GetOrdersAllRelationshipAsync()
        {
            return dbSet
                .Include(u => u.IdUserNavigation)
                .Include(o => o.DbOrderDetails)                 
                   .ThenInclude(od => od.IdProductNavigation.DbProductImages)
                   .ThenInclude(i => i.IdImageNavigation)
            ;
        }


        public async Task<DbOrder?> GetOrderByIDAllRelationshipAsync(int? id)
        {
            return await dbSet
                 .AsSplitQuery() // Sử dụng Split Query
                .Include(o => o.DbOrderDetails)  // Bao gồm các chi tiết đơn hàng
                                    .ThenInclude(od => od.IdProductNavigation) // Bao gồm sản phẩm trong chi tiết đơn hàng
                                        .ThenInclude(p => p.DbProductImages) // Bao gồm hình ảnh sản phẩm
                                            .ThenInclude(pi => pi.IdImageNavigation) // Bao gồm chi tiết của từng hình ảnh
                                .Include(o => o.DbOrderDetails)  // Bao gồm các chi tiết đơn hàng
                                    .ThenInclude(od => od.IdProductNavigation) // Bao gồm sản phẩm trong chi tiết đơn hàng
                                        .ThenInclude(p => p.DbProductColors)  // Bao gồm màu sắc sản phẩm
                                         .ThenInclude(pi => pi.IdColorNavigation) // Bao gồm chi tiết của từng hình ảnh
                                 .Include(o => o.DbOrderDetails)  // Bao gồm các chi tiết đơn hàng
                                    .ThenInclude(od => od.IdProductNavigation) // Bao gồm sản phẩm trong chi tiết đơn hàng
                                        .ThenInclude(p => p.DbProductMaterials)  // Bao gồm màu sắc sản phẩm
                                         .ThenInclude(pi => pi.IdMaterialNavigation) // Bao gồm chi tiết của từng hình ảnh
                                  .Include(o => o.DbOrderDetails)  // Bao gồm các chi tiết đơn hàng
                                    .ThenInclude(od => od.IdProductNavigation) // Bao gồm sản phẩm trong chi tiết đơn hàng
                                        .ThenInclude(p => p.DbAuctions)  // Bao gồm màu sắc sản phẩm
                                         .ThenInclude(pi => pi.DbAuctionRounds) // Bao gồm chi tiết của từng hình ảnh
                                                  
                                 .Include(o => o.DbOrderDetails)  // Bao gồm các chi tiết đơn hàng
                                    .ThenInclude(od => od.IdProductNavigation) // Bao gồm sản phẩm trong chi tiết đơn hàng
                                        .ThenInclude(p => p.DbComments)  // Bao gồm màu sắc sản phẩm

                                  .Include(o => o.DbOrderDetails)  // Bao gồm các chi tiết đơn hàng
                                    .ThenInclude(od => od.IdProductNavigation) // Bao gồm sản phẩm trong chi tiết đơn hàng
                                        .ThenInclude(p => p.DbInvoiceDetails)  // Bao gồm màu sắc sản phẩm


                                  .Include(o => o.DbOrderDetails)  // Bao gồm các chi tiết đơn hàng
                                    .ThenInclude(od => od.IdProductNavigation) // Bao gồm sản phẩm trong chi tiết đơn hàng
                                        .ThenInclude(p => p.DbProductSizes)  // Bao gồm màu sắc sản phẩm
                                         .ThenInclude(pi => pi.IdSizeNavigation) // Bao gồm chi tiết của từng hình ảnh

                                  .Include(o => o.DbOrderDetails)  // Bao gồm các chi tiết đơn hàng
                                    .ThenInclude(od => od.IdProductNavigation) // Bao gồm sản phẩm trong chi tiết đơn hàng
                                        .ThenInclude(p => p.DbRatings)  // Bao gồm màu sắc sản phẩm


                                  .Include(o => o.DbOrderDetails)  // Bao gồm các chi tiết đơn hàng
                                    .ThenInclude(od => od.IdProductNavigation) // Bao gồm sản phẩm trong chi tiết đơn hàng
                                        .ThenInclude(p => p.IdGroupNavigation)  // Bao gồm màu sắc sản phẩm


                                  .Include(o => o.DbOrderDetails)  // Bao gồm các chi tiết đơn hàng
                                    .ThenInclude(od => od.IdProductNavigation) // Bao gồm sản phẩm trong chi tiết đơn hàng
                                        .ThenInclude(p => p.IdCategoryNavigation)  // Bao gồm màu sắc sản phẩm

                                .Include(o => o.IdUserNavigation) // Bao gồm thông tin người dùng
                                .FirstOrDefaultAsync(o => o.Id == id);  // Điều kiện lọc để lấy ra đơn hàng với Id cụ thể
        }
    }
}
