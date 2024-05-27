using ECommerceMVC.Services.Store;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceMVC.Areas.Member.Components.MemberUserInformation
{
    [ViewComponent]
    public class MemberUserInformation : ViewComponent
    {
        private readonly IServiceStore _serviceStore;

        public MemberUserInformation(IServiceStore serviceStore)
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
