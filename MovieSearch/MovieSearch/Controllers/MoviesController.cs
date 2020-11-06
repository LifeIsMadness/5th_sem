using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MovieSearch.Data;
using MovieSearch.Models;
using MovieSearch.ViewModels.Movies;

namespace MovieSearch.Controllers
{
    public class MoviesController : Controller
    {
        private readonly ApplicationDbContext _context;

        private readonly ILogger<MoviesController> _logger;

        private readonly UserManager<ApplicationUser> _userManager;

        public MoviesController(
            ILogger<MoviesController> logger,
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager)
        {
            _logger = logger;
            _context = context;
            _userManager = userManager;
        }

        // GET: Movies
        public async Task<IActionResult> Index(int genreId = 0)
        {
            IQueryable<Movie> movies;
            var viewModel = new MovieIndexViewModel();

            if (genreId > 0)
            {
                movies = _context.Movies.Include(m => m.Genre).Where(m => m.GenreId == genreId);
                if (movies == null)
                    return NotFound();

                var genre = await _context.MovieGenres.FindAsync(genreId);
                viewModel.Genres = new SelectList(_context.MovieGenres, "Id", "Name", genre.Id);

            }
            else
            {
                movies = _context.Movies.Include(m => m.Genre);
                viewModel.Genres = new SelectList(_context.MovieGenres, "Id", "Name");
            }

            viewModel.Movies = await movies.ToListAsync();

            return View(viewModel);
        }

        public async Task<IActionResult> IndexSearch(string searchValue)
        {
            IQueryable<Movie> movies = _context.Movies.Include(m => m.Genre);
            var viewModel = new MovieIndexViewModel();

            if (!string.IsNullOrEmpty(searchValue))
            {
                movies = movies.Where(m => m.Name.Contains(searchValue));
            }


            ViewData["SearchValue"] = searchValue;
            viewModel.Genres = new SelectList(_context.MovieGenres, "Id", "Name");
            viewModel.Movies = await movies.ToListAsync();

            return View("./Index", viewModel);
        }

        // GET: Movies/Details/5
        //TODO: Refactoring
        //details view
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movie = await _context.Movies
                .Include(m => m.Genre)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (movie == null)
            {
                return NotFound();
            }

            var userId = _userManager.GetUserId(User);

            var viewModel = new MovieDetailsViewModel { Movie = movie };
            viewModel.Reviews = _context.Reviews
                .AsNoTracking()
                .Include(r => r.UserProfile)
                .ThenInclude(p => p.User)
                .ThenInclude(u => u.ProfilePicture)
                .Where(r => r.MovieId == id);

            if (userId != null)
            {
                var mark = await _context.MovieMarks
                    .AsNoTracking()
                    .FirstOrDefaultAsync(m =>
                    m.Movie.Id == movie.Id && m.UserProfile.UserId == userId);

                bool isFavourite = IsFavouriteMovie(userId, movie.Id);

                viewModel.Mark = mark;
                if (isFavourite) viewModel.IsFavourite = "In Favourites";
                else viewModel.IsFavourite = "Add Favourite";
            }

            return View(viewModel);
        }

        private bool IsFavouriteMovie(string userId, int movidId)
        {
            var favMovie = _context.FavouriteMovies
               .Include(f => f.UserProfiles)
               .ThenInclude(f => f.Profile)
               .FirstOrDefault(f => f.MovieId == movidId);

            return favMovie.UserProfiles.Any(f => f.Profile.UserId == userId);
        }

        // GET: Movies/Create
        // Only admin can create new a db record. 
        [Authorize(Roles = "SuperAdmin")]
        public IActionResult Create()
        {
            MovieEditViewModel viewModel = new MovieEditViewModel
            {
                Genres = new SelectList(_context.MovieGenres, "Id", "Name"),
            };

            return View(viewModel);
        }

        // POST: Movies/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> Create([Bind("Id,Name,Image,Title,Year,Country,GenreId")] Movie movie)
        {
            if (ModelState.IsValid)
            {
                _context.AddRange(movie, new FavouriteMovie { Movie = movie });
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            var genre = await _context.MovieGenres.FindAsync(movie.GenreId);
            MovieEditViewModel viewModel = new MovieEditViewModel
            {
                Movie = movie,
                Genres = new SelectList(_context.MovieGenres, "Id", "Name", genre.Name),
            };

            return View(viewModel);
        }

        // GET: Movies/Edit/5
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movie = await _context.Movies.Include(m => m.Genre).FirstOrDefaultAsync(m => m.Id == id);
            if (movie == null)
            {
                return NotFound();
            }

            MovieEditViewModel viewModel = new MovieEditViewModel
            {
                Movie = movie,
                Genres = new SelectList(_context.MovieGenres, "Id", "Name"),
            };

            return View(viewModel);
        }

        // POST: Movies/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Image,Title,Year,Country,OveralRating,GenreId")] Movie movie)
        {
            if (id != movie.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(movie);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MovieExists(movie.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }

            MovieEditViewModel viewModel = new MovieEditViewModel
            {
                Movie = movie,
            };

            var genre = await _context.MovieGenres.FindAsync(movie.GenreId);
            viewModel.Genres = new SelectList(_context.MovieGenres, "Id", "Name", genre.Name);
            return View(viewModel);
        }

        // GET: Movies/Delete/5
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movie = await _context.Movies
                .Include(m => m.Genre)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (movie == null)
            {
                return NotFound();
            }

            return View(movie);
        }

        // POST: Movies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var movie = await _context.Movies.FindAsync(id);
            _context.Movies.Remove(movie);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        private bool MovieExists(int id)
        {
            return _context.Movies.Any(e => e.Id == id);
        }

    }
}
