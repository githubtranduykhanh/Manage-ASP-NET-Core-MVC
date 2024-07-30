using AutoMapper;
using ECommerceMVC.Application.Dtos;
using ECommerceMVC.Application.Dtos.DataTable;
using ECommerceMVC.Application.Dtos.NewCategories;
using ECommerceMVC.Application.Dtos.Order;
using ECommerceMVC.Application.Interfaces;
using ECommerceMVC.Domain.Abstract;
using ECommerceMVC.Domain.Abstract.Cloudinary;
using ECommerceMVC.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceMVC.Application.Services.Product
{
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork _unitOfWork;
        protected readonly SignInManager<DbUser> _signInManager;
        protected readonly UserManager<DbUser> _userManager;
        protected readonly IMapper _mapper;
        public OrderService(IUnitOfWork unitOfWork,SignInManager<DbUser> signInManager,UserManager<DbUser> userManager, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _signInManager = signInManager;
            _userManager = userManager;
            _mapper = mapper;
        }
        public async Task<ResponseService<List<OrderModel>>> GetAllAsync()
        {
            try
            {

                var db = await _unitOfWork.OrderRepositorie.GetAsync();
                var list = await db.ToListAsync();


                return new ResponseService<List<OrderModel>>
                {
                    success = true,
                    message = "New Categories update successfully.",
                    data = _mapper.Map<List<DbOrder>,List<OrderModel>>(list)
                };
            }
            catch (Exception ex)
            {
                return new ResponseService<List<OrderModel>>
                {                    
                    message = ex.Message,                    
                };              
            }         
        }

        public async Task<ResponseService<OrderModel>> GetByIdAsync(int? id)
        {
            try
            {

                if(id == null) return new ResponseService<OrderModel>
                {
                    message = "Id is required.",
                };

                var find = await _unitOfWork.OrderRepositorie.GetByIDAsync(id);
                if (find == null) return new ResponseService<OrderModel>
                {
                    message = "Order not found.",
                };

                return new ResponseService<OrderModel>
                {
                    success = true,
                    message = "Get order successfully.",
                    data = _mapper.Map<DbOrder, OrderModel>(find)
                };
            }
            catch (Exception ex)
            {
                return new ResponseService<OrderModel>
                {
                    message = ex.Message,
                };
            }

        }
        public async Task<ResponseDataTable<OrderModel>> GetAllDataTableAsync(RequestDataTable request)
        {
            // Tạo biểu thức điều kiện cho lọc
            Expression<Func<DbOrder, bool>> filterExpression = null;

            if (!string.IsNullOrEmpty(request.Search?.Value))
            {
                filterExpression = e =>  EF.Functions.Like(e.Id.ToString(), $"%{request.Search.Value}%")
                                        || EF.Functions.Like(e.NameUser, $"%{request.Search.Value}%")
                                        || EF.Functions.Like(e.EmailUser, $"%{request.Search.Value}%")
                                        || EF.Functions.Like(e.PhoneUser, $"%{request.Search.Value}%")
                                        || EF.Functions.Like(e.AddressUser, $"%{request.Search.Value}%");
            }

            // Lấy dữ liệu từ repository với bao gồm quan hệ cần thiết
            var query = await _unitOfWork.OrderRepositorie.GetAsync(
                filter: filterExpression,
                orderBy: q => q.OrderBy(o => o.CreatedAt), // Sắp xếp theo CreatedAt
                includeProperties: new Expression<Func<DbOrder, object>>[]
                {
                    p => p.DbOrderDetails,
                    p => p.IdUserNavigation,// Add more include properties as needed                  
                });



            // Xử lý sắp xếp nếu có
            if (request.Order != null && request.Order.Any())
            {
                var orderColumn = request.Order.FirstOrDefault()?.Column ?? 0;
                var sortDirection = request.Order.FirstOrDefault()?.Dir ?? "asc";
                var sortName = request.Columns[orderColumn].Data;

                var sortExpression = GetSortExpression(sortName);

                query = sortDirection == "asc"
                    ? query.OrderBy(sortExpression)
                    : query.OrderByDescending(sortExpression);
            }

            // Lấy tổng số bản ghi trước khi phân trang
            int recordsTotal = await query.CountAsync();

            // Phân trang
            var dataEntities = await query.Skip(request.Start).Take(request.Length).ToListAsync();
            //var data = new List<OrderModel>();

            //foreach (var entity in dataEntities)
            //{
                
            //    data.Add(new OrderModel
            //    {
            //        Id = entity.Id,
            //        TotalAmount = entity.TotalAmount,
            //        Status = entity.Status,
            //        IdUser = entity.IdUser,
            //        NameUser = entity.NameUser,
            //        EmailUser = entity.EmailUser,
            //        AddressUser = entity.AddressUser,
            //        PhoneUser = entity.PhoneUser,
            //        PaymentType = entity.PaymentType,
            //        CreatedAt = entity.CreatedAt,
            //        DbOrderDetails = entity.DbOrderDetails,                
            //    });
            //}

            // Trả về dữ liệu dưới dạng JSON
            return new ResponseDataTable<OrderModel>
            {
                Draw = request.Draw,
                RecordsTotal = recordsTotal,
                RecordsFiltered = recordsTotal, // Điều chỉnh nếu cần
                Data = _mapper.Map<List<DbOrder>, List<OrderModel>>(dataEntities).Select(order =>
                {
                    order.IdUserNavigation = null;
                    return order;
                }).ToList()
            };
        }

        private Expression<Func<DbOrder, object>> GetSortExpression(string sortName)
        {
            return sortName switch
            {
                "id" => (p => p.Id),
                "totalAmount" => (p => p.TotalAmount),
                "status" => (p => p.Status),
                "paymentType" => (p => p.PaymentType),
                "createdAt" => (p => p.CreatedAt),
                _ => (p => p.CreatedAt)
            };
        }


        public async Task<ResponseService> UpdateAsync(OrderModel model)
        {
            try
            {
                if (model == null || model.Id == 0) return new ResponseService
                {
                    message = "Model or ID is required.",
                };

                var find = await _unitOfWork.OrderRepositorie.GetByIDAsync(model.Id);

                if (find == null) return new ResponseService
                {
                    message = "Order update not found.",
                };

                await UpdateUserInfoAsync(find.IdUser, model);

                find.TotalAmount = model.TotalAmount;
                find.Status = model.Status;
                find.PaymentType = model.PaymentType;
                find.CreatedAt = model.CreatedAt;
                find.NameUser = model.NameUser;
                find.EmailUser = model.EmailUser;
                find.AddressUser = model.AddressUser;
                find.PhoneUser = model.PhoneUser;

                _unitOfWork.OrderRepositorie.Update(find);

                await _unitOfWork.SaveChangesAsync();

                return new ResponseService
                {
                    success = true,
                    message = "Order update successfully."
                };

            }
            catch (Exception ex)
            {
                return new ResponseService
                {
                    message = ex.Message
                };
            }
        }

        public async Task UpdateUserInfoAsync(string id, OrderModel model)
        {

            bool isUpdate = false;
            var user = await _userManager.FindByIdAsync(id);
            if(user == null) return;
            if(user.DisplayName != model.NameUser) { user.DisplayName = model.NameUser; isUpdate = true; }
            if(user.Address != model.AddressUser) { user.Address = model.AddressUser; isUpdate = true; }
            if(user.PhoneNumber != model.PhoneUser) { user.PhoneNumber = model.PhoneUser; isUpdate = true; }
            if(user.Email != model.EmailUser) { user.Email = model.EmailUser; isUpdate = true; }       
            if(isUpdate) await _userManager.UpdateAsync(user);
        }


        public async Task<ResponseService> DeleteAsync(int id)
        {
            try
            {
                if (id == null) return new ResponseService
                {
                    message = "Id is required"
                };
                var db = await _unitOfWork.OrderRepositorie.GetOrderWithDetailsAsync(id);
                if (db == null) return new ResponseService
                {
                    message = "Order not found."
                };
                _unitOfWork.OrderDetailsRepositorie.RemoveRange(db.DbOrderDetails);
                _unitOfWork.OrderRepositorie.Delete(db);
                await _unitOfWork.SaveChangesAsync();
                return new ResponseService
                {
                    success = true,
                    message = "Order delete successfully."
                };
            }
            catch (Exception ex)
            {

                return new ResponseService
                {
                    message = ex.Message
                }; 
            }
            
        }      
    }
}
