using WCore.Core.Caching;

namespace WCore.Services.Tax
{
    /// <summary>
    /// Represents default values related to tax services
    /// </summary>
    public static partial class WCoreTaxDefaults
    {
        #region Caching defaults

        /// <summary>
        /// Gets a key for caching
        /// </summary>
        public static CacheKey TaxCategoriesAllCacheKey => new CacheKey("WCore.taxcategory.all");

        #endregion
    }
}