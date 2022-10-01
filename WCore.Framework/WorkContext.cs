using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using WCore.Core;
using WCore.Core.Domain.Directory;
using WCore.Core.Domain.Localization;
using WCore.Core.Domain.Settings;
using WCore.Core.Domain.Users;
using WCore.Core.Http;
using WCore.Services.Authentication;
using WCore.Services.Common;
using WCore.Services.Helpers;
using WCore.Services.Localization;
using WCore.Services.Tasks;
using WCore.Services.Users;
using System;
using System.Linq;
using WCore.Core.Domain.Tax;
using WCore.Core.Domain.Vendors;
using WCore.Services.Vendors;
using WCore.Core.Domain.Common;
using WCore.Services.Directory;
using WCore.Core.Infrastructure;

namespace WCore.Framework
{
    public class WorkContext : IWorkContext
    {
        #region Fields
        private readonly IAuthenticationService _authenticationService;
        private readonly IGenericAttributeService _genericAttributeService;
        private readonly ILanguageService _languageService;
        private readonly ICurrencyService _currencyService;
        private readonly IUserService _userService;
        private readonly IVendorService _vendorService;
        private readonly IStoreContext _storeContext;
        //private readonly IUserAgentHelper _userAgentHelper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly LocalizationSettings _localizationSettings;
        private readonly StoreInformationSettings _storeInformationSettings;
        private readonly CurrencySettings _currencySettings;
        private readonly TaxSettings _taxSettings;

        private User _cachedUser;
        private User _originalUserIfImpersonated;
        private Vendor _cachedVendor;
        private Language _cachedLanguage;
        private Currency _cachedCurrency;
        private Country _cachedCountry;
        private TaxDisplayType? _cachedTaxDisplayType;
        #endregion

        #region Ctor
        public WorkContext(IAuthenticationService authenticationService,
        IGenericAttributeService genericAttributeService,
        ILanguageService languageService,
        ICurrencyService currencyService,
        IHttpContextAccessor httpContextAccessor,
        IUserService userService,
        IVendorService vendorService,
        IStoreContext storeContext,
        //IUserAgentHelper userAgentHelper,
        LocalizationSettings localizationSettings,
        StoreInformationSettings storeInformationSettings,
        CurrencySettings currencySettings,
        TaxSettings taxSettings)
        {
            this._authenticationService = authenticationService;
            this._genericAttributeService = genericAttributeService;
            this._languageService = languageService;
            this._currencyService = currencyService;
            this._httpContextAccessor = httpContextAccessor;
            this._userService = userService;
            this._vendorService = vendorService;
            //this._userAgentHelper = userAgentHelper;

            this._localizationSettings = localizationSettings;
            this._storeInformationSettings = storeInformationSettings;
            this._currencySettings = currencySettings;
            this._taxSettings = taxSettings;
        }
        #endregion

