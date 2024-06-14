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
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using CineTech.Services;

namespace CineTech.Controllers
{
    public class KupovinasController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IMailService _mailService;
        private readonly InEmailSender _inEmailSender;

        public KupovinasController(ApplicationDbContext context, UserManager<IdentityUser> userManager, IMailService mailService, InEmailSender inEmailSender)
        {
            _context = context;
            _userManager = userManager;
            _mailService = mailService;
            _inEmailSender = inEmailSender;
        }

        // GET: Kupovinas
        [Authorize(Roles = "Administrator")]

        public async Task<IActionResult> Index()
        {
            return View(await _context.Kupovina.ToListAsync());
        }

        // GET: Kupovinas/Details/5
        [Authorize(Roles = "Administrator")]

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var kupovina = await _context.Kupovina
                .FirstOrDefaultAsync(m => m.id == id);
            if (kupovina == null)
            {
                return NotFound();
            }

            return View(kupovina);
        }

        // GET: Kupovinas/Create
        [Authorize(Roles = "Administrator, Korisnik")]

        public async Task<IActionResult> Create(int? id)
        {
            var user = await _userManager.GetUserAsync(User);
            var korisnik1 = await _userManager.GetUserIdAsync(user);
            ViewBag.KorisnikId = korisnik1;
            var zauzetaSjedista = await _context.ZauzetaSjedista
                .FirstOrDefaultAsync(m => m.id == id);
            var projekcija = await _context.Projekcija
               .FirstOrDefaultAsync(m => m.id == zauzetaSjedista.ProjekcijaId);
            ViewBag.cijena = projekcija.cijenaOsnovneKarte;
            ViewBag.id = id;
            var nova_cijena = projekcija.cijenaOsnovneKarte;

            if (DateTime.Today.DayOfWeek == DayOfWeek.Wednesday)
            {
                ViewBag.cijena = nova_cijena * 0.9;
            }
                return View();
        }
        [HttpPost]
        [Authorize(Roles = "Administrator, Korisnik")]

        public async Task<IActionResult> CreateSjediste([FromBody] List<int[]> sjedista)
        {
            if (sjedista.IsNullOrEmpty()) { return BadRequest("Niste odabrali mjesto"); }
            var user = await _userManager.GetUserAsync(User);
            var korisnik1 = await _userManager.GetUserIdAsync(user);
            ViewBag.KorisnikId = korisnik1;
            var projekcija = await _context.Projekcija
               .FirstOrDefaultAsync(m => m.id == sjedista[0][2]);
            double nova_cijena = projekcija.cijenaOsnovneKarte*sjedista.Count();
            if (DateTime.Today.DayOfWeek == DayOfWeek.Wednesday)
            {
                nova_cijena = nova_cijena * 0.9;
            }
            var kupovina = new Kupovina { datum = DateTime.Now, vrijeme = DateTime.Now, KorisnikId = korisnik1, cijena=nova_cijena };
            _context.Add(kupovina);
            await _context.SaveChangesAsync();
            var kupovinaId = _context.Kupovina.FirstOrDefault(o => o.id == kupovina.id);
            foreach (var element in sjedista)
            {
                var zauzmiSjediste = new ZauzetaSjedista { red = element[0], redniBrojSjedista = element[1], ProjekcijaId = element[2], TransakcijaId = kupovinaId.id };
                _context.Add(zauzmiSjediste);
            }
            await _context.SaveChangesAsync();
            return Ok(new { redirectUrl = Url.Action("UspjesnaKupovina", "Kupovinas", new { id = kupovinaId.id }) });
        }
        [Authorize(Roles = "Administrator, Korisnik")]

        public async Task<IActionResult> UspjesnaKupovina(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var kupovina = await _context.Kupovina
                .FirstOrDefaultAsync(m => m.id == id);
            var zauzetaSjedista = _context.ZauzetaSjedista.Where(o => o.TransakcijaId == id).ToList();
            var projekcija = _context.Projekcija.FirstOrDefault(o => o.id == zauzetaSjedista.FirstOrDefault().ProjekcijaId);
            var film = await _context.Film.FirstOrDefaultAsync(o => o.id == projekcija.filmId);
            var kinoSala = await _context.KinoSala.FirstOrDefaultAsync(o => o.id == projekcija.kinoSalaId);
            if (kupovina == null)
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
            await _inEmailSender.SendHTMLEmailAsync(mailData);
            //_mailService.SendHTMLMail(mailData);


            var uspjesnaKupovina = new Tuple<Kupovina, List<ZauzetaSjedista>, String, String>(kupovina, zauzetaSjedista, film.naziv, kinoSala.naziv);
            return View(uspjesnaKupovina);
        }
        // POST: Kupovinas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]

        public async Task<IActionResult> Create([Bind("cijena,datum,vrijeme,KorisnikId,ZauzetaSjedistaId")] Kupovina kupovina)
        {
            ViewBag.cijena = kupovina.cijena;
            kupovina.vrijeme = DateTime.Now;
            kupovina.datum = DateTime.Now;
            if (ModelState.IsValid)
            {
                
                

                _context.Add(kupovina);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(kupovina);
        }

        // GET: Kupovinas/Edit/5
        [Authorize(Roles = "Administrator")]

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var kupovina = await _context.Kupovina.FindAsync(id);
            if (kupovina == null)
            {
                return NotFound();
            }
            return View(kupovina);
        }

        // POST: Kupovinas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]

        public async Task<IActionResult> Edit(int id, [Bind("cijena,id,datum,vrijeme,KorisnikId,ZauzetaSjedistaId")] Kupovina kupovina)
        {
            if (id != kupovina.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(kupovina);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!KupovinaExists(kupovina.id))
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
            return View(kupovina);
        }

        // GET: Kupovinas/Delete/5
        [Authorize(Roles = "Administrator")]

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var kupovina = await _context.Kupovina
                .FirstOrDefaultAsync(m => m.id == id);
            if (kupovina == null)
            {
                return NotFound();
            }

            return View(kupovina);
        }

        // POST: Kupovinas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]

        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var kupovina = await _context.Kupovina.FindAsync(id);
            if (kupovina != null)
            {
                _context.Kupovina.Remove(kupovina);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool KupovinaExists(int id)
        {
            return _context.Kupovina.Any(e => e.id == id);
        }
    }
}
