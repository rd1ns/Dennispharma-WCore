using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WCore.Core.Domain.Catalog;
using WCore.Framework.Models;

namespace WCore.Web.Models.Users
{
    public partial class UserAttributeModel : BaseWCoreEntityModel
    {
        public UserAttributeModel()
        {
            Values = new List<UserAttributeValueModel>();
        }

        public string Name { get; set; }

        public bool IsRequired { get; set; }

        /// <summary>
        /// Default value for textboxes
        /// </summary>
        public string DefaultValue { get; set; }

        public AttributeControlType AttributeControlType { get; set; }

        public IList<UserAttributeValueModel> Values { get; set; }

    }

    public partial class UserAttributeValueModel : BaseWCoreEntityModel
    {
        public string Name { get; set; }

        public bool IsPreSelected { get; set; }
    }
}
