using AspNetCoreHero.ToastNotification.Abstractions;
using AutoMapper;
using ECommerceMVC.DataAccess.Data;
using ECommerceMVC.Domain.Entities;
using ECommerceMVC.Infrastructure.Services.Cloudinary;
using ECommerceMVC.UI.Areas.Admin.ViewModels.Group;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace ECommerceMVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin/[controller]/[action]")]
    public class GroupController : BaseController
    {

        public GroupController(SignInManager<DbUser> signInManager, UserManager<DbUser> userManager, ICloudinaryService cloudinaryService, RoleManager<IdentityRole> roleManager, INotyfService notyf, IMapper mapper, ECommerceContext context) : base(signInManager, userManager, cloudinaryService, roleManager, notyf, mapper, context)
        {
        }



        public async Task<IActionResult> Index()
        {
            var list = await _context.DbGroups.ToListAsync();
            ViewBag.ParenstList = await GetParentSelectListAsync();
            return View(list);
        }


        private async Task<SelectList> GetParentSelectListAsync()
        {
            var list = await _context.DbGroups.ToListAsync();

            // Add a null option to the beginning of the list
            list.Insert(0, new DbGroup { Id = 0, Name = "No parent category" });

            // Create SelectList with categories
            return new SelectList(list, "Id", "Name");
        }

        private bool DbGroupExists(int id)
        {
            return _context.DbGroups.Any(e => e.Id == id);
        }


        // GET: Admin/Group/Create
        public async Task<IActionResult> Create()
        {

            ViewBag.ParenstList = await GetParentSelectListAsync();
            return View();
        }


        // GET: Admin/Group/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var db = await _context.DbGroups
                .FirstOrDefaultAsync(m => m.Id == id);
            if (db == null)
            {
                return NotFound();
            }
            ViewBag.ParenstList = await GetParentSelectListAsync();
            return View(db);
        }

        // POST: Admin/Group/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateGroupVM groupVM, IFormFile? Image)
        {
            var db = _mapper.Map<CreateGroupVM, DbGroup>(groupVM);

            if (ModelState.IsValid)
            {
                try
                {

                    if (db.IdGroupParent == 0)
                    {
                        db.IdGroupParent = null;
                    }

                    if (Image != null && Image.Length > 0)
                    {
                        string imageUrl = await _cloudinaryService.UploadFileAsync(Image);
                        if (imageUrl == null)
                        {
                            ModelState.AddModelError(string.Empty, "Failed to upload image.");
                            ViewBag.ParenstList = await GetParentSelectListAsync();
                            return View(db);
                        }
                        db.Image = imageUrl;
                    }
                    else
                    {
                        db.Image = "https://i.pinimg.com/originals/f1/0f/f7/f10ff70a7155e5ab666bcdd1b45b726d.jpg";
                    }
                    _context.Add(db);
                    await _context.SaveChangesAsync();
                    _notyf.Success("Create Group Successfuly");
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, $"Error: {ex.Message}");
                    ViewBag.ParenstList = await GetParentSelectListAsync();
                    return View(db);
                }

            }


            ViewBag.CategoryList = await GetParentSelectListAsync();
            return View(db);
        }

        // GET: Admin/Group/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var db = await _context.DbGroups.FindAsync(id);
            if (db == null)
            {
                return NotFound();
            }

            ViewBag.ParenstList = await GetParentSelectListAsync();
            return View(db);
        }



        // POST: Admin/Group/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CreateGroupVM groupVM, IFormFile? Image)
        {
            //var dbCategory = _mapper.Map<CreateCategoryVM, DbCategory>(categoryVM);         

            if (id != groupVM.Id)
            {
                return NotFound();
            }

            var db = await _context.DbGroups.FindAsync(groupVM.Id);

            if (db == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {

                    if (db.IdGroupParent == 0)
                    {
                        db.IdGroupParent = null;
                    }

                    db.Name = groupVM.Name;
                    db.DisplayOrder = groupVM.DisplayOrder;
                    db.IdGroupParent = groupVM.IdGroupParent;

                    if (Image != null && Image.Length > 0)
                    {
                        string imageUrl = await _cloudinaryService.UploadFileAsync(Image);
                        if (imageUrl == null)
                        {
                            ModelState.AddModelError(string.Empty, "Failed to upload image.");
                            ViewBag.ParenstList = await GetParentSelectListAsync();
                            return View(db);
                        }
                        db.Image = imageUrl;
                    }

                    _context.Update(db);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    if (!DbGroupExists(db.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, $"Error: {ex.Message}");
                        ViewBag.ParenstList = await GetParentSelectListAsync();
                        return View(db);
                    }
                }
                _notyf.Success("Edit Caterory Successfuly");
                return RedirectToAction(nameof(Index));
            }

            ViewBag.ParenstList = await GetParentSelectListAsync();
            return View(db);
        }


        // GET: Admin/Group/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var db = await _context.DbGroups
                .FirstOrDefaultAsync(m => m.Id == id);
            if (db == null)
            {
                return NotFound();
            }

            return View(db);
        }

        // POST: Admin/Group/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var db = await _context.DbGroups.FindAsync(id);
            _context.DbGroups.Remove(db);
            await _context.SaveChangesAsync();
            _notyf.Success("Delete Group Successfuly");
            return RedirectToAction(nameof(Index));
        }

    }
}
