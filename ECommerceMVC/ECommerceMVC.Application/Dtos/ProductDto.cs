using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceMVC.Application.Dtos
{
    public class ProductDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public int IdCategory { get; set; }
        public int IdGroup { get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; }
        public string? Description { get; set; }
        public int View { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime RemovedAt { get; set; }
    }
}
