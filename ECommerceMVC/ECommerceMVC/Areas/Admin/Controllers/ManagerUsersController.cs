using AutoMapper;
using CloudinaryDotNet.Actions;
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
using System.Security.Claims;
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
        private readonly ECommerceContext _context;
        public ManagerUsersController(SignInManager<DbUser> signInManager,  UserManager<DbUser> userManager, ICloudinaryService cloudinaryService, RoleManager<IdentityRole> roleManager, ECommerceContext context)
        {       
            _signInManager = signInManager;       
            _userManager = userManager;
            _cloudinaryService = cloudinaryService;
            _roleManager = roleManager;
            _context = context;
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


            var userRoles = await _userManager.GetRolesAsync(user);
            var userClaims = await _context.UserClaims
                                      .Where(uc => uc.UserId == userId)
                                      .Select(uc => new
                                      {
                                          uc.Id,
                                          Type = uc.ClaimType,
                                          Value = uc.ClaimValue
                                      })
                                      .ToListAsync();

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
                    Avatar = user.Avatar ?? "https://i.pinimg.com/originals/f1/0f/f7/f10ff70a7155e5ab666bcdd1b45b726d.jpg",
                    Roles = userRoles,
                    Claims = userClaims
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


            // Xử lý roles của người dùng
            if (model.Roles != null)
            {
                // Kiểm tra nếu model.Roles là mảng rỗng
                if (model.Roles != null && model.Roles.Count == 1 && model.Roles[0] == null)
                {
                    // Nếu là mảng rỗng, gán roles của người dùng là một mảng rỗng
                    var userRoles = await _userManager.GetRolesAsync(user);
                    var removeRolesResult = await _userManager.RemoveFromRolesAsync(user, userRoles);
                    if (!removeRolesResult.Succeeded)
                    {
                        return BadRequest(new { status = false, message = "Failed to update user roles.", errors = removeRolesResult.Errors });
                    }
                }
                else
                {
                    // Nếu không phải mảng rỗng, cập nhật roles của người dùng với các roles mới
                    var userRoles = await _userManager.GetRolesAsync(user);
                    var removeRolesResult = await _userManager.RemoveFromRolesAsync(user, userRoles);
                    if (!removeRolesResult.Succeeded)
                    {
                        return BadRequest(new { status = false, message = "Failed to update user roles.", errors = removeRolesResult.Errors });
                    }

                    var addRolesResult = await _userManager.AddToRolesAsync(user, model.Roles);
                    if (!addRolesResult.Succeeded)
                    {
                        return BadRequest(new { status = false, message = "Failed to update user roles.", errors = addRolesResult.Errors });
                    }
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

        [HttpGet]
        public async Task<IActionResult> GetRoles()
        {
            var roles = await _roleManager.Roles.ToListAsync();
            return Ok(new { status = true, message = "Get roles successfully.", data = roles });
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

            var claims = await _context.RoleClaims
               .Where(rc => rc.RoleId == role.Id)
               .Select(rc => new
               {
                   id = rc.Id,
                   type = rc.ClaimType,
                   value = rc.ClaimValue
               }).ToListAsync();



            return Ok(new { status = true, message = "Get role by id successfully.", data = new { role.Id,role.Name, roleClaims = claims } });
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


        [HttpGet("{claimId:int}")]
        public async Task<IActionResult> GetRoleClaimById(int claimId)
        {
            if (claimId <= 0)
            {
                return Ok(new { status = false, message = "Claim ID required.", errors = new object[] { "Role ID, Claim Type, and Claim Value required." } });
            }

            
            var claim = await _context.RoleClaims.FindAsync(claimId);
            

            if (claim == null)
            {
                return Ok(new { status = false, message = "Claim not found.", errors = new object[] { "Claim not found." } });
            }

            return Ok(new { status = true, message = "Claim retrieved successfully.", data = new { id = claim.Id ,type = claim.ClaimType, value = claim.ClaimValue } });
        }


        [HttpPost]
        public async Task<IActionResult> CreateRoleClaim([FromBody] ModelRoleClaim model)
        {

            var role = await _roleManager.FindByIdAsync(model.RoleId);
            if (role == null) return Ok(new { status = false, message = "Role not found.", errors = new object[] { "Role not found." } });


            var claim = await _roleManager.GetClaimsAsync(role);
            var findClaim =  claim.FirstOrDefault(c => c.Value == model.ClaimValue && c.Type == model.ClaimType);
            if (findClaim != null) return Ok(new { status = false, message = "Claim already exist.", errors = new object[] { "Claim already exist." } });

            var result = await _roleManager.AddClaimAsync(role, new Claim(model.ClaimType, model.ClaimValue));
            if(!result.Succeeded) return Ok(new { status = false, message = "Failed to create role claim.", errors = result.Errors.Select(e => e.Description).ToArray() });
            var claims = await _context.RoleClaims
                           .Where(rc => rc.RoleId == role.Id)
                           .Select(rc => new
                           {
                               id = rc.Id,
                               type = rc.ClaimType,
                               value = rc.ClaimValue
                           }).ToListAsync();
            return Ok(new { status = true, message = "Role claim create successfully.", data = new { roleClaims = claims } , errors = new object[] { } });
        }


        [HttpPut]
        public async Task<IActionResult> EditRoleClaim([FromBody] ModelRoleClaim model)
        {
            var role = await _roleManager.FindByIdAsync(model.RoleId);
            if (role == null) return Ok(new { status = false, message = "Role not found.", errors = new object[] { "Role not found." } });


            var claim = await _roleManager.GetClaimsAsync(role);
            var findClaim = claim.FirstOrDefault(c => c.Value == model.ClaimValue && c.Type == model.ClaimType);
            if (findClaim != null) return Ok(new { status = false, message = "Claim already exist.", errors = new object[] { "Claim already exist." } });

            var existingClaim = await _context.RoleClaims.FindAsync(model.ClaimId);
            if (existingClaim == null) return Ok(new { status = false, message = "Claim not found.", errors = new object[] { "Claim not found." } });


            var claimCurent = new Claim(existingClaim.ClaimType, existingClaim.ClaimValue);

            var removeResult = await _roleManager.RemoveClaimAsync(role, claimCurent);
            if (!removeResult.Succeeded) return Ok(new { status = false, message = "Failed to Remove Claim.", errors = removeResult.Errors.Select(e => e.Description).ToArray() });

            var addResult = await _roleManager.AddClaimAsync(role, new Claim(model.ClaimType, model.ClaimValue));
            if (!addResult.Succeeded) return Ok(new { status = false, message = "Failed to Add Claim.", errors = addResult.Errors.Select(e => e.Description).ToArray() });

            var claims = await _context.RoleClaims
                .Where(rc => rc.RoleId == role.Id)
                .Select(rc => new
                {
                    id = rc.Id,
                    type = rc.ClaimType,
                    value = rc.ClaimValue
                }).ToListAsync();


            return Ok(new { status = true, message = "Role claim edit successfully.", data = new { roleClaims = claims } , errors = new object[] { } });
        }



        [HttpDelete("{claimId:int}")]
        public async Task<IActionResult> DeleteRoleClaim(int claimId,[FromBody] ModelRoleClaim model)
        {

            var role = await _roleManager.FindByIdAsync(model.RoleId);
            if (role == null) return Ok(new { status = false, message = "Role not found.", errors = new object[] { "Role not found." } });

            var claim = await _context.RoleClaims.FindAsync(model.ClaimId);
            if (claim == null) return Ok(new { status = false, message = "Claim not found.", errors = new object[] { "Claim not found." } });

            var claimCurent = new Claim(claim.ClaimType, claim.ClaimValue);

            var result = await _roleManager.RemoveClaimAsync(role, claimCurent);
            if (!result.Succeeded) return Ok(new { status = false, message = "Failed to Remove Claim.", errors = result.Errors.Select(e => e.Description).ToArray() });

            var claims = await _context.RoleClaims
                .Where(rc => rc.RoleId == role.Id)
                .Select(rc => new
                {
                    id = rc.Id,
                    type = rc.ClaimType,
                    value = rc.ClaimValue
                }).ToListAsync();

            return Ok(new { status = true, message = "Role claim remove successfully.", data = new { roleClaims = claims }, errors = new object[] { } });
        }



        [HttpPost]       
        public async Task<IActionResult> CreateUserClaim([FromBody] ModelUserClaim model)
        {
            var user = await _userManager.FindByIdAsync(model.UserId);
            if (user == null) return Ok(new { status = false, message = "User not found.", errors = new object[] { "User not found." } });

            var claims = await _userManager.GetClaimsAsync(user);
            var existingClaim = claims.FirstOrDefault(c => c.Type == model.ClaimType && c.Value == model.ClaimValue);
            if(existingClaim != null) return Ok(new { status = false, message = "Claim already exist.", errors = new object[] { "Claim already exist." } });




            var result = await _userManager.AddClaimAsync(user, new Claim(model.ClaimType, model.ClaimValue));


            if (!result.Succeeded) return Ok(new { status = false, message = "Failed to create user claim.", errors = result.Errors.Select(e => e.Description).ToArray() });
          
            var userClaims = await _context.UserClaims
                                     .Where(uc => uc.UserId == user.Id)
                                     .Select(uc => new
                                     {
                                         uc.Id,
                                         Type = uc.ClaimType,
                                         Value = uc.ClaimValue
                                     })
                                     .ToListAsync();


            return Ok(new { status = true, message = "User claim create successfully.", data = new { claims = userClaims }, errors = new object[] { } });
        }


        [HttpGet("{claimId:int}")]
        public async Task<IActionResult> GetUserClaim(int claimId)
        {
            if (claimId <= 0)
            {
                return Ok(new { status = false, message = "Claim ID required.", errors = new object[] { "Claim ID required." } });
            }

            var claim = await _context.UserClaims.FindAsync(claimId);

            if (claim == null)
            {               
                return Ok(new { status = false, message = "Claim not found.", errors = new object[] { "Claim not found." } });
            }


            return Ok(new { status = true, message = "Claim retrieved successfully.", data = new { id = claim.Id, type = claim.ClaimType, value = claim.ClaimValue } });


        }



        [HttpPut]
        public async Task<IActionResult> EditUserClaim([FromBody] ModelUserClaim model)
        {


            if (!ModelState.IsValid)
            {
                // Trả về lỗi validation nếu không đầy đủ dữ liệu
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                return BadRequest(new { status = false, message = "Validation errors", errors });
            }


            var user = await _userManager.FindByIdAsync(model.UserId);
            if (user == null) return Ok(new { status = false, message = "User not found.", errors = new object[] { "User not found." } });


            var existingClaim = await _context.UserClaims.FindAsync(model.ClaimId);
            if (existingClaim == null) return Ok(new { status = false, message = "Claim not found.", errors = new object[] { "Claim not found." } });


            var claims = await _userManager.GetClaimsAsync(user);
            var findClaim = claims.FirstOrDefault(c => c.Type == model.ClaimType && c.Value == model.ClaimValue);
            if (findClaim != null) return Ok(new { status = false, message = "Claim already exist.", errors = new object[] { "Claim already exist." } });

           


            var claimCurent = new Claim(existingClaim.ClaimType, existingClaim.ClaimValue);

            var claimNew = new Claim(model.ClaimType, model.ClaimValue);

            var result = await _userManager.ReplaceClaimAsync(user, claimCurent, claimNew);
            if (!result.Succeeded) return Ok(new { status = false, message = "Failed to Remove Claim.", errors = result.Errors.Select(e => e.Description).ToArray() });

            

            var claimsResult = await _context.UserClaims
                .Where(rc => rc.UserId == user.Id)
                .Select(rc => new
                {
                    id = rc.Id,
                    type = rc.ClaimType,
                    value = rc.ClaimValue
                }).ToListAsync();


            return Ok(new { status = true, message = "User claim edit successfully.", data = new { claims = claimsResult }, errors = new object[] { } });
        }





        [HttpDelete("{claimId:int}")]
        public async Task<IActionResult> DeleteUserClaim(int claimId, [FromBody] ModelUserClaim model)
        {

            var user = await _userManager.FindByIdAsync(model.UserId);
            if (user == null) return Ok(new { status = false, message = "User not found.", errors = new object[] { "User not found." } });


            var existingClaim = await _context.UserClaims.FindAsync(model.ClaimId);
            if (existingClaim == null) return Ok(new { status = false, message = "Claim not found.", errors = new object[] { "Claim not found." } });

            var claimCurent = new Claim(existingClaim.ClaimType, existingClaim.ClaimValue);

            var result = await _userManager.RemoveClaimAsync(user, claimCurent);
            if (!result.Succeeded) return Ok(new { status = false, message = "Failed to Remove Claim.", errors = result.Errors.Select(e => e.Description).ToArray() });

            var claims = await _context.UserClaims
                .Where(rc => rc.UserId == user.Id)
                .Select(rc => new
                {
                    id = rc.Id,
                    type = rc.ClaimType,
                    value = rc.ClaimValue
                }).ToListAsync();

            return Ok(new { status = true, message = "Role claim remove successfully.", data = new { claims }, errors = new object[] { } });
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
