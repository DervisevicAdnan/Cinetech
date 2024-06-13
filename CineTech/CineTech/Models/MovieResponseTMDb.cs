using static Org.BouncyCastle.Crypto.Engines.SM2Engine;
using System.Collections.Generic;

namespace CineTech.Models
{
    public class MovieResponseTMDb
    {
        public int Page { get; set; }
        public List<MovieTMDb> Results { get; set; }
    }
}
