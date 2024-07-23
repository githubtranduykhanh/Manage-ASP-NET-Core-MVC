using AspNetCoreHero.ToastNotification.Abstractions;
using AutoMapper;
using ECommerceMVC.DataAccess.Data;
using ECommerceMVC.Domain.Abstract.Cloudinary;
using ECommerceMVC.Domain.Entities;
using ECommerceMVC.Infrastructure.Services.Cloudinary;
using ECommerceMVC.UI.Areas.Admin.ViewModels.Caterory;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace ECommerceMVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin/[controller]/[action]")]
    public class CateroryController : BaseController
    {
        public CateroryController(SignInManager<DbUser> signInManager, UserManager<DbUser> userManager, ICloudinaryService cloudinaryService, RoleManager<IdentityRole> roleManager, INotyfService notyf, IMapper mapper, ECommerceContext context ) : base(signInManager, userManager, cloudinaryService, roleManager, notyf, mapper, context)
        {
        }

        // GET: Admin/Category
        public async Task<IActionResult> Index()
        {          
            var categories = await _context.DbCategories.ToListAsync();
            ViewBag.CategoryList = await GetCategorySelectListAsync();
            return View(categories);
        }

        // GET: Admin/Category/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _context.DbCategories
                .FirstOrDefaultAsync(m => m.Id == id);
            if (category == null)
            {
                return NotFound();
            }
            ViewBag.CategoryList = await GetCategorySelectListAsync();
            return View(category);
        }


        private async Task<SelectList> GetCategorySelectListAsync()
        {
            var categories = await _context.DbCategories.ToListAsync();

            // Add a null option to the beginning of the list
            categories.Insert(0, new DbCategory { Id = 0, Name = "No parent category" });

            // Create SelectList with categories
            return new SelectList(categories, "Id", "Name");         
        }


        // GET: Admin/Category/Create
        public async Task<IActionResult> Create()
        {

            ViewBag.CategoryList = await GetCategorySelectListAsync();
            return View();
        }

        // POST: Admin/Category/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateCateroryVM categoryVM, IFormFile? Image)
        {
            var dbCategory = _mapper.Map<CreateCateroryVM, DbCategory>(categoryVM);

            if (ModelState.IsValid)
            {
                try
                {

                    if (dbCategory.IdCategoryParent == 0)
                    {
                        dbCategory.IdCategoryParent = null;
                    }

                    if (Image != null && Image.Length > 0)
                    {
                        string imageUrl = await _cloudinaryService.UploadFileAsync(Image);
                        if (imageUrl == null)
                        {
                            ModelState.AddModelError(string.Empty, "Failed to upload image.");
                            ViewBag.CategoryList = await GetCategorySelectListAsync();
                            return View(dbCategory);
                        }
                        dbCategory.Image = imageUrl;
                    }
                    else {
                        dbCategory.Image = "https://i.pinimg.com/originals/f1/0f/f7/f10ff70a7155e5ab666bcdd1b45b726d.jpg";
                    }
                    _context.Add(dbCategory);
                    await _context.SaveChangesAsync();
                    _notyf.Success("Create Caterory Successfuly");
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex) 
                {
                    ModelState.AddModelError(string.Empty, $"Error: {ex.Message}");
                    ViewBag.CategoryList = await GetCategorySelectListAsync();
                    return View(dbCategory);
                }
               
            }


            ViewBag.CategoryList = await GetCategorySelectListAsync();
            return View(dbCategory);
        }

        // GET: Admin/Category/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dbCategory = await _context.DbCategories.FindAsync(id);
            if (dbCategory == null)
            {
                return NotFound();
            }

            ViewBag.CategoryList = await GetCategorySelectListAsync();
            return View(dbCategory);
        }

        // POST: Admin/Category/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CreateCateroryVM categoryVM, IFormFile? Image)
        {
            //var dbCategory = _mapper.Map<CreateCategoryVM, DbCategory>(categoryVM);         

            if (id != categoryVM.Id)
            {
                return NotFound();
            }

            var dbCategory = await _context.DbCategories.FindAsync(categoryVM.Id);

            if (dbCategory == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {                  

                    if(categoryVM.IdCategoryParent == 0)
                    {
                        categoryVM.IdCategoryParent = null;
                    }
                    
                    dbCategory.Name = categoryVM.Name;
                    dbCategory.DisplayOrder = categoryVM.DisplayOrder;
                    dbCategory.IdCategoryParent = categoryVM.IdCategoryParent;

                    if (Image != null && Image.Length > 0)
                    {
                        string imageUrl = await _cloudinaryService.UploadFileAsync(Image);
                        if (imageUrl == null)
                        {
                            ModelState.AddModelError(string.Empty, "Failed to upload image.");
                            ViewBag.CategoryList = await GetCategorySelectListAsync();
                            return View(dbCategory);
                        }
                        dbCategory.Image = imageUrl;
                    }
                    
                    _context.Update(dbCategory);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    if (!DbCategoryExists(dbCategory.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, $"Error: {ex.Message}");
                        ViewBag.CategoryList = await GetCategorySelectListAsync();
                        return View(dbCategory);
                    }
                }
                _notyf.Success("Edit Caterory Successfuly");
                return RedirectToAction(nameof(Index));
            }

            ViewBag.CategoryList = await GetCategorySelectListAsync();
            return View(dbCategory);
        }

        // GET: Admin/Category/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dbCategory = await _context.DbCategories
                .FirstOrDefaultAsync(m => m.Id == id);
            if (dbCategory == null)
            {
                return NotFound();
            }

            return View(dbCategory);
        }

        // POST: Admin/Category/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var dbCategory = await _context.DbCategories.FindAsync(id);
            _context.DbCategories.Remove(dbCategory);
            await _context.SaveChangesAsync();
            _notyf.Success("Delete Category Successfuly");
            return RedirectToAction(nameof(Index));
        }

        private bool DbCategoryExists(int id)
        {
            return _context.DbCategories.Any(e => e.Id == id);
        }
    }
}