        #region Methods
        protected virtual string GetUserCookie()
        {
            var cookieName = $"{WCoreCookieDefaults.Prefix}{WCoreCookieDefaults.UserCookie}";
            var userCookie = _httpContextAccessor.HttpContext?.Request?.Cookies[cookieName];
            return userCookie;
        }
        protected virtual void SetUserCookie(int? Id)
        {
            if (_httpContextAccessor.HttpContext?.Response == null)
                return;

            //delete current cookie value
            var cookieName = $"{WCoreCookieDefaults.Prefix}{WCoreCookieDefaults.UserCookie}";
            _httpContextAccessor.HttpContext.Response.Cookies.Delete(cookieName);

            //get date of cookie expiration
            var cookieExpires = 24 * 365; //TODO make configurable
            var cookieExpiresDate = DateTime.Now.AddHours(cookieExpires);

            //if passed guid is empty set cookie as expired

            if (Id == null)
            {
                cookieExpiresDate = DateTime.Now.AddMonths(-1);
            }

            //set new cookie value
            var options = new CookieOptions
            {
                HttpOnly = true,
                Expires = cookieExpiresDate
            };
            _httpContextAccessor.HttpContext.Response.Cookies.Append(cookieName, Id.ToString(), options);
        }
        /// <summary>
        /// Gets or sets the current customer
        /// </summary>
        public virtual User CurrentUser
        {
            get
            {
                //whether there is a cached value
                if (_cachedUser != null)
                    return _cachedUser;

                User user = null;

                //check whether request is made by a background (schedule) task
                if (_httpContextAccessor.HttpContext == null ||
                    _httpContextAccessor.HttpContext.Request.Path.Equals(new PathString($"/{WCoreTaskDefaults.ScheduleTaskPath}"), StringComparison.InvariantCultureIgnoreCase))
                {
                    //in this case return built-in user record for background task
                    user = _userService.GetOrCreateBackgroundTaskUser();
                }

                if (user == null || user.Deleted || !user.Active)
                {
                    //check whether request is made by a search engine, in this case return built -in user record for search engines
                    //if (_userAgentHelper.IsSearchEngine())
                    //    user = _userService.GetOrCreateSearchEngineUser();
                }

                if (user == null || user.Deleted || !user.Active)
                {
                    //try to get registered user
                    user = _authenticationService.GetAuthenticatedUser();
                }

                if (user == null || user.Deleted || !user.Active)
                {
                    //get guest user
                    var userCookie = GetUserCookie();
                    if (!string.IsNullOrEmpty(userCookie))
                    {
                        if (int.TryParse(userCookie, out var userGuid))
                        {
                            //get user from cookie (should not be registered)
                            var userByCookie = _userService.GetById(userGuid, cache => default);
                            if (userByCookie != null)
                                user = userByCookie;
                        }
                    }
                }

                if (user == null || user.Deleted || !user.Active)
                {
                    //create guest if not exists
                    user = _userService.InsertGuestUser();
                }

                if (!user.Deleted && user.Active)
                {
                    //set customer cookie
                    SetUserCookie(user.Id);

                    //cache the found user
                    _cachedUser = user;
                }

                _cachedUser = user;

                return _cachedUser;
            }
            set
            {
                SetUserCookie(value.Id);
                _cachedUser = value;
            }
        }

        /// <summary>
        /// Gets the original customer (in case the current one is impersonated)
        /// </summary>
        public virtual User OriginalUserIfImpersonated => _originalUserIfImpersonated;


        /// <summary>
        /// Gets the current vendor (logged-in manager)
        /// </summary>
        public virtual Vendor CurrentVendor
        {
            get
            {
                //whether there is a cached value
                if (_cachedVendor != null)
                    return _cachedVendor;

                if (CurrentUser == null)
                    return null;

                //try to get vendor
                var vendor = _vendorService.GetVendorById(CurrentUser.VendorId);

                //check vendor availability
                if (vendor == null || vendor.Deleted || !vendor.Active)
                    return null;

                //cache the found vendor
                _cachedVendor = vendor;

                return _cachedVendor;
            }
        }

