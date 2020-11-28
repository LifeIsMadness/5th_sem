using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Logging;
using MovieSearch.Data;
using MovieSearch.Models;

namespace MovieSearch.Controllers
{
    [Authorize]
    public class VoteController : Controller
    {
        private readonly ApplicationDbContext _context;

        private readonly ILogger<VoteController> _logger;

        private readonly UserManager<ApplicationUser> _userManager;

        public VoteController(
            ILogger<VoteController> logger,
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager)
        {
            _logger = logger;
            _context = context;
            _userManager = userManager;
        }

        private bool MarkExists(int id)
        {
            return _context.MovieMarks.Any(m => m.Id == id);
        }

        private bool MarkExists(int id, int value)
        {
            return _context.MovieMarks.Any(m => m.Id == id && m.Value == value);
        }

        private float UpdateOveralRating(int movieId)
        {
            var entries = _context.ChangeTracker.Entries<MovieMark>()
                .Where(e => e.State == EntityState.Deleted)
                .Select(e => e.Entity);

            var movie = _context.Movies.Include(m => m.MovieMarks).
                FirstOrDefault(m => m.Id == movieId);

            var marks = movie.MovieMarks.Except(entries);

            var count = marks.Count();

            movie.OveralRating = 0;

            foreach (var mark in marks)
            { movie.OveralRating += (float)mark.Value / count; }


            movie.OveralRating = (float)Math.Round(movie.OveralRating, 3);

            _context.Movies.Update(movie);

            return movie.OveralRating;
        }

    }
}
