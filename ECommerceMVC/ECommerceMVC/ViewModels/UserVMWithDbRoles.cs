using ECommerceMVC.Data;

namespace ECommerceMVC.ViewModels
{
    public class UserVMWithDbRoles
    {
        public IEnumerable<UserVM> Users { get; set; } = new List<UserVM>();
        public IEnumerable<DbRole> Roles { get; set; } = Enumerable.Empty<DbRole>();
    }
}
