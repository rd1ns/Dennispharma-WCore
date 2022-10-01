using WCore.Core.Domain.Orders;
using WCore.Services.Caching;

namespace WCore.Services.Orders.Caching
{
    /// <summary>
    /// Represents a return request cache event consumer
    /// </summary>
    public partial class ReturnRequestCacheEventConsumer : CacheEventConsumer<ReturnRequest>
    {
    }
}
