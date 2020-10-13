using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MovieSearch.Models.ExtendedUser
{
    public class UserMoviesProfile
    {
        public int Id { get; set; }

        public List<MovieMark> Marks { get; set; }

        public List<Review> Reviews { get; set; }

        [Display(Name = "Movies viewed")]
        public int MoviesViewedCount { get; set; }

        public string UserId { get; set; }  

        public ApplicationUser User { get; set; }

        public List<UserFavourites> FavouriteMovies { get; set; }
    }
}
