using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CineTech.Data;
using CineTech.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace CineTech.Controllers
{
    public class NotifikacijasController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IEmailSender _emailSender;


        public NotifikacijasController(ApplicationDbContext context, UserManager<IdentityUser> userManager, IEmailSender emailSender)
        {
            _context = context;
            _userManager = userManager;
            _emailSender = emailSender;
        }

        // GET: Notifikacijas
        [Authorize(Roles = "Administrator")]

        public async Task<IActionResult> Index()
        {
            return View(await _context.Notifikacija.ToListAsync());
        }

        // GET: Notifikacijas/Details/5
        [Authorize(Roles = "Administrator")]

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var notifikacija = await _context.Notifikacija
                .FirstOrDefaultAsync(m => m.id == id);
            if (notifikacija == null)
            {
                return NotFound();
            }

            return View(notifikacija);
        }
        [Authorize(Roles = "Administrator, Korisnik")]

        // GET: Notifikacijas/Create
        public async Task<IActionResult> Create(int? id)
        {
            var user = await _userManager.GetUserAsync(User);
            var korisnik1 = await _userManager.GetUserNameAsync(user);
            ViewBag.username = korisnik1;
            ViewBag.proslijedi = id;
            ViewBag.korisnikId = user.Id;
            return View();
        }

        // POST: Notifikacijas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Korisnik")]

        public async Task<IActionResult> Create([Bind("KorisnikId,PeriodNotifikacije,StatusNotifikacije")] Notifikacija notifikacija,int filmId)
        {
            var user = await _userManager.FindByIdAsync(notifikacija.KorisnikId);
            var username = user.UserName;
            if (ModelState.IsValid)
            {
                _context.Add(notifikacija);
                await _context.SaveChangesAsync();
                int notifikacijaId = notifikacija.id;
                NotifikacijeFilma notifikacijeFilma = new NotifikacijeFilma
                {
                    FilmId = filmId,
                    NotifikacijaId = notifikacijaId
                };
                _context.NotifikacijeFilma.Add(notifikacijeFilma);
                await _context.SaveChangesAsync();
                return RedirectToAction("UserNotifikacije","Notifikacijas", new { username = User.Identity.Name});
            }
            return View(notifikacija);
        }
        [Authorize(Roles = "Administrator, Korisnik")]

        public async Task<IActionResult> UserNotifikacije(string username)
        {
            var user = await _userManager.FindByNameAsync(username);
            var korisnikId = user.Id;

            // Combine retrievals using JOINs
            var notifikacijeWithFilms = await _context.Notifikacija
                .Where(n => n.KorisnikId == korisnikId)
                .Join(_context.NotifikacijeFilma, n => n.id, nf => nf.NotifikacijaId, (n, nf) => new
                {
                    Notifikacija = n,
                    FilmId = nf.FilmId
                })
                .Join(_context.Film, nf => nf.FilmId, f => f.id, (nf, f) => new
                {
                    Notifikacija = nf.Notifikacija,
                    FilmId = nf.FilmId,
                    FilmNaziv = f.naziv
                })
                .ToListAsync();

            ViewBag.NotifikacijeWithFilms = notifikacijeWithFilms;

            return View(notifikacijeWithFilms);
        }

        [Authorize(Roles = "Administrator")]

        // GET: Notifikacijas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var notifikacija = await _context.Notifikacija.FindAsync(id);
            if (notifikacija == null)
            {
                return NotFound();
            }
            return View(notifikacija);
        }

        // POST: Notifikacijas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]

        public async Task<IActionResult> Edit(int id, [Bind("id,KorisnikId,PeriodNotifikacije,StatusNotifikacije")] Notifikacija notifikacija)
        {
            if (id != notifikacija.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(notifikacija);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NotifikacijaExists(notifikacija.id))
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
            return View(notifikacija);
        }
        [Authorize(Roles = "Administrator")]

        // GET: Notifikacijas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var notifikacija = await _context.Notifikacija
                .FirstOrDefaultAsync(m => m.id == id);
            if (notifikacija == null)
            {
                return NotFound();
            }

            return View(notifikacija);
        }

        // POST: Notifikacijas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]

        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var notifikacija = await _context.Notifikacija.FindAsync(id);
            if (notifikacija != null)
            {
                _context.Notifikacija.Remove(notifikacija);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool NotifikacijaExists(int id)
        {
            return _context.Notifikacija.Any(e => e.id == id);
        }
    }
}
