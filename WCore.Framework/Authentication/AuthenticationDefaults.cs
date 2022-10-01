using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WCore.Framework.Authentication
{

    /// <summary>
    /// Represents default values related to authentication services
    /// </summary>
    public static partial class AuthenticationDefaults
    {
        /// <summary>
        /// The default value used for authentication scheme
        /// </summary>
        public static string AuthenticationScheme => "Authentication";

        /// <summary>
        /// The default value used for external authentication scheme
        /// </summary>
        public static string ExternalAuthenticationScheme => "ExternalAuthentication";

        /// <summary>
        /// The issuer that should be used for any claims that are created
        /// </summary>
        public static string ClaimsIssuer => "CashClaim";

        /// <summary>
        /// The default value for the login path
        /// </summary>
        public static PathString LoginPath => new PathString("/auth");

        /// <summary>
        /// The default value used for the logout path
        /// </summary>
        public static PathString LogoutPath => new PathString("/auth/logout");

        /// <summary>
        /// The default value for the access denied path
        /// </summary>
        public static PathString AccessDeniedPath => new PathString("/page-not-found");

        /// <summary>
        /// The default value of the return URL parameter
        /// </summary>
        public static string ReturnUrlParameter => string.Empty;

        /// <summary>
        /// Gets a key to store external authentication errors to session
        /// </summary>
        public static string ExternalAuthenticationErrorsSessionKey => "errors";
    }
}
