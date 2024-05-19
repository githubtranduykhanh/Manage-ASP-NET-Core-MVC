using System.ComponentModel.DataAnnotations;

namespace ECommerceMVC.ViewModels
{
    public class UserVM
    {
        public int id { get; set; }

        [Required]
        [StringLength(50)]
        public string name { get; set; }

        [StringLength(100)]
        public string address { get; set; }

        [Required]
        [StringLength(15)]
        public string phone { get; set; }

        [Required]
        [StringLength(50)]
        public string email { get; set; }

        [Required]
        public string password { get; set; }

        public int idRole { get; set; }


        public string roleName { get; set; }

        [Required]
        [StringLength(10)]
        public string sex { get; set; }

        public DateTime createdAt { get; set; }

        [Required]
        [StringLength(50)]
        public string loginType { get; set; }

        public string refreshToken { get; set; }

        public string avatar { get; set; }

        public string securityQuestion { get; set; }

        public bool status { get; set; }
    }
}
