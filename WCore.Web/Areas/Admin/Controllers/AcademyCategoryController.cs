using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Linq;
using WCore.Core;
using WCore.Core.Domain.Academies;
using WCore.Core.Domain.Newses;
using WCore.Core.Infrastructure;
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
    public class AcademyCategoryController : BaseAdminController
    {
        #region Fields
        private readonly IAcademyCategoryService _academyCategoryService;
        private readonly IAcademyService _academyService;

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
        public AcademyCategoryController(IAcademyCategoryService academyCategoryService,
            IAcademyService academyService,
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
            this._academyCategoryService = academyCategoryService;
            this._academyService = academyService;

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
        protected virtual void UpdateLocales(AcademyCategory entity, AcademyCategoryModel model)
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
            return Redirect("/Admin/AcademyCategory/List");
        }

        public IActionResult List()
        {
            var searchModel = new AcademyCategorySearchModel();
            return View(searchModel);
        }

        [HttpPost]
        public JsonResult GetFilteredItems(AcademyCategorySearchModel searchModel)
        {
            var academyCategory = _academyCategoryService.GetAllByFilters(
                searchModel.ParentId,
                searchModel.Title,
                searchModel.IsActive,
                searchModel.Deleted,
                searchModel.ShowOn,
                searchModel.skip,
                searchModel.take);

            var model = new AcademyCategoryListModel().PrepareToGrid(searchModel, academyCategory, () =>
            {
                return academyCategory.Select(academyCategory =>
                {
                    var m = academyCategory.ToModel<AcademyCategoryModel>();
                    return m;
                });
            });
            return Json(model);
        }

        public IActionResult AddOrEdit(int Id)
        {
            var entity = _academyCategoryService.GetById(Id);

            if (entity == null)
                entity = new AcademyCategory();

            var model = entity.ToModel<AcademyCategoryModel>();

            Action<AcademyCategoryLocalizedModel, int> localizedModelConfiguration = null;

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
        public async System.Threading.Tasks.Task<IActionResult> AddOrEditAsync(AcademyCategoryModel model, bool continueEditing, bool delete)
        {
            var entity = model.ToEntity<AcademyCategory>();

            #region Delete
            if (delete)
            {
                var _entity = _academyCategoryService.GetById(model.Id);
                _entity.Deleted = true;
                _entity.IsActive = false;
                _academyCategoryService.Update(_entity);
                return Json("Deleted");
            }
            #endregion

            #region Image && Banner
            var uploads = Path.Combine(_webHostEnvironment.WebRootPath, "uploads/academyCategory");
            var academyCategory = _academyCategoryService.GetById(entity.Id);
            var imagePicture = Request.Form.Files["Image"];
            if (imagePicture != null && imagePicture.Length > 0)
            {
                var filePath = Path.Combine(uploads, imagePicture.FileName);
                using var fileStream = new FileStream(filePath, FileMode.OpenOrCreate);
                await imagePicture.CopyToAsync(fileStream);
                entity.Image = "/uploads/academyCategory/" + imagePicture.FileName;

            }
            else
            {
                entity.Image = academyCategory?.Image;
            }

            var bannerPicture = Request.Form.Files["Banner"];
            if (bannerPicture != null && bannerPicture.Length > 0)
            {
                var filePath = Path.Combine(uploads, bannerPicture.FileName);
                using var fileStream = new FileStream(filePath, FileMode.OpenOrCreate);
                await bannerPicture.CopyToAsync(fileStream);
                entity.Banner = "/uploads/academyCategory/" + bannerPicture.FileName;

            }
            else
            {
                entity.Banner = academyCategory?.Banner;
            }

            #endregion

            #region Add Or Update

            if (model.Id == 0)
            {
                _academyCategoryService.Insert(entity);
            }
            else
            {
                _academyCategoryService.Update(entity);
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
