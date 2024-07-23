using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ECommerceMVC.DataAccess.Data;
using ECommerceMVC.Domain.Entities;
using ECommerceMVC.Application.Interfaces;
using ECommerceMVC.Application.Dtos.DataTable;
using ECommerceMVC.UI.Areas.Admin.ViewModels.NewCategories;
using ECommerceMVC.Areas.Admin.Controllers;
using AspNetCoreHero.ToastNotification.Abstractions;
using AutoMapper;
using ECommerceMVC.Domain.Abstract.Cloudinary;
using Microsoft.AspNetCore.Identity;
using ECommerceMVC.Application.Services.NewCategories;
using ECommerceMVC.UI.Areas.Admin.Models.NewCategories;
using ECommerceMVC.Application.Dtos.NewCategories;

namespace ECommerceMVC.UI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class NewCategoriesController : BaseController
    {      
        private readonly INewCategoriesService _newCategoriesService;
       
        public NewCategoriesController(SignInManager<DbUser> signInManager, UserManager<DbUser> userManager, ICloudinaryService cloudinaryService, RoleManager<IdentityRole> roleManager, INotyfService notyf, IMapper mapper, ECommerceContext context, INewCategoriesService newCategoriesService) : base(signInManager, userManager, cloudinaryService, roleManager, notyf, mapper, context)
        {
            _newCategoriesService = newCategoriesService;
        }

        private async Task SetViewBagsAsync()
        {

            var categories = await _context.DbNewCategories.ToListAsync();

            // Add a null option to the beginning of the list
            categories.Insert(0, new DbNewCategory { Id = 0, Name = "No parent category" });

            // Create SelectList with categories          
            ViewBag.CategoryList = new SelectList(categories, "Id", "Name");
        }


        // GET: Admin/NewCategories
        public async Task<IActionResult> Index(RequestDataTable request)
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

            var result = await _newCategoriesService.GetAllDataTableAsync(request);

            return Ok(result);
        }

        // GET: Admin/NewCategories/Details/5
        public async Task<IActionResult> DetailsPartial(int? id)
        {
            if (id == null) return NotFound();        
            var db = await _newCategoriesService.GetByIdAsync(id);
            if (db == null) return NotFound(); 
            var model = _mapper.Map<NewCategoriesModel, NewCategoryVM>(db);
            model.UrlImage = db.Image;
            return PartialView("Partials/DetailsPartial", model);        
        }

        // GET: Admin/NewCategories/Create
        public async Task<IActionResult> CreateOrEditPartial(int? id)
        {
            if(id == null || id == 0)
            {
                ViewBag.Type = "Create";
                await SetViewBagsAsync();
                return PartialView("Partials/CreateOrEditPartial");
            }
            ViewBag.Type = "Edit";
            var db = await _newCategoriesService.GetByIdAsync(id);
            if (db == null) { return NotFound(); }  
            await SetViewBagsAsync();
            var model = _mapper.Map<NewCategoriesModel, NewCategoryVM>(db);
            model.UrlImage = db.Image;
            return PartialView("Partials/CreateOrEditPartial", model);
        }

        // POST: Admin/NewCategories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(NewCategoryVM model)
        {
            if (ModelState.IsValid)
            {             
                var dbNewCategory = new DbNewCategory();
                dbNewCategory.Image = "https://i.pinimg.com/originals/f1/0f/f7/f10ff70a7155e5ab666bcdd1b45b726d.jpg";
                if (model.Image != null) dbNewCategory.Image = await _cloudinaryService.UploadFileAsync(model.Image);
                dbNewCategory.Name = model.Name;
                dbNewCategory.IdNewParent = model.IdNewParent != 0 ? model.IdNewParent : null;
                dbNewCategory.DisplayOrder = model.DisplayOrder;
                await _context.AddAsync(dbNewCategory);
                await _context.SaveChangesAsync();
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
        // POST: Admin/NewCategories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(NewCategoryVM model)
        {
          
            if (ModelState.IsValid)
            {
                var dbMap = _mapper.Map<NewCategoryVM, NewCategoriesModel>(model);
                var resut = await _newCategoriesService.UpdateAsync(dbMap, model.Image);
                return Ok(resut);
            }         
            return Ok(new ResponseModel
            {
                success = false,
                message = "Create errors.",
                EnumErrors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)
            });
        }

        // GET: Admin/NewCategories/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var db = await _newCategoriesService.GetByIdAsync(id);
            if (db == null) { return NotFound(); }       
            var model = _mapper.Map<NewCategoriesModel, NewCategoryVM>(db);
            return PartialView("Partials/DeletePartial", model);         
        }

        // POST: Admin/NewCategories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            return Ok(await _newCategoriesService.DeleteAsync(id));
        }

        private bool DbNewCategoryExists(int id)
        {
            return _context.DbNewCategories.Any(e => e.Id == id);
        }
    }
}
