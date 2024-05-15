﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CineTech.Models
{
    public class Notifikacija
    {
        [Key]
        public int id { get; set; }
        [ForeignKey("Korisnik")]
        public int KorisnikId {  get; set; }
        public Korisnik Korisnik {  get; set; }
        public PeriodNotifikacije PeriodNotifikacije { get; set; }
        public StatusNotifikacije StatusNotifikacije { get; set; }
    }
}
