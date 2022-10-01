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
    public class NewsCategoryController : BaseAdminController
    {
        #region Fields
        private readonly INewsCategoryService _newsCategoryService;
        private readonly INewsService _newsService;

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
        public NewsCategoryController(INewsCategoryService newsCategoryService,
            INewsService newsService,
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
            this._newsCategoryService = newsCategoryService;
            this._newsService = newsService;

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
        protected virtual void UpdateLocales(NewsCategory entity, NewsCategoryModel model)
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
        #endregion

        #region Methods
        public IActionResult Index()
        {
            return Redirect("/Admin/NewsCategory/List");
        }

        public IActionResult List()
        {
            var searchModel = new NewsCategorySearchModel();
            return View(searchModel);
        }

        [HttpPost]
        public JsonResult GetFilteredItems(NewsCategorySearchModel searchModel)
        {
            var newsCategory = _newsCategoryService.GetAllByFilters(
                searchModel.Title,
                searchModel.IsActive,
                searchModel.Deleted,
                searchModel.ShowOn,
                searchModel.skip,
                searchModel.take);

            var model = new NewsCategoryListModel().PrepareToGrid(searchModel, newsCategory, () =>
            {
                return newsCategory.Select(newsCategory =>
                {
                    var m = newsCategory.ToModel<NewsCategoryModel>();
                    return m;
                });
            });
            return Json(model);
        }

        public IActionResult AddOrEdit(int Id)
        {
            var entity = _newsCategoryService.GetById(Id);

            if (entity == null)
                entity = new NewsCategory();

            var model = entity.ToModel<NewsCategoryModel>();

            Action<NewsCategoryLocalizedModel, int> localizedModelConfiguration = null;

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
        public async System.Threading.Tasks.Task<IActionResult> AddOrEditAsync(NewsCategoryModel model, bool continueEditing, bool delete)
        {
            var entity = model.ToEntity<NewsCategory>();

            #region Delete
            if (delete)
            {
                var _entity = _newsCategoryService.GetById(model.Id);
                _entity.Deleted = true;
                _entity.IsActive = false;
                _newsCategoryService.Update(_entity);
                return Json("Deleted");
            }
            #endregion

            #region Image && Banner
            var uploads = Path.Combine(_webHostEnvironment.WebRootPath, "uploads/newsCategory");
            var newsCategory = _newsCategoryService.GetById(entity.Id);
            var imagePicture = Request.Form.Files["Image"];
            if (imagePicture != null && imagePicture.Length > 0)
            {
                var filePath = Path.Combine(uploads, imagePicture.FileName);
                using var fileStream = new FileStream(filePath, FileMode.OpenOrCreate);
                await imagePicture.CopyToAsync(fileStream);
                entity.Image = "/uploads/newsCategory/" + imagePicture.FileName;

            }
            else
            {
                entity.Image = newsCategory?.Image;
            }

            var bannerPicture = Request.Form.Files["Banner"];
            if (bannerPicture != null && bannerPicture.Length > 0)
            {
                var filePath = Path.Combine(uploads, bannerPicture.FileName);
                using var fileStream = new FileStream(filePath, FileMode.OpenOrCreate);
                await bannerPicture.CopyToAsync(fileStream);
                entity.Banner = "/uploads/newsCategory/" + bannerPicture.FileName;

            }
            else
            {
                entity.Banner = newsCategory?.Banner;
            }
            #endregion

            #region Add Or Update

            if (model.Id == 0)
            {
                _newsCategoryService.Insert(entity);
            }
            else
            {
                _newsCategoryService.Update(entity);
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
    }
}
