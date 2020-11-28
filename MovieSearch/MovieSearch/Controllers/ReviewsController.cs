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
    [Authorize]
    public class ReviewsController : Controller
    {
        private readonly ILogger<ReviewsController> _logger;

        private readonly ApplicationDbContext _context;

        private readonly UserManager<ApplicationUser> _userManager;

        public ReviewsController(ILogger<ReviewsController> logger,
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager)
        {
            _logger = logger;
            _context = context;
            _userManager = userManager;
        }

        //// GET: Reviews
        //public async Task<IActionResult> Index()
        //{
        //    var applicationDbContext = _context.Reviews.Include(r => r.Movie).Include(r => r.UserProfile);
        //    return View(await applicationDbContext.ToListAsync());
        //}

        // GET: Reviews/Create
        public async Task<IActionResult> Create(int? movieId)
        {
            if (movieId == null)
            {
                return NotFound();
            }

            var userId = _userManager.GetUserId(User);

            var userProfile = await _context.MoviesProfiles
                    .FirstOrDefaultAsync(p => p.UserId == userId);

            ViewData["MovieId"] = movieId;
            ViewData["UserProfileId"] = userProfile.Id;

            var movie = await _context.Movies.FirstOrDefaultAsync(m => m.Id == movieId);
            ViewData["MovieInfo"] = string.Format("{0}({1})", movie.Name, movie.Year);


            return View();
        }

        // POST: Reviews/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Content,Date,UserProfileId,MovieId")] Review review)
        {
            review.Date = DateTime.Now;
            if (ModelState.IsValid)
            {
                _context.Add(review);
                
                await _context.SaveChangesAsync();
                return RedirectToAction("Details", "Movies", new { id = review.MovieId });
            }

            ViewData["MovieId"] = review.MovieId;
            ViewData["UserProfileId"] = review.UserProfileId;

            var movie = await _context.Movies.FirstOrDefaultAsync(m => m.Id == review.MovieId);
            ViewData["MovieInfo"] = string.Format("{0}({1})", movie.Name, movie.Year);

            return View(review);
        }

        // GET: Reviews/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var review = await _context.Reviews.FindAsync(id);
            if (review == null)
            {
                return NotFound();
            }

            ViewData["MovieId"] = review.MovieId;
            ViewData["UserProfileId"] = review.UserProfileId;

            var movie = await _context.Movies.FirstOrDefaultAsync(m => m.Id == review.MovieId);
            ViewData["MovieInfo"] = string.Format("{0}({1})", movie.Name, movie.Year);

            return View(review);
        }

        // POST: Reviews/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Content,Date,UserProfileId,MovieId")] Review review)
        {
            if (id != review.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    review.Date = DateTime.Now;
                    _context.Update(review);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReviewExists(review.Id))
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
            ViewData["MovieId"] = review.MovieId;
            ViewData["UserProfileId"] = review.UserProfileId;


            var movie = await _context.Movies.FirstOrDefaultAsync(m => m.Id == review.MovieId);
            ViewData["MovieInfo"] = string.Format("{0}({1})", movie.Name, movie.Year);
            return View(review);
        }

        // GET: Reviews/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var review = await _context.Reviews
                .Include(r => r.Movie)
                .Include(r => r.UserProfile)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (review == null)
            {
                return NotFound();
            }

            return View(review);
        }

        // POST: Reviews/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var review = await _context.Reviews.FindAsync(id);

            var userId = _userManager.GetUserId(User);

            var user = await _userManager.Users.Include(u => u.MoviesProfile).FirstOrDefaultAsync(u => u.Id == userId);

            if (review.UserProfileId == user.MoviesProfile.Id
                || await _userManager.IsInRoleAsync(user, "SuperAdmin")
                || await _userManager.IsInRoleAsync(user, "Moderator"))
            {
                _context.Reviews.Remove(review);
                await _context.SaveChangesAsync();

                return RedirectToAction("Details", "Movies", new { id = review.MovieId });
            }
            else
            {
                return BadRequest();
            }
        }

        private bool ReviewExists(int id)
        {
            return _context.Reviews.Any(e => e.Id == id);
        }
    }
}
