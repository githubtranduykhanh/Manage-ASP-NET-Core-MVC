using AutoMapper;
using ECommerceMVC.Config;
using ECommerceMVC.Data;
using ECommerceMVC.Helper.Jwts;
using ECommerceMVC.Helper.Responses;
using ECommerceMVC.Models.Role;
using ECommerceMVC.Models.User;
using ECommerceMVC.Services.Cloudinary;
using ECommerceMVC.Services.Store;
using ECommerceMVC.Services.User;
using ECommerceMVC.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.ComponentModel;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ECommerceMVC.Areas.Admin.Controllers
{
   
    [Area("Admin")]
    [Route("Admin/[controller]/[action]")]
    public class ManagerUsersController : Controller
    {

        private readonly SignInManager<DbUser> _signInManager;
        private readonly UserManager<DbUser> _userManager;
        private readonly ICloudinaryService _cloudinaryService;
        private readonly RoleManager<IdentityRole> _roleManager;
        public ManagerUsersController(SignInManager<DbUser> signInManager,  UserManager<DbUser> userManager, ICloudinaryService cloudinaryService, RoleManager<IdentityRole> roleManager)
        {       
            _signInManager = signInManager;       
            _userManager = userManager;
            _cloudinaryService = cloudinaryService;
            _roleManager = roleManager;
        }


        [TempData]
        public string StatusMessage { get; set; }


        [HttpGet]
        public async Task<IActionResult> TableUser()
        {
            var users = _userManager.Users.ToList();  // Chuyển đổi sang danh sách để có thể lặp qua
            var userDetails = new List<UserDetailViewModel>();

            foreach (var user in users)
            {
                var logins = await _userManager.GetLoginsAsync(user);
                var providers = logins.Select(login => new LoginProviderInfo
                {
                    ProviderName = login.LoginProvider,
                    ProviderKey = login.ProviderKey,
                    ProviderDisplayName = login.ProviderDisplayName
                }).ToList();

                userDetails.Add(new UserDetailViewModel
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    Email = user.Email,
                    DisplayName = user.DisplayName, // Trường tùy chỉnh
                    Address = user.Address,
                    Avatar = user.Avatar,
                    EmailConfirmed = user.EmailConfirmed,
                    PhoneNumberConfirmed = user.PhoneNumberConfirmed,
                    PhoneNumber = user.PhoneNumber,
                    Gender = user.Gender,
                    LoginProviders = providers
                });
            }
           
            return View(userDetails);
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetUserById(string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest(new { status = false, mes = "Validation errors", errors = new object[] { "User Id Empty" } });
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return BadRequest(new { status = false, mes = "Validation errors", errors = new object[] { "Error: User not found." } });
            }

            return Ok(new ResponseData
            {
                status = true,
                mes = "Successfully",
                data = new
                {                   
                    user.Id,
                    user.DisplayName,
                    Birthday = user.Birthday ?? DateTime.Now,
                    Gender = user?.Gender?.Trim() ?? "other",
                    Avatar = user.Avatar ?? "https://i.pinimg.com/originals/f1/0f/f7/f10ff70a7155e5ab666bcdd1b45b726d.jpg"
                }
            });
        }

        [HttpPost]
        public async Task<IActionResult> DeleteUser(string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                StatusMessage = "Error: User ID is required.";
                return RedirectToAction(nameof(TableUser));
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                StatusMessage = "Error: User not found.";
                return RedirectToAction(nameof(TableUser));
            }

            // Lấy thông tin người dùng đang đăng nhập
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser != null && currentUser.Id == userId)
            {
                StatusMessage = "Error: You cannot delete your own account.";
                return RedirectToAction(nameof(TableUser));
            }

            var result = await _userManager.DeleteAsync(user);
            if (result.Succeeded)
            {
                StatusMessage = "User deleted successfully.";
            }
            else
            {
                StatusMessage = "Error: Failed to delete user.";
            }

            return RedirectToAction(nameof(TableUser));
        }

        [HttpPost("{userId}")]
        public async Task<IActionResult> EditUser(string userId,[FromForm] ModelEditDataAndFile model)
        {
            if (!ModelState.IsValid)
            {
                // Trả về lỗi validation nếu không đầy đủ dữ liệu
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                return BadRequest(new { status = false, message = "Validation errors", errors });
            }

            // Xử lý dữ liệu từ model
            var id = model.Id;
            var displayName = model.DisplayName;
            var gender = model.Gender;
            var birthday = model.Birthday;
            var avatarFile = model.Avatar;

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return BadRequest(new { status = false, message = "User not found.", errors = new object[] { "Error: User not found." } });
            }

            user.DisplayName = displayName;
            user.Gender = gender;
            user.Birthday = birthday;



            // Xử lý tệp avatar
            if (avatarFile != null && avatarFile.Length > 0)
            {
                try
                {
                    var fileUrl = await _cloudinaryService.UploadFileAsync(avatarFile);
                    user.Avatar = fileUrl;
                }
                catch (Exception ex)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, new { status = false, message = "Failed to upload avatar.", errors = new object[] { ex.Message } });
                }
            }

            var result = await _userManager.UpdateAsync(user);

            if (result.Succeeded)
            {
                return Ok(new { status = true, message = "Data received successfully"});
            }
            else
            {
                return BadRequest(new { status = false, message = "Data received unsuccessful", errors = new object[] { "Error: Data received unsuccessful." } });
            }

        }


        [HttpGet]
        public async Task<IActionResult> TableUserRole()
        {
            var roles = await _roleManager.Roles.ToListAsync();
           
        
            return View(roles);
        }



        [HttpPost]
        public async Task<IActionResult> CreateRole([FromBody] ModelCreateRole model)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                return Ok(new { status = false, message = "Validation errors", errors });
            }

            // Kiểm tra xem vai trò đã tồn tại chưa
            if (await _roleManager.RoleExistsAsync(model.RoleName))
            {
                return Ok(new { status = false, message = "Role Name already exists.", errors = new object[] { "Role Name already exists." } });
            }
            
            // Nếu chưa tồn tại, tạo mới
            var role = new IdentityRole(model.RoleName);
            await _roleManager.CreateAsync(role);
            // Xử lý sau khi tạo xong
            return Ok(new { status = true, message = "Create role successfully.", errors = new object[] { } });
        }


        [HttpGet("{roleId}")]
        public async Task<IActionResult> GetRoleById(string roleId)
        {
            if (string.IsNullOrEmpty(roleId))
            {              
                return Ok(new { status = false, message = "Role Name required.", errors = new object[] { "Role Name required." } });
            }
            var role = await _roleManager.FindByIdAsync(roleId);
            if(role == null)
            {
                return Ok(new { status = false, message = "Role Name already exists.", errors = new object[] { "Role Name already exists." } });
            }

            
            return Ok(new { status = true, message = "Get role by id successfully.", data = new { role.Id,role.Name } });
        }


        [HttpPost("{roleId}")]
        public async Task<IActionResult> EditRole(string roleId, [FromBody] ModelEditRole model)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                return Ok(new { status = false, message = "Validation errors", errors });
            }

            var role = await _roleManager.FindByIdAsync(roleId);
            if (role == null)
            {
                return Ok(new { status = false, message = "Role not found.", errors = new object[] { "Role not found." } });
            }


            role.Name = model.RoleName;

            var result = await _roleManager.UpdateAsync(role);
            if (!result.Succeeded)
            {
                return Ok(new { status = false, message = "Failed to update role.", errors = result.Errors.Select(e => e.Description).ToArray() });                
            }
            return Ok(new { status = true, message = "Role updated successfully.", errors = new object[] { } });
        }


        [HttpPost]
        public async Task<IActionResult> DeleteRole(string roleId)
        {
            if (string.IsNullOrEmpty(roleId))
            {
                StatusMessage = "Error: Role ID is required.";
                return RedirectToAction(nameof(TableUserRole));
            }

            var role = await _roleManager.FindByIdAsync(roleId);
            if (role == null)
            {
                StatusMessage = "Error: Role not found.";
                return RedirectToAction(nameof(TableUserRole));
            }

            var result = await _roleManager.DeleteAsync(role);
            if (result.Succeeded)
            {
                StatusMessage = "Role deleted successfully.";
            }
            else
            {
                StatusMessage = "Error: Failed to delete role.";
            }

            return RedirectToAction(nameof(TableUserRole));
        }

    }





    public class UserDetailViewModel : DbUser
    {
        [DisplayName("Login Providers")]
        public List<LoginProviderInfo> LoginProviders { get; set; }
    }

    public class LoginProviderInfo
    {
        public string ProviderName { get; set; }
        public string ProviderKey { get; set; }
        public string ProviderDisplayName { get; set; }
    }
}
