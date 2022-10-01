using WCore.Core.Caching;
using System;
using System.Collections.Generic;
using System.Text;

namespace WCore.Services.Common
{
    /// <summary>
    /// Represents default values related to common services
    /// </summary>
    public static partial class WCoreCommonDefaults
    {
        /// <summary>
        /// Gets a request path to the keep alive URL
        /// </summary>
        public static string KeepAlivePath => "keepalive/index";

        #region Address attributes

        /// <summary>
        /// Gets a key for caching
        /// </summary>
        public static string AddressAttributesAllCacheKey => "WCore.addressattribute.all";

        /// <summary>
        /// Gets a key for caching
        /// </summary>
        /// <remarks>
        /// {0} : address attribute ID
        /// </remarks>
        public static string AddressAttributesByIdCacheKey => "WCore.addressattribute.id-{0}";

        /// <summary>
        /// Gets a key for caching
        /// </summary>
        /// <remarks>
        /// {0} : address attribute ID
        /// </remarks>
        public static CacheKey AddressAttributeValuesAllCacheKey => new CacheKey("WCore.country.getall.filters-{0}", AddressAttributeValuesAllPrefixCacheKey);
        /// <summary>
        /// Gets a key pattern to clear cache
        /// </summary>
        public static string AddressAttributeValuesAllPrefixCacheKey => "addressattributevalue.all";

        /// <summary>
        /// Gets a key for caching
        /// </summary>
        /// <remarks>
        /// {0} : address attribute value ID
        /// </remarks>
        public static string AddressAttributeValuesByIdCacheKey => "WCore.addressattributevalue.id-{0}";

        /// <summary>
        /// Gets a key pattern to clear cache
        /// </summary>
        public static string AddressAttributesPrefixCacheKey => "WCore.addressattribute.";

        /// <summary>
        /// Gets a key pattern to clear cache
        /// </summary>
        public static string AddressAttributeValuesPrefixCacheKey => "WCore.addressattributevalue.";

        /// <summary>
        /// Gets a name of the custom address attribute control
        /// </summary>
        /// <remarks>
        /// {0} : address attribute id
        /// </remarks>
        public static string AddressAttributeControlName => "address_attribute_{0}";

        #endregion

        #region Addresses

        /// <summary>
        /// Gets a key for caching
        /// </summary>
        /// <remarks>
        /// {0} : address ID
        /// </remarks>
        public static string AddressesByIdCacheKey => "WCore.address.id-{0}";

        /// <summary>
        /// Gets a key pattern to clear cache
        /// </summary>
        public static string AddressesPrefixCacheKey => "WCore.address.";

        #endregion

        #region Generic attributes

        /// <summary>
        /// Gets a key for caching
        /// </summary>
        /// <remarks>
        /// {0} : entity ID
        /// {1} : key group
        /// </remarks>
        public static CacheKey GenericAttributeCacheKey => new CacheKey("WCore.genericattribute.{0}-{1}");

        #endregion

        #region Maintenance

        /// <summary>
        /// Gets a path to the database backup files
        /// </summary>
        public static string DbBackupsPath => "db_backups\\";

        /// <summary>
        /// Gets a database backup file extension
        /// </summary>
        public static string DbBackupFileExtension => "bak";

        #endregion

        #region Favicon and app icons

        /// <summary>
        /// Gets a name of the file with code for the head element
        /// </summary>
        public static string HeadCodeFileName => "html_code.html";

        /// <summary>
        /// Gets a path to the favicon and app icons
        /// </summary>
        public static string FaviconAndAppIconsPath => "icons\\icons_{0}";

        /// <summary>
        /// Gets a name of the old favicon icon for current store
        /// </summary>
        public static string OldFaviconIconName => "favicon-{0}.ico";

        #endregion

        #region WCoreCommerce official site

        /// <summary>
        /// Gets a path to request the WCoreCommerce official site for copyright warning
        /// </summary>
        /// <remarks>
        /// {0} : store URL
        /// {1} : whether the store based is on the localhost
        /// </remarks>
        public static string WCoreCopyrightWarningPath => "SiteWarnings.aspx?local={0}&url={1}";

        /// <summary>
        /// Gets a path to request the WCoreCommerce official site for news RSS
        /// </summary>
        /// <remarks>
        /// {0} : WCoreCommerce version
        /// {1} : whether the store based is on the localhost
        /// {2} : whether advertisements are hidden
        /// {3} : store URL
        /// </remarks>
        public static string WCoreNewsRssPath => "NewsRSS.aspx?Version={0}&Localhost={1}&HideAdvertisements={2}&StoreURL={3}";

