using WCore.Core.Domain.Orders;
using WCore.Services.Caching;

namespace WCore.Services.Orders.Caching
{
    /// <summary>
    /// Represents a return request reason cache event consumer
    /// </summary>
    public partial class ReturnRequestReasonCacheEventConsumer : CacheEventConsumer<ReturnRequestReason>
    {
        protected override void ClearCache(ReturnRequestReason entity)
        {
            Remove(WCoreOrderDefaults.ReturnRequestReasonAllCacheKey);
        }
    }
}
