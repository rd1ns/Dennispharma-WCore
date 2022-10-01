using WCore.Core.Caching;

namespace WCore.Services.Directory
{
    /// <summary>
    /// Represents default values related to directory services
    /// </summary>
    public static partial class WCoreDirectoryDefaults
    {
        #region Caching defaults

        #region Countries

        /// <summary>
        /// Key for activities list model
        /// </summary>
        /// <remarks>
        /// {0} : Name
        /// {1} : IsActive
        /// {1} : Published
        /// {2} : Deleted
        /// {3} : Skip
        /// {4} : Take
        /// </remarks>
        /// 
        public static CacheKey AllByFilters => new CacheKey("WCore.Country.GetAll-{0}-{1}-{2}-{3}-{4}-{5}", AllByFiltersPrefix);
        /// <summary>
        /// Gets a key pattern to clear cache
        /// </summary>
        public static string AllByFiltersPrefix => "WCore.Country";

        /// <summary>
        /// Gets a key for caching
        /// </summary>
        /// <remarks>
        /// {0} : Two letter ISO code
        /// </remarks>
        public static CacheKey CountriesByTwoLetterCodeCacheKey => new CacheKey("WCore.country.twoletter-{0}", CountriesPrefixCacheKey);

        /// <summary>
        /// Gets a key for caching
        /// </summary>
        /// <remarks>
        /// {0} : Two letter ISO code
        /// </remarks>
        public static CacheKey CountriesByThreeLetterCodeCacheKey => new CacheKey("WCore.country.threeletter-{0}", CountriesPrefixCacheKey);

        /// <summary>
        /// Gets a key for caching
        /// </summary>
        /// <remarks>
        /// {0} : languageId
        /// {1} : showHidden
        /// </remarks>
        public static CacheKey CountriesAllCacheKey => new CacheKey("WCore.country.all-{0}-{1}", CountriesPrefixCacheKey);

        /// <summary>
        /// Gets a key pattern to clear cache
        /// </summary>
        public static string CountriesPrefixCacheKey => "WCore.country.";

        #endregion

        #region Currencies

        /// <summary>
        /// Gets a key for caching
        /// </summary>
        /// <remarks>
        /// {0} : show hidden records?
        /// </remarks>
        public static CacheKey CurrenciesAllCacheKey => new CacheKey("WCore.currency.all-{0}", CurrenciesAllPrefixCacheKey);

        /// <summary>
        /// Gets a key pattern to clear cache
        /// </summary>
        public static string CurrenciesAllPrefixCacheKey => "WCore.currency.all";

        #endregion

        #region Measures

        /// <summary>
        /// Gets a key for caching
        /// </summary>
        public static CacheKey MeasureDimensionsAllCacheKey => new CacheKey("WCore.measuredimension.all");

        /// <summary>
        /// Gets a key for caching
        /// </summary>
        public static CacheKey MeasureWeightsAllCacheKey => new CacheKey("WCore.measureweight.all");

        #endregion

        #region States and provinces

        /// <summary>
        /// Gets a key for caching
        /// </summary>
        /// <remarks>
        /// {0} : country ID
        /// {1} : language ID
        /// {2} : show hidden records?
        /// </remarks>
        public static CacheKey StateProvincesByCountryCacheKey => new CacheKey("WCore.stateprovince.all-{0}-{1}-{2}", StateProvincesPrefixCacheKey);

        /// <summary>
        /// Gets a key for caching
        /// </summary>
        /// <remarks>
        /// {0} : show hidden records?
        /// </remarks>
        public static CacheKey StateProvincesAllCacheKey => new CacheKey("WCore.stateprovince.all-{0}", StateProvincesPrefixCacheKey);

        /// <summary>
        /// Gets a key for caching
        /// </summary>
        /// <remarks>
        /// {0} : abbreviation
        /// {1} : country ID
        /// </remarks>
        public static CacheKey StateProvincesByAbbreviationCacheKey => new CacheKey("WCore.stateprovince.abbreviationcountryid-{0}-{1}", StateProvincesPrefixCacheKey);

        /// <summary>
        /// Gets a key pattern to clear cache
        /// </summary>
        public static string StateProvincesPrefixCacheKey => "WCore.stateprovince.";

        #endregion

        #endregion
    }
    public static partial class WCoreCityDefaults
    {
        #region Caching defaults

        #region Cities

        /// <summary>
        /// Key for activities list model
        /// </summary>
        /// <remarks>
        /// {0} : CountryId
        /// {1} : Name
        /// {2} : IsActive
        /// {3} : Deleted
        /// {4} : Skip
        /// {5} : Take
        /// </remarks>
        /// 
        public static CacheKey AllByFilters => new CacheKey("WCore.City.GetAll-{0}-{1}-{2}-{3}-{4}-{5}", AllByFiltersPrefix);
        /// <summary>
        /// Gets a key pattern to clear cache
        /// </summary>
        public static string AllByFiltersPrefix => "WCore.City";
        #endregion

        #endregion
    }
}
