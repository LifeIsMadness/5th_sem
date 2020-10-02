using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MovieSearch.Models
{
    public class ApplicationUser : IdentityUser
    {  
        public List<MovieMark> Marks { get; set; }

        public List<Review> Reviews { get; set; }

        [Display(Name = "Movies viewed")]
        public int MoviesViewedCount { get; set; }

        //public string Image { get; set; }

    }
}
