using ECommerceMVC.DataAccess.Data;
using ECommerceMVC.Domain.Abstract;
using ECommerceMVC.Domain.Entities;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceMVC.DataAccess.Repositorys
{
   public class NewCategoriesRepositorie : GenericRepository<DbNewCategory>, INewCategoriesRepositorie
    {
        public NewCategoriesRepositorie(ECommerceContext context) : base(context) { }
    }
}
