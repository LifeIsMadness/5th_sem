using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MovieSearch.Models
{
    public class Notification
    {
        public int Id { get; set; }

        [Required]
        public string Text { get; set; }

        public DateTime Date { get; set; }

        public string UserId { get; set; }

        public ApplicationUser User { get; set; }

    }
}
