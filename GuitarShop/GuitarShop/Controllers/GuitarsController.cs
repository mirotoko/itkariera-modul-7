using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GuitarShop.Data;
using GuitarShop.Models;

namespace GuitarShop.Controllers
{
    public class GuitarsController : Controller
    {
        private readonly GuitarShopContext _context;

        public GuitarsController(GuitarShopContext context)
        {
            _context = context;
        }

        // GET: Guitars
        public async Task<IActionResult> Index(string searchString)
        {
            // search funkcionalnost, tursi suputstvie v name, brand i type

            var guitars = from g in _context.Guitar
                          select g;

            if (!string.IsNullOrEmpty(searchString))
            {
                guitars = guitars.Where(s => 
                s.Name.Contains(searchString) ||
                s.Brand.Contains(searchString) ||
                s.Type.Contains(searchString));
            }

            ViewData["CurrentFilter"] = searchString;

            return View(await guitars.ToListAsync());
        }

        // GET: Guitars/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var guitar = await _context.Guitar
                .FirstOrDefaultAsync(m => m.Name == id);
            if (guitar == null)
            {
                return NotFound();
            }

            return View(guitar);
        }

        // GET: Guitars/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Guitars/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Type,Body,Brand,Name,Price,Availability,Interest")] Guitar guitar)
        {
            if (ModelState.IsValid)
            {
                _context.Add(guitar);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(guitar);
        }

        // GET: Guitars/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var guitar = await _context.Guitar.FindAsync(id);
            if (guitar == null)
            {
                return NotFound();
            }
            return View(guitar);
        }

        // POST: Guitars/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Type,Body,Brand,Name,Price,Availability,Interest")] Guitar guitar)
        {
            if (id != guitar.Name)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(guitar);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GuitarExists(guitar.Name))
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
            return View(guitar);
        }

        // GET: Guitars/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var guitar = await _context.Guitar
                .FirstOrDefaultAsync(m => m.Name == id);
            if (guitar == null)
            {
                return NotFound();
            }

            return View(guitar);
        }

        // POST: Guitars/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var guitar = await _context.Guitar.FindAsync(id);
            if (guitar != null)
            {
                _context.Guitar.Remove(guitar);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GuitarExists(string id)
        {
            return _context.Guitar.Any(e => e.Name == id);
        }
    }
}
