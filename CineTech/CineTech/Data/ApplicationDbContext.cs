using CineTech.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CineTech.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Film> Film { get; set; }
        public DbSet<KinoSala> KinoSala { get; set; }
        public DbSet<Korisnik> Korisnik { get; set; }
        public DbSet<Kupovina> Kupovina { get; set; }
        public DbSet<Notifikacija> Notifikacija { get; set; }
        public DbSet<NotifikacijeFilma> NotifikacijeFilma { get; set; }
        public DbSet<Ocjena> Ocjena { get; set; }
        public DbSet<OcjeneFilma> OcjeneFilma { get; set; }
        public DbSet<Projekcija> Projekcija { get; set; }
        public DbSet<Rezervacija> Rezervacija { get; set; }
        public DbSet<Roles> Roles { get; set; }
        public DbSet<Transakcija> Transakcija { get; set; }
        public DbSet<UserRoles> UserRoles { get; set; }
        public DbSet<ZanroviFilma> ZanroviFilma { get; set; }
        public DbSet<ZauzetaSjedista> ZauzetaSjedista { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Film>().ToTable("Film");
            builder.Entity<KinoSala>().ToTable("KinoSala");
            builder.Entity<Korisnik>().ToTable("Korisnik");
            builder.Entity<Kupovina>().ToTable("Kupovina");
            builder.Entity<Notifikacija>().ToTable("Notifikacija");
            builder.Entity<NotifikacijeFilma>().ToTable("NotifikacijeFilma");
            builder.Entity<Ocjena>().ToTable("Ocjena");
            builder.Entity<OcjeneFilma>().ToTable("OcjeneFilma");
            builder.Entity<Projekcija>().ToTable("Projekcija");
            builder.Entity<Rezervacija>().ToTable("Rezervacija");
            builder.Entity<Roles>().ToTable("Roles");
            builder.Entity<Transakcija>().ToTable("Transakcija");
            builder.Entity<UserRoles>().ToTable("UserRoles");
            builder.Entity<ZanroviFilma>().ToTable("ZanroviFilma");
            builder.Entity<ZauzetaSjedista>().ToTable("ZauzetaSjedista");
            base.OnModelCreating(builder);
        }
    }
}
