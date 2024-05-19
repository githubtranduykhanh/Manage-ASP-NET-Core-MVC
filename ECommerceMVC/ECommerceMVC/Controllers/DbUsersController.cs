using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ECommerceMVC.Data;

namespace ECommerceMVC.Controllers
{
    [Route("api/v1/[controller]")]
    public class DbUsersController : Controller
    {
        private readonly ECommerceContext _context;

        public DbUsersController(ECommerceContext context)
        {
            _context = context;
        }

        // GET: DbUsers
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var eCommerceContext = _context.DbUsers.Include(d => d.IdRoleNavigation);
            return View(await eCommerceContext.ToListAsync());
        }

        // GET: DbUsers/Details/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dbUser = await _context.DbUsers
                .Include(d => d.IdRoleNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (dbUser == null)
            {
                return NotFound();
            }

            return View(dbUser);
        }

        // GET: DbUsers/Create
        [HttpGet("Create")]
        public IActionResult Create()
        {
            ViewData["IdRole"] = new SelectList(_context.DbRoles, "Id", "Id");
            return View();
        }

        // POST: DbUsers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Address,Phone,Email,Password,IdRole,Sex,CreatedAt,LoginType,RefreshToken,Avatar,SecurityQuestion,Status")] DbUser dbUser)
        {
            if (ModelState.IsValid)
            {
                _context.Add(dbUser);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdRole"] = new SelectList(_context.DbRoles, "Id", "Id", dbUser.IdRole);
            return View(dbUser);
        }

        // GET: DbUsers/Edit/5
        [HttpGet("Edit/{id}")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dbUser = await _context.DbUsers.FindAsync(id);
            if (dbUser == null)
            {
                return NotFound();
            }
            ViewData["IdRole"] = new SelectList(_context.DbRoles, "Id", "Id", dbUser.IdRole);
            return View(dbUser);
        }

        // POST: DbUsers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost("Edit/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Address,Phone,Email,Password,IdRole,Sex,CreatedAt,LoginType,RefreshToken,Avatar,SecurityQuestion,Status")] DbUser dbUser)
        {
            if (id != dbUser.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(dbUser);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DbUserExists(dbUser.Id))
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
            ViewData["IdRole"] = new SelectList(_context.DbRoles, "Id", "Id", dbUser.IdRole);
            return View(dbUser);
        }

        // GET: DbUsers/Delete/5
        [HttpGet("Delete/{id}")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dbUser = await _context.DbUsers
                .Include(d => d.IdRoleNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (dbUser == null)
            {
                return NotFound();
            }

            return View(dbUser);
        }

        // POST: DbUsers/Delete/5
        [HttpPost("Delete/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var dbUser = await _context.DbUsers.FindAsync(id);
            if (dbUser != null)
            {
                _context.DbUsers.Remove(dbUser);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DbUserExists(int id)
        {
            return _context.DbUsers.Any(e => e.Id == id);
        }
    }
}
