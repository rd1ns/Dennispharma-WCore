using WCore.Core.Domain.Vendors;
using WCore.Services.Caching;

namespace WCore.Services.Vendors.Caching
{
    /// <summary>
    /// Represents a vendor note cache event consumer
    /// </summary>
    public partial class VendorNoteCacheEventConsumer : CacheEventConsumer<VendorNote>
    {
    }
}