        /// <summary>
        /// Gets or sets current user working language
        /// </summary>
        public virtual Language WorkingLanguage
        {
            get
            {
                //whether there is a cached value
                if (_cachedLanguage != null)
                    return _cachedLanguage;

                if (IsAdmin)
                {
                    var primaryStoreLanguage = _languageService.GetAdminDefaultLanguage();
                    if (primaryStoreLanguage != null)
                    {
                        //cache
                        _cachedLanguage = primaryStoreLanguage;
                        return _cachedLanguage;
                    }
                }

                Language detectedLanguage = null;

                //localized URLs are enabled, so try to get language from the requested page URL
                if (_localizationSettings.SeoFriendlyUrlsForLanguagesEnabled)
                    detectedLanguage = GetLanguageFromUrl();

                //whether we should detect the language from the request
                if (detectedLanguage == null && _localizationSettings.AutomaticallyDetectLanguage)
                {
                    //whether language already detected by this way
                    var alreadyDetected = _genericAttributeService.GetAttribute<bool>(CurrentUser,
                        WCoreUserDefaults.LanguageAutomaticallyDetectedAttribute);

                    //if not, try to get language from the request
                    if (!alreadyDetected)
                    {
                        detectedLanguage = GetLanguageFromRequest();
                        if (detectedLanguage != null)
                        {
                            //language already detected
                            _genericAttributeService.SaveAttribute(CurrentUser,
                                WCoreUserDefaults.LanguageAutomaticallyDetectedAttribute, true);
                        }
                    }
                }

                //if the language is detected we need to save it
                if (detectedLanguage != null)
                {
                    //get current saved language identifier
                    var currentLanguageId = _genericAttributeService.GetAttribute<int>(CurrentUser, WCoreUserDefaults.LanguageIdAttribute);

                    //save the detected language identifier if it differs from the current one
                    if (detectedLanguage.Id != currentLanguageId)
                    {
                        _genericAttributeService.SaveAttribute(CurrentUser, WCoreUserDefaults.LanguageIdAttribute, detectedLanguage.Id);
                    }
                }

                //get current customer language identifier
                var customerLanguageId = _genericAttributeService.GetAttribute<int>(CurrentUser,
                    WCoreUserDefaults.LanguageIdAttribute);

                var allStoreLanguages = _languageService.GetAll().ToList();

                //check customer language availability
                var customerLanguage = allStoreLanguages.FirstOrDefault(language => language.Id == customerLanguageId);

                //if the default language for the current store not found, then try to get the first one
                if (customerLanguage == null)
                    customerLanguage = allStoreLanguages.FirstOrDefault(o => o.Id == _storeInformationSettings.DefaultLanguageId);

                //if there are no languages for the current store try to get the first one regardless of the store
                if (customerLanguage == null)
                    customerLanguage = _languageService.GetAllLanguages().FirstOrDefault();

                //cache the found language
                _cachedLanguage = customerLanguage;

                return _cachedLanguage;
            }
            set
            {
                //get passed language identifier
                var languageId = value?.Id ?? 0;

                //and save it
                _genericAttributeService.SaveAttribute(CurrentUser, WCoreUserDefaults.LanguageIdAttribute, languageId);

                //then reset the cached value
                _cachedLanguage = null;
            }
        }
        /// <summary>
        /// Get language from the request
        /// </summary>
        /// <returns>The found language</returns>
        protected virtual Language GetLanguageFromRequest()
        {
            if (_httpContextAccessor.HttpContext?.Request == null)
                return null;

            //get request culture
            var requestCulture = _httpContextAccessor.HttpContext.Features.Get<IRequestCultureFeature>()?.RequestCulture;
            if (requestCulture == null)
                return null;

            //try to get language by culture name
            var requestLanguage = _languageService.GetAllLanguages().FirstOrDefault(language =>
                language.LanguageCulture.Equals(requestCulture.Culture.Name, StringComparison.InvariantCultureIgnoreCase));

            //check language availability
            if (requestLanguage == null || !requestLanguage.Published)
                return null;

            return requestLanguage;
        }

        /// <summary>
        /// Gets or sets current user working currency
        /// </summary>
        public virtual Currency WorkingCurrency
        {
            get
            {
                //whether there is a cached value
                if (_cachedCurrency != null)
                    return _cachedCurrency;

                //return primary store currency when we're in admin area/mode
                if (IsAdmin)
                {
                    var primaryStoreCurrency = _currencyService.GetById(_currencySettings.PrimaryStoreCurrencyId);
                    if (primaryStoreCurrency != null)
                    {
                        _cachedCurrency = primaryStoreCurrency;
                        return primaryStoreCurrency;
                    }
                }

                //find a currency previously selected by a customer
                var customerCurrencyId = _genericAttributeService.GetAttribute<int>(CurrentUser,
                    WCoreUserDefaults.CurrencyIdAttribute);

                var allStoreCurrencies = _currencyService.GetAll();

                //check customer currency availability
                var customerCurrency = allStoreCurrencies.FirstOrDefault(currency => currency.Id == customerCurrencyId);
                if (customerCurrency == null)
                {
                    //it not found, then try to get the default currency for the current language (if specified)
                    customerCurrency = allStoreCurrencies.FirstOrDefault(currency => currency.Id == WorkingLanguage.DefaultCurrencyId);
                }

                //if the default currency for the current store not found, then try to get the first one
                if (customerCurrency == null)
                    customerCurrency = allStoreCurrencies.FirstOrDefault();

                //if there are no currencies for the current store try to get the first one regardless of the store
                if (customerCurrency == null)
                    customerCurrency = _currencyService.GetAllCurrencies().FirstOrDefault();

                //cache the found currency
                _cachedCurrency = customerCurrency;

                return _cachedCurrency;
            }
            set
            {
                //get passed currency identifier
                var currencyId = value?.Id ?? 0;

                //and save it
                _genericAttributeService.SaveAttribute(CurrentUser,
                    WCoreUserDefaults.CurrencyIdAttribute, currencyId);

                //then reset the cached value
                _cachedCurrency = null;
            }
        }

