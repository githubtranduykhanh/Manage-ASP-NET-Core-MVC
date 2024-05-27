using ECommerceMVC.Config;
using ECommerceMVC.Data;
using ECommerceMVC.Helper.Jwts;
using ECommerceMVC.Helper.Responses;
using ECommerceMVC.Services.User;
using ECommerceMVC.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;

namespace ECommerceMVC.Controllers
{
    [Route("api/v1/[controller]")]
    public class AccountController : Controller
    {
        private readonly JwtAuthenticationManager _jwtAuthenticationManager;
        private readonly ECommerceContext _context;
        private readonly JwtSettings _jwtSettings;
        private readonly IServiceUser<DbUser, UserVM> _userService;
        public AccountController(ECommerceContext context, IServiceUser<DbUser, UserVM> userService, IOptions<JwtSettings> jwtSettings, JwtAuthenticationManager jwtAuthenticationManager)
        {
            _context = context;
            _jwtSettings = jwtSettings.Value;
            _jwtAuthenticationManager = jwtAuthenticationManager;
            _userService = userService;
        }

        //POST: api/v1/account/login
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginBaseVM formData)
        {
            // Kiểm tra thông tin đăng nhập và tạo JWT nếu hợp lệ
            if (!ModelState.IsValid) return Json(new ResponseData()
            {
                status = false,
                mes = "Missing input !!",
            });
            var user = await _userService.AuthenticateAsync(formData);

            if (user == null) return Json(new ResponseData()
            {
                status = false,
                mes = "User does not exist !!",
            });
            var refreshToken = _jwtAuthenticationManager.RefreshToken(user.Id.ToString(), user.RoleName);
            var isAddRefreshToken = await _userService.AddRefreshToken(user.Id, refreshToken);
            if (isAddRefreshToken == null) return Json(new ResponseData()
            {
                status = false,
                mes = "Adding Refresh Token Failed !!",
            });

            var accessToken = _jwtAuthenticationManager.GenerateToken(user.Id.ToString(), user.RoleName);        
            // Tạo một cookie
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true, // Chỉ cho phép cookie được truy cập qua HTTP, không thể truy cập qua JavaScript
                Secure = true, // Chỉ gửi cookie qua HTTPS
                Expires = DateTime.Now.AddDays(1) // Thời gian hết hạn của cookie
            };
            // Thiết lập giá trị cho cookie
            Response.Cookies.Append("refreshToken", refreshToken, cookieOptions);

            return Json(new ResponseData()
            {
                status = true,
                mes = "Successful",
                data = new
                {
                    accessToken,
                    refreshToken
                }
            });
        }
    }
}
