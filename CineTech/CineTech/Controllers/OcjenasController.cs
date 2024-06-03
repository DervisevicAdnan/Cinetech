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
using System.Transactions;
using Microsoft.VisualBasic;
using System.Data.SqlTypes;
using Microsoft.CodeAnalysis.Elfie.Serialization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Authorization;


namespace CineTech.Controllers
{
    public class OcjenasController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;


        public OcjenasController(ApplicationDbContext context, UserManager<IdentityUser> userManager,SignInManager<IdentityUser>signInManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
           
        }
       /* public async Task<IActionResult> OcjeneFilma(int? id)
        {
            var igrice = await _context.OcjeneFilma.ToListAsync();
            var ocjena = await _context.Ocjena
                .FirstOrDefaultAsync(m => m.id == id);
            var o = new OcjeneFilma
            {
                FilmId = (int)id,
                OcjenaId = ocjena.id
            };
            //igrice.RemoveAll(komentar => komentar.FilmId != id);
            igrice.Add(o);
            await _context.SaveChangesAsync();
            return View(igrice);
        }*/
        // GET: Ocjenas
        public async Task<IActionResult> Index(int ?id)

        {
            var ocjena = await _context.Ocjena
               .Where(o => o.FilmId == id)
               .ToListAsync();
            if (ocjena == null)
            {
                return NotFound();
            }

            return View(ocjena);// return View(await _context.Ocjena.ToListAsync());
        }

        // GET: Ocjenas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            ViewBag.ocjenaid = id;
            if (id == null)
            {
                return NotFound();
            }

            var ocjena = await _context.Ocjena
                .FirstOrDefaultAsync(m => m.id == id);
            if (ocjena == null)
            {
                return NotFound();
            }

            return View(ocjena);
        }

        //[Authorize(Roles="Administrator, Korisnik")]
        // GET: Ocjenas/OcjeneFilma/5
        public async Task<IActionResult> OcjeneFilma(int? id)
        {
            ViewBag.Ocjeneid = id;
            if (id == null)
            {
                return NotFound();
            }

            var ocjena = await _context.Ocjena
                .Where(o => o.FilmId == id)
                .ToListAsync();
            ViewBag.Ocjeneid = id;
            if (ocjena == null)
            {
                return NotFound();
            }

            return View(ocjena);
        }

        // GET: Ocjenas/OcijeniFilm/5
        [HttpGet]
        public async Task<IActionResult> OcijeniFilm(int? id)
        {
            var user = await _userManager.GetUserAsync(User);
            var korisnik1 = await _userManager.GetUserNameAsync(user);
            var korisnik = await _userManager.GetUserIdAsync(user);
            ViewBag.KorisnikId = korisnik1;
            ViewBag.FilmId = id;
            return View();
        }

        // POST: Ocjenas/OcijeniFilm/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> OcijeniFilm([Bind("ocjenaFilma,komentar,datum,korisnikId,FilmId")] Ocjena ocjena)
        {
            var user = await _userManager.GetUserAsync(User);
            var korisnik = await _userManager.GetUserIdAsync(user);
            ocjena.datum = DateTime.Today;
            ocjena.korisnikId = korisnik;

            if (ModelState.IsValid)
            {
               /* if (ocjena.korisnikId != korisnik)
                {
                    return NotFound();
                }*/
                _context.Add(ocjena);
                await _context.SaveChangesAsync();
                return RedirectToAction("Details", "Films", new { id = ocjena.FilmId });
            }
            return View(ocjena);
        }


        // GET: Ocjenas/Create
        [HttpGet]

        public async Task<IActionResult> Create(int?id)
        {
            var user = await _userManager.GetUserAsync(User);
            var korisnik1 = await _userManager.GetUserNameAsync(user);
            var korisnik = await _userManager.GetUserIdAsync(user);
            ViewBag.KorisnikId = korisnik1;
            ViewBag.FilmId = id;
            return View();
        }

        // POST: Ocjenas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Create([Bind("id,ocjenaFilma,komentar,datum,korisnikId,FilmId")] Ocjena ocjena)
        {
            var user = await _userManager.GetUserAsync(User);
            var korisnik = await _userManager.GetUserIdAsync(user);          
            ocjena.datum = DateTime.Today;
            ocjena.korisnikId = korisnik;

            
            if (ModelState.IsValid)
            {

                if (ocjena.korisnikId != korisnik)
                {
                    return NotFound();
                }
                _context.Add(ocjena);
                await _context.SaveChangesAsync();
                /*var ocjeneFilma = new OcjeneFilma
                {
                    FilmId = 3, 
                    OcjenaId = ocjena.id 
                };*/

                //_context.OcjeneFilma.Add(ocjeneFilma);
                //await _context.SaveChangesAsync();

                //var film = await _context.Film.FirstOrDefaultAsync();
                //          ocjeneFilma.FilmId = film.id;
                //          ocjeneFilma.OcjenaId = ocjena.id;
                //          _context.OcjeneFilma.Add(ocjeneFilma);
                //          await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            /*if (ModelState.IsValid)
            {
                _context.Add(ocjena);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(ocjena);*/
            return View(ocjena);
        }

        // GET: Ocjenas/Edit/5
        [Authorize(Roles = "Administrator, Korisnik")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var ocjena = await _context.Ocjena.FindAsync(id);
            var user = await _userManager.GetUserAsync(User);
            var korisnik = await _userManager.GetUserIdAsync(user);
            ViewBag.UserId = korisnik;
            ViewBag.UserFilm = ocjena.FilmId;
            if (ocjena == null)
            {
                return NotFound();
            }
            return View(ocjena);
        }

        // POST: Ocjenas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,ocjenaFilma,komentar,datum,korisnikId,FilmId")] Ocjena ocjena)
        {
            if (id != ocjena.id)
            {
                return NotFound();
            }
            var user = await _userManager.GetUserAsync(User);
            var korisnik = await _userManager.GetUserIdAsync(user);
            ocjena.korisnikId = korisnik;
            ViewBag.UserFilm = ocjena.FilmId;


            if (ModelState.IsValid)
            {
                if (ocjena.korisnikId != korisnik)
                {
                    return RedirectToAction("OcjeneFilma", "Ocjenas", new { id = ocjena.FilmId });
                }
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
                return RedirectToAction("OcjeneFilma", "Ocjenas", new { id = ocjena.FilmId });
            }
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
            return RedirectToAction("OcjeneFilma", "Ocjenas", new { id = ocjena.FilmId });
        }
        private OcjeneFilma KreirajOcjenuFilma()
        {
            try
            {
                return Activator.CreateInstance<OcjeneFilma>();
            }
            catch
            {
                throw new InvalidOperationException($"Can't create an instance of '{nameof(Models.OcjeneFilma)}'. " +
                    $"Ensure that '{nameof(Models.OcjeneFilma)}' is not an abstract class and has a parameterless constructor, or alternatively " +
                    $"override the register page in /Areas/Identity/Pages/Account/Register.cshtml");
            }
        }
        private bool OcjenaExists(int id)
        {
            return _context.Ocjena.Any(e => e.id == id);
        }
    }
}
