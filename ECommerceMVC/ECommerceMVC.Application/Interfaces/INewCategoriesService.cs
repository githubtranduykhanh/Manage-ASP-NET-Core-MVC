using ECommerceMVC.Application.Dtos;
using ECommerceMVC.Application.Dtos.DataTable;
using ECommerceMVC.Application.Dtos.NewCategories;
using ECommerceMVC.Domain.Entities;
using Microsoft.AspNetCore.Http;

namespace ECommerceMVC.Application.Interfaces
{
    public interface INewCategoriesService
    {
        Task<ResponseDataTable<NewCategoriesModel>> GetAllDataTableAsync(RequestDataTable request);
        Task<NewCategoriesModel?> GetByIdAsync(int? id);
        Task<ResponseService> UpdateAsync(NewCategoriesModel model, IFormFile image);
        Task<ResponseService> DeleteAsync(int id);
    }
}