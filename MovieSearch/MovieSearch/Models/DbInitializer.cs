using Microsoft.AspNetCore.Identity;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MovieSearch.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieSearch.Models
{
    public static class DbInitializer
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var dbContext = new ApplicationDbContext(
                serviceProvider.GetRequiredService<
                    DbContextOptions<ApplicationDbContext>>()))
            using(var userManager = 
                serviceProvider.GetRequiredService<
                    UserManager<ApplicationUser>>())
            {
                if (!dbContext.Movies.Any())
                {
                    dbContext.AddRange(
                        new Movie
                        {
                            Name = "test",
                            Country = "test",
                            Genre = GetMovieGenre(0),
                            Image = "test",
                            OveralRating = 9.1F,
                            Title = "Some text here...",
                            Year = 2000


                        }

                    );
                }
                else 
                if (!dbContext.Roles.Any(r => r.Name == "Admin"))
                {
                    dbContext.Roles.Add(new IdentityRole { Name = "Admin" });
                }
                else return;

                dbContext.SaveChanges();
            }

        }

        private static MovieGenre GetMovieGenre(int index)
        {
            return MovieGenres()[index];
        }

        private static List<MovieGenre> MovieGenres() => new List<MovieGenre>
        {
            new MovieGenre
            {
                Name = "Ужасы"
            },
            new MovieGenre
            {
                Name = "Фантастика"
            },
            new MovieGenre
            {
                Name = "Драма"
            },
            new MovieGenre
            {
                Name = "Комедия"
            },
            new MovieGenre
            {
                Name = "Триллер"
            },
            new MovieGenre
            {
                Name = "Боевик"
            },
            new MovieGenre
            {
                Name = "Приключение"
            },

        };
    }
}
