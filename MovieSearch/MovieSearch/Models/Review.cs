using MovieSearch.Models.ExtendedUser;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MovieSearch.Models
{
    public class Review
    {
        public int Id { get; set; }

        [Required]
        [Column(TypeName = "Text")]
        public string Content { get; set; }
  
        [DataType(DataType.DateTime)]
        public DateTime Date { get; set; }

        public int UserProfileId { get; set; }

        public UserMoviesProfile UserProfile { get; set; }

        public int MovieId { get; set; }

        public Movie Movie { get; set; }
    }
}
