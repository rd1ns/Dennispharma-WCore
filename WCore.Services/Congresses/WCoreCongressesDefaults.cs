using WCore.Core.Caching;

namespace WCore.Services.Congresses
{
    /// <summary>
    /// Represents default values related to congress services
    /// </summary>
    public static partial class WCoreCongressesDefaults
    {
        #region Caching defaults

        /// <summary>
        /// Key for activities list model
        /// </summary>
        /// <remarks>
        /// {0} : Title
        /// {1} : IsArchived
        /// {2} : IsActive
        /// {3} : Deleted
        /// {4} : ShowOn
        /// {5} : Skip
        /// {6} : Take
        /// {7} : StartDate
        /// {8} : EndDate
        /// </remarks>
        /// 
        public static CacheKey AllByFilters => new CacheKey("WCore.Congress.GetAll.filters-{0}-{1}-{2}-{3}-{4}-{5}-{6}-{7}-{8}", AllByFiltersPrefix);

        /// <summary>
        /// Gets a key pattern to clear cache
        /// </summary>
        public static string AllByFiltersPrefix => "WCore.Congress";

        #endregion
    }

    /// <summary>
    /// Represents default values related to congress paper types services
    /// </summary>
    public static partial class WCoreCongressPaperTypesDefaults
    {
        #region Caching defaults

        /// <summary>
        /// Key for activities list model
        /// </summary>
        /// <remarks>
        /// {0} : CongressId
        /// {1} : Title
        /// {2} : IsActive
        /// {3} : Deleted
        /// {4} : ShowOn
        /// {5} : Skip
        /// {6} : Take
        /// </remarks>
        /// 
        public static CacheKey AllByFilters => new CacheKey("WCore.CongressPaperTypes.GetAll-{0}-{1}-{2}-{3}-{4}-{5}-{6}", AllByFiltersPrefix);

        /// <summary>
        /// Gets a key pattern to clear cache
        /// </summary>
        public static string AllByFiltersPrefix => "WCore.CongressPaperTypes";

        #endregion
    }

    /// <summary>
    /// Represents default values related to congress paper services
    /// </summary>
    public static partial class WCoreCongressPapersDefaults
    {
        #region Caching defaults

        /// <summary>
        /// Key for activities list model
        /// </summary>
        /// <remarks>
        /// {0} CongressId
        /// {1} CongressPaperTypeId
        /// {2} Title
        /// {3} Code
        /// {4} OwnersName
        /// {5} OwnersSurname
        /// {6} IsActive
        /// {7} Deleted
        /// {8} ShowOn
        /// {9} Skip
        /// {10} Take
        /// </remarks>
        /// 
        public static CacheKey AllByFilters => new CacheKey("WCore.CongressPapers.GetAll-{0}-{1}-{2}-{3}-{4}-{5}-{6}-{7}-{8}-{9}-{10}", AllByFiltersPrefix);

        /// <summary>
        /// Gets a key pattern to clear cache
        /// </summary>
        public static string AllByFiltersPrefix => "WCore.CongressPapers";

        #endregion
    }

    /// <summary>
    /// Represents default values related to congress presentation types services
    /// </summary>
    public static partial class WCoreCongressPresentationTypesDefaults
    {
        #region Caching defaults

        /// <summary>
        /// Key for activities list model
        /// </summary>
        /// <remarks>
        /// {0} : CongressId
        /// {1} : Title
        /// {2} : IsActive
        /// {3} : Deleted
        /// {4} : ShowOn
        /// {5} : Skip
        /// {6} : Take
        /// </remarks>
        /// 
        public static CacheKey AllByFilters => new CacheKey("WCore.CongressPresentationTypes.getall-{0}-{1}-{2}-{3}-{4}-{5}-{6}", AllByFiltersPrefix);

        /// <summary>
        /// Gets a key pattern to clear cache
        /// </summary>
        public static string AllByFiltersPrefix => "WCore.CongressPresentationTypes";

        #endregion
    }

    /// <summary>
    /// Represents default values related to congress presentation services
    /// </summary>
    public static partial class WCoreCongressPresentationsDefaults
    {
        #region Caching defaults

        /// <summary>
        /// Key for activities list model
        /// </summary>
        /// <remarks>
        /// {0} CongressId
        /// {1} CongressPresentationTypeId
        /// {2} Title
        /// {3} Code
        /// {4} OwnersName
        /// {5} OwnersSurname
        /// {6} IsActive
        /// {7} Deleted
        /// {8} ShowOn
        /// {9} Skip
        /// {10} Take
        /// </remarks>
        /// 
        public static CacheKey AllByFilters => new CacheKey("WCore.CongressPresentations.GetAll-{0}-{1}-{2}-{3}-{4}-{5}-{6}-{7}-{8}-{9}-{10}", AllByFiltersPrefix);

        /// <summary>
        /// Gets a key pattern to clear cache
        /// </summary>
        public static string AllByFiltersPrefix => "WCore.CongressPresentations";

        #endregion
    }

    /// <summary>
    /// Represents default values related to congress images services
    /// </summary>
    public static partial class WCoreCongressImagesDefaults
    {
        #region Caching defaults

        /// <summary>
        /// Key for activities list model
        /// </summary>
        /// <remarks>
        /// {0} CongressId
        /// {1} Skip
        /// {2} Take
        /// </remarks>
        /// 
        public static CacheKey AllByFilters => new CacheKey("WCore.CongressImages.GetAll-{0}-{1}-{2}", AllByFiltersPrefix);

        /// <summary>
        /// Gets a key pattern to clear cache
        /// </summary>
        public static string AllByFiltersPrefix => "WCore.CongressImages";

        #endregion
    }
}
