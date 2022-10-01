using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Linq;
using WCore.Core;
using WCore.Core.Domain.Academies;
using WCore.Core.Domain.Newses;
using WCore.Core.Infrastructure;
using WCore.Framework.Extensions;
using WCore.Framework.Factories;
using WCore.Framework.Filters;
using WCore.Framework.Models;
using WCore.Services.Academies;
using WCore.Services.Localization;
using WCore.Services.Newses;
using WCore.Services.Seo;
using WCore.Services.Settings;
using WCore.Web.Areas.Admin.Helpers;
using WCore.Web.Areas.Admin.Infrastructure.Mapper;
using WCore.Web.Areas.Admin.Models.Academies;
using WCore.Web.Areas.Admin.Models.Newses;

namespace WCore.Web.Areas.Admin.Controllers
{
    public class AcademyController : BaseAdminController
    {
        #region Fields
        private readonly IAcademyService _academyService;
        private readonly IAcademyImageService _academyImageService;
        private readonly IAcademyFileService _academyFileService;
        private readonly IAcademyVideoService _academyVideoService;
        private readonly IAcademyCategoryService _academyCategoryService;

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
        public AcademyController(IAcademyService academyService,
            IAcademyImageService academyImageService,
            IAcademyFileService academyFileService,
            IAcademyVideoService academyVideoService,
            IAcademyCategoryService academyCategoryService,
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
            this._academyService = academyService;
            this._academyImageService = academyImageService;
            this._academyFileService= academyFileService;
            this._academyVideoService= academyVideoService;
            this._academyCategoryService = academyCategoryService;

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
        protected virtual void UpdateLocales(Academy entity, AcademyModel model)
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
        protected virtual void UpdateImageLocales(AcademyImage entity, AcademyImageModel model)
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
            return Redirect("/Admin/Academy/List");
        }

        public IActionResult List()
        {
            var searchModel = new AcademySearchModel();
            return View(searchModel);
        }

        [HttpPost]
        public JsonResult GetFilteredItems(AcademySearchModel searchModel)
        {
            var academy = _academyService.GetAllByFilters(
                searchModel.AcademyCategoryId,
                searchModel.Title,
                searchModel.IsArchived,
                searchModel.IsActive,
                searchModel.Deleted,
                searchModel.ShowOn,
                searchModel.skip,
                searchModel.take);

            var model = new AcademyListModel().PrepareToGrid(searchModel, academy, () =>
            {
                return academy.Select(academy =>
                {
                    var m = academy.ToModel<AcademyModel>();
                    m.AcademyCategory = _academyCategoryService.GetById(academy.AcademyCategoryId).ToModel<AcademyCategoryModel>();
                    return m;
                });
            });
            return Json(model);
        }

