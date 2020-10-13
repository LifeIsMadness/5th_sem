using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MovieSearch.Models;
using MovieSearch.Models.ExtendedUser;

namespace MovieSearch.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Movie> Movies { get; set; }

        public DbSet<MovieGenre> MovieGenres { get; set; }

        public DbSet<MovieMark> MovieMarks { get; set; }

        public DbSet<Review> Reviews { get; set; }

        public DbSet<UserMoviesProfile> MoviesProfiles { get; set; }

        public DbSet<FavouriteMovie> FavouriteMovies { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {

            builder.Entity<UserFavourites>()
                .HasKey(f => new { f.MovieId, f.ProfileId });

            base.OnModelCreating(builder);
        }
    }
}
