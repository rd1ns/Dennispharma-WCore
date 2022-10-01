using System.Collections.Generic;
using WCore.Framework.Models;
using WCore.Framework.Mvc.ModelBinding;

namespace WCore.Web.Areas.Admin.Models.Users
{
    /// <summary>
    /// Represents a user attribute model
    /// </summary>
    public partial class UserAttributeModel : BaseWCoreEntityModel, ILocalizedModel<UserAttributeLocalizedModel>
    {
        #region Ctor

        public UserAttributeModel()
        {
            Locales = new List<UserAttributeLocalizedModel>();
            UserAttributeValueSearchModel = new UserAttributeValueSearchModel();
        }

        #endregion

        #region Properties

        [WCoreResourceDisplayName("Admin.Users.UserAttributes.Fields.Name")]
        public string Name { get; set; }

        [WCoreResourceDisplayName("Admin.Users.UserAttributes.Fields.IsRequired")]
        public bool IsRequired { get; set; }

        [WCoreResourceDisplayName("Admin.Users.UserAttributes.Fields.AttributeControlType")]
        public int AttributeControlTypeId { get; set; }

        [WCoreResourceDisplayName("Admin.Users.UserAttributes.Fields.AttributeControlType")]
        public string AttributeControlTypeName { get; set; }

        [WCoreResourceDisplayName("Admin.Users.UserAttributes.Fields.DisplayOrder")]
        public int DisplayOrder { get; set; }

        public IList<UserAttributeLocalizedModel> Locales { get; set; }

        public UserAttributeValueSearchModel UserAttributeValueSearchModel { get; set; }

        #endregion
    }

    public partial class UserAttributeLocalizedModel : ILocalizedLocaleModel
    {
        public int LanguageId { get; set; }

        [WCoreResourceDisplayName("Admin.Users.UserAttributes.Fields.Name")]
        public string Name { get; set; }
    }
}