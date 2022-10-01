using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using WCore.Core;
using WCore.Core.Caching;
using WCore.Core.Domain;
using WCore.Core.Domain.Common;
using WCore.Framework;
using WCore.Framework.Extensions;
using WCore.Framework.Filters;
using WCore.Framework.Models;
using WCore.Services.Common;
using WCore.Services.Directory;
using WCore.Services.Localization;
using WCore.Services.Seo;
using WCore.Services.Settings;
using WCore.Services.Users;
using WCore.Web.Areas.Admin.Infrastructure.Mapper;
using WCore.Web.Areas.Admin.Models.Common;
using WCore.Web.Areas.Admin.Models.Directory;
using WCore.Web.Areas.Admin.Models.Users;
using WCore.Web.Models;

namespace WCore.Web.Areas.Admin.Controllers
{
    public class CommonController : BaseAdminController
    {
        #region Fields
        private readonly IUserService _userService;

        private readonly ICurrencyService _currencyService;
        private readonly ICountryService _countryService;
        private readonly ICityService _cityService;
        private readonly ISettingService _settingService;
        private readonly IStaticCacheManager _staticCacheManager;
        private readonly IUrlRecordService _urlRecordService;


        private readonly ILocalizationService _localizationService;
        private readonly IWorkContext _workContext;
        private readonly IWebHelper _webHelper;
        #endregion

        #region Ctor
        public CommonController(IUserService userService,
            ICurrencyService currencyService,
            ICountryService countryService,
            ICityService cityService,
            ISettingService settingService,
            IStaticCacheManager staticCacheManager,
            ILocalizationService localizationService,
            IUrlRecordService urlRecordService,
            IWorkContext workContext,
            IWebHelper webHelper)
        {
            this._userService = userService;
            this._currencyService = currencyService;
            this._settingService = settingService;
            this._countryService = countryService;
            this._cityService = cityService;
            this._staticCacheManager = staticCacheManager;
            this._localizationService = localizationService;
            this._urlRecordService = urlRecordService;
            this._workContext = workContext;
            this._webHelper = webHelper;

            CreatedUserId = _workContext.CurrentUser.Id;
        }
        #endregion

        #region Methods

        public virtual IActionResult ClearCache(string returnUrl = "")
        {
            _staticCacheManager.Clear();

            //home page
            if (string.IsNullOrEmpty(returnUrl))
                return RedirectToAction("Index", "Home", new { area = AreaNames.Admin });

            //prevent open redirection attack
            if (!Url.IsLocalUrl(returnUrl))
                return RedirectToAction("Index", "Home", new { area = AreaNames.Admin });

            return Redirect(returnUrl);
        }
        public IActionResult PageNotFound()
        {
            return View();
        }
        [HttpPost]
        public IActionResult GetAllCountries(string name)
        {
            //var countries = _countryService.GetAllByFilters(searchValue: name).Select(o => { return o.ToModel<CountryModel>(); }).ToList();
            //var model = new Select2_CountryModel
            //{
            //    items = countries,
            //    incomplate_results = false,
            //    total_count = countries.Count()
            //};
            return Json(null);
        }
        [HttpPost]
        public IActionResult GetAllCurrencies(string name)
        {
            var currencies = _currencyService.GetAllCurrencies().Select(o =>
            {
                var m = new SelectListItem();
                m.Value = o.Id.ToString();
                m.Text = o.Name;
                return m;
            }).ToList();
            return Json(currencies);
        }
        [HttpPost]
        public IActionResult GetAllCities(string name, int countryId)
        {
            var cities = _cityService.GetAllByFilters(countryId, Name: name).Select(o => { return o.ToModel<CityModel>(); });
            var model = new Select2_CityModel
            {
                items = cities.ToList(),
                incomplate_results = false,
                total_count = cities.Count()
            };
            return Json(model);
        }
        [HttpPost]
        public IActionResult GetAllDistircts(string name, int countryId, int cityId)
        {
            //var districts = _districtService.GetAllByFilters(countryId, cityId, searchValue: name).Select(o => { return o.ToModel<DistrictModel>(); });
            //var model = new Select2_DistrictModel
            //{
            //    items = districts.ToList(),
            //    incomplate_results = false,
            //    total_count = districts.Count()
            //};
            return Json(null);
        }
        #endregion

        #region Country
        public IActionResult Country()
        {
            var model = new CountrySearchModel();
            return View(model);
        }
        [HttpPost]
        public JsonResult GetCountryFilteredItems(CountrySearchModel searchModel)
        {
            var countries = _countryService.GetAllByFilters(
                searchModel.Name,
                searchModel.IsActive,
                searchModel.Deleted,
                searchModel.Published,
                searchModel.skip,
                searchModel.take);

            var model = new CountryListModel().PrepareToGrid(searchModel, countries, () =>
            {
                return countries.Select(country =>
                {
                    var m = country.ToModel<CountryModel>();
                    return m;
                });
            });
            return Json(model);
        }

