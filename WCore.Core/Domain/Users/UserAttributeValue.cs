﻿using WCore.Core.Domain.Localization;

namespace WCore.Core.Domain.Users
{
    /// <summary>
    /// Represents a user attribute value
    /// </summary>
    public partial class UserAttributeValue : BaseEntity, ILocalizedEntity
    {
        /// <summary>
        /// Gets or sets the user attribute identifier
        /// </summary>
        public int UserAttributeId { get; set; }

        /// <summary>
        /// Gets or sets the checkout attribute name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the value is pre-selected
        /// </summary>
        public bool IsPreSelected { get; set; }

        /// <summary>
        /// Gets or sets the display order
        /// </summary>
        public int DisplayOrder { get; set; }
    }
}
