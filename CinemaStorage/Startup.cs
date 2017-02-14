using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(CinemaStorage.Startup))]
namespace CinemaStorage
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);

            // добавление тестовых данных в БД
            //TestDataInitialization();
        }

        /// <summary>
        /// Заполнить БД тестовыми данными (если она пуста).
        /// </summary>
        private void TestDataInitialization()
        {
            using(var repo = new Models.ApplicationDbContext())
            {
                if (repo.Movies.Count() > 0)
                    return;

                #region samples list
                var m1 = new Models.Movie
                {
                    Title = "Титаник",
                    Genre = "Драма",
                    Description = "A seventeen-year-old aristocrat falls in love with a kind but poor artist aboard the luxurious, ill-fated R.M.S. Titanic.",
                    Date = DateTime.Parse("01.01.1997"),
                    Director = "Дж. Кэмерон"
                };

                var m2 = new Models.Movie
                {
                    Title = "Форест Гамп",
                    Genre = "Драма, Комедия",
                    Description = "Forrest Gump, while not intelligent, has accidentally been present at many historic moments, but his true love, Jenny Curran, eludes him.",
                    Date = DateTime.Parse("01.01.1994"),
                    Director = "Р. Земекис"
                };

                var m3 = new Models.Movie
                {
                    Title = "Афоня",
                    Genre = "Комедия",
                    Description = "The 1975 film by Georgi Daneliya 'Afonya' was an unexpected commercial hit in USSR. The main character Borshev A.N. is a Plumber who spends his free time, as well as working hours, drinking with his buddies whom he even doesn't recognize the next day after another heavy drink.",
                    Date = DateTime.Parse("01.01.1975"),
                    Director = "Г. Данелия"
                };

                var m4 = new Models.Movie
                {
                    Title = "Терминатор",
                    Genre = "Боевик",
                    Description = "A seemingly indestructible humanoid cyborg is sent from 2029 to 1984 to assassinate a waitress, whose unborn son will lead humanity in a war against the machines, while a soldier from that war is sent to protect her at all costs.",
                    Date = DateTime.Parse("01.01.1984"),
                    Director = "Дж. Кэмерон"
                };

                var m5 = new Models.Movie
                {
                    Title = "Парк Юрского Периода",
                    Genre = "Приключения",
                    Description = "During a preview tour, a theme park suffers a major power breakdown that allows its cloned dinosaur exhibits to run amok.",
                    Date = DateTime.Parse("01.01.1993"),
                    Director = "С. Спилберг"
                };

                var m6 = new Models.Movie
                {
                    Title = "Охотники за привидениями",
                    Genre = "Фантастический боевик",
                    Description = "Three former parapsychology professors set up shop as a unique ghost removal service.",
                    Date = DateTime.Parse("01.01.1984"),
                    Director = "И. Рейтман"
                };

                var m7 = new Models.Movie
                {
                    Title = "Семь",
                    Genre = "Триллер, Детектив",
                    Description = "Two detectives, a rookie and a veteran, hunt a serial killer who uses the seven deadly sins as his modus operandi.",
                    Date = DateTime.Parse("01.01.1995"),
                    Director = "Д. Финчер"
                };

                var m8 = new Models.Movie
                {
                    Title = "Матрица",
                    Genre = "Фантастический боевик",
                    Description = "A computer hacker learns from mysterious rebels about the true nature of his reality and his role in the war against its controllers.",
                    Date = DateTime.Parse("01.01.1999"),
                    Director = "Братья Вачовски"
                };

                var m9 = new Models.Movie
                {
                    Title = "Властелин Колец: Две крепости",
                    Genre = "Фэнтези",
                    Description = "While Frodo and Sam edge closer to Mordor with the help of the shifty Gollum, the divided fellowship makes a stand against Sauron's new ally, Saruman, and his hordes of Isengard.",
                    Date = DateTime.Parse("01.01.2002"),
                    Director = "П. Джексон"
                };

                var m10 = new Models.Movie
                {
                    Title = "Начало",
                    Genre = "Боевик",
                    Description = "A thief, who steals corporate secrets through use of dream-sharing technology, is given the inverse task of planting an idea into the mind of a CEO.",
                    Date = DateTime.Parse("01.01.2010"),
                    Director = "К. Нолан"
                };

                var m11 = new Models.Movie
                {
                    Title = "Темный рыцарь",
                    Genre = "Боевик",
                    Description = "When the menace known as the Joker wreaks havoc and chaos on the people of Gotham, the caped crusader must come to terms with one of the greatest psychological tests of his ability to fight injustice.",
                    Date = DateTime.Parse("01.01.2008"),
                    Director = "К. Нолан"
                };
                #endregion

                repo.Movies.AddRange(new[] { m1, m2, m3, m4, m5, m6, m7, m8, m9, m10, m11 });

                repo.SaveChanges();
            }
        }
    }
}
