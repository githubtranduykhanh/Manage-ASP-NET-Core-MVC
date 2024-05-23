using AutoMapper;
using ECommerceMVC.Config;
using ECommerceMVC.Data;
using ECommerceMVC.Helper.Jwts;
using ECommerceMVC.Helper.Responses;
using ECommerceMVC.Services.User;
using ECommerceMVC.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace ECommerceMVC.Areas.Account.Controllers
{
    [Area("Account")]
    [Route("Account")]
    public class LoginController : Controller
    {

        private readonly JwtAuthenticationManager _jwtAuthenticationManager;
        private readonly ECommerceContext _context;
        private readonly JwtSettings _jwtSettings;
        private readonly IServiceUser<DbUser, UserVM> _userService;
        private readonly IMapper _mapper;
        public LoginController(ECommerceContext context, IServiceUser<DbUser, UserVM> userService, IOptions<JwtSettings> jwtSettings, JwtAuthenticationManager jwtAuthenticationManager, IMapper mapper)
        {
            _context = context;
            _jwtSettings = jwtSettings.Value;
            _jwtAuthenticationManager = jwtAuthenticationManager;
            _userService = userService;
            _mapper = mapper;
        }

        [HttpGet("Login")]
        //GET: Account/Login
        public IActionResult Index()
        {
            return View();
        }
        //POST: Account/Login
        [HttpPost("Login")]
        [Authorize]
        public async Task<IActionResult> Login([FromBody] LoginBaseVM formData)
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
            var refreshToken = _jwtAuthenticationManager.RefreshToken(user.id.ToString(), user.roleName);
            var isAddRefreshToken = await _userService.AddRefreshToken(user.id, refreshToken);
            if (isAddRefreshToken == null) return Json(new ResponseData()
            {
                status = false,
                mes = "Adding Refresh Token Failed !!",
            });

            var accessToken = _jwtAuthenticationManager.GenerateToken(user.id.ToString(), user.roleName);
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
                    refreshToken,
                    role = user.roleName,
                }
            });
        }

        // POST Account/Register
        [HttpPost("Register")]              
        public async Task<IActionResult> Register([FromBody] RegisterVM userRegister)
        {
            if (!ModelState.IsValid)
            {
                // Log hoặc trả về chi tiết các lỗi
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return BadRequest(new { Errors = errors });
            }
            var user = _mapper.Map<DbUser>(userRegister);
            var isRegister = await _userService.IsRegisterAsync(user);
            if(isRegister != null) return Json(new ResponseData()
            {
                mes = "Email or Phone Number already exists"
            });
            
            await _userService.CreateAsync(user);
            // Lưu sản phẩm vào cơ sở dữ liệu
            // Trả về 201 Created và URL của sản phẩm mới
            return Json(new ResponseData()
            {
                status = true,
                mes = "Successful",
                data = user
            });
            //return CreatedAtRoute("GetUser", new { id = user.id },user);
        }


        // POST Account/Register
        [HttpGet("RefreshToken")]
        public async Task<IActionResult> RefreshToken()
        {
            // Lấy giá trị của cookie với tên cụ thể (ví dụ: "refreshToken")
            if (Request.Cookies.ContainsKey("refreshToken"))
            {
                var refreshTokenCookie = Request.Cookies["refreshToken"];

                // Sử dụng giá trị của refresh token từ cookie
                var refreshTokenUser = await _userService.GetRefreshToken(refreshTokenCookie);
                if (refreshTokenUser == null) {
                    return BadRequest(new { message = "Refresh token not found" });
                }

                var isTokenExp = _jwtAuthenticationManager.IsTokenExpired(refreshTokenCookie);
                if (!isTokenExp)
                {
                    return Unauthorized(new { message = "Refresh token expired" });
                }

                var newAccessToken = _jwtAuthenticationManager.GenerateToken(refreshTokenUser.Id.ToString(), refreshTokenUser.IdRoleNavigation?.Name ?? "User");

                if (newAccessToken != null)
                {
                    return Ok(new { newAccessToken });
                }
                else
                {
                    return Unauthorized(new { message = "Invalid refresh token" });
                }
            }
            return BadRequest(new { message = "Refresh token not found" });
        }
    }
}
