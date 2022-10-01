using WCore.Core.Caching;

namespace WCore.Services.Academies
{
    /// <summary>
    /// Represents default values related to academy services
    /// </summary>
    public static partial class WCoreAcademyDefaults
    {
        #region Caching defaults

        /// <summary>
        /// Key for list model
        /// </summary>
        /// <remarks>
        /// {0} : AcademyCategoryId
        /// {1} : Title
        /// {2} : IsArchived
        /// {3} : IsActive
        /// {4} : Deleted
        /// {5} : ShowOn
        /// {6} : Skip
        /// {7} : Take
        /// </remarks>
        /// 
        public static CacheKey AllByFilters => new CacheKey("WCore.Academy.GetAll.filters-{0}-{1}-{2}-{3}-{4}-{5}-{6}-{7}", AllByFiltersPrefix);

        /// <summary>
        /// Gets a key pattern to clear cache
        /// </summary>
        public static string AllByFiltersPrefix => "WCore.Academy";

        #endregion
    }

    /// <summary>
    /// Represents default values related to academy category services
    /// </summary>
    public static partial class WCoreAcademyCategoryDefaults
    {
        #region Caching defaults

        /// <summary>
        /// Key for activities list model
        /// </summary>
        /// <remarks>
        /// {0} : ParentId
        /// {1} : Title
        /// {2} : IsActive
        /// {3} : Deleted
        /// {4} : ShowOn
        /// {5} : Skip
        /// {6} : Take
        /// </remarks>
        /// 
        public static CacheKey AllByFilters => new CacheKey("WCore.AcademyCategory.GetAll-{0}-{1}-{2}-{3}-{4}-{5}-{6}", AllByFiltersPrefix);

        /// <summary>
        /// Gets a key pattern to clear cache
        /// </summary>
        public static string AllByFiltersPrefix => "WCore.AcademyCategory";

        #endregion
    }
    /// <summary>
    /// Represents default values related to academy images services
    /// </summary>
    public static partial class WCoreAcademyImageDefaults
    {
        #region Caching defaults

        /// <summary>
        /// Key for activities list model
        /// </summary>
        /// <remarks>
        /// {0} AcademyId
        /// {1} Skip
        /// {2} Take
        /// </remarks>
        /// 
        public static CacheKey AllByFilters => new CacheKey("WCore.AcademyImages.GetAll-{0}-{1}-{2}", AllByFiltersPrefix);

        /// <summary>
        /// Gets a key pattern to clear cache
        /// </summary>
        public static string AllByFiltersPrefix => "WCore.AcademyImages";

        #endregion
    }

    /// <summary>
    /// Represents default values related to academy file services
    /// </summary>
    public static partial class WCoreAcademyFileDefaults
    {
        #region Caching defaults

        /// <summary>
        /// Key for activities list model
        /// </summary>
        /// <remarks>
        /// {0} AcademyId
        /// {1} Skip
        /// {2} Take
        /// </remarks>
        /// 
        public static CacheKey AllByFilters => new CacheKey("WCore.AcademyFiles.GetAll-{0}-{1}-{2}", AllByFiltersPrefix);

        /// <summary>
        /// Gets a key pattern to clear cache
        /// </summary>
        public static string AllByFiltersPrefix => "WCore.AcademyFiles";

        #endregion
    }

    /// <summary>
    /// Represents default values related to academy video services
    /// </summary>
    public static partial class WCoreAcademyVideoDefaults
    {
        #region Caching defaults

        /// <summary>
        /// Key for activities list model
        /// </summary>
        /// <remarks>
        /// {0} AcademyId
        /// {1} AcademyVideoResource
        /// {2} Skip
        /// {3} Take
        /// </remarks>
        /// 
        public static CacheKey AllByFilters => new CacheKey("WCore.AcademyVideos.GetAll-{0}-{1}-{2}-{3}", AllByFiltersPrefix);

        /// <summary>
        /// Gets a key pattern to clear cache
        /// </summary>
        public static string AllByFiltersPrefix => "WCore.AcademyVideos";

        #endregion
    }
}
