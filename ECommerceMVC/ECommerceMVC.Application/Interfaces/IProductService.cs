using ECommerceMVC.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceMVC.Application.Interfaces
{
    public interface IProductService
    {
        Task<IEnumerable<DbProduct>> GetProductsAsync();
    }
}
