using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MovieSearch.Data;
using MovieSearch.Models;
using MovieSearch.RenderHelper;

namespace MovieSearch.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [NoDirectAccess]
    [Authorize]
    public class MarksApiController : ControllerBase
    {
        private readonly ILogger<VoteController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public MarksApiController(
            ILogger<VoteController> logger,
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager)
        {
            _logger = logger;
            _context = context;
            _userManager = userManager;
        }


        // PUT: api/MarksApi/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMovieMark(int id, [FromBody] MovieMark mark)
        {
            //using (System.IO.StreamReader reader = new System.IO.StreamReader(Request.Body, Encoding.UTF8))
            //{
            //    string msg = await reader.ReadToEndAsync();
            //    return Ok(msg);
            //}


            if (id != mark.Id)  
            {
                return BadRequest();
            }

            var user = await _userManager.GetUserAsync(User);

            var userProfile = await _context.MoviesProfiles
                    .FirstOrDefaultAsync(p => p.UserId == user.Id);

            _context.Entry(mark).State = EntityState.Modified;
            float rating;

            try
            {
                rating = UpdateOveralRating(mark.MovieId);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MovieMarkExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(new
            {
                rating
            });
        }

        // POST: api/MarksApi
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<MovieMark>> PostMovieMark([FromBody] MovieMark mark)
        {
            var user = await _userManager.GetUserAsync(User);

            var userProfile = await _context.MoviesProfiles
                    .FirstOrDefaultAsync(p => p.UserId == user.Id);

            mark.UserProfileId = userProfile.Id;
            _context.MovieMarks.Add(mark);
            userProfile.MoviesViewedCount++;

            var rating = UpdateOveralRating(mark.MovieId);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                markId = mark.Id,
                userProfileId = mark.UserProfileId,
                rating
            });
        }

        // DELETE: api/MarksApi/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<MovieMark>> DeleteMovieMark(int id)
        {
            var user = await _userManager.GetUserAsync(User);

            var userProfile = await _context.MoviesProfiles
                    .FirstOrDefaultAsync(p => p.UserId == user.Id);

            var mark = await _context.MovieMarks.FindAsync(id);
            if (mark == null)
            {
                return NotFound();
            }

            _context.MovieMarks.Remove(mark);
            userProfile.MoviesViewedCount--;
            var rating = UpdateOveralRating(mark.MovieId);

            await _context.SaveChangesAsync();

            return Ok(new
            {
                rating
            });
        }

        private bool MovieMarkExists(int id)
        {
            return _context.MovieMarks.Any(e => e.Id == id);
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
