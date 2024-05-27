using ECommerceMVC.Services.Store;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceMVC.Areas.Member.Controllers
{
    [Area("Member")]
    public class HomeController : Controller
    {
        private readonly IServiceStore _serviceStore;

        public HomeController(IServiceStore serviceStore)
        {
            _serviceStore = serviceStore;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var userStore = _serviceStore.GetUserStore();
            return View(userStore);
        }
    }
}
