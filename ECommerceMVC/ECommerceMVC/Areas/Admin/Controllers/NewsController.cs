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
    public class NewsController : Controller
    {
        private readonly ECommerceContext _context;

        public NewsController(ECommerceContext context)
        {
            _context = context;
        }

        // GET: Admin/News
        public async Task<IActionResult> Index()
        {
            var eCommerceContext = _context.DbNews.Include(d => d.IdNewCategoryNavigation);
            return View(await eCommerceContext.ToListAsync());
        }

        // GET: Admin/News/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dbNew = await _context.DbNews
                .Include(d => d.IdNewCategoryNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (dbNew == null)
            {
                return NotFound();
            }

            return View(dbNew);
        }

        // GET: Admin/News/Create
        public IActionResult Create()
        {
            ViewData["IdNewCategory"] = new SelectList(_context.DbNewCategories, "Id", "Image");
            return View();
        }

        // POST: Admin/News/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Image,Describe,CreatedAt,View,Content,IdNewCategory")] DbNew dbNew)
        {
            if (ModelState.IsValid)
            {
                _context.Add(dbNew);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdNewCategory"] = new SelectList(_context.DbNewCategories, "Id", "Image", dbNew.IdNewCategory);
            return View(dbNew);
        }

        // GET: Admin/News/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dbNew = await _context.DbNews.FindAsync(id);
            if (dbNew == null)
            {
                return NotFound();
            }
            ViewData["IdNewCategory"] = new SelectList(_context.DbNewCategories, "Id", "Image", dbNew.IdNewCategory);
            return View(dbNew);
        }

        // POST: Admin/News/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Image,Describe,CreatedAt,View,Content,IdNewCategory")] DbNew dbNew)
        {
            if (id != dbNew.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(dbNew);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DbNewExists(dbNew.Id))
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
            ViewData["IdNewCategory"] = new SelectList(_context.DbNewCategories, "Id", "Image", dbNew.IdNewCategory);
            return View(dbNew);
        }

        // GET: Admin/News/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dbNew = await _context.DbNews
                .Include(d => d.IdNewCategoryNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (dbNew == null)
            {
                return NotFound();
            }

            return View(dbNew);
        }

        // POST: Admin/News/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var dbNew = await _context.DbNews.FindAsync(id);
            if (dbNew != null)
            {
                _context.DbNews.Remove(dbNew);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DbNewExists(int id)
        {
            return _context.DbNews.Any(e => e.Id == id);
        }
    }
}
