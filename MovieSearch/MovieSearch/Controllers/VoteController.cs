using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Logging;
using MovieSearch.Data;
using MovieSearch.Migrations;
using MovieSearch.Models;
using MovieSearch.Models.ExtendedUser;
using SQLitePCL;

namespace MovieSearch.Controllers
{
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
        public IActionResult Index()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Vote([Bind("Id,Value,MovieId,UserProfileId")] MovieMark mark)
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
            else _context.MovieMarks.Update(mark);

            UpdateOveralRating(mark.Value, mark.MovieId);
            await _context.SaveChangesAsync();

            return RedirectToAction("Details", "Movies", new { id = mark.MovieId });
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddDeleteFavouriteMovie(int id)
        {
            var userId = _userManager.GetUserId(User);

            var userProfile = await _context.MoviesProfiles
                    .Include(p => p.FavouriteMovies)
                    .FirstOrDefaultAsync(p => p.UserId == userId);

            if(!userProfile.FavouriteMovies.Any(m => m.MovieId == id))
            {
                userProfile.FavouriteMovies.Add(new UserFavourites { MovieId = id, ProfileId = userProfile.Id });
            }
            else
            {
                var favMovie = userProfile.FavouriteMovies.FirstOrDefault(m => m.MovieId == id);
                userProfile.FavouriteMovies.Remove(favMovie);
            }

            await _context.SaveChangesAsync();

            return RedirectToAction("Details", "Movies", new { id = id });
        }

        private bool MarkExists(int id)
        {
            return _context.MovieMarks.Any(m => m.Id == id);
        }

        private async void UpdateOveralRating(int value, int movieId)
        {
            var movie = await _context.Movies.FindAsync(movieId);
            var count = _context.Movies.Count();

            movie.OveralRating = 0;
            foreach (var mark in movie.MovieMarks)
            { movie.OveralRating += mark.Value / count;}

            _context.Movies.Update(movie);
        }

    }
}
