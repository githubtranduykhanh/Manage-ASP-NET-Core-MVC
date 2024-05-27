using ECommerceMVC.Models;
using ECommerceMVC.ViewModels;

namespace ECommerceMVC.Services.Store
{
    public class ServiceStore : IServiceStore
    {
        private UserStore _userStore = new UserStore();

        public void DeleteUserStatus()
        {
            _userStore.Status = false;
            _userStore.UserVM = null;
        }

        public bool GetStatus()
        {
            return _userStore.Status;
        }

        public UserStore GetUserStore()
        {
            return _userStore;
        }

        public void SetUserStore(UserVM userVM)
        {
            _userStore.Status = true;
            _userStore.UserVM = userVM;
        }
    }
}
