using ECommerceMVC.Application.Dtos.OrderDetail;
using ECommerceMVC.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceMVC.Application.Dtos.Order
{
    public class OrderModel
    {
        public int Id { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "TotalAmount must be greater than zero.")]
        public double TotalAmount { get; set; }

        [Required]
        public string Status { get; set; } = null!;

        [Required]
        public string IdUser { get; set; } = null!;

        [Required]
        public string NameUser { get; set; } = null!;

        [Required]
        [EmailAddress]
        public string EmailUser { get; set; } = null!;

        [Required]
        public string AddressUser { get; set; } = null!;

        [Required]
        [Phone]
        public string PhoneUser { get; set; } = null!;

        [Required]
        public string PaymentType { get; set; } = null!;

        [Required]
        public DateTime CreatedAt { get; set; }

        public virtual ICollection<OrderDetailModel> DbOrderDetails { get; set; } = new List<OrderDetailModel>();

        [DisplayName("User")]
        public virtual DbUser IdUserNavigation { get; set; } = null!;
    }
}
