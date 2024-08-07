using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceMVC.Application.Dtos.Order
{
    public class OrderStatistical
    {
        public int Pending { get; set; } = 0;
        public int Processing { get; set; } = 0;
        public int Completed { get; set; } = 0;
        public int Cancelled { get; set; } = 0;
    }
}