        /// <summary>
        /// Gets a path to request the WCoreCommerce official site for available categories of marketplace extensions
        /// </summary>
        public static string WCoreExtensionsCategoriesPath => "ExtensionsXml.aspx?getCategories=1";

        /// <summary>
        /// Gets a path to request the WCoreCommerce official site for available versions of marketplace extensions
        /// </summary>
        public static string WCoreExtensionsVersionsPath => "ExtensionsXml.aspx?getVersions=1";

        /// <summary>
        /// Gets a path to request the WCoreCommerce official site for marketplace extensions
        /// </summary>
        /// <remarks>
        /// {0} : extension category identifier
        /// {1} : extension version identifier
        /// {2} : extension price identifier
        /// {3} : search term
        /// {4} : page index
        /// {5} : page size
        /// </remarks>
        public static string WCoreExtensionsPath => "ExtensionsXml.aspx?category={0}&version={1}&price={2}&searchTerm={3}&pageIndex={4}&pageSize={5}";

        #endregion
    }

    /// <summary>
    /// Represents default values related to country services
    /// </summary>
    public static partial class WCoreCountriesDefaults
    {
        #region Caching defaults

        /// <summary>
        /// Key for countries list model
        /// </summary>
        /// <remarks>
        /// {0} : Page
        /// {1} : PageSize
        /// {2} : AvailablePageSizes
        /// {3} : Draw
        /// {4} : Start
        /// {5} : Length
        /// {6} : Take
        /// {7} : Skip
        /// {8} : AuthorizedUserId
        /// {9} : UserId
        /// {10} : CreatedUserId
        /// {11} : Deleted
        /// {12} : IsActive
        /// {13} : Name
        /// </remarks>
        public static CacheKey AllByFilters => new CacheKey("WCore.country.getall.filters-{0}-{1}-{2}-{3}-{4}-{5}-{6}-{7}-{8}-{9}-{10}-{11}-{12}-{13}", AllByFiltersPrefix);

        /// <summary>
        /// Gets a key pattern to clear cache
        /// </summary>
        public static string AllByFiltersPrefix => "WCore.country";

        #endregion
    }

    /// <summary>
    /// Represents default values related to cities services
    /// </summary>
    public static partial class WCoreCitiesDefaults
    {
        #region Caching defaults

        /// <summary>
        /// Key for cars list model
        /// </summary>
        /// <remarks>
        /// {0} : Page
        /// {1} : PageSize
        /// {2} : AvailablePageSizes
        /// {3} : Draw
        /// {4} : Start
        /// {5} : Length
        /// {6} : Take
        /// {7} : Skip
        /// {8} : AuthorizedUserId
        /// {9} : UserId
        /// {10} : CreatedUserId
        /// {11} : Deleted
        /// {12} : IsActive
        /// {13} : Name
        /// {14} : CountryId
        /// </remarks>
        public static CacheKey AllByFilters => new CacheKey("WCore.city.getall.filters-{0}-{1}-{2}-{3}-{4}-{5}-{6}-{7}-{8}-{9}-{10}-{11}-{12}-{13}-{14}", AllByFiltersPrefix);

        /// <summary>
        /// Gets a key pattern to clear cache
        /// </summary>
        public static string AllByFiltersPrefix => "WCore.city";

        #endregion
    }

    /// <summary>
    /// Represents default values related to cities services
    /// </summary>
    public static partial class WCoreDistrictsDefaults
    {
        #region Caching defaults

        /// <summary>
        /// Key for districts list model
        /// </summary>
        /// <remarks>
        /// {0} : Page
        /// {1} : PageSize
        /// {2} : AvailablePageSizes
        /// {3} : Draw
        /// {4} : Start
        /// {5} : Length
        /// {6} : Take
        /// {7} : Skip
        /// {8} : AuthorizedUserId
        /// {9} : UserId
        /// {10} : CreatedUserId
        /// {11} : Deleted
        /// {12} : IsActive
        /// {13} : Name
        /// {14} : CountryId
        /// {15} : CityId
        /// </remarks>
        public static CacheKey AllByFilters => new CacheKey("WCore.district.getall.filters-{0}-{1}-{2}-{3}-{4}-{5}-{6}-{7}-{8}-{9}-{10}-{11}-{12}-{13}-{14}-{15}", AllByFiltersPrefix);

        /// <summary>
        /// Gets a key pattern to clear cache
        /// </summary>
        public static string AllByFiltersPrefix => "WCore.district";

        #endregion
    }
}
