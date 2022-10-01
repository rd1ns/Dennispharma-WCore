using WCore.Framework.Models;
using WCore.Framework.Mvc.ModelBinding;

namespace WCore.Web.Areas.Admin.Models.Settings
{
    /// <summary>
    /// Represents an address settings model
    /// </summary>
    public partial class AddressSettingsModel : BaseWCoreModel, ISettingsModel
    {
        #region Properties

        public int ActiveStoreScopeConfiguration { get; set; }

        [WCoreResourceDisplayName("Admin.Configuration.Settings.UserUser.AddressFormFields.CompanyEnabled")]
        public bool CompanyEnabled { get; set; }

        [WCoreResourceDisplayName("Admin.Configuration.Settings.UserUser.AddressFormFields.CompanyRequired")]
        public bool CompanyRequired { get; set; }

        [WCoreResourceDisplayName("Admin.Configuration.Settings.UserUser.AddressFormFields.StreetAddressEnabled")]
        public bool StreetAddressEnabled { get; set; }

        [WCoreResourceDisplayName("Admin.Configuration.Settings.UserUser.AddressFormFields.StreetAddressRequired")]
        public bool StreetAddressRequired { get; set; }

        [WCoreResourceDisplayName("Admin.Configuration.Settings.UserUser.AddressFormFields.StreetAddress2Enabled")]
        public bool StreetAddress2Enabled { get; set; }

        [WCoreResourceDisplayName("Admin.Configuration.Settings.UserUser.AddressFormFields.StreetAddress2Required")]
        public bool StreetAddress2Required { get; set; }

        [WCoreResourceDisplayName("Admin.Configuration.Settings.UserUser.AddressFormFields.ZipPostalCodeEnabled")]
        public bool ZipPostalCodeEnabled { get; set; }

        [WCoreResourceDisplayName("Admin.Configuration.Settings.UserUser.AddressFormFields.ZipPostalCodeRequired")]
        public bool ZipPostalCodeRequired { get; set; }

        [WCoreResourceDisplayName("Admin.Configuration.Settings.UserUser.AddressFormFields.CityEnabled")]
        public bool CityEnabled { get; set; }

        [WCoreResourceDisplayName("Admin.Configuration.Settings.UserUser.AddressFormFields.CityRequired")]
        public bool CityRequired { get; set; }

        [WCoreResourceDisplayName("Admin.Configuration.Settings.UserUser.AddressFormFields.CountyEnabled")]
        public bool CountyEnabled { get; set; }

        [WCoreResourceDisplayName("Admin.Configuration.Settings.UserUser.AddressFormFields.CountyRequired")]
        public bool CountyRequired { get; set; }

        [WCoreResourceDisplayName("Admin.Configuration.Settings.UserUser.AddressFormFields.CountryEnabled")]
        public bool CountryEnabled { get; set; }

        [WCoreResourceDisplayName("Admin.Configuration.Settings.UserUser.AddressFormFields.StateProvinceEnabled")]
        public bool StateProvinceEnabled { get; set; }

        [WCoreResourceDisplayName("Admin.Configuration.Settings.UserUser.AddressFormFields.PhoneEnabled")]
        public bool PhoneEnabled { get; set; }

        [WCoreResourceDisplayName("Admin.Configuration.Settings.UserUser.AddressFormFields.PhoneRequired")]
        public bool PhoneRequired { get; set; }

        [WCoreResourceDisplayName("Admin.Configuration.Settings.UserUser.AddressFormFields.FaxEnabled")]
        public bool FaxEnabled { get; set; }

        [WCoreResourceDisplayName("Admin.Configuration.Settings.UserUser.AddressFormFields.FaxRequired")]
        public bool FaxRequired { get; set; }

        #endregion
    }
}