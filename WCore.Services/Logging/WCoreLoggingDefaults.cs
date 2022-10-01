using WCore.Core.Caching;

namespace WCore.Services.Logging
{
    /// <summary>
    /// Represents default values related to logging services
    /// </summary>
    public static partial class WCoreLoggingDefaults
    {
        #region Caching defaults

        /// <summary>
        /// Gets a key for caching
        /// </summary>
        public static CacheKey ActivityTypeAllCacheKey => new CacheKey("WCore.activitytype.all");

        /// <summary>
        /// Key for countries list model
        /// </summary>
        /// <remarks>
        /// {0} : userId
        /// {1} : PageStatus
        /// {2} : Take
        /// {3} : Skip
        /// </remarks>
        public static CacheKey AllByFilters => new CacheKey("WCore.log.getall.filters-{0}-{1}-{2}-{3}", AllByFiltersPrefix);

        /// <summary>
        /// Gets a key pattern to clear cache
        /// </summary>
        public static string AllByFiltersPrefix => "WCore.log";

        #endregion
    }
}