        /// <summary>
        /// Gets or sets current user working country
        /// </summary>
        public virtual Country WorkingCountry
        {
            get
            {
                var _countryService = EngineContext.Current.Resolve<ICountryService>();
                //whether there is a cached value
                if (_cachedCountry != null)
                    return _cachedCountry;

                //find a country previously selected by a customer
                var customerCountryId = _genericAttributeService.GetAttribute<int>(CurrentUser,
                    WCoreUserDefaults.CountryIdAttribute);

                //check customer country availability
                var customerCountry = _countryService.GetCountryById(customerCountryId);

                //if the default country for the current store not found, then try to get the first one
                if (customerCountry == null)
                    customerCountry = _countryService.GetCountryById(_storeInformationSettings.DefaultCountryId);

                //if the default country for the current store not found, then try to get the first one
                if (customerCountry == null)
                    customerCountry = _countryService.GetAllByFilters(IsActive: true, Deleted: false).FirstOrDefault();

                //cache the found country
                _cachedCountry = customerCountry;

                return _cachedCountry;
            }
            set
            {
                //get passed country identifier
                var countryId = value?.Id ?? 0;

                //and save it
                _genericAttributeService.SaveAttribute(CurrentUser,
                    WCoreUserDefaults.CountryIdAttribute, countryId);

                //then reset the cached value
                _cachedCountry = null;
            }
        }


        /// <summary>
        /// Get language from the requested page URL
        /// </summary>
        /// <returns>The found language</returns>
        protected virtual Language GetLanguageFromUrl()
        {
            if (_httpContextAccessor.HttpContext?.Request == null)
                return null;

            //whether the requsted URL is localized
            var path = _httpContextAccessor.HttpContext.Request.Path.Value;
            if (!path.IsLocalizedUrl(_httpContextAccessor.HttpContext.Request.PathBase, false, out var language))
                return null;

            return language;
        }
        #endregion

        /// <summary>
        /// Gets or sets value indicating whether we're in admin area
        /// </summary>
        public virtual bool IsAdmin { get; set; }
        /// <summary>
        /// Gets or sets current tax display type
        /// </summary>
        public virtual TaxDisplayType TaxDisplayType
        {
            get
            {
                //whether there is a cached value
                if (_cachedTaxDisplayType.HasValue)
                    return _cachedTaxDisplayType.Value;

                var taxDisplayType = TaxDisplayType.IncludingTax;

                //whether customers are allowed to select tax display type
                if (_taxSettings.AllowUsersToSelectTaxDisplayType && CurrentUser != null)
                {
                    //try to get previously saved tax display type
                    var taxDisplayTypeId = _genericAttributeService.GetAttribute<int?>(CurrentUser,
                        WCoreUserDefaults.TaxDisplayTypeIdAttribute, _storeContext.CurrentStore.Id);
                    if (taxDisplayTypeId.HasValue)
                    {
                        taxDisplayType = (TaxDisplayType)taxDisplayTypeId.Value;
                    }
                    else
                    {
                        //default tax type by customer roles
                        var defaultRoleTaxDisplayType = _userService.GetUserDefaultTaxDisplayType(CurrentUser);
                        if (defaultRoleTaxDisplayType != null)
                        {
                            taxDisplayType = defaultRoleTaxDisplayType.Value;
                        }
                    }
                }
                else
                {
                    //default tax type by customer roles
                    var defaultRoleTaxDisplayType = _userService.GetUserDefaultTaxDisplayType(CurrentUser);
                    if (defaultRoleTaxDisplayType != null)
                    {
                        taxDisplayType = defaultRoleTaxDisplayType.Value;
                    }
                    else
                    {
                        //or get the default tax display type
                        taxDisplayType = _taxSettings.TaxDisplayType;
                    }
                }

                //cache the value
                _cachedTaxDisplayType = taxDisplayType;

                return _cachedTaxDisplayType.Value;

            }
            set
            {
                //whether customers are allowed to select tax display type
                if (!_taxSettings.AllowUsersToSelectTaxDisplayType)
                    return;

                //save passed value
                _genericAttributeService.SaveAttribute(CurrentUser,
                    WCoreUserDefaults.TaxDisplayTypeIdAttribute, (int)value, _storeContext.CurrentStore.Id);

                //then reset the cached value
                _cachedTaxDisplayType = null;
            }
        }
    }
}
