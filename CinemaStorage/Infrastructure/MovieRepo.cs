using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using CinemaStorage.Models;

namespace CinemaStorage.Infrastructure
{
    /// <summary>
    /// Репозиторий для выборки фильмов и связанных данных.
    /// </summary>
    public class MovieRepo : IDisposable
    {
        private ApplicationDbContext _cnt;

        public MovieRepo(ApplicationDbContext context)
        {
            _cnt = context;
        }

        public void Dispose()
        {
            ((IDisposable)_cnt).Dispose();
        }

        #region Methods

        /// <summary>
        /// Получить число всех загруженных фильмов.
        /// </summary>
        /// <returns></returns>
        public int GetTotalMovieCount()
        {
            return _cnt.Movies.Count();
        }

        /// <summary>
        /// Получить пользователя по логину.
        /// </summary>
        /// <param name="login">Логин (e-mail)</param>
        /// <returns></returns>
        public ApplicationUser GetUser(string login)
        {
            return _cnt.Users.FirstOrDefault(u => u.UserName == login);
        }

        /// <summary>
        /// Получить список фильмов.
        /// </summary>
        /// <param name="order_type">Тип сортировки.</param>
        /// <param name="take_records">Число выбираемых записей.</param>
        /// <param name="skip_records">Число записей, пропускаемых с начала.</param>
        /// <returns></returns>
        public IEnumerable<Movie> GetMovies(MovieOrderType order_type, int take_records = 5, int skip_records = 0)
        {
            var found = _cnt.Movies.Include(m => m.PostedBy);

            switch (order_type)
            {
                case MovieOrderType.ByTitleAsc:
                    {
                        found = found.OrderBy(m => m.Title);
                        break;
                    }
                case MovieOrderType.ByTitleDesc:
                    {
                        found = found.OrderByDescending(m => m.Title);
                        break;
                    }
                case MovieOrderType.ByDateAsc:
                    {
                        found = found.OrderBy(m => m.Date);
                        break;
                    }
                case MovieOrderType.ByDateDesc:
                    {
                        found = found.OrderByDescending(m => m.Date);
                        break;
                    }
                case MovieOrderType.ByDirectorAsc:
                    {
                        found = found.OrderBy(m => m.Director);
                        break;
                    }
                case MovieOrderType.ByDirectorDesc:
                    {
                        found = found.OrderByDescending(m => m.Director);
                        break;
                    }
            }

            if (skip_records > 0)
                found = found.Skip(skip_records);

            found = found.Take(take_records);

            return found.ToArray();
        }

        /// <summary>
        /// Получить список последних фильмов.
        /// </summary>
        /// <param name="count">Число выбираемых записей.</param>
        /// <returns></returns>
        public IEnumerable<Movie> GetLastMovies(int count = 5)
        {
            return _cnt.Movies.OrderByDescending(m => m.Id).Take(count).ToArray();
        }

        /// <summary>
        /// Получить фильм по ID.
        /// </summary>
        /// <param name="id">ID фильма.</param>
        /// <returns></returns>
        public Movie GetMovie(long id)
        {
            var record = _cnt.Movies.Include(m => m.PostedBy).Where(m => m.Id == id).FirstOrDefault();

            if (record == null)
                return null;

            _cnt.Entry(record).Reference(r => r.PostedBy).Load();
            return record;

        }
        
        /// <summary>
        /// Добавить фильм.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public long AddMovie(Movie item)
        {
            bool got_same = _cnt.Movies.Any(m => m.Title == item.Title && m.Date.Year == m.Date.Year);

            if (got_same)
                throw new Exception("Такой фильм уже существует!");

            _cnt.Movies.Add(item);
            _cnt.SaveChanges();

            return item.Id;
        }

        /// <summary>
        /// Редактировать фильм.
        /// </summary>
        /// <param name="item"></param>
        public void EditMovie(Movie item)
        {
            _cnt.Entry(item).State = EntityState.Modified;
            _cnt.SaveChanges();
        }
        #endregion
    }

    /// <summary>
    /// Тип сортировки для списка фильмов.
    /// </summary>
    public enum MovieOrderType
    {
        ByTitleAsc = 0,
        ByTitleDesc = 1,
        ByDateAsc = 2,
        ByDateDesc = 3,
        ByDirectorAsc = 4,
        ByDirectorDesc = 5
    }
}