using ECommerceMVC.Application.Dtos.Order;
using ECommerceMVC.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceMVC.Application.Dtos.OrderDetail
{
    public class OrderDetailModel
    {
        [Required]
        public int IdProduct { get; set; }

        [Required]
        public int IdOrder { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1.")]
        public int Quantity { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Unit Price must be greater than zero.")]
        public double UnitPrice { get; set; }
        public virtual OrderProduct IdProductNavigation { get; set; } = null!;
    }
}
