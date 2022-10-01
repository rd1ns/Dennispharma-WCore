using WCore.Core.Caching;

namespace WCore.Services.Menus
{
    /// <summary>
    /// Represents default values related to menus services
    /// </summary>
    public static partial class WCoreMenusDefaults
    {
        #region Caching defaults

        /// <summary>
        /// Key for menus list model
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
        /// {13} : Name
        /// {14} : IsHidden
        /// {15} : ParentId
        /// </remarks>
        public static CacheKey AllByFilters => new CacheKey("WCore.menu.getall.filters-{0}-{1}-{2}-{3}-{4}-{5}-{6}-{7}-{8}-{9}-{10}-{11}-{12}-{13}-{14}-{15}", AllByFiltersPrefix);

        /// <summary>
        /// Key for menus model
        /// </summary>
        /// <remarks>
        /// {0} : MenuId
        /// {1} : ControllerName
        /// {2} : ActionName
        /// {3} : AreaName
        /// </remarks>
        public static CacheKey MenuByParentId => new CacheKey("WCore.menu.getall.menubyparentid-{0}-{1}-{2}-{3}", AllByFiltersPrefix);

        /// <summary>
        /// Key for menus model
        /// </summary>
        /// <remarks>
        /// {0} : ParentId
        /// {1} : IsActive
        /// {2} : Skip
        /// {3} : Take
        /// </remarks>
        public static CacheKey MenuById => new CacheKey("WCore.menu.getall.menubyid-{0}-{1}-{2}-{3}", AllByFiltersPrefix);

        /// <summary>
        /// Key for menus model
        /// </summary>
        /// <remarks>
        /// {0} : RoleGroupId
        /// {1} : ParentId
        /// </remarks>
        public static CacheKey UserMenuByParentId => new CacheKey("WCore.menu.getall.usermenubyparentid-{0}-{1}", AllByFiltersPrefix);

        /// <summary>
        /// Gets a key pattern to clear cache
        /// </summary>
        public static string AllByFiltersPrefix => "WCore.menu.getall";

        #endregion
    }
}
