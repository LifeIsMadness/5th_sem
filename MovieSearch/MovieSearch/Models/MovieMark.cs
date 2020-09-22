using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieSearch.Models
{
    public class MovieMark
    {
        public int Id { get; set; }

        public int Value { get; set; }

        public int MovieId { get; set; }

        public Movie Movie { get; set; }

        public string UserId { get; set; }

        public ApplicationUser User { get; set; }
    }
}
