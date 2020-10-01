using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MovieSearch.Data;
using MovieSearch.Models;
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
        public async Task<IActionResult> Vote(int id, int value)
        {
            var movie = await _context.Movies.FindAsync(id);
            if (movie == null)
            {
                return NotFound();
            }

            var user = await _userManager.GetUserAsync(User);

            if (!MarkExistsAndReturn(user.Id, movie.Id, out MovieMark existingModel))
            {
                _context.MovieMarks.Add(new MovieMark() { Value = value, Movie = movie, User = user });
                user.MoviesViewedCount++;
            }
            else
            {
                existingModel.Value = value;
                _context.MovieMarks.Update(existingModel);
            }

            
            UpdateOveralRating(value, movie.Id);
            await _context.SaveChangesAsync();

            return RedirectToAction("Details", "Movies", new { id = id });
        }

        private bool MarkExistsAndReturn(string userId, int movieId, out MovieMark mark)
        {
            if (_context.MovieMarks.Any(m => m.UserId == userId && m.MovieId == movieId))
            {
                mark = _context.MovieMarks.FirstOrDefault(m => m.UserId == userId && m.MovieId == movieId);
                return true;
            }
            else
            {
                mark = null;
                return false;
            }
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
