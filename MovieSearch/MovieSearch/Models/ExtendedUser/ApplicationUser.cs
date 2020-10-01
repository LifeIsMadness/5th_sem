using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieSearch.Models
{
    public class ApplicationUser : IdentityUser
    {  
        public List<MovieMark> Marks { get; set; }

        public List<Review> Reviews { get; set; }

        public int MoviesViewedCount { get; set; }

    }
}
