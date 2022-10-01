using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WCore.Core.Caching;

namespace WCore.Web.Infrastructure.Cache
{
    public static partial class WCoreModelCacheDefaults
    {
        /// <summary>
        /// Key for nopCommerce.com news cache
        /// </summary>
        public static CacheKey OfficialNewsModelKey => new CacheKey("WCore.pres.admin.official.news");
    }
}
