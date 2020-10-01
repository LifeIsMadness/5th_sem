using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MovieSearch.Models
{
    public class Movie
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Image { get; set; }
       
        [Column(TypeName = "NVARCHAR(500)")]
        public string Title { get; set; }

        public int Year { get; set; }

        public string Country { get; set; }

        [DefaultValue(0.0)]
        public float OveralRating { get; set; }

        public int GenreId { get; set; }

        public MovieGenre Genre { get; set; }

        public List<MovieMark> MovieMarks { get; set; }
    }
}
