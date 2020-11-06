using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieSearch.Models
{
    public class ProfilePicture
    {
        public int Id { get; set; }

        public byte[] Picture { get; set; }

        public List<ApplicationUser> Profiles { get; set; }
    }
}