        public IActionResult AddOrEdit(int Id)
        {
            var entity = _academyService.GetById(Id);

            if (entity == null)
                entity = new Academy();

            var model = entity.ToModel<AcademyModel>();

            Action<AcademyLocalizedModel, int> localizedModelConfiguration = null;

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
        public async System.Threading.Tasks.Task<IActionResult> AddOrEditAsync(AcademyModel model, bool continueEditing, bool delete)
        {
            var entity = model.ToEntity<Academy>();

            #region Delete
            if (delete)
            {
                var _entity = _academyService.GetById(model.Id);
                _entity.Deleted = true;
                _entity.IsActive = false;
                _academyService.Update(_entity);
                return Json("Deleted");
            }
            #endregion

            #region Image && Banner
            var uploads = Path.Combine(_webHostEnvironment.WebRootPath, "uploads/academy");
            var academy = _academyService.GetById(entity.Id);
            var imagePicture = Request.Form.Files["Image"];
            if (imagePicture != null && imagePicture.Length > 0)
            {
                var filePath = Path.Combine(uploads, imagePicture.FileName);
                using var fileStream = new FileStream(filePath, FileMode.OpenOrCreate);
                await imagePicture.CopyToAsync(fileStream);
                entity.Image = "/uploads/academy/" + imagePicture.FileName;

            }
            else
            {
                entity.Image = academy?.Image;
            }
            var bannerPicture = Request.Form.Files["Banner"];
            if (bannerPicture != null && bannerPicture.Length > 0)
            {
                var filePath = Path.Combine(uploads, bannerPicture.FileName);
                using var fileStream = new FileStream(filePath, FileMode.OpenOrCreate);
                await bannerPicture.CopyToAsync(fileStream);
                entity.Banner = "/uploads/academy/" + bannerPicture.FileName;

            }
            else
            {
                entity.Banner = academy?.Banner;
            }
            #endregion

            #region Add Or Update

            if (model.Id == 0)
            {
                _academyService.Insert(entity);
            }
            else
            {
                _academyService.Update(entity);
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

        #region Academy Image
        [HttpPost]
        public JsonResult GetAcademyImageFilteredItems(AcademyImageSearchModel searchModel)
        {
            var academyImages = _academyImageService.GetAllByFilters(
                searchModel.AcademyId,
                searchModel.skip,
                searchModel.take);

            var model = new AcademyImageListModel().PrepareToGrid(searchModel, academyImages, () =>
            {
                return academyImages.Select(academyImage =>
                {
                    var m = academyImage.ToModel<AcademyImageModel>();
                    return m;
                });
            });
            return Json(model);
        }

        public IActionResult AcademyImage(int Id, int AcademyId)
        {
            var entity = _academyImageService.GetById(Id);

            if (entity == null)
                entity = new AcademyImage();

            var model = entity.ToModel<AcademyImageModel>();

            Action<AcademyImageLocalizedModel, int> localizedModelConfiguration = null;

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
        public async System.Threading.Tasks.Task<IActionResult> AcademyImage(AcademyImageModel model, bool continueEditing, bool delete)
        {
            var entity = model.ToEntity<AcademyImage>();

            #region Delete

            if (delete)
            {
                var _entity = _academyImageService.GetById(model.Id);
                _academyImageService.Delete(_entity.Id);

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
                    var imageModel = await _imageHelper.ConvertImage(file, false, true, "academy");
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
                _academyImageService.Insert(entity);
            }
            else
            {
                _academyImageService.Update(entity);
            }

            #endregion

            //Locales
            UpdateImageLocales(entity, model);

            return Json(entity);
        }

        #endregion

        #region Academy File
        [HttpPost]
        public JsonResult GetAcademyFileFilteredItems(AcademyFileSearchModel searchModel)
        {
            var academyFiles = _academyFileService.GetAllByFilters(
                searchModel.AcademyId,
                searchModel.skip,
                searchModel.take);

            var model = new AcademyFileListModel().PrepareToGrid(searchModel, academyFiles, () =>
            {
                return academyFiles.Select(academyFile =>
                {
                    var m = academyFile.ToModel<AcademyFileModel>();
                    return m;
                });
            });
            return Json(model);
        }

        public IActionResult AcademyFile(int Id, int AcademyId)
        {
            var entity = _academyFileService.GetById(Id);

            if (entity == null)
                entity = new AcademyFile();

            var model = entity.ToModel<AcademyFileModel>();

            return View(model);
        }
        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public async System.Threading.Tasks.Task<IActionResult> AcademyFile(AcademyFileModel model, bool continueEditing, bool delete)
        {
            var entity = model.ToEntity<AcademyFile>();

            #region Delete

            if (delete)
            {
                var _entity = _academyFileService.GetById(model.Id);
                _academyFileService.Delete(_entity.Id);

                _wCoreFileProvider.DeleteFile(string.Concat(_webHostEnvironment.WebRootPath, _entity.Path));

                return Json("Deleted");
            }

            #endregion

            #region File
            var uploads = Path.Combine(_webHostEnvironment.WebRootPath, "uploads/academy/files");

            foreach (var file in Request.Form.Files)
            {
                if (file.Length > 0)
                {
                    var filePath = Path.Combine(uploads, file.FileName);
                    using var fileStream = new FileStream(filePath, FileMode.OpenOrCreate);
                    await file.CopyToAsync(fileStream);
                    entity.Path = "/uploads/academy/files/" + file.FileName;
                }
            }

            if (!Request.Form.Files.Any() && entity.Id != 0)
            {
                var u = _academyFileService.GetById(entity.Id);
                entity.Path = u.Path;
            }
            #endregion

            #region Add Or Update

            if (model.Id == 0)
            {
                _academyFileService.Insert(entity);
            }
            else
            {
                _academyFileService.Update(entity);
            }

            #endregion

            return Json(entity);
        }

        #endregion

        #region Academy Video
        [HttpPost]
        public JsonResult GetAcademyVideoFilteredItems(AcademyVideoSearchModel searchModel)
        {
            var academyVideos = _academyVideoService.GetAllByFilters(
                searchModel.AcademyId,
                searchModel.AcademyVideoResource,
                searchModel.skip,
                searchModel.take);

            var model = new AcademyVideoListModel().PrepareToGrid(searchModel, academyVideos, () =>
            {
                return academyVideos.Select(academyVideo =>
                {
                    var m = academyVideo.ToModel<AcademyVideoModel>();
                    m.AcademyVideoResourceName = m.AcademyVideoResource.GetLocalizedEnum(_localizationService, _workContext.WorkingLanguage.Id);
                    return m;
                });
            });
            return Json(model);
        }

        public IActionResult AcademyVideo(int Id, int AcademyId)
        {
            var entity = _academyVideoService.GetById(Id);

            if (entity == null)
                entity = new AcademyVideo();

            var model = entity.ToModel<AcademyVideoModel>();

            model.AcademyVideoResources = model.AcademyVideoResource.ToSelectList().ToList();

            return View(model);
        }
        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public async System.Threading.Tasks.Task<IActionResult> AcademyVideo(AcademyVideoModel model, bool continueEditing, bool delete)
        {
            var entity = model.ToEntity<AcademyVideo>();

            #region Delete

            if (delete)
            {
                var _entity = _academyVideoService.GetById(model.Id);
                _academyVideoService.Delete(_entity.Id);

                _wCoreFileProvider.DeleteFile(string.Concat(_webHostEnvironment.WebRootPath, _entity.Path));

                return Json("Deleted");
            }

            #endregion

            #region Video
            var uploads = Path.Combine(_webHostEnvironment.WebRootPath, "uploads/academy/videos");

            foreach (var file in Request.Form.Files)
            {
                if (file.Length > 0)
                {
                    var filePath = Path.Combine(uploads, file.FileName);
                    using var fileStream = new FileStream(filePath, FileMode.OpenOrCreate);
                    await file.CopyToAsync(fileStream);
                    entity.Path = "/uploads/academy/videos/" + file.FileName;
                }
            }

            if (!Request.Form.Files.Any() && entity.Id != 0)
            {
                var u = _academyVideoService.GetById(entity.Id);
                entity.Path = u.Path;
            }
            #endregion

            #region Add Or Update

            if (model.Id == 0)
            {
                _academyVideoService.Insert(entity);
            }
            else
            {
                _academyVideoService.Update(entity);
            }

            #endregion

            return Json(entity);
        }

        #endregion
    }
}
