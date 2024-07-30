using ECommerceMVC.Application.Dtos;
using ECommerceMVC.Application.Dtos.DataTable;
using ECommerceMVC.Application.Dtos.Order;
using ECommerceMVC.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceMVC.Application.Interfaces
{
    public interface IOrderService
    {
        Task<ResponseService<List<OrderModel>>> GetAllAsync();
        Task<ResponseService<OrderModel>> GetByIdAsync(int? id);
        Task<ResponseDataTable<OrderModel>> GetAllDataTableAsync(RequestDataTable request);
        Task<ResponseService> UpdateAsync(OrderModel model);
        Task UpdateUserInfoAsync(string id, OrderModel model);
        Task<ResponseService> DeleteAsync(int id);
    }
}
