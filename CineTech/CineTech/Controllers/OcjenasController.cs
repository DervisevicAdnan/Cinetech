﻿using System;
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

        // GET: Ocjenas
        [Authorize(Roles = "Administrator")]

        public async Task<IActionResult> Index(int ?id)

        {
            return View(await _context.Ocjena.ToListAsync());
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
        [Authorize(Roles = "Administrator, Korisnik")]

        public async Task<IActionResult> OcijeniFilm(int? id)
        {
            var film = await _context.Film.FindAsync(id);
            if (film.StatusPrikazivanja != StatusPrikazivanja.Aktuelan) return RedirectToAction("NajavljeniFilmovi", "Films");
           
            var user = await _userManager.GetUserAsync(User);
            var korisnik1 = await _userManager.GetUserNameAsync(user);
            var korisnik = await _userManager.GetUserIdAsync(user);
            var postojecaOcjena = await _context.Ocjena
            .FirstOrDefaultAsync(o => o.FilmId == id && o.korisnikId == korisnik);

            if (postojecaOcjena != null)
            {
                // Ako korisnik već ima ocjenu, preusmjeri ga na neku drugu stranicu
                return RedirectToAction("Details", "Films",new { id });
            }

            ViewBag.KorisnikId = korisnik1;
            ViewBag.FilmId = id;
            return View();
        }

        // POST: Ocjenas/OcijeniFilm/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Korisnik")]

        public async Task<IActionResult> OcijeniFilm([Bind("ocjenaFilma,komentar,datum,korisnikId,FilmId")] Ocjena ocjena)
        {
            var user = await _userManager.GetUserAsync(User);
            var korisnik = await _userManager.GetUserIdAsync(user);
            ocjena.datum = DateTime.Today;
            ocjena.korisnikId = korisnik;

            if (ModelState.IsValid)
            { 
                var film = await _context.Film.FindAsync(ocjena.FilmId);
            if (film.StatusPrikazivanja == StatusPrikazivanja.Aktuelan)
            {
                if ((User.IsInRole("Administrator")) || ocjena.korisnikId == korisnik)
                {
                    _context.Add(ocjena);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Details", "Films", new { id = ocjena.FilmId });
                }
                else if (ocjena.korisnikId != korisnik)
                {

                    return RedirectToAction("OcjeneFilma", "Ocjenas", new { id = ocjena.FilmId });

                }
            }
            else
            {
                    return RedirectToAction("NajavljeniFilmovi", "Films");
            }

                
            }
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
            
            if ((User.IsInRole("Administrator"))||ocjena.korisnikId==korisnik)
            {
                ViewBag.UserId = korisnik;
                ViewBag.UserFilm = ocjena.FilmId;
                if (ocjena == null)
                {
                    return NotFound();
                }
            }
            else if (ocjena.korisnikId != korisnik)
            {

                return RedirectToAction("OcjeneFilma", "Ocjenas", new { id = ocjena.FilmId });

            }
            return View(ocjena);
        }

        // POST: Ocjenas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Korisnik")]

        public async Task<IActionResult> Edit(int id, [Bind("id,ocjenaFilma,komentar,datum,korisnikId,FilmId")] Ocjena ocjena)
        {
            if (id != ocjena.id)
            {
                return NotFound();
            }
            var user = await _userManager.GetUserAsync(User);
            var korisnik = await _userManager.GetUserIdAsync(user);
            //ocjena.korisnikId = korisnik;
            ViewBag.UserFilm = ocjena.FilmId;
            ViewBag.korisnikId = ocjena.korisnikId;
            

            if (ModelState.IsValid)
            {
                if ((User.IsInRole("Administrator")) || ocjena.korisnikId == korisnik)
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
                }
                else if (ocjena.korisnikId != korisnik)
                {
                    return RedirectToAction("OcjeneFilma", "Ocjenas", new { id = ocjena.FilmId });
                }
            }
            return View(ocjena);
        }

        // GET: Ocjenas/Delete/5
        [Authorize(Roles = "Administrator, Korisnik")]


        public async Task<IActionResult> Delete(int? id)
        {
            ViewBag.Id = id;
            if (id == null)
            {
                return NotFound();
            }
            var user = await _userManager.GetUserAsync(User);
            var korisnik = await _userManager.GetUserIdAsync(user);
            var ocjena = await _context.Ocjena
                .FirstOrDefaultAsync(m => m.id == id);

            if ((User.IsInRole("Administrator")) || ocjena.korisnikId == korisnik)
            {
                if (ocjena == null)
                {
                    return NotFound();
                }
            }
            else if (ocjena.korisnikId != korisnik)
            {
                return RedirectToAction("OcjeneFilma", "Ocjenas", new { id = ocjena.FilmId });
            }

            return View(ocjena);
        }

        // POST: Ocjenas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Korisnik")]

        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            var korisnik = await _userManager.GetUserIdAsync(user);
            var ocjena = await _context.Ocjena.FindAsync(id);
            if ((User.IsInRole("Administrator")) || ocjena.korisnikId == korisnik)
            {
                if (ocjena != null)
                {
                    _context.Ocjena.Remove(ocjena);
                }
            }
            else if (ocjena.korisnikId != korisnik)
            {
                return RedirectToAction("OcjeneFilma", "Ocjenas", new { id = ocjena.FilmId });
            }


            await _context.SaveChangesAsync();
            return RedirectToAction("OcjeneFilma", "Ocjenas", new { id = ocjena.FilmId });
        }
        private bool OcjenaExists(int id)
        {
            return _context.Ocjena.Any(e => e.id == id);
        }
    }
}
