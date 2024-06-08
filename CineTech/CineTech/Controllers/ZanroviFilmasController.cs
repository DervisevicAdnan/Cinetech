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
    public class ZanroviFilmasController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ZanroviFilmasController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ZanroviFilmas
        public async Task<IActionResult> Index()
        {
            return View(await _context.ZanroviFilma.ToListAsync());
        }

        // GET: ZanroviFilmas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var zanroviFilma = await _context.ZanroviFilma
                .FirstOrDefaultAsync(m => m.id == id);
            if (zanroviFilma == null)
            {
                return NotFound();
            }

            return View(zanroviFilma);
        }

        // GET: ZanroviFilmas/Create
        public IActionResult Create(int? id)
        {
            ViewBag.id = id;
            return View();
        }

        // POST: ZanroviFilmas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        /*
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("idFilma,zanrFilma")] ZanroviFilma zanroviFilma)
        {
            if (ModelState.IsValid)
            {
                _context.Add(zanroviFilma);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(zanroviFilma);
        }*/
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int idFilma, List<Zanr> zanrFilma)
        {
            if (ModelState.IsValid)
            {
                foreach (var zanr in zanrFilma)
                {
                    var zanroviFilma = new ZanroviFilma
                    {
                        idFilma = idFilma,
                        zanrFilma = zanr
                    };
                    _context.ZanroviFilma.Add(zanroviFilma);
                }
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(new ZanroviFilma { idFilma = idFilma });
        }

        // GET: ZanroviFilmas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var zanroviFilma = await _context.ZanroviFilma.FindAsync(id);
            if (zanroviFilma == null)
            {
                return NotFound();
            }
            return View(zanroviFilma);
        }

        // POST: ZanroviFilmas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,idFilma,zanrFilma")] ZanroviFilma zanroviFilma)
        {
            if (id != zanroviFilma.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(zanroviFilma);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ZanroviFilmaExists(zanroviFilma.id))
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
            return View(zanroviFilma);
        }

        // GET: ZanroviFilmas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var zanroviFilma = await _context.ZanroviFilma
                .FirstOrDefaultAsync(m => m.id == id);
            if (zanroviFilma == null)
            {
                return NotFound();
            }

            return View(zanroviFilma);
        }

        // POST: ZanroviFilmas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var zanroviFilma = await _context.ZanroviFilma.FindAsync(id);
            if (zanroviFilma != null)
            {
                _context.ZanroviFilma.Remove(zanroviFilma);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ZanroviFilmaExists(int id)
        {
            return _context.ZanroviFilma.Any(e => e.id == id);
        }
    }
}
