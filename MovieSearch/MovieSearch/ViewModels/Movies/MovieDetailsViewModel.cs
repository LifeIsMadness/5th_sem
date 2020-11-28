using MovieSearch.Models;
using System.Collections.Generic;

namespace MovieSearch.ViewModels.Movies
{
    public class MovieDetailsViewModel
    {
        public Movie Movie { get; set; }

        public MovieMark Mark { get; set; }

        public IEnumerable<Review> Reviews { get; set; }

        public string IsFavourite { get; set; }

        public int ProfileId { get; set; }

    }
}
