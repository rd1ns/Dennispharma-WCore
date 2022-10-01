using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Linq;
using WCore.Core;
using WCore.Core.Domain.Newses;
using WCore.Core.Infrastructure;
using WCore.Framework.Factories;
using WCore.Framework.Filters;
using WCore.Framework.Models;
using WCore.Services.Localization;
using WCore.Services.Newses;
using WCore.Services.Seo;
using WCore.Services.Settings;
using WCore.Web.Areas.Admin.Helpers;
using WCore.Web.Areas.Admin.Infrastructure.Mapper;
using WCore.Web.Areas.Admin.Models.Newses;

namespace WCore.Web.Areas.Admin.Controllers
{
    public class NewsController : BaseAdminController
    {
        #region Fields
        private readonly INewsService _newsService;
        private readonly INewsImageService _newsImageService;
        private readonly INewsCategoryService _newsCategoryService;

        private readonly IWCoreFileProvider _wCoreFileProvider;
        private readonly ILocalizationService _localizationService;
        private readonly ILocalizedEntityService _localizedEntityService;
        private readonly ILocalizedModelFactory _localizedModelFactory;
        private readonly IUrlRecordService _urlRecordService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ISettingService _settingService;
        private readonly IWebHelper _webHelper;
        private readonly IWorkContext _workContext;

        private readonly ImageHelper _imageHelper;
        #endregion

        #region Ctor
        public NewsController(INewsService newsService,
            INewsImageService newsImageService,
            INewsCategoryService newsCategoryService,
            IWCoreFileProvider wCoreFileProvider,
            ILocalizationService localizationService,
            ILocalizedEntityService localizedEntityService,
            ILocalizedModelFactory localizedModelFactory,
            IUrlRecordService urlRecordService,
            IWebHostEnvironment webHostEnvironment,
            ISettingService settingService,
            IWebHelper webHelper,
            IWorkContext workContext)
        {
            this._newsService = newsService;
            this._newsImageService = newsImageService;
            this._newsCategoryService = newsCategoryService;

            this._wCoreFileProvider = wCoreFileProvider;
            this._localizedEntityService = localizedEntityService;
            this._localizationService = localizationService;
            this._localizedModelFactory = localizedModelFactory;
            this._urlRecordService = urlRecordService;
            this._webHostEnvironment = webHostEnvironment;
            this._settingService = settingService;
            this._webHelper = webHelper;
            this._workContext = workContext;

            _imageHelper = new ImageHelper();

        }
        #endregion

        #region Utilities
        protected virtual void UpdateLocales(News entity, NewsModel model)
        {
            foreach (var localized in model.Locales)
            {
                _localizedEntityService.SaveLocalizedValue(entity,
                    x => x.Title,
                    localized.Title,
                    localized.LanguageId);

                _localizedEntityService.SaveLocalizedValue(entity,
                    x => x.Body,
                    localized.Body,
                    localized.LanguageId);


                //search engine name
                var seName = _urlRecordService.ValidateSeName(entity, localized.SeName, localized.Title, false);
                _urlRecordService.SaveSlug(entity, seName, localized.LanguageId);
            }
        }
        protected virtual void UpdateImageLocales(NewsImage entity, NewsImageModel model)
        {
            foreach (var localized in model.Locales)
            {
                _localizedEntityService.SaveLocalizedValue(entity,
                    x => x.Title,
                    localized.Title,
                    localized.LanguageId);

                _localizedEntityService.SaveLocalizedValue(entity,
                    x => x.Slogan,
                    localized.Slogan,
                    localized.LanguageId);

                _localizedEntityService.SaveLocalizedValue(entity,
                    x => x.Description,
                    localized.Description,
                    localized.LanguageId);
            }
        }
        #endregion

        #region Methods
        public IActionResult Index()
        {
            return Redirect("/Admin/News/List");
        }

        public IActionResult List()
        {
            var searchModel = new NewsSearchModel();
            return View(searchModel);
        }

        [HttpPost]
        public JsonResult GetFilteredItems(NewsSearchModel searchModel)
        {
            var news = _newsService.GetAllByFilters(
                searchModel.NewsCategoryId,
                searchModel.Title,
                searchModel.IsArchived,
                searchModel.IsActive,
                searchModel.Deleted,
                searchModel.ShowOn,
                searchModel.ShowOnHome,
                searchModel.StartDate,
                searchModel.EndDate,
                searchModel.skip,
                searchModel.take);

            var model = new NewsListModel().PrepareToGrid(searchModel, news, () =>
            {
                return news.Select(news =>
                {
                    var m = news.ToModel<NewsModel>();
                    m.NewsCategory = _newsCategoryService.GetById(news.NewsCategoryId).ToModel<NewsCategoryModel>();
                    return m;
                });
            });
            return Json(model);
        }

