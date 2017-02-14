using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CinemaStorage.Infrastructure;

namespace CinemaStorage.Controllers
{
    public class MoviesController : Controller
    {
        private MovieRepo _repo;
       
        /// <summary>
        /// Главная страница.
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            using (_repo = new MovieRepo(new Models.ApplicationDbContext()))
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
            using (_repo = new MovieRepo(new Models.ApplicationDbContext()))
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
                    MoviesList = _repo.GetMovies(MovieOrderType.ByTitleAsc, records_per_page, (page_number - 1) * records_per_page).ToArray()
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

            using (_repo = new MovieRepo(new Models.ApplicationDbContext()))
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

            using (_repo = new MovieRepo(new Models.ApplicationDbContext()))
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

            using (_repo = new MovieRepo(new Models.ApplicationDbContext()))
            {
                try
                {
                    // привязка информации о пользователе
                    string login = User.Identity.Name;
                    var user = _repo.GetUser(login);
                    model.PostedBy = user; 
                    model.UserId = user.Id;

                    // привязка загруженного изображения
                    if (PosterLocationHelper.IsFileUploaded(model.PosterPath))
                        model.PosterPath = PosterLocationHelper.MakeRelativeImagePath(model.PosterPath);
                    else
                        model.PosterPath = null;

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
        /// Загрузить выбранное изображение в папку постеров (/Content/posters)
        /// </summary>
        /// <returns></returns>
        public JsonResult UploadImageAjax()
        {
            if (System.Web.HttpContext.Current.Request.Files.AllKeys.Any())
            {
                var image = System.Web.HttpContext.Current.Request.Files["Images"];
                if (image == null)
                    return Json(false);

                if (!image.ContentType.Contains("image/"))
                    return Json(false);

                if (!PosterLocationHelper.IsFolderExists())
                    PosterLocationHelper.FolderInitialize();

                string img_path_full = PosterLocationHelper.MakeAbsoluteImagePath(image.FileName);

                try
                {
                    image.SaveAs(img_path_full);
                    return Json(true);
                }
                catch (Exception)
                {
                    return Json(false);
                }
            }

            return Json(null);
        }
    }
}
