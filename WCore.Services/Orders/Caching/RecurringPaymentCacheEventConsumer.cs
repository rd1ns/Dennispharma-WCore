using WCore.Core.Domain.Orders;
using WCore.Services.Caching;

namespace WCore.Services.Orders.Caching
{
    /// <summary>
    /// Represents a recurring payment cache event consumer
    /// </summary>
    public partial class RecurringPaymentCacheEventConsumer : CacheEventConsumer<RecurringPayment>
    { 
    }
}