        public IActionResult CountryAddOrEdit(int Id, bool IsPopup = false)
        {
            var entity = _countryService.GetById(Id).ToModel<CountryModel>();

            if (entity == null)
                entity = new CountryModel();

            if (entity.Id == 0)
            {
                entity.CreatedUserId = _workContext.CurrentUser.Id;
                entity.CreatedUser = _workContext.CurrentUser.ToModel<UserModel>();
            }
            else
            {
                if (entity.CreatedUserId == 0)
                {
                    entity.CreatedUserId = _workContext.CurrentUser.Id;
                    entity.CreatedUser = _workContext.CurrentUser.ToModel<UserModel>();
                }
                entity.CreatedUser = _userService.GetById(entity.CreatedUserId).ToModel<UserModel>();
            }

            entity.UserId = _workContext.CurrentUser.Id;
            entity.User = _workContext.CurrentUser.ToModel<UserModel>();

            return View(entity);
        }

        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public IActionResult CountryAddOrEdit(CountryModel model, bool continueEditing, bool delete)
        {
            var entity = model.ToEntity<Country>();

            entity.UserId = _workContext.CurrentUser.Id;

            if (delete)
            {
                var _entity = _countryService.GetById(model.Id);
                _entity.Deleted = true;
                _entity.IsActive = false;
                _countryService.Update(_entity);
                return Json("Deleted");
            }

            if (model.Id == 0)
            {
                entity = _countryService.Insert(entity);
            }
            _countryService.Update(entity);

            return Json(continueEditing);
        }
        #endregion

        #region City
        public IActionResult City()
        {
            var model = new CitySearchModel();
            return View(model);
        }
        [HttpPost]
        public JsonResult GetCityFilteredItems(CitySearchModel searchModel)
        {
            var cities = _cityService.GetAllByFilters(
                searchModel.CountryId,
                searchModel.Name,
                searchModel.IsActive,
                searchModel.Deleted,
                searchModel.skip,
                searchModel.take);

            var model = new CityListModel().PrepareToGrid(searchModel, cities, () =>
            {
                return cities.Select(city =>
                {
                    var m = city.ToModel<CityModel>();
                    m.Country = _countryService.GetById(city.CountryId).ToModel<CountryModel>();
                    return m;
                });
            });
            return Json(model);
        }

        public IActionResult CityAddOrEdit(int Id, int? countryId = null, bool IsPopup = false)
        {
            var entity = _cityService.GetById(Id).ToModel<CityModel>();

            if (entity == null)
                entity = new CityModel();

            if (entity.Id == 0)
            {
                entity.CreatedUserId = _workContext.CurrentUser.Id;
                entity.CreatedUser = _workContext.CurrentUser.ToModel<UserModel>();
            }
            else
            {
                if (entity.CreatedUserId == 0)
                {
                    entity.CreatedUserId = _workContext.CurrentUser.Id;
                    entity.CreatedUser = _workContext.CurrentUser.ToModel<UserModel>();
                }
                entity.CreatedUser = _userService.GetById(entity.CreatedUserId).ToModel<UserModel>();
            }

            entity.UserId = _workContext.CurrentUser.Id;
            entity.User = _workContext.CurrentUser.ToModel<UserModel>();



            //var countries = _countryService.GetAllByFilters();
            //entity.Countries = new SelectList(countries, "Id", "Name", entity.CountryId).ToList();

            return View(entity);
        }

        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public IActionResult CityAddOrEdit(CityModel model, bool continueEditing, bool delete)
        {
            var entity = model.ToEntity<City>();

            entity.UserId = _workContext.CurrentUser.Id;

            if (delete)
            {
                var _entity = _cityService.GetById(model.Id);
                _entity.Deleted = true;
                _entity.IsActive = false;
                _cityService.Update(_entity);
                return Json("Deleted");
            }

            if (model.Id == 0)
            {
                entity = _cityService.Insert(entity);
            }
            _cityService.Update(entity);

            return Json(continueEditing);
        }
        #endregion

        #region Common

        //action displaying notification (warning) to a store owner that entered SE URL already exists
        public virtual IActionResult UrlReservedWarning(string entityId, string entityName, string seName)
        {
            if (string.IsNullOrEmpty(seName))
                return Json(new { Result = string.Empty });

            int.TryParse(entityId, out var parsedEntityId);
            var validatedSeName = _urlRecordService.ValidateSeName(parsedEntityId, entityName, seName, null, false);

            if (seName.Equals(validatedSeName, StringComparison.InvariantCultureIgnoreCase))
                return Json(new { Result = string.Empty });

            return Json(new { Result = string.Format(_localizationService.GetResource("Admin.System.Warnings.URL.Reserved"), validatedSeName) });
        }
        #endregion
    }
}
