using AutoMapper;
using ECommerceMVC.Data;
using ECommerceMVC.Helper.Responses;
using ECommerceMVC.Services.Store;
using ECommerceMVC.Services.User;
using ECommerceMVC.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ECommerceMVC.Areas.Account.Controllers
{
    [Authorize]
    [Area("Account")]
    [Route("Account/[controller]")]
    public class CurrentUserController : Controller
    {
        private readonly IServiceStore _serviceStore;
        private readonly IServiceUser<DbUser, UserVM> _userService;
        private readonly IMapper _mapper;
        public CurrentUserController(IServiceStore serviceStore, IServiceUser<DbUser, UserVM> userService, IMapper mapper)
        {
            _userService = userService;
            _serviceStore = serviceStore;
            _mapper = mapper;
        }


        //GET Account/CurrentUser
        [HttpGet]       
        public async Task<IActionResult> Index()
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
            {
                return BadRequest("Invalid user ID in token.");
            }
            var user = await _userService.GetByIdAsync(Convert.ToInt32(userId));
            if (user == null) {
                return BadRequest("Invalid user ID in server.");
            }

            _serviceStore.SetUserStore(user);

            
            return Json(new ResponseData { status = true, data = _mapper.Map<UserCurrentVM>(user) });
        }
    }
}
