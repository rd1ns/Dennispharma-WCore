using System;
using System.Collections.Generic;
using System.Text;
using WCore.Core.Caching;

namespace WCore.Services.Users
{
    /// <summary>
    /// Represents default values related to user services
    /// </summary>
    public static partial class WCoreUserServicesDefaults
    {
        /// <summary>
        /// Gets a password salt key size
        /// </summary>
        public static int PasswordSaltKeySize => 5;

        /// <summary>
        /// Gets a max username length
        /// </summary>
        public static int UserUsernameLength => 100;

        /// <summary>
        /// Gets a default hash format for user password
        /// </summary>
        public static string DefaultHashedPasswordFormat => "SHA512";

        /// <summary>
        /// Gets default prefix for user
        /// </summary>
        public static string UserAttributePrefix => "user_attribute_";

        #region Caching defaults

        #region User attributes

        /// <summary>
        /// Gets a key for caching
        /// </summary>
        public static CacheKey UserAttributesAllCacheKey => new CacheKey("WCore.userattribute.all");

        /// <summary>
        /// Gets a key for caching
        /// </summary>
        /// <remarks>
        /// {0} : user attribute ID
        /// </remarks>
        public static CacheKey UserAttributeValuesAllCacheKey => new CacheKey("WCore.userattributevalue.all-{0}");

        #endregion

        #region User roles

        /// <summary>
        /// Gets a key for caching
        /// </summary>
        /// <remarks>
        /// {0} : show hidden records?
        /// </remarks>
        public static CacheKey UserRolesAllCacheKey => new CacheKey("WCore.userrole.all-{0}", UserRolesPrefixCacheKey);

        /// <summary>
        /// Gets a key for caching
        /// </summary>
        /// <remarks>
        /// {0} : system name
        /// </remarks>
        public static CacheKey UserRolesBySystemNameCacheKey => new CacheKey("WCore.userrole.systemname-{0}", UserRolesPrefixCacheKey);

        /// <summary>
        /// Gets a key pattern to clear cache
        /// </summary>
        public static string UserRolesPrefixCacheKey => "WCore.userrole.";

        /// <summary>
        /// Gets a key for caching
        /// </summary>
        /// <remarks>
        /// {0} : user identifier
        /// {1} : show hidden
        /// </remarks>
        public static CacheKey UserRoleIdsCacheKey => new CacheKey("WCore.user.userrole.ids-{0}-{1}", UserUserRolesPrefixCacheKey);

        /// <summary>
        /// Gets a key for caching
        /// </summary>
        /// <remarks>
        /// {0} : user identifier
        /// {1} : show hidden
        /// </remarks>
        public static CacheKey UserRolesCacheKey => new CacheKey("WCore.user.userrole-{0}-{1}", UserUserRolesPrefixCacheKey);

        /// <summary>
        /// Gets a key pattern to clear cache
        /// </summary>
        public static string UserUserRolesPrefixCacheKey => "WCore.user.userrole";

        /// <summary>
        /// Gets a key for caching
        /// </summary>
        /// <remarks>
        /// {0} : user identifier
        /// </remarks>
        public static CacheKey UserAddressesByUserIdCacheKey => new CacheKey("WCore.user.addresses.by.id-{0}", UserAddressesPrefixCacheKey);

        /// <summary>
        /// Gets a key for caching
        /// </summary>
        /// <remarks>
        /// {0} : user identifier
        /// {1} : address identifier
        /// </remarks>
        public static CacheKey UserAddressCacheKeyCacheKey => new CacheKey("WCore.user.addresses.address-{0}-{1}", UserAddressesPrefixCacheKey);

        /// <summary>
        /// Gets a key pattern to clear cache
        /// </summary>
        public static string UserAddressesPrefixCacheKey => "WCore.user.addresses";

        #endregion

        #region User password

        /// <summary>
        /// Gets a key for caching current user password lifetime
        /// </summary>
        /// <remarks>
        /// {0} : user identifier
        /// </remarks>
        public static CacheKey UserPasswordLifetimeCacheKey => new CacheKey("WCore.users.passwordlifetime-{0}");

        #endregion

        #endregion

    }
}
