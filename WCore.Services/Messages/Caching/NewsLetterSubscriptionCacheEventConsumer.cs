using WCore.Core.Domain.Messages;
using WCore.Services.Caching;

namespace WCore.Services.Messages.Caching
{
    /// <summary>
    /// Represents news letter subscription cache event consumer
    /// </summary>
    public partial class NewsLetterSubscriptionCacheEventConsumer : CacheEventConsumer<NewsLetterSubscription>
    {    
    }
}
