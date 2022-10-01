using WCore.Core.Caching;
using System;
using System.Collections.Generic;
using System.Text;

namespace WCore.Services.Common
{
    /// <summary>
    /// Represents default values related to template services
    /// </summary>
    public static partial class WCoreTemplatesDefaults
    {
        #region Caching defaults

        /// <summary>
        /// Key for countries list model
        /// </summary>
        /// <remarks>
        /// {0} : userId
        /// {1} : SettingStatus
        /// {2} : Take
        /// {3} : Skip
        /// </remarks>
        public static CacheKey AllByFilters => new CacheKey("WCore.template.getall.filters-{0}-{1}-{2}-{3}", AllByFiltersPrefix);

        /// <summary>
        /// Gets a key pattern to clear cache
        /// </summary>
        public static string AllByFiltersPrefix => "WCore.template";

        #endregion
    }
}
