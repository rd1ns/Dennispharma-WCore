using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Linq;
using WCore.Core;
using WCore.Core.Domain.Vendors;
using WCore.Framework.Factories;
using WCore.Framework.Filters;
using WCore.Framework.Models;
using WCore.Services.Common;
using WCore.Services.Directory;
using WCore.Services.Localization;
using WCore.Services.Seo;
using WCore.Services.Settings;
using WCore.Services.Vendors;
using WCore.Web.Areas.Admin.Infrastructure.Mapper;
using WCore.Web.Areas.Admin.Models.Vendors;

namespace WCore.Web.Areas.Admin.Controllers
{
    public class VendorController : BaseAdminController
    {
        #region Fields
        private readonly IVendorService _vendorService;

        private readonly ICountryService _countryService;
        private readonly ICityService _cityService;
        private readonly ILocalizationService _localizationService;
        private readonly ILocalizedEntityService _localizedEntityService;
        private readonly ILocalizedModelFactory _localizedModelFactory;
        private readonly IUrlRecordService _urlRecordService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ISettingService _settingService;
        private readonly IWebHelper _webHelper;
        private readonly IWorkContext _workContext;
        #endregion

        #region Ctor
        public VendorController(IVendorService vendorService,
            ICountryService countryService,
            ICityService cityService,
            ILocalizationService localizationService,
            ILocalizedEntityService localizedEntityService,
            ILocalizedModelFactory localizedModelFactory,
            IUrlRecordService urlRecordService,
            IWebHostEnvironment webHostEnvironment,
            ISettingService settingService,
            IWebHelper webHelper,
            IWorkContext workContext)
        {
            this._vendorService = vendorService;

            this._countryService = countryService;
            this._cityService = cityService;
            this._localizedEntityService = localizedEntityService;
            this._localizationService = localizationService;
            this._localizedModelFactory = localizedModelFactory;
            this._urlRecordService = urlRecordService;
            this._webHostEnvironment = webHostEnvironment;
            this._settingService = settingService;
            this._webHelper = webHelper;
            this._workContext = workContext;

        }
        #endregion

        #region Utilities

        protected virtual void UpdateLocales(Vendor entity, VendorModel model)
        {
            foreach (var localized in model.Locales)
            {
                _localizedEntityService.SaveLocalizedValue(entity,
                    x => x.Name,
                    localized.Name,
                    localized.LanguageId);
                _localizedEntityService.SaveLocalizedValue(entity,
                    x => x.Description,
                    localized.Description,
                    localized.LanguageId);

                _localizedEntityService.SaveLocalizedValue(entity,
                    x => x.MetaTitle,
                    localized.MetaTitle,
                    localized.LanguageId);

                _localizedEntityService.SaveLocalizedValue(entity,
                    x => x.MetaKeywords,
                    localized.MetaKeywords,
                    localized.LanguageId);

                _localizedEntityService.SaveLocalizedValue(entity,
                    x => x.MetaDescription,
                    localized.MetaDescription,
                    localized.LanguageId);


                //search engine name
                var seName = _urlRecordService.ValidateSeName(entity, localized.SeName, localized.Name, false);
                _urlRecordService.SaveSlug(entity, seName, localized.LanguageId);
            }
        }
        #endregion

        #region Methods
        public IActionResult Index()
        {
            return Redirect("/Admin/Vendor/List");
        }

        public IActionResult List()
        {
            var searchModel = new VendorSearchModel();
            return View(searchModel);
        }

        [HttpPost]
        public JsonResult GetFilteredItems(VendorSearchModel searchModel)
        {
            var activities = _vendorService.GetAllByFilters(searchModel.Name, searchModel.Email, searchModel.skip, searchModel.take);

            var model = new VendorListModel().PrepareToGrid(searchModel, activities, () =>
            {
                return activities.Select(vendor =>
                {
                    var m = vendor.ToModel<VendorModel>();;
                    return m;
                });
            });
            return Json(model);
        }

        public IActionResult AddOrEdit(int Id)
        {
            var entity = _vendorService.GetById(Id);

            if (entity == null)
                entity = new Vendor();

            var model = entity.ToModel<VendorModel>();

            Action<VendorLocalizedModel, int> localizedModelConfiguration = null;

            if (model.Id != 0)
            {
                model.SeName = _urlRecordService.GetSeName(entity, 0, true, false);
                localizedModelConfiguration = (locale, languageId) =>
                {
                    locale.Name = _localizationService.GetLocalized(entity, e => e.Name, languageId, false, false);
                    locale.Description = _localizationService.GetLocalized(entity, e => e.Description, languageId, false, false);
                    locale.MetaKeywords = _localizationService.GetLocalized(entity, e => e.MetaKeywords, languageId, false, false);
                    locale.MetaDescription = _localizationService.GetLocalized(entity, e => e.MetaDescription, languageId, false, false);
                    locale.MetaTitle = _localizationService.GetLocalized(entity, e => e.MetaTitle, languageId, false, false);
                    locale.SeName = _urlRecordService.GetSeName(entity, languageId, false, false);
                };
            }

            model.Locales = _localizedModelFactory.PrepareLocalizedModels(localizedModelConfiguration);
            return View(model);
        }

        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        [HttpPost, ParameterBasedOnFormName("send-info", "sendInfo")]
        public async System.Threading.Tasks.Task<IActionResult> AddOrEditAsync(VendorModel model, bool continueEditing, bool sendInfo, bool delete)
        {
            var entity = model.ToEntity<Vendor>();

            #region Delete
            if (delete)
            {
                var _entity = _vendorService.GetById(model.Id);
                _entity.Deleted = true;
                _vendorService.Update(_entity);
                return Json("Deleted");
            }
            #endregion

            #region Image
            var uploads = Path.Combine(_webHostEnvironment.WebRootPath, "uploads/vendor");

            foreach (var file in Request.Form.Files)
            {
                if (file.Length > 0)
                {
                    var filePath = Path.Combine(uploads, file.FileName);
                    using var fileStream = new FileStream(filePath, FileMode.OpenOrCreate);
                    await file.CopyToAsync(fileStream);
                    entity.Image = "/uploads/vendor/" + file.FileName;
                }
            }

            if (!Request.Form.Files.Any() && entity.Id != 0)
            {
                var u = _vendorService.GetById(entity.Id);
                entity.Image = u.Image;
            }
            #endregion

            #region Add Or Update

            if (model.Id == 0)
            {
                _vendorService.Insert(entity);
            }
            else
            {
                _vendorService.Update(entity);
            }
            //search engine name
            model.SeName = _urlRecordService.ValidateSeName(entity, model.SeName, entity.Name, true);
            _urlRecordService.SaveSlug(entity, model.SeName, 0);

            //locales
            UpdateLocales(entity, model);
            #endregion

            return Json(continueEditing);
        }
        #endregion
    }
}
