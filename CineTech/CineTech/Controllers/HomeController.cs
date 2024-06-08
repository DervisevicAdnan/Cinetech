using CineTech.Data;
using CineTech.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using static CineTech.Controllers.FilmsController;

namespace CineTech.Controllers
{
    public class HomeController : Controller

    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ApplicationDbContext _filmoviController;
        private readonly ILogger<HomeController> _logger;
        private readonly RoleManager<IdentityRole> _roleManager;




        public HomeController(ILogger<HomeController> logger, ApplicationDbContext filmoviController, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _logger = logger;
            _filmoviController = filmoviController;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<IActionResult> Index()
        {
            var aktuelniFilmovi = await _filmoviController.Film
            .Where(f => f.StatusPrikazivanja == StatusPrikazivanja.Aktuelan)
            .ToListAsync();
            var filmGenres = new Dictionary<int, List<Zanr>>();

            foreach (var film in aktuelniFilmovi)
            {
                var genres = await _filmoviController.ZanroviFilma
                    .Where(z => z.idFilma == film.id)
                    .Select(z => z.zanrFilma)
                    .ToListAsync();
                filmGenres[film.id] = genres;
            }

            ViewBag.FilmGenres = filmGenres;
            return View(aktuelniFilmovi);
        }

        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult KupovinaView()
        {
            return View();
        }
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> AdminView()
        {
            var aktuelniFilmovi = await _filmoviController.Film
            .ToListAsync();
            return View(aktuelniFilmovi);
        }
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var film = await _filmoviController.Film
                .FirstOrDefaultAsync(m => m.id == id);
            if (film == null)
            {
                return NotFound();
            }

            return View(film);
        }
        public async Task<IActionResult> Edit(int id)
        {
            var film = await _filmoviController.Film.FindAsync(id);
            return View("~/Views/Films/Edit.cshtml");
        }
        [Authorize(Roles = "Administrator")]
        public IActionResult SviKorisnici()
        {
             var users = _userManager.Users.ToList();
            users = users.OrderByDescending(u => _userManager.GetRolesAsync(u).Result.Contains("Administrator"))
                 .ThenBy(u => _userManager.GetRolesAsync(u).Result.Contains("Korisnik"))
                 .ToList();
            return View(users);
        }
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Delete(string username)
        {
            var user = await _userManager.FindByNameAsync(username);
            ViewBag.userid = user;
            var result = await _userManager.DeleteAsync(user);
            if (result.Succeeded)
            {
                await _filmoviController.SaveChangesAsync();
                return RedirectToAction("SviKorisnici");
            }
            return RedirectToAction("SviKorisnici");
        }
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> SviKorisniciEdit(string username)
        {
            

            var user = await _userManager.FindByNameAsync(username);
            if (user == null)
            {
                return NotFound();
            }

            var userRoles = await _userManager.GetRolesAsync(user);
            var roles = _roleManager.Roles.ToList();

            ViewBag.UserId = user.Id;
            ViewBag.UserName = user.UserName;
            ViewBag.Roles = roles;
            ViewBag.UserRoles = userRoles;

            return View();
        }
        [Authorize(Roles = "Administrator")]
        // POST: Users/EditRoles/id
        [HttpPost]
        public async Task<IActionResult> SviKorisniciEdit(string username, List<string> selectedRoles)
        {
            if (string.IsNullOrEmpty(username))
            {
                return BadRequest("Username varijabla prilikom slanja je prazna");
            }

            var user = await _userManager.FindByNameAsync(username);
            if (user == null)
            {
                return NotFound();
            }

            var currentRoles = await _userManager.GetRolesAsync(user);

            var result = await _userManager.RemoveFromRolesAsync(user, currentRoles);
            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Ne mogu maæi rolu");
                return View();
            }

            result = await _userManager.AddToRolesAsync(user, selectedRoles ?? new List<string>());
            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Ne mogu dati rolu");
                return View();
            }

            return RedirectToAction("SviKorisnici");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

    }
}
