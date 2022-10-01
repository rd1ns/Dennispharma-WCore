using WCore.Core.Caching;
using WCore.Core.Domain.Localization;

namespace WCore.Services.Localization
{
    /// <summary>
    /// Represents default values related to localization services
    /// </summary>
    public static partial class WCoreLocalizationDefaults
    {
        #region Locales

        /// <summary>
        /// Gets a prefix of locale resources for the admin area
        /// </summary>
        public static string AdminLocaleStringResourcesPrefix => "Admin.";

        /// <summary>
        /// Gets a prefix of locale resources for enumerations 
        /// </summary>
        public static string EnumLocaleStringResourcesPrefix => "Enums.";

        /// <summary>
        /// Gets a prefix of locale resources for permissions 
        /// </summary>
        public static string PermissionLocaleStringResourcesPrefix => "Permission.";

        /// <summary>
        /// Gets a prefix of locale resources for plugin friendly names 
        /// </summary>
        public static string PluginNameLocaleStringResourcesPrefix => "Plugins.FriendlyName.";

        #endregion

        #region Caching defaults

        #region Languages

        /// <summary>
        /// Gets a key for caching
        /// </summary>
        public static CacheKey DefaultLanguageCacheKey => new CacheKey("WCore.language.isdefault", LanguagesByStorePrefix, WCoreEntityCacheDefaults<Language>.AllPrefix);
        /// <summary>
        /// Gets a key for caching
        /// </summary>
        public static CacheKey AdminDefaultLanguageCacheKey => new CacheKey("WCore.language.isadmindefault", LanguagesByStorePrefix, WCoreEntityCacheDefaults<Language>.AllPrefix);
        public static CacheKey LanguagesAllWithoutParametersCacheKey => new CacheKey("WCore.language.all", LanguagesByStorePrefix, WCoreEntityCacheDefaults<Language>.AllPrefix);
        public static CacheKey AllCountCachKey => new CacheKey("WCore.language.all.count", LanguagesByStorePrefix, WCoreEntityCacheDefaults<Language>.AllPrefix);

        /// <summary>
        /// Gets a key for caching
        /// </summary>
        /// <remarks>
        /// {0} : store ID
        /// {1} : show hidden records?
        /// </remarks>
        public static CacheKey LanguagesAllCacheKey => new CacheKey("WCore.language.all.{0}-{1}", LanguagesByStorePrefix, WCoreEntityCacheDefaults<Language>.AllPrefix);

        /// <summary>
        /// Gets a key pattern to clear cache
        /// </summary>
        /// <remarks>
        /// {0} : store ID
        /// </remarks>
        public static string LanguagesByStoreIdPrefixCacheKey => "WCore.language.all-{0}";
        /// <summary>
        /// Gets a key pattern to clear cache
        /// </summary>
        /// <remarks>
        /// {0} : store ID
        /// </remarks>
        public static string LanguagesByStorePrefix => "WCore.language.all.{0}";

        #endregion        

        #region Localized properties

        /// <summary>
        /// Gets a key for caching
        /// </summary>
        /// <remarks>
        /// {0} : language ID
        /// {1} : entity ID
        /// {2} : locale key group
        /// {3} : locale key
        /// </remarks>
        public static CacheKey LocalizedPropertyCacheKey => new CacheKey("WCore.localizedproperty.value.{0}-{1}-{2}-{3}");

        #endregion

        #endregion
    }

    /// <summary>
    /// Represents default values related to page services
    /// </summary>
    public static partial class WCoreLocaleStringResourceDefaults
    {
        #region Caching defaults
        /// <summary>
        /// Key for LocaleStringResource model
        /// </summary>
        /// <remarks>
        /// {0} : LanguageId
        /// </remarks>
        public static CacheKey ByLanguage => new CacheKey("WCore.localestringresource.bylanguage.{0}", AllByFiltersPrefix);
        /// <summary>
        /// Key for LocaleStringResource model
        /// </summary>
        /// <remarks>
        /// {0} : Name
        /// </remarks>
        public static CacheKey ByName => new CacheKey("WCore.localestringresource.byname.{0}", AllByFiltersPrefix);

        /// <summary>
        /// Key for LocaleStringResources model
        /// </summary>
        /// <remarks>
        /// {0} : LanguageId
        /// {1} : ResourceKey
        /// </remarks>
        public static CacheKey LocaleStringResourcesByNameCacheKey => new CacheKey("WCore.localestringresource.byname.{0}-{1}", AllByFiltersPrefix);
        /// <summary>
        /// Key for LocaleStringResources model
        /// </summary>
        /// <remarks>
        /// {0} : LanguageId
        /// </remarks>
        public static CacheKey LocaleStringResourcesAllPublicCacheKey => new CacheKey("WCore.localestringresource.bylanguage.public.{0}", AllByFiltersPrefix);
        /// <summary>
        /// Key for LocaleStringResources model
        /// </summary>
        /// <remarks>
        /// {0} : LanguageId
        /// </remarks>
        public static CacheKey LocaleStringResourcesAllAdminCacheKey => new CacheKey("WCore.localestringresource.bylanguage.admin.{0}", AllByFiltersPrefix);

        /// <summary>
        /// Key for home page model
        /// </summary>
        public static CacheKey HomePage => new CacheKey("WCore.localestringresource.all-HomePage", AllByFiltersPrefix);

        /// <summary>
        /// Gets a key pattern to clear cache
        /// </summary>
        public static string AllByFiltersPrefix => "WCore.localestringresource";

        #endregion
    }
}
