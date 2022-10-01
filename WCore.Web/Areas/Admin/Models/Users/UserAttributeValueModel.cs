using System.Collections.Generic;
using WCore.Framework.Models;
using WCore.Framework.Mvc.ModelBinding;

namespace WCore.Web.Areas.Admin.Models.Users
{
    /// <summary>
    /// Represents a user attribute value model
    /// </summary>
    public partial class UserAttributeValueModel : BaseWCoreEntityModel, ILocalizedModel<UserAttributeValueLocalizedModel>
    {
        #region Ctor

        public UserAttributeValueModel()
        {
            Locales = new List<UserAttributeValueLocalizedModel>();
        }

        #endregion

        #region Properties

        public int UserAttributeId { get; set; }

        [WCoreResourceDisplayName("Admin.Users.UserAttributes.Values.Fields.Name")]
        public string Name { get; set; }

        [WCoreResourceDisplayName("Admin.Users.UserAttributes.Values.Fields.IsPreSelected")]
        public bool IsPreSelected { get; set; }

        [WCoreResourceDisplayName("Admin.Users.UserAttributes.Values.Fields.DisplayOrder")]
        public int DisplayOrder {get;set;}

        public IList<UserAttributeValueLocalizedModel> Locales { get; set; }

        #endregion
    }

    public partial class UserAttributeValueLocalizedModel : ILocalizedLocaleModel
    {
        public int LanguageId { get; set; }

        [WCoreResourceDisplayName("Admin.Users.UserAttributes.Values.Fields.Name")]
        public string Name { get; set; }
    }
}