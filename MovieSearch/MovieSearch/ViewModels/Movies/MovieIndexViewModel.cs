using Microsoft.AspNetCore.Mvc.Rendering;
using MovieSearch.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieSearch.ViewModels.Movies
{
    public class MovieIndexViewModel
    {
        public IEnumerable<Movie> Movies { get; set; }

        public SelectList Genres { get; set; }
    }
}
