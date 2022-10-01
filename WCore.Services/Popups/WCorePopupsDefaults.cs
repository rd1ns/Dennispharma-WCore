using WCore.Core.Caching;
using System;
using System.Collections.Generic;
using System.Text;

namespace WCore.Services.Common
{
    /// <summary>
    /// Represents default values related to Popup services
    /// </summary>
    public static partial class WCorePopupsDefaults
    {
        #region Caching defaults

        /// <summary>
        /// Key for countries list model
        /// </summary>
        /// <remarks>
        /// {0} : ShowUrl
        /// {1} : ShowOn
        /// {2} : Skip 
        /// {3} : Take 
        /// </remarks>
        public static CacheKey AllByFilters => new CacheKey("WCore.Popup.getall.filters-{0}-{1}-{2}-{3}", AllByFiltersPrefix);

        /// <summary>
        /// Gets a key pattern to clear cache
        /// </summary>
        public static string AllByFiltersPrefix => "WCore.Popup";

        #endregion
    }
}
