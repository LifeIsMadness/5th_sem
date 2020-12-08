using Microsoft.AspNetCore.Identity;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MovieSearch.Data;
using MovieSearch.Models.ExtendedUser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieSearch.Models
{
    public enum Roles
    {
        SuperAdmin,
        Moderator,
        Basic
    }

    public static class DbInitializer
    {
        public static void Initialize(ApplicationDbContext dbContext, UserManager<ApplicationUser> userManager)
        {

            if (!dbContext.Movies.Any())
            {
                dbContext.AddRange(
                    new Movie
                    {
                        Name = "Шерлок Холмс",
                        Country = "США, Германия, Великобритания",
                        Genre = new MovieGenre { Name = "Боевик" },
                        Image = "/img/sherlock_holmes_1.jpg",
                        Title = "Величайший в истории сыщик Шерлок Холмс вместе со своим верным соратником Ватсоном вступают в схватку, " +
                        "требующую нешуточной физической и умственной подготовки, ведь их враг представляет угрозу для всего Лондона.",
                        Year = 2009,
                        FavouriteMovie = new FavouriteMovie()

                    },
                    new Movie
                    {
                        Name = "Остров проклятых",
                        Country = "США",
                        Genre = new MovieGenre { Name = "Триллер" },
                        Image = "/img/shutter_island.jpg",
                        Title = "Два американских судебных пристава отправляются на один из островов в штате Массачусетс, " +
                        "чтобы расследовать исчезновение пациентки клиники для умалишенных преступников. " +
                        "При проведении расследования им придется столкнуться с паутиной лжи, " +
                        "обрушившимся ураганом и смертельным бунтом обитателей клиники.",
                        Year = 2009,
                        FavouriteMovie = new FavouriteMovie()

                    },
                    new Movie
                    {
                        Name = "Один дома",
                        Country = "США",
                        Genre = new MovieGenre { Name = "Комедия" },
                        Image = "/img/home_alone.jpg",
                        Title = "Американское семейство отправляется из Чикаго в Европу, но в спешке сборов бестолковые родители забывают дома... одного из своих детей. " +
                        "Юное создание, однако, не теряется и демонстрирует чудеса изобретательности. " +
                        "И когда в дом залезают грабители, им приходится не раз пожалеть о встрече с милым крошкой.",
                        Year = 1990,
                        FavouriteMovie = new FavouriteMovie()
                    },
                    new Movie
                    {
                        Name = "Мстители: Война бесконечности",
                        Country = "США",
                        Genre = new MovieGenre { Name = "Фантастика" },
                        Image = "/img/infinity_war.jpg",
                        Title = "Пока Мстители и их союзники продолжают защищать мир от различных опасностей, " +
                        "с которыми не смог бы справиться один супергерой, новая угроза возникает из космоса: Танос.",
                        Year = 2018,
                        FavouriteMovie = new FavouriteMovie()

                    },
                    new Movie
                    {
                        Name = "Оно",
                        Country = "США, Канада",
                        Genre = new MovieGenre { Name = "Ужасы" },
                        Image = "/img/it.jpg",
                        Title = "Когда в городке Дерри штата Мэн начинают пропадать дети," +
                        " несколько ребят сталкиваются со своими величайшими страхами - не только с группой школьных хулиганов, " +
                        "но со злобным клоуном Пеннивайзом, чьи проявления жестокости и список жертв уходят в глубь веков.",
                        Year = 2017,
                        FavouriteMovie = new FavouriteMovie()

                    }



                ) ;
            }
            else return;

            dbContext.SaveChanges();

        }

        public static async Task SeedRolesAsync(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            //Seed Roles
            if (!roleManager.Roles.Any())
            {
                await roleManager.CreateAsync(new IdentityRole(Roles.SuperAdmin.ToString()));
                await roleManager.CreateAsync(new IdentityRole(Roles.Moderator.ToString()));
                await roleManager.CreateAsync(new IdentityRole(Roles.Basic.ToString()));
            }
        }

        public static async Task SeedSuperAdminAsync(
            UserManager<ApplicationUser> userManager, 
            RoleManager<IdentityRole> roleManager,
            ApplicationDbContext _context)
        {
            //Seed Default User
            var defaultUser = new ApplicationUser
            {
                UserName = "luckstriker073@gmail.com",
                Email = "luckstriker073@gmail.com",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true
            };

            var user = await userManager.FindByEmailAsync(defaultUser.Email);
            if (user == null)
            {
                var result = await userManager.CreateAsync(defaultUser, "Strong2020-");
                await userManager.AddToRoleAsync(defaultUser, Roles.Basic.ToString());
                await userManager.AddToRoleAsync(defaultUser, Roles.Moderator.ToString());
                await userManager.AddToRoleAsync(defaultUser, Roles.SuperAdmin.ToString());

                user = await userManager.FindByEmailAsync(defaultUser.Email);
                await _context.MoviesProfiles.AddAsync(new UserMoviesProfile { User = user });

                await _context.SaveChangesAsync();
            }


        }
    }
}
