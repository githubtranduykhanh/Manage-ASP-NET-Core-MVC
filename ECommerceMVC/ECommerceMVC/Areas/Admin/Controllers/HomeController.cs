using AspNetCoreHero.ToastNotification.Abstractions;
using AutoMapper;
using ECommerceMVC.Application.Interfaces;
using ECommerceMVC.DataAccess.Data;
using ECommerceMVC.Domain.Abstract.Cloudinary;
using ECommerceMVC.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace ECommerceMVC.Areas.Admin.Controllers
{
   
    [Area("Admin")]  
    public class HomeController : BaseController
    {
        private readonly IDashboardsService _dashboardsService;

        public HomeController(SignInManager<DbUser> signInManager, UserManager<DbUser> userManager, ICloudinaryService cloudinaryService, RoleManager<IdentityRole> roleManager, INotyfService notyf, IMapper mapper, ECommerceContext context, IDashboardsService dashboardsService) : base(signInManager, userManager, cloudinaryService, roleManager, notyf, mapper, context)
        {
            _dashboardsService = dashboardsService;
        }
        public async Task<IActionResult> Index()
        {
            int currentYear = DateTime.Now.Year;
            var resut = await _dashboardsService.GetDataAsync(currentYear);
            return View(resut.data);
        }


        public async Task<IActionResult> JsonData()
        {
            try
            {
                int currentYear = DateTime.Now.Year;

                // Lấy dữ liệu từ dịch vụ
                var resutData = await _dashboardsService.GetDataAsync(currentYear);
                var resutChart = await _dashboardsService.GetChartAsync(currentYear);

             

                var options = new JsonSerializerOptions
                {
                    NumberHandling = JsonNumberHandling.AllowNamedFloatingPointLiterals
                };

                return Json(new { resutData, resutChart }, options);
            }
            catch (Exception ex)
            {
                // Xử lý lỗi (log lỗi hoặc trả về thông báo lỗi)
                // Bạn có thể log lỗi hoặc trả về một đối tượng lỗi như dưới đây:
                return StatusCode(500, new { message = "Có lỗi xảy ra khi xử lý yêu cầu.", error = ex.Message });
            }
        }



     
        public async Task<IActionResult> GetDataChart(int year)
        {
            try
            {
                year = year != 0 ? year : DateTime.Now.Year;             
                var resutChart = await _dashboardsService.GetChartAsync(year);
                return Ok(resutChart);
            }
            catch (Exception ex)
            {
                // Xử lý lỗi (log lỗi hoặc trả về thông báo lỗi)
                // Bạn có thể log lỗi hoặc trả về một đối tượng lỗi như dưới đây:
                return StatusCode(500, new { message = "Có lỗi xảy ra khi xử lý yêu cầu.", error = ex.Message });
            }
        }

    }

}
