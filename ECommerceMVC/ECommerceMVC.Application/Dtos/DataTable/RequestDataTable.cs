using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceMVC.Application.Dtos.DataTable
{
    public class RequestDataTable
    {
        public int Draw { get; set; }
        public int Start { get; set; }
        public int Length { get; set; }
        public Search Search { get; set; } = new Search();
        public List<Column> Columns { get; set; } = new List<Column>();
        public List<Order> Order { get; set; } = new List<Order>();
    }
}
