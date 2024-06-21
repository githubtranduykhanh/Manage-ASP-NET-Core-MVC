using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceMVC.Areas.Admin.Controllers
{
    [Authorize]
    [Area("Admin")]
    [Route("Admin/[controller]/[action]")]
    public class AccountSettingsController : Controller
    {
        [HttpGet]       
        public IActionResult Account()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Notification()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Connection()
        {
            return View();
        }
    }
}
