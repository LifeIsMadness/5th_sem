using Microsoft.AspNetCore.Identity;
using MovieSearch.Models.ExtendedUser;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MovieSearch.Models
{
    public class ApplicationUser : IdentityUser
    {

        public UserMoviesProfile MoviesProfile { get; set; }

        public byte[] ProfilePicture { get; set; }

    }
}
