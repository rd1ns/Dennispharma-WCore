using WCore.Framework.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace WCore.Web.Areas.Admin.Models.Common
{
    /// <summary>
    /// Represents a generic attribute
    /// </summary>
    public partial class GenericAttributeModel : BaseWCoreEntityModel
    {
        /// <summary>
        /// Gets or sets the entity identifier
        /// </summary>
        public int EntityId { get; set; }

        /// <summary>
        /// Gets or sets the key group
        /// </summary>
        public string KeyGroup { get; set; }

        /// <summary>
        /// Gets or sets the key
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// Gets or sets the value
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// Gets or sets the created or updated date
        /// </summary>
        public DateTime? CreatedOrUpdatedDate { get; set; }
    }
}
