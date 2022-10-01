using WCore.Core.Caching;

namespace WCore.Services.Users
{
    /// <summary>
    /// Represents default values related to users services
    /// </summary>
    public static partial class WCoreUsersDefaults
    {
        #region Caching defaults

        /// <summary>
        /// Key for users list model
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
        /// {13} : NameSurname
        /// {14} : Email
        /// {15} : UserType
        /// {16} : UserBranchId
        /// {17} : UserCompanyId
        /// {18} : RoleGroupId
        /// {19} : DepartmentId
        /// {20} : ProfessionId
        /// {21} : IsWorking
        /// {22} : UseShift
        /// </remarks>
        public static CacheKey AllByFilters => new CacheKey("WCore.user.getall.filters-{0}-{1}-{2}-{3}-{4}-{5}-{6}-{7}-{8}-{9}-{10}-{11}-{12}-{13}-{14}-{15}-{16}-{17}-{18}-{19}-{20}-{21}-{22}", AllByFiltersPrefix);

        /// <summary>
        /// Key for users count
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
        /// {13} : NameSurname
        /// {14} : Email
        /// {15} : UserType
        /// {16} : UserBranchId
        /// {17} : UserCompanyId
        /// {18} : RoleGroupId
        /// {19} : DepartmentId
        /// {20} : ProfessionId
        /// {21} : IsWorking
        /// {22} : UseShift
        /// </remarks>
        public static CacheKey AllByFilterCount => new CacheKey("WCore.user.getall.filtercount-{0}-{1}-{2}-{3}-{4}-{5}-{6}-{7}-{8}-{9}-{10}-{11}-{12}-{13}-{14}-{15}-{16}-{17}-{18}-{19}-{20}-{21}-{22}", AllByFiltersPrefix);
        
        /// <summary>
        /// Key for user list model
        /// </summary>
        /// <remarks>
        /// {0} : FirstName
        /// </remarks>
        public static CacheKey AllUserByName => new CacheKey("WCore.user.getall.name-{0}", AllByFiltersPrefix);

        /// <summary>
        /// Key for user email model
        /// </summary>
        /// <remarks>
        /// {0} : E-Mail
        /// </remarks>
        public static CacheKey UserByEmail => new CacheKey("WCore.user.getall.email-{0}", AllByFiltersPrefix);

        /// <summary>
        /// Key for user model
        /// </summary>
        /// <remarks>
        /// {0} : E-Mail
        /// </remarks>
        public static CacheKey UserById => new CacheKey("WCore.user.getall.id-{0}", AllByFiltersPrefix);

        /// <summary>
        /// Key for user position model
        /// </summary>
        /// <remarks>
        /// {0} : Position
        /// </remarks>
        public static CacheKey UserByPosition => new CacheKey("WCore.user.getall.position-{0}", AllByFiltersPrefix);

        /// <summary>
        /// Gets a key pattern to clear cache
        /// </summary>
        public static string AllByFiltersPrefix => "WCore.user.getall";

        /// <summary>
        /// Gets a name of generic attribute to store the value of 'AdminAreaStoreScopeConfiguration'
        /// </summary>
        public static string AdminAreaStoreScopeConfigurationAttribute => "AdminAreaStoreScopeConfiguration";

        #endregion
    }

    /// <summary>
    /// Represents default values related to user role services
    /// </summary>
    public static partial class WCoreUserRolesDefaults
    {
        #region Caching defaults

        /// <summary>
        /// Key for user role tag list model
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
        public static CacheKey AllByFilters => new CacheKey("WCore.userrole.getall.filters-{0}-{1}-{2}-{3}-{4}-{5}-{6}-{7}-{8}-{9}-{10}-{11}-{12}-{13}-{14}-{15}-{16}-{17}-{18}-{19}-{20}-{21}-{22}", AllByFiltersPrefix);

        /// <summary>
        /// Gets a key pattern to clear cache
        /// </summary>
        public static string AllByFiltersPrefix => "WCore.userrole.getallfilters";

