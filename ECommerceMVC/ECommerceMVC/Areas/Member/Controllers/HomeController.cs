using Microsoft.AspNetCore.Mvc;

namespace ECommerceMVC.Areas.Member.Controllers
{
    [Area("Member")]
    public class HomeController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
    }
}
