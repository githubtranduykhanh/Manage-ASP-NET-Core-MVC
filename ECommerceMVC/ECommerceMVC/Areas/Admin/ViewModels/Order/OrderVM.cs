using ECommerceMVC.Domain.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using ECommerceMVC.Application.Dtos.OrderDetail;

namespace ECommerceMVC.UI.Areas.Admin.ViewModels.Order
{
    public class OrderVM
    {
        public int Id { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "TotalAmount must be greater than zero.")]
        public double TotalAmount { get; set; }

        [Required(ErrorMessage = "Status Order is required")]
        public string Status { get; set; } = null!;

        [Required(ErrorMessage = "IdUser Order is required")]
        public string IdUser { get; set; } = null!;

        [Required(ErrorMessage = "Name User Order is required")]
        public string NameUser { get; set; } = null!;

        [Required(ErrorMessage = "Email User Order is required")]
        [EmailAddress]
        public string EmailUser { get; set; } = null!;

        [Required(ErrorMessage = "Address User Order is required")]
        public string AddressUser { get; set; } = null!;

        [Required(ErrorMessage = "Phone User Order is required")]
        [Phone]
        public string PhoneUser { get; set; } = null!;

        [Required(ErrorMessage = "Payment Type Order is required")]
        public string PaymentType { get; set; } = null!;

        [Required(ErrorMessage = "CreatedAt Order is required")]
        public DateTime CreatedAt { get; set; }

        public ICollection<OrderDetailModel> DbOrderDetails { get; set; } = new List<OrderDetailModel>();

        public DbUser? User { get; set; } = null!;
    }
}
