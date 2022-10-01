using WCore.Core.Domain.Messages;
using WCore.Services.Caching;

namespace WCore.Services.Messages.Caching
{
    /// <summary>
    /// Represents an email account cache event consumer
    /// </summary>
    public partial class EmailAccountCacheEventConsumer : CacheEventConsumer<EmailAccount>
    {
        /// <summary>
        /// Clear cache data
        /// </summary>
        /// <param name="entity">Entity</param>
        protected override void ClearCache(EmailAccount entity)
        {
            Remove(WCoreMessageDefaults.EmailAccountsAllCacheKey);
        }
    }
}
