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
    public class ProjekcijasController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProjekcijasController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Projekcijas
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Projekcija.Include(p => p.Film).Include(p => p.kinoSala);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Projekcijas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var projekcija = await _context.Projekcija
                .Include(p => p.Film)
                .Include(p => p.kinoSala)
                .FirstOrDefaultAsync(m => m.id == id);
            if (projekcija == null)
            {
                return NotFound();
            }

            return View(projekcija);
        }

        // GET: Projekcijas/Create
        public IActionResult Create()
        {
            ViewData["filmId"] = new SelectList(_context.Film, "id", "id");
            ViewData["kinoSalaId"] = new SelectList(_context.KinoSala, "id", "id");
            return View();
        }

        // POST: Projekcijas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,datum,vrijeme,cijenaOsnovneKarte,kinoSalaId,filmId")] Projekcija projekcija)
        {
            if (ModelState.IsValid)
            {
                _context.Add(projekcija);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["filmId"] = new SelectList(_context.Film, "id", "id", projekcija.filmId);
            ViewData["kinoSalaId"] = new SelectList(_context.KinoSala, "id", "id", projekcija.kinoSalaId);
            return View(projekcija);
        }

        // GET: Projekcijas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var projekcija = await _context.Projekcija.FindAsync(id);
            if (projekcija == null)
            {
                return NotFound();
            }
            ViewData["filmId"] = new SelectList(_context.Film, "id", "id", projekcija.filmId);
            ViewData["kinoSalaId"] = new SelectList(_context.KinoSala, "id", "id", projekcija.kinoSalaId);
            return View(projekcija);
        }

        // POST: Projekcijas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,datum,vrijeme,cijenaOsnovneKarte,kinoSalaId,filmId")] Projekcija projekcija)
        {
            if (id != projekcija.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(projekcija);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProjekcijaExists(projekcija.id))
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
            ViewData["filmId"] = new SelectList(_context.Film, "id", "id", projekcija.filmId);
            ViewData["kinoSalaId"] = new SelectList(_context.KinoSala, "id", "id", projekcija.kinoSalaId);
            return View(projekcija);
        }

        // GET: Projekcijas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var projekcija = await _context.Projekcija
                .Include(p => p.Film)
                .Include(p => p.kinoSala)
                .FirstOrDefaultAsync(m => m.id == id);
            if (projekcija == null)
            {
                return NotFound();
            }

            return View(projekcija);
        }

        // POST: Projekcijas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var projekcija = await _context.Projekcija.FindAsync(id);
            if (projekcija != null)
            {
                _context.Projekcija.Remove(projekcija);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProjekcijaExists(int id)
        {
            return _context.Projekcija.Any(e => e.id == id);
        }
    }
}
