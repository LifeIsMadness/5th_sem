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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Vote(string returnUrl, 
            [Bind("Id,Value,MovieId,UserProfileId")] MovieMark mark)
        {
            var user = await _userManager.GetUserAsync(User);

            var userProfile = await _context.MoviesProfiles
                    .FirstOrDefaultAsync(p => p.UserId == user.Id);

            if (!MarkExists(mark.Id))
            {
                mark.UserProfileId = userProfile.Id;
                _context.MovieMarks.Add(mark);
                userProfile.MoviesViewedCount++;
            }
            else if (MarkExists(mark.Id, mark.Value)) _context.MovieMarks.Remove(mark);
            else _context.MovieMarks.Update(mark);

            UpdateOveralRating(mark.MovieId);

            await _context.SaveChangesAsync();

            if (string.IsNullOrEmpty(returnUrl))
                return RedirectToAction("Details", "Movies", new { id = mark.MovieId });
            else return Redirect(returnUrl);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddDeleteFavouriteMovie(int id, string returnUrl)
        {
            var userId = _userManager.GetUserId(User);

            var userProfile = await _context.MoviesProfiles
                    .Include(p => p.FavouriteMovies)
                    .FirstOrDefaultAsync(p => p.UserId == userId);

            var favMovie = await _context.FavouriteMovies.FirstOrDefaultAsync(f => f.MovieId == id);

            //'Cause m2m between fav model and user profile
            if(!userProfile.FavouriteMovies.Any(m => m.MovieId == favMovie.Id))
            {
                userProfile.FavouriteMovies.Add(new UserFavourites { MovieId = favMovie.Id, ProfileId = userProfile.Id });
            }
            else
            {
                var userFavMovie = userProfile.FavouriteMovies.FirstOrDefault(m => m.MovieId == favMovie.Id);
                userProfile.FavouriteMovies.Remove(userFavMovie);
            }

            await _context.SaveChangesAsync();

            if (string.IsNullOrEmpty(returnUrl))
                return RedirectToAction("Details", "Movies", new { id = id });
            else return Redirect(returnUrl);
        }

        private bool MarkExists(int id)
        {
            return _context.MovieMarks.Any(m => m.Id == id);
        }

        private bool MarkExists(int id, int value)
        {
            return _context.MovieMarks.Any(m => m.Id == id && m.Value == value);
        }

        private void UpdateOveralRating(int movieId)
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
            { movie.OveralRating += (float)mark.Value / count;}


            movie.OveralRating = (float)Math.Round(movie.OveralRating, 3);

            _context.Movies.Update(movie);
        }

    }
}
