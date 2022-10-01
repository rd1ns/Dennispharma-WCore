using WCore.Core.Domain.Vendors;
using WCore.Services.Caching;

namespace WCore.Services.Vendors.Caching
{
    /// <summary>
    /// Represents a vendor cache event consumer
    /// </summary>
    public partial class VendorCacheEventConsumer : CacheEventConsumer<Vendor>
    {
    }
}
