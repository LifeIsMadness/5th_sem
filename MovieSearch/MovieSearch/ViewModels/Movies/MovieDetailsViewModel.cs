using MovieSearch.Migrations;
using MovieSearch.Models;
using MovieSearch.Models.ExtendedUser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieSearch.ViewModels.Movies
{
    public class MovieDetailsViewModel
    {
        public Movie Movie { get; set; }

        public MovieMark Mark { get; set; }

        public IEnumerable<Review> Reviews { get; set; }

        public string IsFavourite { get; set; }

        //public int Mark { get; set; }

    }
}
