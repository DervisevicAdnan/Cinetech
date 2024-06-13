using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CineTech.Data;
using CineTech.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;
using Microsoft.DotNet.Scaffolding.Shared.CodeModifier.CodeChange;
using RestSharp;
using Newtonsoft.Json.Linq;

namespace CineTech.Controllers
{
    public class FilmsController : Controller
    {
        private readonly ApplicationDbContext _context;
       // private readonly MovieService movieService;

        public FilmsController(ApplicationDbContext context /*MovieService movieService*/)
        {
            _context = context;
           // this.movieService = movieService;
        }
        // GET: Films
        public async Task<IActionResult> Index()
        {
            var aktuelniFilmovi = await _context.Film
            .Where(f => f.StatusPrikazivanja == StatusPrikazivanja.Aktuelan)
            .ToListAsync();
            return View(aktuelniFilmovi);
        }
        [Authorize(Roles = "Administrator")]
        public IActionResult AdminIndex()
        {

            return View();
        }
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> AdminFilmovi()
        {
            var filmovi = await _context.Film.OrderBy(f => f.StatusPrikazivanja == StatusPrikazivanja.Aktuelan ? 1 :
                      f.StatusPrikazivanja == StatusPrikazivanja.UNajavi ? 2 : 3)
            .ToListAsync();
            return View(filmovi);
        }

        // GET: Films/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var film = await _context.Film
                .FirstOrDefaultAsync(m => m.id == id);
            if (film == null)
            {
                return NotFound();
            }

            return View(film);
        }
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> AdminDetails(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var film = await _context.Film
                .FirstOrDefaultAsync(m => m.id == id);
            if (film == null)
            {
                return NotFound();
            }

            return View(film);
        }

        // GET: Films/Create
        [Authorize(Roles = "Administrator")]
        public  IActionResult Create()
        {
            return View();
        }

        // POST: Films/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Create([Bind("id,naziv,naslovnaSlika,opis,redatelj,glumci,releseDate,trailer,StatusPrikazivanja")] Film film)
        {
            if (ModelState.IsValid)
            {
                _context.Add(film);
                await _context.SaveChangesAsync();
                return RedirectToAction("Create","ZanroviFilmas", new { id = film.id });
            }
            return View(film);
        }
        public IActionResult OdabirZanra(int id)
        {
            // You can fetch the film using the id if necessary
            var film = _context.Film.Find(id);
            var zanrovi = _context.ZanroviFilma.ToList();
            ViewBag.zanrovi = zanrovi;
            // Pass the film or film ID to the view as needed
            return View();
        }
        // GET: Films/Edit/5
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var film = await _context.Film.FindAsync(id);
            if (film == null)
            {
                return NotFound();
            }
            return View(film);
        }

        // POST: Films/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Edit(int id, [Bind("id,naziv,naslovnaSlika,opis,redatelj,glumci,releseDate,trailer,StatusPrikazivanja")] Film film)
        {
            if (id != film.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(film);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FilmExists(film.id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(AdminFilmovi));
            }
            return View(film);
        }

        // GET: Films/Delete/5
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var film = await _context.Film
                .FirstOrDefaultAsync(m => m.id == id);
            if (film == null)
            {
                return NotFound();
            }

            return View(film);
        }

        // POST: Films/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var film = await _context.Film.FindAsync(id);
            if (film != null)
            {
                _context.Film.Remove(film);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(AdminFilmovi));
        }

       public async Task<IActionResult> NajavljeniFilmovi()
        {
             var najavljeniFilmovi = await _context.Film
            .Where(f => f.StatusPrikazivanja == StatusPrikazivanja.UNajavi)
            .ToListAsync();
            var filmGenres = new Dictionary<int, List<Zanr>>();

            foreach (var film in najavljeniFilmovi)
            {
                var genres = await _context.ZanroviFilma
                    .Where(z => z.idFilma == film.id)
                    .Select(z => z.zanrFilma)
                    .ToListAsync();
                filmGenres[film.id] = genres;
            }
            ViewBag.FilmGenres = filmGenres;

            return View(najavljeniFilmovi);
        }
        private bool FilmExists(int id)
        {
            return _context.Film.Any(e => e.id == id);
        }
        public IActionResult NajgledanijiFilmovi()
        {
            var prosjecneOcjenePoFilmu = _context.Ocjena
                .GroupBy(o => o.FilmId)
                .Select(g => new
                {
                    FilmId = g.Key,
                    ProsjecnaOcjena = g.Average(o => o.ocjenaFilma)
                })
                .ToList();

            prosjecneOcjenePoFilmu = prosjecneOcjenePoFilmu.OrderByDescending(o => o.ProsjecnaOcjena).ToList();

            var najgledanijiFilmoviIds = prosjecneOcjenePoFilmu.Take(5).Select(o => o.FilmId).ToList();

            var najgledanijiFilmovi = _context.Film.Where(f => najgledanijiFilmoviIds.Contains(f.id)).ToList();
            

            if (najgledanijiFilmovi == null)
            {
                return View(new List<Film>());
            }
            najgledanijiFilmovi = najgledanijiFilmovi.OrderBy(f => najgledanijiFilmoviIds.IndexOf(f.id)).ToList();
            return View(najgledanijiFilmovi.AsEnumerable());
        }
        /*public async Task<IActionResult> NajgledanijiFilmoviUSvijetu()
        {
            var jsonData = await movieService.GetTopRatedMoviesAsync();
            var movies = JObject.Parse(jsonData)["results"];

            return View(movies);
        }*/
    }
}
