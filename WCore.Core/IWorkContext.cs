using WCore.Core.Domain.Common;
using WCore.Core.Domain.Directory;
using WCore.Core.Domain.Localization;
using WCore.Core.Domain.Tax;
using WCore.Core.Domain.Users;
using WCore.Core.Domain.Vendors;

namespace WCore.Core
{
    public interface IWorkContext
    {
        /// <summary>
        /// Gets or sets current the current user
        /// </summary>
        User CurrentUser { get; set; }

        /// <summary>
        /// Gets the original customer (in case the current one is impersonated)
        /// </summary>
        User OriginalUserIfImpersonated { get; }

        /// <summary>
        /// Gets the current vendor (logged-in manager)
        /// </summary>
        Vendor CurrentVendor { get; }

        /// <summary>
        /// Gets or sets current user working language
        /// </summary>
        Language WorkingLanguage { get; set; }

        /// <summary>
        /// Gets or sets current user working currency
        /// </summary>
        Currency WorkingCurrency { get; set; }

        /// <summary>
        /// Gets or sets current user working country
        /// </summary>
        Country WorkingCountry { get; set; }

        /// <summary>
        /// Gets or sets current tax display type
        /// </summary>
        TaxDisplayType TaxDisplayType { get; set; }

        /// <summary>
        /// Gets or sets value indicating whether we're in admin area
        /// </summary>
        bool IsAdmin { get; set; }
    }
}
