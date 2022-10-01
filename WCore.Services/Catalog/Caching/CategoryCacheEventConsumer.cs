using WCore.Core.Domain.Catalog;
using WCore.Services.Caching;
using WCore.Services.Discounts;

namespace WCore.Services.Catalog.Caching
{
    /// <summary>
    /// Represents a category cache event consumer
    /// </summary>
    public partial class CategoryCacheEventConsumer : CacheEventConsumer<Category>
    {
        /// <summary>
        /// Clear cache data
        /// </summary>
        /// <param name="entity">Entity</param>
        protected override void ClearCache(Category entity)
        {
            var prefix = _cacheKeyService.PrepareKeyPrefix(WCoreCatalogDefaults.CategoriesByParentCategoryPrefixCacheKey, entity);
            RemoveByPrefix(prefix);
            prefix = _cacheKeyService.PrepareKeyPrefix(WCoreCatalogDefaults.CategoriesByParentCategoryPrefixCacheKey, entity.ParentCategoryId);
            RemoveByPrefix(prefix);

            prefix = _cacheKeyService.PrepareKeyPrefix(WCoreCatalogDefaults.CategoriesChildIdentifiersPrefixCacheKey, entity);
            RemoveByPrefix(prefix);
            prefix = _cacheKeyService.PrepareKeyPrefix(WCoreCatalogDefaults.CategoriesChildIdentifiersPrefixCacheKey, entity.ParentCategoryId);
            RemoveByPrefix(prefix);
            
            RemoveByPrefix(WCoreCatalogDefaults.CategoriesDisplayedOnHomepagePrefixCacheKey);
            RemoveByPrefix(WCoreCatalogDefaults.CategoriesAllPrefixCacheKey);
            RemoveByPrefix(WCoreCatalogDefaults.CategoryBreadcrumbPrefixCacheKey);
            
            RemoveByPrefix(WCoreCatalogDefaults.CategoryNumberOfProductsPrefixCacheKey);

            RemoveByPrefix(WCoreDiscountDefaults.DiscountCategoryIdsPrefixCacheKey);
        }
    }
}