        #endregion
    }

    /// <summary>
    /// Represents default values related to user role services
    /// </summary>
    public static partial class WCoreUserPersonalInformationsDefaults
    {
        #region Caching defaults

        /// <summary>
        /// Key for user personal informations tag list model
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
        public static CacheKey AllByFilters => new CacheKey("WCore.userpersonalinformation.getall.filters-{0}-{1}-{2}-{3}-{4}-{5}-{6}-{7}-{8}-{9}-{10}-{11}-{12}-{13}-{14}-{15}-{16}-{17}-{18}-{19}-{20}-{21}-{22}", AllByFiltersPrefix);

        /// <summary>
        /// Gets a key pattern to clear cache
        /// </summary>
        public static string AllByFiltersPrefix => "WCore.userpersonalinformation";

        #endregion
    }

    /// <summary>
    /// Represents default values related to user role services
    /// </summary>
    public static partial class WCoreUserFileTypesDefaults
    {
        #region Caching defaults

        /// <summary>
        /// Key for user file types tag list model
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
        public static CacheKey AllByFilters => new CacheKey("WCore.userfiletype.getall.filters-{0}-{1}-{2}-{3}-{4}-{5}-{6}-{7}-{8}-{9}-{10}-{11}-{12}-{13}-{14}-{15}-{16}-{17}-{18}-{19}-{20}-{21}-{22}", AllByFiltersPrefix);

        /// <summary>
        /// Gets a key pattern to clear cache
        /// </summary>
        public static string AllByFiltersPrefix => "WCore.userfiletype";

        #endregion
    }

    /// <summary>
    /// Represents default values related to user role services
    /// </summary>
    public static partial class WCoreUserEvaluationItemsDefaults
    {
        #region Caching defaults

        /// <summary>
        /// Key for user evaluation items tag list model
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
        public static CacheKey AllByFilters => new CacheKey("WCore.userevaluationitem.getall.filters-{0}-{1}-{2}-{3}-{4}-{5}-{6}-{7}-{8}-{9}-{10}-{11}-{12}-{13}-{14}-{15}-{16}-{17}-{18}-{19}-{20}-{21}-{22}", AllByFiltersPrefix);

        /// <summary>
        /// Gets a key pattern to clear cache
        /// </summary>
        public static string AllByFiltersPrefix => "WCore.userevaluationitem";

        #endregion
    }

    /// <summary>
    /// Represents default values related to user evaluations services
    /// </summary>
    public static partial class WCoreUserEvaluationsDefaults
    {
        #region Caching defaults

        /// <summary>
        /// Key for user evaluations tag list model
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
        public static CacheKey AllByFilters => new CacheKey("WCore.userevaluation.getall.filters-{0}-{1}-{2}-{3}-{4}-{5}-{6}-{7}-{8}-{9}-{10}-{11}-{12}-{13}-{14}-{15}-{16}-{17}-{18}-{19}-{20}-{21}-{22}", AllByFiltersPrefix);

        /// <summary>
        /// Gets a key pattern to clear cache
        /// </summary>
        public static string AllByFiltersPrefix => "WCore.userevaluation";

        #endregion
    }

    /// <summary>
    /// Represents default values related to user evaluations services
    /// </summary>
    public static partial class WCoreUserFilesDefaults
    {
        #region Caching defaults

        /// <summary>
        /// Key for user files tag list model
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
        public static CacheKey AllByFilters => new CacheKey("WCore.userfile.getall.filters-{0}-{1}-{2}-{3}-{4}-{5}-{6}-{7}-{8}-{9}-{10}-{11}-{12}-{13}-{14}-{15}-{16}-{17}-{18}-{19}-{20}-{21}-{22}", AllByFiltersPrefix);

        /// <summary>
        /// Gets a key pattern to clear cache
        /// </summary>
        public static string AllByFiltersPrefix => "WCore.userfile";

        #endregion
    }
}
