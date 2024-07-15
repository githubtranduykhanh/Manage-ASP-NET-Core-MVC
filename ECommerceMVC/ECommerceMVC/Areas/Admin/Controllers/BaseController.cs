using AspNetCoreHero.ToastNotification.Abstractions;
using AutoMapper;
using ECommerceMVC.Data;
using ECommerceMVC.Services.Cloudinary;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceMVC.Areas.Admin.Controllers
{
    public class BaseController : Controller
    {
        protected readonly SignInManager<DbUser> _signInManager;
        protected readonly UserManager<DbUser> _userManager;
        protected readonly ICloudinaryService _cloudinaryService;
        protected readonly RoleManager<IdentityRole> _roleManager;
        protected readonly ECommerceContext _context;
        protected readonly INotyfService _notyf;
        protected readonly IMapper _mapper;

        [TempData]
        public string StatusMessage { get; set; }

        public BaseController(
            SignInManager<DbUser> signInManager,
            UserManager<DbUser> userManager,
            ICloudinaryService cloudinaryService,
            RoleManager<IdentityRole> roleManager,
            INotyfService notyf,
            IMapper mapper,
            ECommerceContext context)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _cloudinaryService = cloudinaryService;
            _roleManager = roleManager;
            _context = context;
            _notyf = notyf;
            _mapper = mapper;
        }
    }
}
