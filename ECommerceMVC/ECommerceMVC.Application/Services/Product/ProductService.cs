using ECommerceMVC.Application.Interfaces;
using ECommerceMVC.Domain.Abstract;
using ECommerceMVC.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceMVC.Application.Services.Product
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        public ProductService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<IEnumerable<DbProduct>> GetProductsAsync()
        {
            return await _unitOfWork.ProductRepositorie.GetAllAsync();
        }
    }
}
