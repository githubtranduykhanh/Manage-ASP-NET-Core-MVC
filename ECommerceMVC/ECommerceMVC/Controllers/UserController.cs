using ECommerceMVC.Data;
using ECommerceMVC.Helper.Excel;
using ECommerceMVC.Helper.Jwts;
using ECommerceMVC.Helper.Responses;
using ECommerceMVC.Services;
using ECommerceMVC.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceMVC.Controllers
{
    [Route("api/v1/[controller]")]
    public class UserController : Controller
    {
        private readonly IServiceUser<DbUser, UserVM> _userService;
        public UserController(IServiceUser<DbUser, UserVM> userService)
        {         
            _userService = userService;        
        }

        // GET api/v1/user/all
        [HttpGet("all")]
        [Authorize]
        public async Task<IActionResult> GetUsers()
        {
            var entities = await _userService.GetAllAsync();
            if (entities == null) return Ok(new ResponseData()
            {
                status = false,
                mes = "Empty users",
                data = new object[] { }
            });
            return Ok(new ResponseData()
            {
                status = true,
                mes = "Successful",
                data = entities
            });
        }

        // GET api/v1/user/list
        [HttpGet("list")]
        public async Task<IActionResult> GetListUsers()
        {
            var entities = await _userService.GetAllAsync();
            if (entities == null) return Ok(new ResponseData()
            {
                status = false,
                mes = "Empty users",
                data = new object[] { }
            });
            return Ok(new ResponseData()
            {
                status = true,
                mes = "Successful",
                data = entities
            });
        }
    }
}
