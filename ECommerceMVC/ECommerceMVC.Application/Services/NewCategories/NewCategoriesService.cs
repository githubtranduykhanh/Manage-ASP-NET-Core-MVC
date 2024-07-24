using AutoMapper;
using ECommerceMVC.Application.Dtos;
using ECommerceMVC.Application.Dtos.DataTable;
using ECommerceMVC.Application.Dtos.NewCategories;
using ECommerceMVC.Application.Extensions;
using ECommerceMVC.Application.Interfaces;
using ECommerceMVC.Domain.Abstract;
using ECommerceMVC.Domain.Abstract.Cloudinary;
using ECommerceMVC.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ECommerceMVC.Application.Services.NewCategories
{
    public class NewCategoriesService : INewCategoriesService
    {
        private readonly IUnitOfWork _unitOfWork;
        protected readonly IMapper _mapper;
        protected readonly ICloudinaryService _cloudinaryService;
        public NewCategoriesService(IUnitOfWork unitOfWork, IMapper mapper, ICloudinaryService cloudinaryService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _cloudinaryService = cloudinaryService;
        }
        public async Task<ResponseDataTable<NewCategoriesModel>> GetAllDataTableAsync(RequestDataTable request)
        {
            // Tạo biểu thức điều kiện cho lọc
            Expression<Func<DbNewCategory, bool>> filterExpression = null;

            if (!string.IsNullOrEmpty(request.Search?.Value))
            {
                filterExpression = e => EF.Functions.Like(e.Name, $"%{request.Search.Value}%");
            }

            // Lấy dữ liệu từ repository với bao gồm quan hệ cần thiết
            var query = await _unitOfWork.NewCategoriesRepositorie.GetAsync(
                filter: filterExpression,             
                includeProperties: new Expression<Func<DbNewCategory, object>>[]
                {
                    p => p.DbNews, // Add more include properties as needed                  
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

            var data = new List<NewCategoriesModel>();

            foreach (var entity in dataEntities)
            {
                var newParent = await _unitOfWork.NewCategoriesRepositorie.GetByIDAsync(entity.IdNewParent);
                data.Add(new NewCategoriesModel
                {
                    Id = entity.Id,
                    Name = entity.Name,
                    Image = entity.Image,
                    DisplayOrder = entity.DisplayOrder,
                    NewParentName = newParent?.Name ?? ""
                });
            }

            // Trả về dữ liệu dưới dạng JSON
            return new ResponseDataTable<NewCategoriesModel>
            {
                Draw = request.Draw,
                RecordsTotal = recordsTotal,
                RecordsFiltered = recordsTotal, // Điều chỉnh nếu cần
                Data = data
            };
        }

        private Expression<Func<DbNewCategory, object>> GetSortExpression(string sortName)
        {
            return sortName switch
            {
                "name" => (p => p.Name),
                "image" => (p => p.Image),
                "displayOrder" => (p => p.DisplayOrder),
                "newParentName" => (p => p.IdNewParent),
                _ => (p => p.Name)
            };
        }

        public async Task<NewCategoriesModel?> GetByIdAsync(int? id) 
        {
            if(id == null) return null;
            var db = await _unitOfWork.NewCategoriesRepositorie.GetByIDAsync(id);    
            var model = _mapper.Map<DbNewCategory, NewCategoriesModel>(db);
            var newParent = await _unitOfWork.NewCategoriesRepositorie.GetByIDAsync(model.IdNewParent);
            if (newParent != null) model.NewParentName = newParent.Name;
            return db == null ? null : model;
        }


        public async Task<ResponseService> UpdateAsync(NewCategoriesModel model, IFormFile image)
        {
            try
            {
                if (model == null || model.Id == null) return new ResponseService
                {
                    message = "Model is required"
                };
                var db = await _unitOfWork.NewCategoriesRepositorie.GetByIDAsync(model.Id);
                if (db == null) return new ResponseService
                {
                    message = "New Categories not found."
                };
                db.IdNewParent = model.IdNewParent == 0 ? null : model.IdNewParent;
                db.Name = model.Name;
                db.DisplayOrder = model.DisplayOrder;            
                if(image != null)
                {
                    var urlImage = await _cloudinaryService.UploadFileAsync(image);
                    db.Image = urlImage;
                }
                _unitOfWork.NewCategoriesRepositorie.Update(db);
                await _unitOfWork.SaveChangesAsync();
                return new ResponseService
                {
                    success = true,
                    message = "New Categories update successfully."
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


        public async Task<ResponseService> DeleteAsync(int id)
        {
            if (id == null) return new ResponseService
            {
                message = "Id is required"
            };
            var db = await _unitOfWork.NewCategoriesRepositorie.GetByIDAsync(id);
            if(db == null) return new ResponseService
            {
                message = "New Categories not found."
            };

            var list = await _unitOfWork.NewCategoriesRepositorie.GetAsync( n => n.IdNewParent == db.Id);

            if(list != null && list.Any())
            {
                foreach (var item in list)
                {
                    item.IdNewParent = null;
                    _unitOfWork.NewCategoriesRepositorie.Update(item);
                }
            }

            _unitOfWork.NewCategoriesRepositorie.Delete(db);
            await _unitOfWork.SaveChangesAsync();
            return new ResponseService
            {
                success = true,
                message = "New Categories delete successfully."
            };
        }
    }
}
