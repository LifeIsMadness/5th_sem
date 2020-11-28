using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using Microsoft.Extensions.Logging;
using MovieSearch.Data;
using MovieSearch.Models;
using MovieSearch.ViewModels.UserMovies;

namespace MovieSearch.ViewModels
{
    [Authorize]
    public class UserMoviesController : Controller
    {

        public class MovieComparer : IEqualityComparer<Movie>
        {
            public bool Equals([AllowNull] Movie x, [AllowNull] Movie y)
            {
                if (y == null || x == null)
                {
                    return false;
                }

                // TODO: write your implementation of Equals() here
                return x.Id == y.Id;
            }

            public int GetHashCode([DisallowNull] Movie obj)
            {
                return base.GetHashCode();
            }
        }

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

        [Route("ViewedMovies/{userId:Guid}/[Action]", Name = "Favourites")]
        public async Task<IActionResult> Favourites(string userId)
        {
            var currentUserId = _userManager.GetUserId(User);

            var userProfile = await _context.MoviesProfiles
                .Include(u => u.User)
                .Include(p => p.FavouriteMovies)
                .ThenInclude(f => f.Movie)
                .ThenInclude(f => f.Movie)
                .ThenInclude(m => m.Genre)
                .Include(p => p.Marks)
                .ThenInclude(m => m.Movie)
                .FirstOrDefaultAsync(p => p.UserId == userId);

            if(userProfile == null)
            {
                return NotFound();
            }

            // Double select 'cause FavouriteMovie has Movie;
            var favourites = userProfile.FavouriteMovies
                .Select(f => f.Movie)
                .Select(f => f.Movie)
                .OrderBy(f => f.Name);

            var marks = userProfile.Marks.OrderBy(m => m.Movie.Name);

            var markedMovies = marks.Select(m => m.Movie);

            var unmarkedMovies = favourites.Except(markedMovies, new MovieComparer());

            var moviesAndMarks = favourites.Zip(marks).ToList();

            foreach (var movie in unmarkedMovies)
            {
                moviesAndMarks.Add((movie, null));
            }

            moviesAndMarks = moviesAndMarks.OrderBy(m => m.First.Name).ToList();

            var viewModel = new UserMoviesViewModel
            {
                MoviesAndMarks = moviesAndMarks,

                ForCurrentUser = userId == currentUserId,

                UserName = userProfile.User.UserName,

                ReturnUrl = Url.RouteUrl("Favourites", new { UserId = userId }) //"/UserMovies/"
            };

            return View(viewModel);
        }

        [Route("ViewedMovies/{userId:Guid}/[Action]", Name = "Viewed")]
        public async Task<IActionResult> ViewedMovies(string userId)
        {
            var currentUserId = _userManager.GetUserId(User);
            var userProfile = await _context.MoviesProfiles
                .Include(p => p.User)
                .Include(p => p.Marks)
                .ThenInclude(m => m.Movie)
                .FirstOrDefaultAsync(p => p.UserId == userId);

            if (userProfile == null)
            {
                return NotFound();
            }

            // Double select 'cause FavouriteMovie has Movie;
            var movies = userProfile.Marks.Select(m => m.Movie).OrderBy(m => m.Name);

            var marks = userProfile.Marks.OrderBy(m => m.Movie.Name);

            var moviesAndMarks = movies.Zip(marks);

            var viewModel = new UserMoviesViewModel
            {
                MoviesAndMarks = moviesAndMarks,

                ForCurrentUser = userId == currentUserId,

                UserName = userProfile.User.UserName,

                ReturnUrl = Url.RouteUrl("Viewed", new { UserId = userId })
            };

            return View(viewModel);
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

            string isFavourite;

            //'Cause m2m between fav model and user profile
            if (!userProfile.FavouriteMovies.Any(m => m.MovieId == favMovie.Id))
            {
                userProfile.FavouriteMovies.Add(new UserFavourites { MovieId = favMovie.Id, ProfileId = userProfile.Id });
                isFavourite = "In Favourites";
            }
            else
            {
                var userFavMovie = userProfile.FavouriteMovies.FirstOrDefault(m => m.MovieId == favMovie.Id);
                userProfile.FavouriteMovies.Remove(userFavMovie);
                isFavourite = "Add Favourite ";
            }

            await _context.SaveChangesAsync();

            return Json( new { isFavourite });

            //if (string.IsNullOrEmpty(returnUrl))
            //    return RedirectToAction("Details", "Movies", new { id = id });
            //else return Redirect(returnUrl);
        }
    }
}
