using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Linq;
using WCore.Core;
using WCore.Core.Domain.Congresses;
using WCore.Core.Infrastructure;
using WCore.Framework.Factories;
using WCore.Framework.Filters;
using WCore.Framework.Models;
using WCore.Services.Congresses;
using WCore.Services.Localization;
using WCore.Services.Seo;
using WCore.Services.Settings;
using WCore.Web.Areas.Admin.Helpers;
using WCore.Web.Areas.Admin.Infrastructure.Mapper;
using WCore.Web.Areas.Admin.Models.Congresses;

namespace WCore.Web.Areas.Admin.Controllers
{
    public class CongressController : BaseAdminController
    {
        #region Fields
        private readonly ICongressService _congressService;
        private readonly ICongressImageService _congressImageService;
        private readonly ICongressPaperService _congressPaperService;
        private readonly ICongressPaperTypeService _congressPaperTypeService;
        private readonly ICongressPresentationService _congressPresentationService;
        private readonly ICongressPresentationTypeService _congressPresentationTypeService;

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
        public CongressController(ICongressService congressService,
            ICongressImageService congressImageService,
            ICongressPaperService congressPaperService,
            ICongressPaperTypeService congressPaperTypeService,
            ICongressPresentationService congressPresentationService,
            ICongressPresentationTypeService congressPresentationTypeService,
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
            this._congressService = congressService;
            this._congressImageService = congressImageService;
            this._congressPaperService = congressPaperService;
            this._congressPaperTypeService = congressPaperTypeService;
            this._congressPresentationService = congressPresentationService;
            this._congressPresentationTypeService = congressPresentationTypeService;

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
        protected virtual void UpdateLocales(Congress entity, CongressModel model)
        {
            foreach (var localized in model.Locales)
            {
                _localizedEntityService.SaveLocalizedValue(entity,
                    x => x.Title,
                    localized.Title,
                    localized.LanguageId);

                _localizedEntityService.SaveLocalizedValue(entity,
                    x => x.MiniTitle,
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

                _localizedEntityService.SaveLocalizedValue(entity,
                    x => x.AwardWinningWorks,
                    localized.AwardWinningWorks,
                    localized.LanguageId);


                //search engine name
                var seName = _urlRecordService.ValidateSeName(entity, localized.SeName, localized.Title, false);
                _urlRecordService.SaveSlug(entity, seName, localized.LanguageId);
            }
        }
        protected virtual void UpdatePresentationTypeLocales(CongressPresentationType entity, CongressPresentationTypeModel model)
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
            }
        }
        protected virtual void UpdatePresentationLocales(CongressPresentation entity, CongressPresentationModel model)
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
            }
        }
        protected virtual void UpdatePaperTypeLocales(CongressPaperType entity, CongressPaperTypeModel model)
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
            }
        }
        protected virtual void UpdatePaperLocales(CongressPaper entity, CongressPaperModel model)
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
            }
        }
        protected virtual void UpdateImageLocales(CongressImage entity, CongressImageModel model)
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
            return Redirect("/Admin/Congress/List");
        }

        public IActionResult List()
        {
            var searchModel = new CongressSearchModel();
            return View(searchModel);
        }

        [HttpPost]
        public JsonResult GetFilteredItems(CongressSearchModel searchModel)
        {
            var activities = _congressService.GetAllByFilters(
                searchModel.Title,
                searchModel.IsArchived,
                searchModel.IsActive,
                searchModel.Deleted,
                searchModel.ShowOn,
                searchModel.StartDate,
                searchModel.EndDate,
                searchModel.skip,
                searchModel.take);

            var model = new CongressListModel().PrepareToGrid(searchModel, activities, () =>
            {
                return activities.Select(congress =>
                {
                    var m = congress.ToModel<CongressModel>();
                    return m;
                });
            });
            return Json(model);
        }

        public IActionResult AddOrEdit(int Id)
        {
            var entity = _congressService.GetById(Id);

            if (entity == null)
                entity = new Congress();

            var model = entity.ToModel<CongressModel>();

            Action<CongressLocalizedModel, int> localizedModelConfiguration = null;

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
        public async System.Threading.Tasks.Task<IActionResult> AddOrEditAsync(CongressModel model, bool continueEditing, bool delete)
        {
            var entity = model.ToEntity<Congress>();

            #region Delete
            if (delete)
            {
                var _entity = _congressService.GetById(model.Id);
                _entity.Deleted = true;
                _entity.IsActive = false;
                _congressService.Update(_entity);
                return Json("Deleted");
            }
            #endregion

            #region Image && Banner
            var uploads = Path.Combine(_webHostEnvironment.WebRootPath, "uploads/congress");
            var congress = _congressService.GetById(entity.Id);
            var imagePicture = Request.Form.Files["Image"];
            if (imagePicture != null && imagePicture.Length > 0)
            {
                var filePath = Path.Combine(uploads, imagePicture.FileName);
                using var fileStream = new FileStream(filePath, FileMode.OpenOrCreate);
                await imagePicture.CopyToAsync(fileStream);
                entity.Image = "/uploads/congress/" + imagePicture.FileName;

            }
            else
            {
                entity.Image = congress?.Image;
            }
            var bannerPicture = Request.Form.Files["Banner"];
            if (bannerPicture != null && bannerPicture.Length > 0)
            {
                var filePath = Path.Combine(uploads, bannerPicture.FileName);
                using var fileStream = new FileStream(filePath, FileMode.OpenOrCreate);
                await bannerPicture.CopyToAsync(fileStream);
                entity.Banner = "/uploads/congress/" + bannerPicture.FileName;

            }
            else
            {
                entity.Banner = congress?.Banner;
            }
            #endregion

            #region Add Or Update

            if (model.Id == 0)
            {
                _congressService.Insert(entity);
            }
            else
            {
                _congressService.Update(entity);
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

        #region Congress Presentation Type
        [HttpPost]
        public JsonResult GetCongressPresentationTypeFilteredItems(CongressPresentationTypeSearchModel searchModel)
        {
            var congressPresentationTypes = _congressPresentationTypeService.GetAllByFilters(
                searchModel.CongressId,
                searchModel.Title,
                searchModel.IsActive,
                searchModel.Deleted,
                searchModel.ShowOn,
                searchModel.skip,
                searchModel.take);

            var model = new CongressPresentationTypeListModel().PrepareToGrid(searchModel, congressPresentationTypes, () =>
            {
                return congressPresentationTypes.Select(congressPresentationType =>
                {
                    var m = congressPresentationType.ToModel<CongressPresentationTypeModel>();
                    return m;
                });
            });
            return Json(model);
        }

        public IActionResult PresentationType(int Id,int CongressId)
        {
            var entity = _congressPresentationTypeService.GetById(Id);

            if (entity == null)
            {
                entity = new CongressPresentationType();
                entity.CongressId = CongressId;
            }

            var model = entity.ToModel<CongressPresentationTypeModel>();

            Action<CongressPresentationTypeLocalizedModel, int> localizedModelConfiguration = null;

            if (model.Id != 0)
            {
                localizedModelConfiguration = (locale, languageId) =>
                {
                    locale.Title = _localizationService.GetLocalized(entity, e => e.Title, languageId, false, false);
                    locale.Body = _localizationService.GetLocalized(entity, e => e.Body, languageId, false, false);
                };
            }

            model.Congress = _congressService.GetById(entity.CongressId).ToModel<CongressModel>();
            model.Locales = _localizedModelFactory.PrepareLocalizedModels(localizedModelConfiguration);

            return View(model);
        }

        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public IActionResult PresentationType(CongressPresentationTypeModel model, bool continueEditing, bool delete)
        {
            var entity = model.ToEntity<CongressPresentationType>();

            #region Delete
            if (delete)
            {
                var _entity = _congressPresentationTypeService.GetById(model.Id);
                _entity.Deleted = true;
                _entity.IsActive = false;
                _congressPresentationTypeService.Update(_entity);
                return Json("Deleted");
            }
            #endregion            

            #region Add Or Update

            if (model.Id == 0)
            {
                _congressPresentationTypeService.Insert(entity);
            }
            else
            {
                _congressPresentationTypeService.Update(entity);
            }

            //locales
            UpdatePresentationTypeLocales(entity, model);
            #endregion

            return Json(entity);
        }

        #endregion

        #region Congress Presentation

        [HttpPost]
        public JsonResult GetCongressPresentationFilteredItems(CongressPresentationSearchModel searchModel)
        {
            var congressPresentations = _congressPresentationService.GetAllByFilters(
                searchModel.CongressPresentationTypeId,
                searchModel.CongressId,
                searchModel.Title,
                searchModel.Code,
                searchModel.OwnersName,
                searchModel.OwnersSurname,
                searchModel.IsActive,
                searchModel.Deleted,
                searchModel.ShowOn,
                searchModel.skip,
                searchModel.take);

            var model = new CongressPresentationListModel().PrepareToGrid(searchModel, congressPresentations, () =>
            {
                return congressPresentations.Select(congressPresentation =>
                {
                    var m = congressPresentation.ToModel<CongressPresentationModel>();
                    return m;
                });
            });
            return Json(model);
        }

        public IActionResult Presentation(int Id)
        {
            var entity = _congressPaperService.GetById(Id);

            if (entity == null)
                entity = new CongressPaper();

            var model = entity.ToModel<CongressPresentationModel>();

            Action<CongressPresentationLocalizedModel, int> localizedModelConfiguration = null;

            if (model.Id != 0)
            {
                localizedModelConfiguration = (locale, languageId) =>
                {
                    locale.Title = _localizationService.GetLocalized(entity, e => e.Title, languageId, false, false);
                    locale.Body = _localizationService.GetLocalized(entity, e => e.Body, languageId, false, false);
                };
            }

            model.Locales = _localizedModelFactory.PrepareLocalizedModels(localizedModelConfiguration);

            return View(model);
        }

        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public IActionResult Presentation(CongressPresentationModel model, bool continueEditing, bool delete)
        {
            var entity = model.ToEntity<CongressPresentation>();

            #region Delete
            if (delete)
            {
                var _entity = _congressPresentationService.GetById(model.Id);
                _entity.Deleted = true;
                _entity.IsActive = false;
                _congressPresentationService.Update(_entity);
                return Json("Deleted");
            }
            #endregion            

            #region Add Or Update

            if (model.Id == 0)
            {
                _congressPresentationService.Insert(entity);
            }
            else
            {
                _congressPresentationService.Update(entity);
            }

            //locales
            UpdatePresentationLocales(entity, model);
            #endregion

            return Json(entity);
        }
        #endregion

        #region Congress Paper Type
        [HttpPost]
        public JsonResult GetCongressPaperTypeFilteredItems(CongressPaperTypeSearchModel searchModel)
        {
            var congressPaperTypes = _congressPaperTypeService.GetAllByFilters(
                searchModel.CongressId,
                searchModel.Title,
                searchModel.IsActive,
                searchModel.Deleted,
                searchModel.ShowOn,
                searchModel.skip,
                searchModel.take);

            var model = new CongressPaperTypeListModel().PrepareToGrid(searchModel, congressPaperTypes, () =>
            {
                return congressPaperTypes.Select(congressPaperType =>
                {
                    var m = congressPaperType.ToModel<CongressPaperTypeModel>();
                    return m;
                });
            });
            return Json(model);
        }

        public IActionResult PaperType(int Id, int CongressId)
        {
            var entity = _congressPaperTypeService.GetById(Id);

            if (entity == null)
                entity = new CongressPaperType();

            var model = entity.ToModel<CongressPaperTypeModel>();

            Action<CongressPaperTypeLocalizedModel, int> localizedModelConfiguration = null;

            if (model.Id != 0)
            {
                localizedModelConfiguration = (locale, languageId) =>
                {
                    locale.Title = _localizationService.GetLocalized(entity, e => e.Title, languageId, false, false);
                    locale.Body = _localizationService.GetLocalized(entity, e => e.Body, languageId, false, false);
                };
            }

            model.Congress = _congressService.GetById(entity.CongressId).ToModel<CongressModel>();
            model.Locales = _localizedModelFactory.PrepareLocalizedModels(localizedModelConfiguration);

            return View(model);
        }

        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public IActionResult PaperType(CongressPaperTypeModel model, bool continueEditing, bool delete)
        {
            var entity = model.ToEntity<CongressPaperType>();

            #region Delete
            if (delete)
            {
                var _entity = _congressPaperTypeService.GetById(model.Id);
                _entity.Deleted = true;
                _entity.IsActive = false;
                _congressPaperTypeService.Update(_entity);
                return Json("Deleted");
            }
            #endregion            

            #region Add Or Update

            if (model.Id == 0)
            {
                _congressPaperTypeService.Insert(entity);
            }
            else
            {
                _congressPaperTypeService.Update(entity);
            }

            //locales
            UpdatePaperTypeLocales(entity, model);
            #endregion

            return Json(entity);
        }

        #endregion

        #region Congress Paper

        [HttpPost]
        public JsonResult GetCongressPaperFilteredItems(CongressPaperSearchModel searchModel)
        {
            var congressPapers = _congressPaperService.GetAllByFilters(
                searchModel.CongressPaperTypeId,
                searchModel.CongressId,
                searchModel.Title,
                searchModel.Code,
                searchModel.OwnersName,
                searchModel.OwnersSurname,
                searchModel.IsActive,
                searchModel.Deleted,
                searchModel.ShowOn,
                searchModel.skip,
                searchModel.take);

            var model = new CongressPaperListModel().PrepareToGrid(searchModel, congressPapers, () =>
            {
                return congressPapers.Select(congressPaper =>
                {
                    var m = congressPaper.ToModel<CongressPaperModel>();
                    return m;
                });
            });
            return Json(model);
        }

        public IActionResult Paper(int Id)
        {
            var entity = _congressPaperService.GetById(Id);

            if (entity == null)
                entity = new CongressPaper();

            var model = entity.ToModel<CongressPaperModel>();

            Action<CongressPaperLocalizedModel, int> localizedModelConfiguration = null;

            if (model.Id != 0)
            {
                localizedModelConfiguration = (locale, languageId) =>
                {
                    locale.Title = _localizationService.GetLocalized(entity, e => e.Title, languageId, false, false);
                    locale.Body = _localizationService.GetLocalized(entity, e => e.Body, languageId, false, false);
                };
            }

            model.Locales = _localizedModelFactory.PrepareLocalizedModels(localizedModelConfiguration);

            return View(model);
        }

        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public IActionResult Paper(CongressPaperModel model, bool continueEditing, bool delete)
        {
            var entity = model.ToEntity<CongressPaper>();

            #region Delete
            if (delete)
            {
                var _entity = _congressPaperService.GetById(model.Id);
                _entity.Deleted = true;
                _entity.IsActive = false;
                _congressPaperService.Update(_entity);
                return Json("Deleted");
            }
            #endregion            

            #region Add Or Update

            if (model.Id == 0)
            {
                _congressPaperService.Insert(entity);
            }
            else
            {
                _congressPaperService.Update(entity);
            }

            //Locales
            UpdatePaperLocales(entity, model);
            #endregion

            return Json(entity);
        }

        #endregion

        #region Congress Image
        [HttpPost]
        public JsonResult GetCongressImageFilteredItems(CongressImageSearchModel searchModel)
        {
            var congressImages = _congressImageService.GetAllByFilters(
                searchModel.CongressId,
                searchModel.skip,
                searchModel.take);

            var model = new CongressImageListModel().PrepareToGrid(searchModel, congressImages, () =>
            {
                return congressImages.Select(congressImage =>
                {
                    var m = congressImage.ToModel<CongressImageModel>();
                    return m;
                });
            });
            return Json(model);
        }

        public IActionResult CongressImage(int Id, int CongressId)
        {
            var entity = _congressImageService.GetById(Id);

            if (entity == null)
                entity = new CongressImage();

            var model = entity.ToModel<CongressImageModel>();

            Action<CongressImageLocalizedModel, int> localizedModelConfiguration = null;

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
        public async System.Threading.Tasks.Task<IActionResult> CongressImage(CongressImageModel model, bool continueEditing, bool delete)
        {
            var entity = model.ToEntity<CongressImage>();

            #region Delete

            if (delete)
            {
                var _entity = _congressImageService.GetById(model.Id);
                _congressImageService.Delete(_entity.Id);

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
                    var imageModel = await _imageHelper.ConvertImage(file, false, true, "congressImage");
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
                _congressImageService.Insert(entity);
            }
            else
            {
                _congressImageService.Update(entity);
            }

            #endregion

            //Locales
            UpdateImageLocales(entity, model);

            return Json(entity);
        }

        #endregion
    }
}
