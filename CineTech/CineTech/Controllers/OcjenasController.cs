using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CineTech.Data;
using CineTech.Models;

namespace CineTech.Controllers
{
    public class OcjenasController : Controller
    {
        private readonly ApplicationDbContext _context;

        public OcjenasController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Ocjenas
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Ocjena.Include(o => o.korisnik);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Ocjenas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ocjena = await _context.Ocjena
                .Include(o => o.korisnik)
                .FirstOrDefaultAsync(m => m.id == id);
            if (ocjena == null)
            {
                return NotFound();
            }

            return View(ocjena);
        }

        // GET: Ocjenas/Create
        public IActionResult Create()
        {
           ViewData["korisnikId"] = new SelectList(_context.Korisnik, "Id", "Id");
            return View();
        }

        // POST: Ocjenas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
         [ValidateAntiForgeryToken]
         public async Task<IActionResult> Create([Bind("id,ocjena,komentar,datum,korisnikId")] Ocjena ocjena)
         {
             if (ModelState.IsValid)
             {
                 _context.Add(ocjena);
                 await _context.SaveChangesAsync();
                 return RedirectToAction(nameof(Index));
             }
             ViewData["korisnikId"] = new SelectList(_context.Korisnik, "Id", "Id", ocjena.korisnikId);
             return View();
         }


        // GET: Ocjenas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ocjena = await _context.Ocjena.FindAsync(id);
            if (ocjena == null)
            {
                return NotFound();
            }
            ViewData["korisnikId"] = new SelectList(_context.Korisnik, "Id", "Id", ocjena.korisnikId);
            return View(ocjena);
        }

        // POST: Ocjenas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,ocjena,komentar,datum,korisnikId")] Ocjena ocjena)
        {
            if (id != ocjena.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(ocjena);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OcjenaExists(ocjena.id))
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
            ViewData["korisnikId"] = new SelectList(_context.Korisnik, "Id", "Id", ocjena.korisnikId);
            return View(ocjena);
        }

        // GET: Ocjenas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ocjena = await _context.Ocjena
                .Include(o => o.korisnik)
                .FirstOrDefaultAsync(m => m.id == id);
            if (ocjena == null)
            {
                return NotFound();
            }

            return View(ocjena);
        }

        // POST: Ocjenas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var ocjena = await _context.Ocjena.FindAsync(id);
            if (ocjena != null)
            {
                _context.Ocjena.Remove(ocjena);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OcjenaExists(int id)
        {
            return _context.Ocjena.Any(e => e.id == id);
        }
    }
}
