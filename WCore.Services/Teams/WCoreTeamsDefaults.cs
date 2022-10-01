using WCore.Core.Caching;

namespace WCore.Services.Teams
{
    /// <summary>
    /// Represents default values related to congress services
    /// </summary>
    public static partial class WCoreTeamsDefaults
    {
        #region Caching defaults

        /// <summary>
        /// Key for activities list model
        /// </summary>
        /// <remarks>
        /// {0} : TeamCategoryId
        /// {1} : Name
        /// {2} : Surname
        /// {3} : IsActive
        /// {4} : Deleted
        /// {5} : ShowOn
        /// {6} : ShowOnHome
        /// {7} : Skip
        /// {8} : Take
        /// </remarks>
        /// 
        public static CacheKey AllByFilters => new CacheKey("WCore.Team.GetAll.filters-{0}-{1}-{2}-{3}-{4}-{5}-{6}-{7}-{8}", AllByFiltersPrefix);

        /// <summary>
        /// Gets a key pattern to clear cache
        /// </summary>
        public static string AllByFiltersPrefix => "WCore.Team";

        #endregion
    }

    /// <summary>
    /// Represents default values related to congress paper types services
    /// </summary>
    public static partial class WCoreTeamCategoriesDefaults
    {
        #region Caching defaults

        /// <summary>
        /// Key for activities list model
        /// </summary>
        /// <remarks>
        /// {0} : ParrentId
        /// {1} : Title
        /// {2} : IsActive
        /// {3} : Deleted
        /// {4} : ShowOn
        /// {5} : Skip
        /// {6} : Take
        /// </remarks>
        /// 
        public static CacheKey AllByFilters => new CacheKey("WCore.TeamCategory.GetAll-{0}-{1}-{2}-{3}-{4}-{5}-{6}", AllByFiltersPrefix);

        /// <summary>
        /// Gets a key pattern to clear cache
        /// </summary>
        public static string AllByFiltersPrefix => "WCore.TeamCategory";

        #endregion
    }
}
