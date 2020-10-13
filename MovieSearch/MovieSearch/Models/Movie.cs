using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MovieSearch.Models
{
    public class Movie
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Image { get; set; }

        [Required]
        [Column(TypeName = "NVARCHAR(500)")]
        public string Title { get; set; }

        [Required]
        public int Year { get; set; }

        [Required]
        public string Country { get; set; }

        [DefaultValue(0.0)]
        [Display(Name = "Overal Rating")]
        public float OveralRating { get; set; }

        public int GenreId { get; set; }

        public MovieGenre Genre { get; set; }

        public List<MovieMark> MovieMarks { get; set; }

        public FavouriteMovie FavouriteMovie { get; set; }
    }
}
