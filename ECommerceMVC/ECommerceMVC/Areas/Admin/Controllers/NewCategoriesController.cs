using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ECommerceMVC.DataAccess.Data;
using ECommerceMVC.Domain.Entities;

namespace ECommerceMVC.UI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class NewCategoriesController : Controller
    {
        private readonly ECommerceContext _context;

        public NewCategoriesController(ECommerceContext context)
        {
            _context = context;
        }

        // GET: Admin/NewCategories
        public async Task<IActionResult> Index()
        {
            return View(await _context.DbNewCategories.ToListAsync());
        }

        // GET: Admin/NewCategories/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dbNewCategory = await _context.DbNewCategories
                .FirstOrDefaultAsync(m => m.Id == id);
            if (dbNewCategory == null)
            {
                return NotFound();
            }

            return View(dbNewCategory);
        }

        // GET: Admin/NewCategories/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/NewCategories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Image,DisplayOrder,IdNewParent")] DbNewCategory dbNewCategory)
        {
            if (ModelState.IsValid)
            {
                _context.Add(dbNewCategory);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(dbNewCategory);
        }

        // GET: Admin/NewCategories/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dbNewCategory = await _context.DbNewCategories.FindAsync(id);
            if (dbNewCategory == null)
            {
                return NotFound();
            }
            return View(dbNewCategory);
        }

        // POST: Admin/NewCategories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Image,DisplayOrder,IdNewParent")] DbNewCategory dbNewCategory)
        {
            if (id != dbNewCategory.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(dbNewCategory);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DbNewCategoryExists(dbNewCategory.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(dbNewCategory);
        }

        // GET: Admin/NewCategories/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dbNewCategory = await _context.DbNewCategories
                .FirstOrDefaultAsync(m => m.Id == id);
            if (dbNewCategory == null)
            {
                return NotFound();
            }

            return View(dbNewCategory);
        }

        // POST: Admin/NewCategories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var dbNewCategory = await _context.DbNewCategories.FindAsync(id);
            if (dbNewCategory != null)
            {
                _context.DbNewCategories.Remove(dbNewCategory);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DbNewCategoryExists(int id)
        {
            return _context.DbNewCategories.Any(e => e.Id == id);
        }
    }
}
