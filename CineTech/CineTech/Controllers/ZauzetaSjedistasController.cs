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
    public class ZauzetaSjedistasController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ZauzetaSjedistasController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ZauzetaSjedistas
        public async Task<IActionResult> Index()
        {
            return View(await _context.ZauzetaSjedista.ToListAsync());
        }

        // GET: ZauzetaSjedistas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var zauzetaSjedista = await _context.ZauzetaSjedista
                .FirstOrDefaultAsync(m => m.id == id);
            if (zauzetaSjedista == null)
            {
                return NotFound();
            }

            return View(zauzetaSjedista);
        }
        [HttpGet]

        // GET: ZauzetaSjedistas/Create
        public async Task<IActionResult> OdabirSjedista2(int? id)
        {
            ViewBag.id = id;
            if (id == null)
            {
                return BadRequest("Neispravan ID projekcije.");
            }

            var projekcija = await _context.Projekcija
                                               .FirstOrDefaultAsync(p => p.id == id);

            if (projekcija == null)
            {
                return NotFound();
            }
            var kinosala = _context.KinoSala
                                   .FirstOrDefault(k => k.id == projekcija.kinoSalaId);

            if (kinosala == null)
            {
                return NotFound();
            }

            var sviRedoviMjesta = from red in Enumerable.Range(1, kinosala.brojRedova)
                                  from mjesto in Enumerable.Range(1, kinosala.brojKolona)
                                  select new  { Red = red, Mjesto = mjesto };

            var rezervisanaMjesta = _context.ZauzetaSjedista
                                            .Where(r => r.ProjekcijaId == id)
                                            .Select(r => new { Red = r.red, Mjesto = r.redniBrojSjedista })
                                            .ToList();

            var slobodnaMjesta = sviRedoviMjesta
                                 .Where(m => !rezervisanaMjesta.Any(rm => rm.Red == m.Red && rm.Mjesto == m.Mjesto))
                                 .ToList();

            var slobodnaMjestaNoviTip =  slobodnaMjesta
    .Select(t => new ZauzetaSjedista { red = t.Red, redniBrojSjedista = t.Mjesto, ProjekcijaId=ViewBag.id })
    .ToList();

            return View(slobodnaMjestaNoviTip);
        }


        // POST: ZauzetaSjedistas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> OdabirSjedista([Bind("id,red,redniBrojSjedista,ProjekcijaId")] ZauzetaSjedista zauzetaSjedista)
        {
            if (ModelState.IsValid)
            {
                _context.Add(zauzetaSjedista);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(zauzetaSjedista);
        }

        // GET: ZauzetaSjedistas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var zauzetaSjedista = await _context.ZauzetaSjedista.FindAsync(id);
            if (zauzetaSjedista == null)
            {
                return NotFound();
            }
            return View(zauzetaSjedista);
        }

        // POST: ZauzetaSjedistas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,red,redniBrojSjedista,ProjekcijaId")] ZauzetaSjedista zauzetaSjedista)
        {
            if (id != zauzetaSjedista.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(zauzetaSjedista);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ZauzetaSjedistaExists(zauzetaSjedista.id))
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
            return View(zauzetaSjedista);
        }

        // GET: ZauzetaSjedistas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var zauzetaSjedista = await _context.ZauzetaSjedista
                .FirstOrDefaultAsync(m => m.id == id);
            if (zauzetaSjedista == null)
            {
                return NotFound();
            }

            return View(zauzetaSjedista);
        }

        // POST: ZauzetaSjedistas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var zauzetaSjedista = await _context.ZauzetaSjedista.FindAsync(id);
            if (zauzetaSjedista != null)
            {
                _context.ZauzetaSjedista.Remove(zauzetaSjedista);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ZauzetaSjedistaExists(int id)
        {
            return _context.ZauzetaSjedista.Any(e => e.id == id);
        }
    }
}
