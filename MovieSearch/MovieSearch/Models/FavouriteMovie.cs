using MovieSearch.Models.ExtendedUser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieSearch.Models
{
    public class FavouriteMovie
    {
        public int Id { get; set; }

        public int MovieId {get;set;}

        public Movie Movie { get; set; }

        public List<UserFavourites> UserProfiles { get; set; }
    }
}
