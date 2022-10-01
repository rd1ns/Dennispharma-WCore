using WCore.Core.Domain.Messages;
using WCore.Services.Caching;

namespace WCore.Services.Messages.Caching
{
    /// <summary>
    /// Represents a message template cache event consumer
    /// </summary>
    public partial class MessageTemplateCacheEventConsumer : CacheEventConsumer<MessageTemplate>
    {
        /// <summary>
        /// Clear cache data
        /// </summary>
        /// <param name="entity">Entity</param>
        protected override void ClearCache(MessageTemplate entity)
        {
            RemoveByPrefix(WCoreMessageDefaults.MessageTemplatesAllPrefixCacheKey);
            var prefix = _cacheKeyService.PrepareKeyPrefix(WCoreMessageDefaults.MessageTemplatesByNamePrefixCacheKey, entity.Name);
            RemoveByPrefix(prefix);
        }
    }
}
