using ECommerceMVC.Services.Store;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceMVC.Areas.Member.Components.AdminUserInformation
{
    [ViewComponent]
    public class AdminUserInformation : ViewComponent
    {
        private readonly IServiceStore _serviceStore;

        public AdminUserInformation(IServiceStore serviceStore)
        {
            _serviceStore = serviceStore;
        }

        public IViewComponentResult Invoke()
        {
            var userStore = _serviceStore.GetUserStore();
            return View(userStore);
        }
    }
}
