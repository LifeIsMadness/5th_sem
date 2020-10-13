using MovieSearch.Models.ExtendedUser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieSearch.Models
{
    public class UserFavourites
    {
        public int ProfileId { get; set; }

        public UserMoviesProfile Profile { get; set; }

        public int MovieId { get; set; }

        public FavouriteMovie Movie { get; set; }
    }
}
