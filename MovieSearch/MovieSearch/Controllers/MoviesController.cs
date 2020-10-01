using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MovieSearch.Data;
using MovieSearch.Models;

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
            if (genreId > 0)
            {
                movies = _context.Movies.Include(m => m.Genre).Where(m => m.GenreId == genreId);
                if (movies == null)
                    return NotFound();

                var genre = await _context.MovieGenres.FindAsync(genreId);
                ViewData["Genres"] = new SelectList(_context.MovieGenres, "Id", "Name", genre.Id);
      
            }
            else
            {
                movies = _context.Movies.Include(m => m.Genre);
                ViewData["Genres"] = new SelectList(_context.MovieGenres, "Id", "Name");
            }
            
            return View(await movies.ToListAsync());
        }

        public async Task<IActionResult> IndexSearch(string searchValue)
        {
            IQueryable<Movie> movies = _context.Movies.Include(m => m.Genre);

            if(!string.IsNullOrEmpty(searchValue))
            {
                movies = movies.Where(m => m.Name.Contains(searchValue));
            }

            ViewData["Genres"] = new SelectList(_context.MovieGenres, "Id", "Name");
         
            return View("./Index", await movies.ToListAsync());
        }

        // GET: Movies/Details/5
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

            var userId =  _userManager.GetUserId(User);
            var user = _userManager.Users.Include(u => u.Marks).FirstOrDefault(u => u.Id == userId);
            if (user != null)
            {
                ViewData["MovieMarkValue"] = user.Marks.FirstOrDefault(m => m.Movie.Id == movie.Id)?.Value;
            }

            return View(movie);
        }

        // GET: Movies/Create
        // Only admin can create new a db record. 
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            ViewData["GenreId"] = new SelectList(_context.MovieGenres, "Id", "Name");
            return View();
        }

        // POST: Movies/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([Bind("Id,Name,Image,Title,Year,Country,GenreId")] Movie movie)
        {
            if (ModelState.IsValid)
            {
                _context.Add(movie);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            var genre = await _context.MovieGenres.FindAsync(movie.GenreId);
            ViewData["GenreId"] = new SelectList(_context.MovieGenres, "Id", "Name", genre.Name);
            return View(movie);
        }

        // GET: Movies/Edit/5
        [Authorize(Roles = "Admin")]
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
            ViewData["GenreId"] = new SelectList(_context.MovieGenres, "Id", "Name");
            return View(movie);
        }

        // POST: Movies/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
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

            var genre = await _context.MovieGenres.FindAsync(movie.GenreId);
            ViewData["GenreId"] = new SelectList(_context.MovieGenres, "Id", "Name", genre.Name);
            return View(movie);
        }

        // GET: Movies/Delete/5
        [Authorize(Roles = "Admin")]
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
        [Authorize(Roles = "Admin")]
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
