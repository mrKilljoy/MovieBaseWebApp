using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CinemaStorage.Controllers
{
    public class MoviesController : Controller
    {
        private Infrastructure.MovieRepo _repo;
       
        /// <summary>
        /// Главная страница.
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            CheckPosterFolder();

            using (_repo = new Infrastructure.MovieRepo(new Models.ApplicationDbContext()))
            {
                var collection = _repo.GetLastMovies();

                return View(collection);
            };
        }

        /// <summary>
        /// Каталог фильмов
        /// </summary>
        /// <param name="id">Номер страницы.</param>
        /// <returns></returns>
        public ActionResult Catalog(int? id)
        {
            using (_repo = new Infrastructure.MovieRepo(new Models.ApplicationDbContext()))
            {
                int page_number = id ?? 1;
                if (page_number < 0)
                    page_number = 1;

                int records_per_page = 5;
                int total_records = _repo.GetTotalMovieCount();
                int total_pages = (int)Math.Ceiling((double)total_records / records_per_page);

                if (page_number > total_pages)
                    page_number = total_pages;

                var model = new Models.CatalogViewModel
                {   
                    PageNumber = page_number,
                    RecordsPerPage = records_per_page,
                    TotalRecords = total_records,
                    TotalPages = total_pages,
                    MoviesList = _repo.GetMovies(Infrastructure.MovieOrderType.ByTitleAsc, records_per_page, (page_number - 1) * records_per_page).ToArray()
                };

                return View(model);
            };
        }

        /// <summary>
        /// Информация о выбранном фильме.
        /// </summary>
        /// <param name="id">ID фильма.</param>
        /// <returns></returns>
        public ActionResult Details(int? id)
        {
            if (id == null)
                return View();

            using (_repo = new Infrastructure.MovieRepo(new Models.ApplicationDbContext()))
            {
                var found = _repo.GetMovie((long)id);

                return View(found);
            };
        }

        /// <summary>
        /// Редактирование фильма.
        /// </summary>
        /// <param name="id">ID фильма.</param>
        /// <returns></returns>
        [Authorize]
        public ActionResult Edit(int? id)
        {
            if (id == null)
                return View();

            using (_repo = new Infrastructure.MovieRepo(new Models.ApplicationDbContext()))
            {
                var found = _repo.GetMovie((long)id);

                return View(found);
            };
        }

        /// <summary>
        /// Отправка результатов редактирования.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        public ActionResult Edit(Models.Movie model)
        {
            if (model == null)
                return View();

            using (_repo = new Infrastructure.MovieRepo(new Models.ApplicationDbContext()))
            {
                try
                {
                    string login = User.Identity.Name;
                    var user = _repo.GetUser(login);
                    model.PostedBy = user; // привязка информации о пользователе
                    model.UserId = user.Id;

                    if (model.Id == 0)
                        _repo.AddMovie(model);
                    else
                        _repo.EditMovie(model);

                    // переход на страницу просмотра
                    return RedirectToAction("Details", new { id = model.Id } );
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Ошибка при сохранении данных: " + ex.Message);
                    return View(model);
                }
            };
        }

        /// <summary>
        /// Проверить наличие каталога для постеров.
        /// </summary>
        private void CheckPosterFolder()
        {
            string core_folder_path = HttpRuntime.AppDomainAppPath;
            string poster_folder_path = HttpContext.Server.MapPath("/Content/posters");

            bool is_folder_exists = System.IO.Directory.Exists(poster_folder_path);

            try
            {
                // если каталог отсутствует - создать его
                if (!is_folder_exists)
                    System.IO.Directory.CreateDirectory(poster_folder_path);
            }
            catch (Exception)
            {
            }
        }
    }
}
