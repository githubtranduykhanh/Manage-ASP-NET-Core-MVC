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
    public class OrdersController : Controller
    {
        private readonly ECommerceContext _context;

        public OrdersController(ECommerceContext context)
        {
            _context = context;
        }

        // GET: Admin/Orders
        public async Task<IActionResult> Index()
        {
            var eCommerceContext = _context.DbOrders.Include(d => d.IdUserNavigation);
            return View(await eCommerceContext.ToListAsync());
        }

        // GET: Admin/Orders/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dbOrder = await _context.DbOrders
                .Include(d => d.IdUserNavigation)             
                .FirstOrDefaultAsync(m => m.Id == id);
            if (dbOrder == null)
            {
                return NotFound();
            }

            return View(dbOrder);
        }

        // GET: Admin/Orders/Create
        public IActionResult Create()
        {
            ViewData["IdUser"] = new SelectList(_context.DbUsers, "Id", "Id");
            return View();
        }

        // POST: Admin/Orders/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,TotalAmount,Status,IdUser,NameUser,EmailUser,AddressUser,PhoneUser,PaymentType,CreatedAt")] DbOrder dbOrder)
        {
            if (ModelState.IsValid)
            {
                _context.Add(dbOrder);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdUser"] = new SelectList(_context.DbUsers, "Id", "Id", dbOrder.IdUser);
            return View(dbOrder);
        }

        // GET: Admin/Orders/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dbOrder = await _context.DbOrders.FindAsync(id);
            if (dbOrder == null)
            {
                return NotFound();
            }
            ViewData["IdUser"] = new SelectList(_context.DbUsers, "Id", "Id", dbOrder.IdUser);
            return View(dbOrder);
        }

        // POST: Admin/Orders/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,TotalAmount,Status,IdUser,NameUser,EmailUser,AddressUser,PhoneUser,PaymentType,CreatedAt")] DbOrder dbOrder)
        {
            if (id != dbOrder.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(dbOrder);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DbOrderExists(dbOrder.Id))
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
            ViewData["IdUser"] = new SelectList(_context.DbUsers, "Id", "Id", dbOrder.IdUser);
            return View(dbOrder);
        }

        // GET: Admin/Orders/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dbOrder = await _context.DbOrders
                .Include(d => d.IdUserNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (dbOrder == null)
            {
                return NotFound();
            }

            return View(dbOrder);
        }

        // POST: Admin/Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var dbOrder = await _context.DbOrders.FindAsync(id);
            if (dbOrder != null)
            {
                _context.DbOrders.Remove(dbOrder);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DbOrderExists(int id)
        {
            return _context.DbOrders.Any(e => e.Id == id);
        }
    }
}
