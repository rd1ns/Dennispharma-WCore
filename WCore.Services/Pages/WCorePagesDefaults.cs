using WCore.Core.Caching;
using System;
using System.Collections.Generic;
using System.Text;

namespace WCore.Services.Common
{
    /// <summary>
    /// Represents default values related to page services
    /// </summary>
    public static partial class WCorePagesDefaults
    {
        #region Caching defaults

        /// <summary>
        /// Key for countries list model
        /// </summary>
        /// <remarks>
        /// {0} : SearchValue
        /// {1} : PageType
        /// {2} : FooterLocation
        /// {3} : ParentId
        /// {4} : IsActive
        /// {5} : Deleted
        /// {6} : ShowOn
        /// {7} : HomePage
        /// {8} : RedirectPage
        /// {9} : Skip
        /// {10} : Take
        /// </remarks>
        public static CacheKey AllByFilters => new CacheKey("WCore.page.all-{0}-{1}-{2}-{3}-{4}-{5}-{6}-{7}-{8}-{9}-{10}", AllByFiltersPrefix);

        /// <summary>
        /// Key for home page model
        /// </summary>
        public static CacheKey HomePage => new CacheKey("WCore.page.HomePage", AllByFiltersPrefix);

        /// <summary>
        /// Gets a key pattern to clear cache
        /// </summary>
        public static string AllByFiltersPrefix => "WCore.page";

        #endregion
    }
}
