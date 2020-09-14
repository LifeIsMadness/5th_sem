using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieSearch.Models
{
    public class MovieGenre
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public List<Movie> Movies { get; set; }
    }
}
