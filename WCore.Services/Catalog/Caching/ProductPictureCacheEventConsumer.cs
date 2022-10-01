using WCore.Core.Domain.Catalog;
using WCore.Services.Caching;

namespace WCore.Services.Catalog.Caching
{
    /// <summary>
    /// Represents a product picture mapping cache event consumer
    /// </summary>
    public partial class ProductPictureCacheEventConsumer : CacheEventConsumer<ProductPicture>
    {
    }
}
