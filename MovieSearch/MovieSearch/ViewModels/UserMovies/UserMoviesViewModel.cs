using MovieSearch.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieSearch.ViewModels.UserMovies
{
    public class UserMoviesViewModel
    {
        public bool ForCurrentUser { get; set; }
      
        public string ReturnUrl { get; set; }

        public string UserName { get; set; }
        
        public IEnumerable<(Movie Movie, MovieMark Mark)> MoviesAndMarks { get; set; }
    }
}
