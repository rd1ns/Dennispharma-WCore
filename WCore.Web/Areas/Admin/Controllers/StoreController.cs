using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Linq;
using WCore.Core;
using WCore.Core.Domain.Stores;
using WCore.Framework.Factories;
using WCore.Framework.Filters;
using WCore.Framework.Models;
using WCore.Services.Directory;
using WCore.Services.Localization;
using WCore.Services.Seo;
using WCore.Services.Settings;
using WCore.Services.Stores;
using WCore.Web.Areas.Admin.Infrastructure.Mapper;
using WCore.Web.Areas.Admin.Models.Stores;

namespace WCore.Web.Areas.Admin.Controllers
{
    public class StoreController : BaseAdminController
    {
        #region Fields
        private readonly IStoreService _storeService;

        private readonly ICountryService _countryService;
        private readonly ILanguageService _languageService;
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
        public StoreController(IStoreService storeService,
            ILanguageService languageService,
            ILocalizationService localizationService,
            ILocalizedEntityService localizedEntityService,
            ILocalizedModelFactory localizedModelFactory,
            IUrlRecordService urlRecordService,
            IWebHostEnvironment webHostEnvironment,
            ISettingService settingService,
            IWebHelper webHelper,
            IWorkContext workContext)
        {
            this._storeService = storeService;

            this._languageService = languageService;
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

        protected virtual void UpdateLocales(Store entity, StoreModel model)
        {
            foreach (var localized in model.Locales)
            {
                _localizedEntityService.SaveLocalizedValue(entity,
                    x => x.Name,
                    localized.Name,
                    localized.LanguageId);
            }
        }
        #endregion

        #region Methods
        public IActionResult Index()
        {
            return Redirect("/Admin/Store/List");
        }

        public IActionResult List()
        {
            var searchModel = new StoreSearchModel();
            return View(searchModel);
        }

        [HttpPost]
        public JsonResult GetFilteredItems(StoreSearchModel searchModel)
        {
            var activities = _storeService.GetAllByFilters();

            var model = new StoreListModel().PrepareToGrid(searchModel, activities, () =>
            {
                return activities.Select(store =>
                {
                    var m = store.ToModel<StoreModel>(); ;
                    return m;
                });
            });
            return Json(model);
        }

        public IActionResult AddOrEdit(int Id)
        {
            var entity = _storeService.GetById(Id);

            if (entity == null)
                entity = new Store();

            var model = entity.ToModel<StoreModel>();

            Action<StoreLocalizedModel, int> localizedModelConfiguration = null;

            if (model.Id != 0)
            {
                localizedModelConfiguration = (locale, languageId) =>
                {
                    locale.Name = _localizationService.GetLocalized(entity, e => e.Name, languageId, false, false);
                };
            }

            model.Locales = _localizedModelFactory.PrepareLocalizedModels(localizedModelConfiguration);
            model.AvailableLanguages = new SelectList(_languageService.GetAllLanguages(showHidden: true), "Id", "Name", entity.DefaultLanguageId).ToList();
            return View(model);
        }

        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        [HttpPost, ParameterBasedOnFormName("send-info", "sendInfo")]
        public IActionResult AddOrEdit(StoreModel model, bool continueEditing)
        {
            var entity = model.ToEntity<Store>();

            #region Add Or Update

            if (model.Id == 0)
            {
                _storeService.Insert(entity);
            }
            else
            {
                _storeService.Update(entity);
            }

            //locales
            UpdateLocales(entity, model);
            #endregion

            return Json(continueEditing);
        }
        #endregion
    }
}
