using WCore.Core.Domain.Tax;
using WCore.Services.Caching;

namespace WCore.Services.Tax.Caching
{
    /// <summary>
    /// Represents a TAX category cache event consumer
    /// </summary>
    public partial class TaxCategoryCacheEventConsumer : CacheEventConsumer<TaxCategory>
    {
        /// <summary>
        /// Clear cache data
        /// </summary>
        /// <param name="entity">Entity</param>
        protected override void ClearCache(TaxCategory entity)
        {
            Remove(WCoreTaxDefaults.TaxCategoriesAllCacheKey);
        }
    }
}
