using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using WCore.Core;
using WCore.Core.Domain.Galleries;
using WCore.Core.Infrastructure;
using WCore.Framework.Extensions;
using WCore.Framework.Factories;
using WCore.Framework.Filters;
using WCore.Framework.Models;
using WCore.Services.Galleries;
using WCore.Services.Localization;
using WCore.Services.Seo;
using WCore.Services.Settings;
using WCore.Web.Areas.Admin.Helpers;
using WCore.Web.Areas.Admin.Infrastructure.Mapper;
using WCore.Web.Areas.Admin.Models.Galleries;

namespace WCore.Web.Areas.Admin.Controllers
{
    public class GalleryController : BaseAdminController
    {
        #region Fields
        private readonly IGalleryService _galleryService;
        private readonly IGalleryImageService _galleryImageService;
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
        public GalleryController(IGalleryService galleryService,
            IGalleryImageService galleryImageService,
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
            this._galleryService = galleryService;
            this._galleryImageService = galleryImageService;
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

        protected virtual void UpdateLocales(Gallery entity, GalleryModel model)
        {
            foreach (var localized in model.Locales)
            {
                _localizedEntityService.SaveLocalizedValue(entity,
                    x => x.Title,
                    localized.Title,
                    localized.LanguageId);

                _localizedEntityService.SaveLocalizedValue(entity,
                    x => x.ShortBody,
                    localized.ShortBody,
                    localized.LanguageId);

                _localizedEntityService.SaveLocalizedValue(entity,
                    x => x.Body,
                    localized.Body,
                    localized.LanguageId);
            }
        }
        protected virtual void UpdateLocales(GalleryImage entity, GalleryImageModel model)
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

                _localizedEntityService.SaveLocalizedValue(entity,
                    x => x.Link,
                    localized.Link,
                    localized.LanguageId);

                _localizedEntityService.SaveLocalizedValue(entity,
                    x => x.LinkText,
                    localized.LinkText,
                    localized.LanguageId);
            }
        }
        #endregion

        #region Methods
        public IActionResult Index()
        {
            return Redirect("/Admin/Gallery/List");
        }

        public IActionResult List()
        {
            var searchModel = new GallerySearchModel();
            return View(searchModel);
        }

        [HttpPost]
        public JsonResult GetFilteredItems(GallerySearchModel searchModel)
        {
            var activities = _galleryService.GetAllByFilters(searchModel.GalleryType, searchModel.IsActive, searchModel.Deleted, searchModel.ShowOn, searchModel.skip, searchModel.take);

            var model = new GalleryListModel().PrepareToGrid(searchModel, activities, () =>
            {
                return activities.Select(gallery =>
                {
                    var m = gallery.ToModel<GalleryModel>();
                    m.GalleryTypeName = m.GalleryType.GetLocalizedEnum(_localizationService, _workContext.WorkingLanguage.Id);
                    return m;
                });
            });
            return Json(model);
        }

        public IActionResult AddOrEdit(int Id)
        {
            var entity = _galleryService.GetById(Id);

            if (entity == null)
                entity = new Gallery();

            var model = entity.ToModel<GalleryModel>();

            Action<GalleryLocalizedModel, int> localizedModelConfiguration = null;

            if (model.Id != 0)
            {
                localizedModelConfiguration = (locale, languageId) =>
                {
                    locale.Title = _localizationService.GetLocalized(entity, e => e.Title, languageId, false, false);
                    locale.Body = _localizationService.GetLocalized(entity, e => e.Body, languageId, false, false);
                    locale.ShortBody = _localizationService.GetLocalized(entity, e => e.ShortBody, languageId, false, false);
                    locale.Image = _localizationService.GetLocalized(entity, e => e.Image, languageId, false, false);
                };
            }

            model.Locales = _localizedModelFactory.PrepareLocalizedModels(localizedModelConfiguration);

            model.GalleryTypes = model.GalleryType.ToSelectList().ToList();

            return View(model);
        }

        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public async System.Threading.Tasks.Task<IActionResult> AddOrEdit(GalleryModel model, bool continueEditing, bool delete)
        {
            var entity = model.ToEntity<Gallery>();

            #region Delete
            if (delete)
            {
                var _entity = _galleryService.GetById(model.Id);
                _entity.Deleted = true;
                _entity.IsActive = false;
                _galleryService.Update(_entity);
                return Json("Deleted");
            }
            #endregion

            #region Image
            foreach (var file in Request.Form.Files)
            {
                if (file.Length > 0)
                {
                    var imageModel = await _imageHelper.ConvertImage(file, false, false, "gallery");
                    entity.Image = imageModel.Original;
                }
            }

            if (!Request.Form.Files.Any() && entity.Id != 0)
            {
                var u = _galleryService.GetById(entity.Id);
                entity.Image = u.Image;
            }
            #endregion

            #region Add Or Update

            if (model.Id == 0)
            {
                _galleryService.Insert(entity);
            }
            else
            {
                _galleryService.Update(entity);
            }

            //locales
            UpdateLocales(entity, model);
            #endregion

            return Json(entity);
        }
        #endregion

        #region Gallery Image Methods
        [HttpPost]
        public JsonResult GetGalleryImageFilteredItems(GalleryImageSearchModel searchModel)
        {
            var galleryImages = _galleryImageService.GetAllByFilters(searchModel.GalleryId, searchModel.skip, searchModel.take);

            var model = new GalleryImageListModel().PrepareToGrid(searchModel, galleryImages, () =>
            {
                return galleryImages.Select(galleryImage =>
                {
                    var m = galleryImage.ToModel<GalleryImageModel>();
                    return m;
                });
            });
            return Json(model);
        }


        public IActionResult GalleryImage(int Id)
        {
            var entity = _galleryImageService.GetById(Id);

            if (entity == null)
                entity = new GalleryImage();

            var model = entity.ToModel<GalleryImageModel>();

            Action<GalleryImageLocalizedModel, int> localizedModelConfiguration = null;

            if (model.Id != 0)
            {
                localizedModelConfiguration = (locale, languageId) =>
                {
                    locale.Title = _localizationService.GetLocalized(entity, e => e.Title, languageId, false, false);
                    locale.Slogan = _localizationService.GetLocalized(entity, e => e.Slogan, languageId, false, false);
                    locale.Description = _localizationService.GetLocalized(entity, e => e.Description, languageId, false, false);
                    locale.Link = _localizationService.GetLocalized(entity, e => e.Link, languageId, false, false);
                    locale.LinkText = _localizationService.GetLocalized(entity, e => e.LinkText, languageId, false, false);
                };
            }

            model.Locales = _localizedModelFactory.PrepareLocalizedModels(localizedModelConfiguration);
            return View(model);
        }

        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public async System.Threading.Tasks.Task<IActionResult> GalleryImage(GalleryImageModel model, bool continueEditing, bool delete)
        {
            var entity = model.ToEntity<GalleryImage>();

            #region Delete
            if (delete)
            {
                var _entity = _galleryImageService.GetById(model.Id);
                _galleryImageService.Delete(_entity.Id);

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
                    var imageModel = await _imageHelper.ConvertImage(file,false, true, "galleryimage");
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
                _galleryImageService.Insert(entity);
            }
            else
            {
                _galleryImageService.Update(entity);
            }

            #endregion

            //locales
            UpdateLocales(entity, model);

            return Json(entity);
        }
        #endregion
    }
}
