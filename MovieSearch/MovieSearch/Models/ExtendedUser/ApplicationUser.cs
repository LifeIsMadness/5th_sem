using Microsoft.AspNetCore.Identity;
using MovieSearch.Models.ExtendedUser;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MovieSearch.Models
{
    public class ApplicationUser : IdentityUser
    {

        public UserMoviesProfile MoviesProfile { get; set; }

        public int? ProfilePictureId { get; set; }

        [ForeignKey("ProfilePictureId")]
        public ProfilePicture ProfilePicture { get; set; }

    }
}
