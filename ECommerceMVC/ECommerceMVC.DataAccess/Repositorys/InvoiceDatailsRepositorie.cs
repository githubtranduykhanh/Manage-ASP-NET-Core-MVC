using ECommerceMVC.DataAccess.Data;
using ECommerceMVC.Domain.Abstract;
using ECommerceMVC.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceMVC.DataAccess.Repositorys
{
    public class InvoiceDatailsRepositorie : GenericRepository<DbInvoiceDetail>, IInvoiceDatailsRepositorie
    {
        public InvoiceDatailsRepositorie(ECommerceContext context) : base(context) { }
    }
}
