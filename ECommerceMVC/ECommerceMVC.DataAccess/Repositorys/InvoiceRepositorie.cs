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
    public class InvoiceRepositorie : GenericRepository<DbInvoice>, IInvoiceRepositorie
    {
        public InvoiceRepositorie(ECommerceContext context) : base(context) { }
    }
}
