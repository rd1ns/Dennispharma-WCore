using WCore.Core;
using WCore.Core.Domain.Settings;
using WCore.Services.Common;
using WCore.Services.Localization;
using WCore.Web.Models.Common;
using WCore.Web.Models.Directory;
using WCore.Web.Models.Localization;
using System.Globalization;
using System.Linq;
using WCore.Services.Directory;

namespace WCore.Web.Factories
{
    public interface ICommonModelFactory
    {
        LanguageSelectorModel PrepareLanguageSelectorModel();
        CountrySelectorModel PrepareCountrySelectorModel();
        CurrencySelectorModel PrepareCurrencySelectorModel();
    }

    public class CommonModelFactory : ICommonModelFactory
    {
        #region Fields

        private readonly ILocalizationService _localizationService;
        private readonly ILanguageService _languageService;
        private readonly ICurrencyService _currencyService;
        private readonly ICountryService _countryService;
        private readonly IWorkContext _workContext;
        private readonly LocalizationSettings _localizationSettings;
        #endregion

        #region Ctor
        public CommonModelFactory(ILocalizationService localizationService,
        ILanguageService languageService,
        ICurrencyService currencyService,
        ICountryService countryService,
        IWorkContext workContext,
        LocalizationSettings localizationSettings)
        {
            this._localizationService = localizationService;
            this._languageService = languageService;
            this._currencyService = currencyService;
            this._countryService = countryService;
            this._workContext = workContext;
            this._localizationSettings = localizationSettings;
        }
        #endregion

        #region Methods

        /// <summary>
        /// Prepare the Country selector model
        /// </summary>
        /// <returns>Country selector model</returns>
        public virtual CountrySelectorModel PrepareCountrySelectorModel()
        {
            var workingCountry = _workContext.WorkingCountry;
            var availableCountries = _countryService.GetAllByFilters(IsActive: true, Published: true, Deleted: false)
                    .Select(x => new CountryModel
                    {
                        Id = x.Id,
                        Name = x.Name
                    }).ToList();

            var model = new CountrySelectorModel
            {
                CurrentCountry = new CountryModel()
                {
                    Id = workingCountry.Id,
                    Name = workingCountry.Name
                },
                AvailableCountries = availableCountries
            };

            return model;
        }

        /// <summary>
        /// Prepare the language selector model
        /// </summary>
        /// <returns>Language selector model</returns>
        public virtual LanguageSelectorModel PrepareLanguageSelectorModel()
        {
            var workingLanguage = _workContext.WorkingLanguage;
            var availableLanguages = _languageService
                    .GetAllLanguages(AllowSelection: true, Published: true)
                    .Select(x => new LanguageModel
                    {
                        Id = x.Id,
                        Name = x.Name,
                        FlagImageFileName = x.FlagImageFileName,
                    }).ToList();

            var model = new LanguageSelectorModel
            {
                CurrentLanguage = new LanguageModel()
                {
                    Id = workingLanguage.Id,
                    Name = workingLanguage.Name,
                    FlagImageFileName = workingLanguage.FlagImageFileName
                },
                AvailableLanguages = availableLanguages,
                UseImages = _localizationSettings.UseImagesForLanguageSelection
            };

            return model;
        }

        /// <summary>
        /// Prepare the currency selector model
        /// </summary>
        /// <returns>Currency selector model</returns>
        public virtual CurrencySelectorModel PrepareCurrencySelectorModel()
        {
            var workingCurrency = _workContext.WorkingCurrency;
            var availableCurrencies = _currencyService
                .GetAllCurrencies()
                .Select(x =>
                {
                    //currency char
                    var currencySymbol = !string.IsNullOrEmpty(x.DisplayLocale)
                        ? new RegionInfo(x.DisplayLocale).CurrencySymbol
                        : x.CurrencyCode;

                    //model
                    var currencyModel = new CurrencyModel
                    {
                        Id = x.Id,
                        Name = _localizationService.GetLocalized(x, y => y.Name),
                        CurrencyCode = currencySymbol
                    };

                    return currencyModel;
                }).ToList();

            var model = new CurrencySelectorModel
            {
                CurrentCurrency = new CurrencyModel()
                {
                    Name = workingCurrency.Name,
                    CurrencyCode = workingCurrency.DisplayLocale,
                    Id = workingCurrency.Id
                }
                ,
                AvailableCurrencies = availableCurrencies
            };

            return model;
        }
        #endregion
    }
}
