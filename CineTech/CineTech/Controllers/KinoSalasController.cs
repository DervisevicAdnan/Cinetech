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
    public class KinoSalasController : Controller
    {
        private readonly ApplicationDbContext _context;

        public KinoSalasController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: KinoSalas
        public async Task<IActionResult> Index()
        {
            return View(await _context.KinoSala.ToListAsync());
        }

        // GET: KinoSalas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var kinoSala = await _context.KinoSala
                .FirstOrDefaultAsync(m => m.id == id);
            if (kinoSala == null)
            {
                return NotFound();
            }

            return View(kinoSala);
        }

        // GET: KinoSalas/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: KinoSalas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,naziv,brojRedova,brojKolona")] KinoSala kinoSala)
        {
            if (ModelState.IsValid)
            {
                _context.Add(kinoSala);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(kinoSala);
        }

        // GET: KinoSalas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var kinoSala = await _context.KinoSala.FindAsync(id);
            if (kinoSala == null)
            {
                return NotFound();
            }
            return View(kinoSala);
        }

        // POST: KinoSalas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,naziv,brojRedova,brojKolona")] KinoSala kinoSala)
        {
            if (id != kinoSala.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(kinoSala);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!KinoSalaExists(kinoSala.id))
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
            return View(kinoSala);
        }

        // GET: KinoSalas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var kinoSala = await _context.KinoSala
                .FirstOrDefaultAsync(m => m.id == id);
            if (kinoSala == null)
            {
                return NotFound();
            }

            return View(kinoSala);
        }

        // POST: KinoSalas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var kinoSala = await _context.KinoSala.FindAsync(id);
            if (kinoSala != null)
            {
                _context.KinoSala.Remove(kinoSala);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool KinoSalaExists(int id)
        {
            return _context.KinoSala.Any(e => e.id == id);
        }
    }
}
