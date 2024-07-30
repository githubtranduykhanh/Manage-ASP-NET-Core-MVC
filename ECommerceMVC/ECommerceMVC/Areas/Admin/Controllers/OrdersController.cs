using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ECommerceMVC.DataAccess.Data;
using ECommerceMVC.Domain.Entities;
using AspNetCoreHero.ToastNotification.Abstractions;
using AutoMapper;
using ECommerceMVC.Application.Interfaces;
using ECommerceMVC.Domain.Abstract.Cloudinary;
using Microsoft.AspNetCore.Identity;
using ECommerceMVC.Application.Services.NewCategories;
using ECommerceMVC.Areas.Admin.Controllers;
using ECommerceMVC.Application.Dtos.NewCategories;
using ECommerceMVC.UI.Areas.Admin.ViewModels.NewCategories;
using ECommerceMVC.Application.Dtos.Order;
using ECommerceMVC.UI.Areas.Admin.ViewModels.Order;
using ECommerceMVC.UI.Areas.Admin.Models.NewCategories;
using ECommerceMVC.Application.Dtos.DataTable;

namespace ECommerceMVC.UI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class OrdersController : BaseController
    {
        
        private readonly IOrderService _orderService;

        public OrdersController(SignInManager<DbUser> signInManager, UserManager<DbUser> userManager, ICloudinaryService cloudinaryService, RoleManager<IdentityRole> roleManager, INotyfService notyf, IMapper mapper, ECommerceContext context, IOrderService orderService) : base(signInManager, userManager, cloudinaryService, roleManager, notyf, mapper, context)
        {
            _orderService = orderService;
        }

        // GET: Admin/Orders
        public async Task<IActionResult> Index()
        {          
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> GetAllDataTable(RequestDataTable request)
        {
            if (request == null)
            {
                return BadRequest();
            }

            var result = await _orderService.GetAllDataTableAsync(request);

            return Ok(result);
        }


        // GET: Admin/Orders/DetailsPartial/5
        public async Task<IActionResult> DetailsPartial(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var db = await _orderService.GetByIdAsync(id);
            if (db == null || db.success == false || db.data == null) { return NotFound(); }
            var model = _mapper.Map<OrderModel, OrderVM>(db.data);
            return PartialView("Partials/DetailsPartial", model);
        }

        // GET: Admin/Orders/Create
        public async Task<IActionResult> CreateOrEditPartial(int? id)
        {
            if (id == null || id == 0)
            {
                ViewBag.Type = "Create";
                return PartialView("Partials/CreateOrEditPartial");
            }
            ViewBag.Type = "Edit";
            var db = await _orderService.GetByIdAsync(id);
            if (db == null || db.success == false || db.data == null) { return NotFound(); }
            var model = _mapper.Map<OrderModel, OrderVM>(db.data);        
            return PartialView("Partials/CreateOrEditPartial", model);
        }

        // POST: Admin/Orders/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(OrderVM model)
        {
            if (ModelState.IsValid)
            {
                return Ok(new ResponseModel
                {
                    success = true,
                    message = "Create successfully."
                });
            }

            return Ok(new ResponseModel
            {
                success = false,
                message = "Create errors.",
                EnumErrors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)
            });
        }

        // POST: Admin/Orders/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(OrderVM model)
        {
            if (ModelState.IsValid)
            {
                return Ok(await _orderService.UpdateAsync(_mapper.Map<OrderVM, OrderModel>(model)));
            }

            return Ok(new ResponseModel
            {
                success = false,
                message = "Edit errors.",
                EnumErrors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)
            });
        }

        // GET: Admin/Orders/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var db = await _orderService.GetByIdAsync(id);
            if (db == null || db.success == false || db.data == null) { return NotFound(); }
            var model = _mapper.Map<OrderModel, OrderVM>(db.data);
            return PartialView("Partials/DeletePartial", model);
        }

        // POST: Admin/Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            return Ok(await _orderService.DeleteAsync(id));
        }

        private bool DbOrderExists(int id)
        {
            return _context.DbOrders.Any(e => e.Id == id);
        }
    }
}
