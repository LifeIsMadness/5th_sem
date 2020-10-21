using MovieSearch.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieSearch.ViewModels.UserMovies
{
    public class UserMoviesIndexViewModel
    {
        public IEnumerable<Movie> FavMovies { get; set; }
    }
}
