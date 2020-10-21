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
                            Name = "Шерлок Холмс",
                            Country = "США, Германия, Великобритания",
                            Genre = new MovieGenre { Name = "Боевик" },
                            Image = "/img/sherlock_holmes_1",
                            Title = "Величайший в истории сыщик Шерлок Холмс вместе со своим верным соратником Ватсоном вступают в схватку, " +
                            "требующую нешуточной физической и умственной подготовки, ведь их враг представляет угрозу для всего Лондона.",
                            Year = 2009

                        },
                        new Movie
                        {
                            Name = "Остров проклятых",
                            Country = "США",
                            Genre = new MovieGenre { Name = "Триллер" },
                            Image = "/img/shutter_island",
                            Title = "Два американских судебных пристава отправляются на один из островов в штате Массачусетс, " +
                            "чтобы расследовать исчезновение пациентки клиники для умалишенных преступников. " +
                            "При проведении расследования им придется столкнуться с паутиной лжи, " +
                            "обрушившимся ураганом и смертельным бунтом обитателей клиники.",
                            Year = 2009

                        },
                        new Movie
                        {
                            Name = "Один дома",
                            Country = "США",
                            Genre = new MovieGenre { Name = "Комедия" },
                            Image = "/img/home_alone",
                            Title = "Американское семейство отправляется из Чикаго в Европу, но в спешке сборов бестолковые родители забывают дома... одного из своих детей. " +
                            "Юное создание, однако, не теряется и демонстрирует чудеса изобретательности. " +
                            "И когда в дом залезают грабители, им приходится не раз пожалеть о встрече с милым крошкой.",
                            Year = 1990

                        },
                        new Movie
                        {
                            Name = "Мстители: Война бесконечности",
                            Country = "США",
                            Genre = new MovieGenre { Name = "Фантастика"},
                            Image = "/img/infinity_war",
                            Title = "Пока Мстители и их союзники продолжают защищать мир от различных опасностей, " +
                            "с которыми не смог бы справиться один супергерой, новая угроза возникает из космоса: Танос.",
                            Year = 2018

                        },
                        new Movie
                        {
                            Name = "Оно",
                            Country = "США, Канада",
                            Genre = new MovieGenre { Name = "Ужасы" },
                            Image = "/img/it",
                            Title = "Когда в городке Дерри штата Мэн начинают пропадать дети," +
                            " несколько ребят сталкиваются со своими величайшими страхами - не только с группой школьных хулиганов, " +
                            "но со злобным клоуном Пеннивайзом, чьи проявления жестокости и список жертв уходят в глубь веков.",
                            Year = 2017

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

    }
}
