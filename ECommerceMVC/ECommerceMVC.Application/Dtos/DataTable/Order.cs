using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceMVC.Application.Dtos.DataTable
{
    public class Order
    {
        public int Column { get; set; }
        public string Dir { get; set; } = "asc"; // hoặc "desc"
    }
}
