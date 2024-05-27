using ECommerceMVC.Models;
using ECommerceMVC.ViewModels;

namespace ECommerceMVC.Services.Store
{
    public interface IServiceStore
    {
        void SetUserStore(UserVM userVM);
        bool GetStatus();
        void DeleteUserStatus();
        UserStore GetUserStore();
    }
}
