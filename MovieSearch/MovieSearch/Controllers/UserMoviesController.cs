using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MovieSearch.Data;
using MovieSearch.Models;
using MovieSearch.ViewModels.UserMovies;

namespace MovieSearch.ViewModels
{
    public class UserMoviesController : Controller
    {
        private readonly ApplicationDbContext _context;

        private readonly ILogger<UserMoviesController> _logger;

        private readonly UserManager<ApplicationUser> _userManager;

        public UserMoviesController(
            ILogger<UserMoviesController> logger,
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager)
        {
            _logger = logger;
            _context = context;
            _userManager = userManager;
        }

        [Authorize]
        public async Task<IActionResult> Index()
        {
            var userId = _userManager.GetUserId(User);
            var userProfile = await _context.MoviesProfiles
                .Include(p => p.FavouriteMovies)
                .ThenInclude(f => f.Movie)
                .ThenInclude(f => f.Movie)
                .ThenInclude(m => m.Genre)
                .FirstOrDefaultAsync(p => p.UserId == userId);

            // Double select 'cause FavouriteMovie has Movie;
            var favourites = userProfile.FavouriteMovies.Select(f => f.Movie).Select(f => f.Movie);
            var viewModel = new UserMoviesIndexViewModel { FavMovies = favourites };

            return View(viewModel);
        }
    }
}
