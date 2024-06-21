using AutoMapper;
using ECommerceMVC.Config;
using ECommerceMVC.Data;
using ECommerceMVC.Helper.Jwts;
using ECommerceMVC.Services.Store;
using ECommerceMVC.Services.User;
using ECommerceMVC.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace ECommerceMVC.Areas.Admin.Controllers
{
    [Authorize]
    [Area("Admin")]
    [Route("Admin/[controller]/[action]")]
    public class ManagerUsersController : Controller
    {

        private readonly IServiceUser<DbUser, UserVM> _userService;
        private readonly IMapper _mapper;
        private readonly IServiceStore _serviceStore;
        public ManagerUsersController(IServiceStore serviceStore, IServiceUser<DbUser, UserVM> userService, IMapper mapper)
        {       
            _userService = userService;
            _mapper = mapper;
            _serviceStore = serviceStore;
        }
        [HttpGet]
        public async Task<IActionResult> TableUser()
        {
            var users = await _userService.GetAllAsync();
            
            
            var data = new UserVMWithDbRoles
            {
                Users = users,          
            };

            return View(data);
        }
    }
}