        public IActionResult AddOrEdit(int Id)
        {
            var entity = _newsService.GetById(Id);

            if (entity == null)
                entity = new News();

            var model = entity.ToModel<NewsModel>();

            Action<NewsLocalizedModel, int> localizedModelConfiguration = null;

            if (model.Id != 0)
            {
                model.SeName = _urlRecordService.GetSeName(entity, 0, true, false);
                localizedModelConfiguration = (locale, languageId) =>
                {
                    locale.Title = _localizationService.GetLocalized(entity, e => e.Title, languageId, false, false);
                    locale.Body = _localizationService.GetLocalized(entity, e => e.Body, languageId, false, false);
                    locale.SeName = _urlRecordService.GetSeName(entity, languageId, false, false);
                };
            }

            model.Locales = _localizedModelFactory.PrepareLocalizedModels(localizedModelConfiguration);

            return View(model);
        }

        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public async System.Threading.Tasks.Task<IActionResult> AddOrEditAsync(NewsModel model, bool continueEditing, bool delete)
        {
            var entity = model.ToEntity<News>();

            #region Delete
            if (delete)
            {
                var _entity = _newsService.GetById(model.Id);
                _entity.Deleted = true;
                _entity.IsActive = false;
                _newsService.Update(_entity);
                return Json("Deleted");
            }
            #endregion

            #region Image && Banner
            var uploads = Path.Combine(_webHostEnvironment.WebRootPath, "uploads/news");
            var news = _newsService.GetById(entity.Id);
            var imagePicture = Request.Form.Files["Image"];
            if (imagePicture != null && imagePicture.Length > 0)
            {
                var filePath = Path.Combine(uploads, imagePicture.FileName);
                using var fileStream = new FileStream(filePath, FileMode.OpenOrCreate);
                await imagePicture.CopyToAsync(fileStream);
                entity.Image = "/uploads/news/" + imagePicture.FileName;

            }
            else
            {
                entity.Image = news?.Image;
            }
            var bannerPicture = Request.Form.Files["Banner"];
            if (bannerPicture != null && bannerPicture.Length > 0)
            {
                var filePath = Path.Combine(uploads, bannerPicture.FileName);
                using var fileStream = new FileStream(filePath, FileMode.OpenOrCreate);
                await bannerPicture.CopyToAsync(fileStream);
                entity.Banner = "/uploads/news/" + bannerPicture.FileName;

            }
            else
            {
                entity.Banner = news?.Banner;
            }
            #endregion

            #region Add Or Update

            if (model.Id == 0)
            {
                _newsService.Insert(entity);
            }
            else
            {
                _newsService.Update(entity);
            }
            //search engine name
            model.SeName = _urlRecordService.ValidateSeName(entity, model.SeName, entity.Title, true);
            _urlRecordService.SaveSlug(entity, model.SeName, 0);

            //locales
            UpdateLocales(entity, model);
            #endregion

            return Json(continueEditing);
        }
        #endregion

        #region News Image
        [HttpPost]
        public JsonResult GetNewsImageFilteredItems(NewsImageSearchModel searchModel)
        {
            var newsImages = _newsImageService.GetAllByFilters(
                searchModel.NewsId,
                searchModel.skip,
                searchModel.take);

            var model = new NewsImageListModel().PrepareToGrid(searchModel, newsImages, () =>
            {
                return newsImages.Select(newsImage =>
                {
                    var m = newsImage.ToModel<NewsImageModel>();
                    return m;
                });
            });
            return Json(model);
        }

        public IActionResult NewsImage(int Id, int NewsId)
        {
            var entity = _newsImageService.GetById(Id);

            if (entity == null)
                entity = new NewsImage();

            var model = entity.ToModel<NewsImageModel>();

            Action<NewsImageLocalizedModel, int> localizedModelConfiguration = null;

            if (model.Id != 0)
            {
                localizedModelConfiguration = (locale, languageId) =>
                {
                    locale.Title = _localizationService.GetLocalized(entity, e => e.Title, languageId, false, false);
                    locale.Slogan = _localizationService.GetLocalized(entity, e => e.Slogan, languageId, false, false);
                    locale.Description = _localizationService.GetLocalized(entity, e => e.Description, languageId, false, false);
                };
            }

            model.Locales = _localizedModelFactory.PrepareLocalizedModels(localizedModelConfiguration);

            return View(model);
        }
        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public async System.Threading.Tasks.Task<IActionResult> NewsImage(NewsImageModel model, bool continueEditing, bool delete)
        {
            var entity = model.ToEntity<NewsImage>();

            #region Delete

            if (delete)
            {
                var _entity = _newsImageService.GetById(model.Id);
                _newsImageService.Delete(_entity.Id);

                _wCoreFileProvider.DeleteFile(string.Concat(_webHostEnvironment.WebRootPath, _entity.Original));
                _wCoreFileProvider.DeleteFile(string.Concat(_webHostEnvironment.WebRootPath, _entity.Big));
                _wCoreFileProvider.DeleteFile(string.Concat(_webHostEnvironment.WebRootPath, _entity.Medium));
                _wCoreFileProvider.DeleteFile(string.Concat(_webHostEnvironment.WebRootPath, _entity.Small));

                return Json("Deleted");
            }

            #endregion

            #region Image

            foreach (var file in Request.Form.Files)
            {
                if (file.Length > 0)
                {
                    var imageModel = await _imageHelper.ConvertImage(file, false, true, "newsImage");
                    entity.Original = imageModel.Original;
                    entity.Big = imageModel.Big;
                    entity.Medium = imageModel.Medium;
                    entity.Small = imageModel.Small;
                }
            }

            #endregion

            #region Add Or Update

            if (model.Id == 0)
            {
                _newsImageService.Insert(entity);
            }
            else
            {
                _newsImageService.Update(entity);
            }

            #endregion

            //Locales
            UpdateImageLocales(entity, model);

            return Json(entity);
        }

        #endregion
    }
}
