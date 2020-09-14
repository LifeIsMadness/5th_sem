using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieSearch.Models
{
    public class Movie
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Title { get; set; }

        public int Year { get; set; }

        public string Country { get; set; }

        public float OveralRating { get; set; }

        public int GenreId { get; set; }

        public MovieGenre Genre { get; set; }
    }
}
