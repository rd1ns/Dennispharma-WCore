using WCore.Core.Domain.Catalog;
using WCore.Services.Caching;

namespace WCore.Services.Catalog.Caching
{
    /// <summary>
    /// Represents a product warehouse inventory cache event consumer
    /// </summary>
    public partial class ProductWarehouseInventoryCacheEventConsumer : CacheEventConsumer<ProductWarehouseInventory>
    {
    }
}
