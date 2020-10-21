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

            UpdateOveralRating(mark.MovieId);

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

            return RedirectToAction("Details", "Movies", new { id = id });
        }

        private bool MarkExists(int id)
        {
            return _context.MovieMarks.Any(m => m.Id == id);
        }

        private void UpdateOveralRating(int movieId)
        {
            var movie = _context.Movies.Find(movieId);
            var count = _context.MovieMarks.Where(m => m.MovieId == movieId).Count();

            if (!_context.MovieMarks.Any(m => m.MovieId == movieId)) 
                count++;

            movie.OveralRating = 0;
            foreach (var mark in movie.MovieMarks)
            { movie.OveralRating += mark.Value / count;}

            _context.Movies.Update(movie);
        }

    }
}
