using WCore.Core.Caching;

namespace WCore.Services.Roles
{
    /// <summary>
    /// Represents default values related to roles services
    /// </summary>
    public static partial class WCoreRolesDefaults
    {
        #region Caching defaults

        /// <summary>
        /// Key for roles tag list model
        /// </summary>
        /// <remarks>
        /// {0} : Page
        /// {1} : PageSize
        /// {2} : AvailablePageSizes
        /// {3} : Draw
        /// {4} : Start
        /// {5} : Length
        /// {6} : Take
        /// {7} : Skip
        /// {8} : AuthorizedUserId
        /// {9} : UserId
        /// {10} : CreatedUserId
        /// {11} : Deleted
        /// {12} : IsActive
        /// {13} : ActivityName
        /// {14} : ActivityEvaluation
        /// {15} : ActivityType
        /// {16} : CompanyId
        /// {17} : IsPlanned
        /// {18} : PlannedStartDate
        /// {19} : PlannedEndDate
        /// {20} : IsReminder
        /// {21} : ReminderDate
        /// {22} : RelatedUserId
        /// </remarks>
        public static CacheKey AllByFilters => new CacheKey("WCore.role.getall.filters-{0}-{1}-{2}-{3}-{4}-{5}-{6}-{7}-{8}-{9}-{10}-{11}-{12}-{13}-{14}-{15}-{16}-{17}-{18}-{19}-{20}-{21}-{22}", AllByFiltersPrefix);

        /// <summary>
        /// Gets a key pattern to clear cache
        /// </summary>
        public static string AllByFiltersPrefix => "WCore.role";

        #endregion
    }

    /// <summary>
    /// Represents default values related to role groups services
    /// </summary>
    public static partial class WCoreRoleGroupsDefaults
    {
        #region Caching defaults

        /// <summary>
        /// Key for role groups tag list model
        /// </summary>
        /// <remarks>
        /// {0} : Page
        /// {1} : PageSize
        /// {2} : AvailablePageSizes
        /// {3} : Draw
        /// {4} : Start
        /// {5} : Length
        /// {6} : Take
        /// {7} : Skip
        /// {8} : AuthorizedUserId
        /// {9} : UserId
        /// {10} : CreatedUserId
        /// {11} : Deleted
        /// {12} : IsActive
        /// {13} : ActivityName
        /// {14} : ActivityEvaluation
        /// {15} : ActivityType
        /// {16} : CompanyId
        /// {17} : IsPlanned
        /// {18} : PlannedStartDate
        /// {19} : PlannedEndDate
        /// {20} : IsReminder
        /// {21} : ReminderDate
        /// {22} : RelatedUserId
        /// </remarks>
        public static CacheKey AllByFilters => new CacheKey("WCore.rolegroup.getall.filters-{0}-{1}-{2}-{3}-{4}-{5}-{6}-{7}-{8}-{9}-{10}-{11}-{12}-{13}-{14}-{15}-{16}-{17}-{18}-{19}-{20}-{21}-{22}", AllByFiltersPrefix);

        /// <summary>
        /// Gets a key pattern to clear cache
        /// </summary>
        public static string AllByFiltersPrefix => "WCore.rolegroup";

        #endregion
    }

    /// <summary>
    /// Represents default values related to temproles services
    /// </summary>
    public static partial class WCoreTempRolesDefaults
    {
        #region Caching defaults

        /// <summary>
        /// Key for temproles tag list model
        /// </summary>
        /// <remarks>
        /// {0} : Page
        /// {1} : PageSize
        /// {2} : AvailablePageSizes
        /// {3} : Draw
        /// {4} : Start
        /// {5} : Length
        /// {6} : Take
        /// {7} : Skip
        /// {8} : AuthorizedUserId
        /// {9} : UserId
        /// {10} : CreatedUserId
        /// {11} : Deleted
        /// {12} : IsActive
        /// {13} : ActivityName
        /// {14} : ActivityEvaluation
        /// {15} : ActivityType
        /// {16} : CompanyId
        /// {17} : IsPlanned
        /// {18} : PlannedStartDate
        /// {19} : PlannedEndDate
        /// {20} : IsReminder
        /// {21} : ReminderDate
        /// {22} : RelatedUserId
        /// </remarks>
        public static CacheKey AllByFilters => new CacheKey("WCore.temprole.getall.filters-{0}-{1}-{2}-{3}-{4}-{5}-{6}-{7}-{8}-{9}-{10}-{11}-{12}-{13}-{14}-{15}-{16}-{17}-{18}-{19}-{20}-{21}-{22}", AllByFiltersPrefix);

        /// <summary>
        /// Gets a key pattern to clear cache
        /// </summary>
        public static string AllByFiltersPrefix => "WCore.temprole";

        #endregion
    }

    /// <summary>
    /// Represents default values related to temprole groups services
    /// </summary>
    public static partial class WCoreTempRoleGroupsDefaults
    {
        #region Caching defaults

        /// <summary>
        /// Key for temprole groups tag list model
        /// </summary>
        /// <remarks>
        /// {0} : Page
        /// {1} : PageSize
        /// {2} : AvailablePageSizes
        /// {3} : Draw
        /// {4} : Start
        /// {5} : Length
        /// {6} : Take
        /// {7} : Skip
        /// {8} : AuthorizedUserId
        /// {9} : UserId
        /// {10} : CreatedUserId
        /// {11} : Deleted
        /// {12} : IsActive
        /// {13} : ActivityName
        /// {14} : ActivityEvaluation
        /// {15} : ActivityType
        /// {16} : CompanyId
        /// {17} : IsPlanned
        /// {18} : PlannedStartDate
        /// {19} : PlannedEndDate
        /// {20} : IsReminder
        /// {21} : ReminderDate
        /// {22} : RelatedUserId
        /// </remarks>
        public static CacheKey AllByFilters => new CacheKey("WCore.temprolegroup.getall.filters-{0}-{1}-{2}-{3}-{4}-{5}-{6}-{7}-{8}-{9}-{10}-{11}-{12}-{13}-{14}-{15}-{16}-{17}-{18}-{19}-{20}-{21}-{22}", AllByFiltersPrefix);

        /// <summary>
        /// Gets a key pattern to clear cache
        /// </summary>
        public static string AllByFiltersPrefix => "WCore.temprolegroup";

        #endregion
    }
}
