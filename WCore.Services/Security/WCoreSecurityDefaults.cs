using System;
using System.Collections.Generic;
using System.Text;
using WCore.Core.Caching;

namespace WCore.Services.Security
{
    /// <summary>
    /// Represents default values related to security services
    /// </summary>
    public static partial class WCoreSecurityDefaults
    {
        #region reCAPTCHA

        /// <summary>
        /// Gets a reCAPTCHA script URL
        /// </summary>
        /// <remarks>
        /// {0} : Id of recaptcha instance on page
        /// {1} : Render type
        /// {2} : language if exists
        /// </remarks>
        public static string RecaptchaScriptPath => "api.js?onload=onloadCallback{0}&render={1}{2}";

        /// <summary>
        /// Gets a reCAPTCHA validation URL
        /// </summary>
        /// <remarks>
        /// {0} : private key
        /// {1} : response value
        /// {2} : IP address
        /// </remarks>
        public static string RecaptchaValidationPath => "api/siteverify?secret={0}&response={1}&remoteip={2}";

        #endregion

        #region Caching defaults

        #region Access control list

        /// <summary>
        /// Gets a key for caching
        /// </summary>
        /// <remarks>
        /// {0} : entity ID
        /// {1} : entity name
        /// </remarks>
        public static CacheKey AclRecordByEntityIdNameCacheKey => new CacheKey("WCore.aclrecord.entityid-name-{0}-{1}");

        #endregion

        #region Permissions

        /// <summary>
        /// Gets a key for caching
        /// </summary>
        /// <remarks>
        /// {0} : permission system name
        /// {1} : user role ID
        /// </remarks>
        public static CacheKey PermissionsAllowedCacheKey => new CacheKey("WCore.permission.allowed-{0}-{1}", PermissionsAllowedPrefixCacheKey);

        /// <summary>
        /// Gets a key for caching
        /// </summary>
        /// <remarks>
        /// {0} : permission system name
        /// </remarks>
        public static string PermissionsAllowedPrefixCacheKey => "WCore.permission.allowed-{0}";

        /// <summary>
        /// Gets a key for caching
        /// </summary>
        /// <remarks>
        /// {0} : user role ID
        /// </remarks>
        public static CacheKey PermissionsAllByUserRoleIdCacheKey => new CacheKey("WCore.permission.allbyuserroleid-{0}", PermissionsAllByUserRoleIdPrefixCacheKey);

        /// <summary>
        /// Gets a key pattern to clear cache
        /// </summary>
        public static string PermissionsAllByUserRoleIdPrefixCacheKey => "WCore.permission.allbyuserroleid";

        #endregion

        #endregion
    }
}
