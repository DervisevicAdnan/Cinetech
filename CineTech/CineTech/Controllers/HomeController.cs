using CineTech.Data;
using CineTech.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
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
        private readonly ApplicationDbContext _context;




        public HomeController(ILogger<HomeController> logger, ApplicationDbContext filmoviController, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, ApplicationDbContext context)
        {
            _logger = logger;
            _filmoviController = filmoviController;
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
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
        [Authorize(Roles = "Administrator, Korisnik")]
        public IActionResult KupovinaView()
        {
            return View();
        }
        [HttpPost]
        [Authorize(Roles = "Administrator, Korisnik")]

        public async Task<IActionResult> KupovinaView([FromBody] List<int[]> sjedista)
        {
            var projekcija = _context.Projekcija.FirstOrDefault(o => o.id == sjedista[0][2]);
            var nova_cijena = projekcija.cijenaOsnovneKarte*sjedista.Count();

            if (DateTime.Today.DayOfWeek == DayOfWeek.Wednesday)
            {
                nova_cijena = nova_cijena * 0.9;
            }

            var podaci = new { Sjedista = sjedista, NovaCijena = nova_cijena };
            TempData["Sjedista"] = JsonConvert.SerializeObject(podaci);

            return Json(new { redirectUrl = Url.Action("KupovinaView", "Home") });
        }



        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> AdminView()
        {
            var aktuelniFilmovi = await _filmoviController.Film
            .ToListAsync();
            return View(aktuelniFilmovi);
        }
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> StatistikaView()
        {
            var totalMovies = await _filmoviController.Film.CountAsync();

            var genres = await _filmoviController.ZanroviFilma
                .GroupBy(z => z.zanrFilma)
                .Select(g => new
                {
                    Genre = g.Key.ToString(),
                    Count = g.Count()
                }).ToListAsync();

            var totalEarnings = await _filmoviController.Kupovina.SumAsync(t => t.cijena);
            var totalTicketsSold = await _filmoviController.Kupovina.CountAsync();
            var totalRegisteredUsers = await _userManager.Users.CountAsync();

            var monthlyProfits = await _filmoviController.Kupovina
                .GroupBy(t => new { t.datum.Year, t.datum.Month })
                .Select(g => new
                {
                    Year = g.Key.Year,
                    Month = g.Key.Month,
                    Profit = g.Sum(t => t.cijena)
                })
                .OrderByDescending(mp => mp.Year)
                .ThenByDescending(mp => mp.Month)
                .Take(6) // Last 6 months
                .ToListAsync();

            ViewBag.TotalMovies = totalMovies;
            ViewBag.Genres = genres;
            ViewBag.TotalEarnings = totalEarnings;
            ViewBag.TotalTicketsSold = totalTicketsSold;
            ViewBag.TotalRegisteredUsers = totalRegisteredUsers;
            ViewBag.MonthlyProfits = monthlyProfits;

            return View();
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
        [Authorize(Roles = "Administrator")]

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
