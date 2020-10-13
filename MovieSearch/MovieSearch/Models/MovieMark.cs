using MovieSearch.Models.ExtendedUser;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MovieSearch.Models
{
    public class MovieMark
    {
        public int Id { get; set; }

        [Range(0, 10)]
        public int Value { get; set; }

        public int MovieId { get; set; }

        public Movie Movie { get; set; }

        public int UserProfileId { get; set; }

        public UserMoviesProfile UserProfile { get; set; }
    }
}
