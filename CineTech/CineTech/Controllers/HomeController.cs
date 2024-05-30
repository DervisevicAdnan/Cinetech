using CineTech.Data;
using CineTech.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using static CineTech.Controllers.FilmsController;

namespace CineTech.Controllers
{
    public class HomeController : Controller
        
    {
        private readonly ApplicationDbContext _filmoviController;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext filmoviController)
        {
            _logger = logger;
            _filmoviController= filmoviController;
        }

        public async Task<IActionResult> Index()
        {
            var aktuelniFilmovi = await _filmoviController.Film
            .Where(f => f.StatusPrikazivanja == StatusPrikazivanja.Aktuelan)
            .ToListAsync();
            return View(aktuelniFilmovi);
        }

        public IActionResult Privacy()
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

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
