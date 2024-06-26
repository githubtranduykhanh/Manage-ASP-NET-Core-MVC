using ECommerceMVC.Data;
using ECommerceMVC.Services.Store;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ECommerceMVC.Areas.Identity.Components.IdentityUserInformation
{
    [ViewComponent]
    public class IdentityUserInformation : ViewComponent
    {
        
        private readonly SignInManager<DbUser> _signInManager;
        private readonly UserManager<DbUser> _userManager;
        

        public IdentityUserInformation(SignInManager<DbUser> signInManager, UserManager<DbUser> userManager)
        {
            _signInManager = signInManager;          
            _userManager = userManager;
        }
      

        public async Task<IViewComponentResult> InvokeAsync()
        {

            if (!_signInManager.IsSignedIn((ClaimsPrincipal)User))
            {
                return View(new UserInformationVM());
            }
            var user = await _userManager.GetUserAsync((ClaimsPrincipal)User);

            if (user == null)
            {
                return View(new UserInformationVM());
            }

            if (user.Avatar == null) user.Avatar = "https://i.pinimg.com/originals/f1/0f/f7/f10ff70a7155e5ab666bcdd1b45b726d.jpg";
            if (user.DisplayName == null) user.DisplayName = user.UserName;

            var userInformationVM = new UserInformationVM
            {
                Status = true,
                User = user
            };

            return View(userInformationVM);
        }
    }
    public class UserInformationVM
    {
        public bool Status { get; set; } = false;
        public DbUser? User { get; set; } = null!;
        
    }

}
