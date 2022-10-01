using WCore.Core.Caching;

namespace WCore.Services.Newses
{
    /// <summary>
    /// Represents default values related to congress services
    /// </summary>
    public static partial class WCoreNewsesDefaults
    {
        #region Caching defaults

        /// <summary>
        /// Key for activities list model
        /// </summary>
        /// <remarks>
        /// {0} : NewsCategoryId
        /// {1} : Title
        /// {2} : IsArchived
        /// {3} : IsActive
        /// {4} : Deleted
        /// {5} : ShowOn
        /// {6} : ShowOnHome
        /// {7} : Skip
        /// {8} : Take
        /// {9} : StartDate
        /// {10} : EndDate
        /// </remarks>
        /// 
        public static CacheKey AllByFilters => new CacheKey("WCore.News.GetAll.filters-{0}-{1}-{2}-{3}-{4}-{5}-{6}-{7}-{8}-{9}-{10}", AllByFiltersPrefix);

        /// <summary>
        /// Gets a key pattern to clear cache
        /// </summary>
        public static string AllByFiltersPrefix => "WCore.News";

        #endregion
    }

    /// <summary>
    /// Represents default values related to congress paper types services
    /// </summary>
    public static partial class WCoreNewsCategoriesDefaults
    {
        #region Caching defaults

        /// <summary>
        /// Key for activities list model
        /// </summary>
        /// <remarks>
        /// {0} : Title
        /// {1} : IsActive
        /// {2} : Deleted
        /// {3} : ShowOn
        /// {4} : Skip
        /// {5} : Take
        /// </remarks>
        /// 
        public static CacheKey AllByFilters => new CacheKey("WCore.NewsCategory.GetAll-{0}-{1}-{2}-{3}-{4}-{5}", AllByFiltersPrefix);

        /// <summary>
        /// Gets a key pattern to clear cache
        /// </summary>
        public static string AllByFiltersPrefix => "WCore.NewsCategory";

        #endregion
    }
    /// <summary>
    /// Represents default values related to congress images services
    /// </summary>
    public static partial class WCoreNewsImagesDefaults
    {
        #region Caching defaults

        /// <summary>
        /// Key for activities list model
        /// </summary>
        /// <remarks>
        /// {0} NewsId
        /// {1} Skip
        /// {2} Take
        /// </remarks>
        /// 
        public static CacheKey AllByFilters => new CacheKey("WCore.NewsImages.GetAll-{0}-{1}-{2}", AllByFiltersPrefix);

        /// <summary>
        /// Gets a key pattern to clear cache
        /// </summary>
        public static string AllByFiltersPrefix => "WCore.NewsImages";

        #endregion
    }
}
