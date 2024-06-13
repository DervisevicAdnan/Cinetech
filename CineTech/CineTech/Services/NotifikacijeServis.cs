using CineTech.Data;
using CineTech.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore; // Dodaj ovu direktivu

namespace CineTech.Services
{
    public class NotifikacijeServis
    {
        private readonly ApplicationDbContext _context;
        private readonly IMailService _mailService;

        public NotifikacijeServis(ApplicationDbContext context, IMailService mailService)
        {
            _context = context;
            _mailService = mailService;
        }

        private int GetDaysBeforePremiere(PeriodNotifikacije period)
        {
            return period switch
            {
                PeriodNotifikacije.DanPrije => 1,
                PeriodNotifikacije.SedmicuPrije => 7,
                PeriodNotifikacije.MjesecPrije => 30,
                PeriodNotifikacije.PriOdredjivanjuDatuma => 0,
                _ => throw new ArgumentOutOfRangeException(nameof(period), period, null)
            };
        }

        public async Task NotifyAsync()
        {
            var danas = DateTime.UtcNow.Date;

            var notifikacije = await _context.Notifikacija
                .Where(n => n.StatusNotifikacije == StatusNotifikacije.NaCekanju)
                .ToListAsync();

            foreach (var notifikacija in notifikacije)
            {
                var notifikacijeFilma = await _context.NotifikacijeFilma
                    .Where(nf => nf.NotifikacijaId == notifikacija.id)
                    .ToListAsync();

                foreach (var nf in notifikacijeFilma)
                {
                    var film = await _context.Film.FindAsync(nf.FilmId);
                    if (film != null)
                    {
                        var daysBeforePremiere = GetDaysBeforePremiere(notifikacija.PeriodNotifikacije);
                        var danZaNotifikaciju = film.releseDate.AddDays(-daysBeforePremiere);

                        if (danZaNotifikaciju == danas)
                        {
                            var user = await _context.Korisnik.FindAsync(notifikacija.id);
                            NotifikacijaMailData mailData = new NotifikacijaMailData();
                            mailData.EmailToId = user.Email;
                            mailData.EmailToName = "";
                            mailData.NazivFilma = film.naziv;
                            mailData.DatumPredstavljanja = film.releseDate.Date.ToString();
                            _mailService.SendNotifikacijaMail(mailData);

                            notifikacija.StatusNotifikacije = StatusNotifikacije.Dostavljena;
                            _context.Notifikacija.Update(notifikacija);
                        }
                    }
                }
            }

            await _context.SaveChangesAsync();
        }
    }
}
