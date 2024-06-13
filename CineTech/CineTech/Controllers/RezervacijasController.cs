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
using Microsoft.IdentityModel.Tokens;
using CineTech.Services;

namespace CineTech.Controllers
{
    public class RezervacijasController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IMailService _mailService;

        public RezervacijasController(ApplicationDbContext context, UserManager<IdentityUser> userManager, IMailService mailService)
        {
            _context = context;
            _userManager = userManager;
            _mailService = mailService;
        }

        // GET: Rezervacijas
        public async Task<IActionResult> Index()
        {
            return View(await _context.Rezervacija.ToListAsync());
        }

        // GET: Rezervacijas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rezervacija = await _context.Rezervacija
                .FirstOrDefaultAsync(m => m.id == id);
            if (rezervacija == null)
            {
                return NotFound();
            }

            return View(rezervacija);
        }
        public async Task<IActionResult> UspjesnaRezervacija(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rezervacija = await _context.Rezervacija
                .FirstOrDefaultAsync(m => m.id == id);
            var zauzetaSjedista = _context.ZauzetaSjedista.Where(o => o.TransakcijaId == id).ToList();
            var projekcija = _context.Projekcija.FirstOrDefault(o => o.id == zauzetaSjedista.FirstOrDefault().ProjekcijaId);
            var film = await _context.Film.FirstOrDefaultAsync(o => o.id == projekcija.filmId);
            var kinoSala = await _context.KinoSala.FirstOrDefaultAsync(o => o.id == projekcija.kinoSalaId);
            if (rezervacija == null)
            {
                return NotFound();
            }
            var user = await _userManager.GetUserAsync(User);
            HTMLMailData mailData = new HTMLMailData();
            mailData.EmailToId = user.Email;
            mailData.EmailToName = "";
            mailData.NazivFilma = film.naziv;
            mailData.TerminProjekcije = projekcija.datum.ToShortDateString() + ", " + projekcija.vrijeme.ToShortTimeString();
            mailData.NazivSale = kinoSala.naziv;
            mailData.ZauzetaSjedista = zauzetaSjedista;
            _mailService.SendRezervacijaMail(mailData);
            var uspjesnaRezervacija = new Tuple<Rezervacija, List<ZauzetaSjedista>, String, String>(rezervacija, zauzetaSjedista, film.naziv, kinoSala.naziv);
            return View(uspjesnaRezervacija);
        }
        // GET: Rezervacijas/Create
        public async Task<IActionResult> Create(int?id)
        {
            var user = await _userManager.GetUserAsync(User);
            var korisnik1 = await _userManager.GetUserIdAsync(user);
            ViewBag.KorisnikId = korisnik1;
            ViewBag.id = id;
            return View();
        }
        public async Task<IActionResult> CreateSjediste(int red,int redniBrojSjedista,int projekcijaId)
        {
            var user = await _userManager.GetUserAsync(User);
            var korisnik1 = await _userManager.GetUserIdAsync(user);
            ViewBag.KorisnikId = korisnik1;
            var postjiZauzeta = _context.ZauzetaSjedista.Where(o => o.red == red && o.redniBrojSjedista == redniBrojSjedista && o.ProjekcijaId == projekcijaId);
            if(postjiZauzeta.Any())
            {
                return NotFound(); //AKO VEC POSTOJI NAPRAVI VIEW 
            }
            var zauzmiSjediste = new ZauzetaSjedista { red = red, redniBrojSjedista = redniBrojSjedista, ProjekcijaId = projekcijaId };
            _context.Add(zauzmiSjediste);
            await _context.SaveChangesAsync();
            var sjedisteId = _context.ZauzetaSjedista.FirstOrDefault(o => o.id == zauzmiSjediste.id);
            var rezervacija = new Rezervacija { datum = DateTime.Now, vrijeme = DateTime.Now, KorisnikId = korisnik1 };
            _context.Add(rezervacija);
            await _context.SaveChangesAsync();
            return View(rezervacija);
        }
        [HttpPost]
        public async Task<IActionResult> CreateSjediste([FromBody] List<int[]> sjedista)
        {
            if (sjedista.IsNullOrEmpty()) { return BadRequest("Niste odabrali mjesto"); }
            var user = await _userManager.GetUserAsync(User);
            var korisnik1 = await _userManager.GetUserIdAsync(user);
            ViewBag.KorisnikId = korisnik1;
            var rezervacija = new Rezervacija { datum = DateTime.Now, vrijeme = DateTime.Now, KorisnikId = korisnik1 };
            _context.Add(rezervacija);
            await _context.SaveChangesAsync();
            var rezervacijaId = _context.Rezervacija.FirstOrDefault(o => o.id == rezervacija.id);
            foreach (var element in sjedista)
            {
                var zauzmiSjediste = new ZauzetaSjedista { red = element[0], redniBrojSjedista = element[1], ProjekcijaId = element[2] ,TransakcijaId=rezervacijaId.id};
                _context.Add(zauzmiSjediste);
            }
            await _context.SaveChangesAsync();
            return Ok(new { redirectUrl = Url.Action("UspjesnaRezervacija", "Rezervacijas", new { id=rezervacijaId.id}) });
        }

        // POST: Rezervacijas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("datum,vrijeme,KorisnikId,ZauzetaSjedistaId")] Rezervacija rezervacija)
        {
            
            if (ModelState.IsValid)
            {
                _context.Add(rezervacija);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(rezervacija);
        }

        // GET: Rezervacijas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rezervacija = await _context.Rezervacija.FindAsync(id);
            if (rezervacija == null)
            {
                return NotFound();
            }
            return View(rezervacija);
        }

        // POST: Rezervacijas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,datum,vrijeme,KorisnikId,ZauzetaSjedistaId")] Rezervacija rezervacija)
        {
            if (id != rezervacija.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(rezervacija);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RezervacijaExists(rezervacija.id))
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
            return View(rezervacija);
        }

        // GET: Rezervacijas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rezervacija = await _context.Rezervacija
                .FirstOrDefaultAsync(m => m.id == id);
            if (rezervacija == null)
            {
                return NotFound();
            }

            return View(rezervacija);
        }

        // POST: Rezervacijas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var rezervacija = await _context.Rezervacija.FindAsync(id);
            if (rezervacija != null)
            {
                _context.Rezervacija.Remove(rezervacija);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RezervacijaExists(int id)
        {
            return _context.Rezervacija.Any(e => e.id == id);
        }
    }
}
