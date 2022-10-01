using System.ComponentModel.DataAnnotations;
using WCore.Framework.Models;
using WCore.Framework.Mvc.ModelBinding;

namespace WCore.Web.Areas.Admin.Models.Settings
{
    /// <summary>
    /// Represents a user settings model
    /// </summary>
    public partial class UserSettingsModel : BaseWCoreModel, ISettingsModel
    {
        #region Properties

        public int ActiveStoreScopeConfiguration { get; set; }

        [WCoreResourceDisplayName("Admin.Configuration.Settings.UserUser.UsernamesEnabled")]
        public bool UsernamesEnabled { get; set; }

        [WCoreResourceDisplayName("Admin.Configuration.Settings.UserUser.AllowUsersToChangeUsernames")]
        public bool AllowUsersToChangeUsernames { get; set; }

        [WCoreResourceDisplayName("Admin.Configuration.Settings.UserUser.CheckUsernameAvailabilityEnabled")]
        public bool CheckUsernameAvailabilityEnabled { get; set; }

        [WCoreResourceDisplayName("Admin.Configuration.Settings.UserUser.UsernameValidationEnabled")]
        public bool UsernameValidationEnabled { get; set; }

        [WCoreResourceDisplayName("Admin.Configuration.Settings.UserUser.UsernameValidationUseRegex")]
        public bool UsernameValidationUseRegex { get; set; }

        [WCoreResourceDisplayName("Admin.Configuration.Settings.UserUser.UsernameValidationRule")]
        public string UsernameValidationRule { get; set; }       

        [WCoreResourceDisplayName("Admin.Configuration.Settings.UserUser.UserRegistrationType")]
        public int UserRegistrationType { get; set; }

        [WCoreResourceDisplayName("Admin.Configuration.Settings.UserUser.AllowUsersToUploadAvatars")]
        public bool AllowUsersToUploadAvatars { get; set; }

        [WCoreResourceDisplayName("Admin.Configuration.Settings.UserUser.DefaultAvatarEnabled")]
        public bool DefaultAvatarEnabled { get; set; }

        [WCoreResourceDisplayName("Admin.Configuration.Settings.UserUser.ShowUsersLocation")]
        public bool ShowUsersLocation { get; set; }

        [WCoreResourceDisplayName("Admin.Configuration.Settings.UserUser.ShowUsersJoinDate")]
        public bool ShowUsersJoinDate { get; set; }

        [WCoreResourceDisplayName("Admin.Configuration.Settings.UserUser.AllowViewingProfiles")]
        public bool AllowViewingProfiles { get; set; }

        [WCoreResourceDisplayName("Admin.Configuration.Settings.UserUser.NotifyNewUserRegistration")]
        public bool NotifyNewUserRegistration { get; set; }

        [WCoreResourceDisplayName("Admin.Configuration.Settings.UserUser.RequireRegistrationForDownloadableProducts")]
        public bool RequireRegistrationForDownloadableProducts { get; set; }

        [WCoreResourceDisplayName("Admin.Configuration.Settings.UserUser.AllowUsersToCheckGiftCardBalance")]
        public bool AllowUsersToCheckGiftCardBalance { get; set; }

        [WCoreResourceDisplayName("Admin.Configuration.Settings.UserUser.HideDownloadableProductsTab")]
        public bool HideDownloadableProductsTab { get; set; }

        [WCoreResourceDisplayName("Admin.Configuration.Settings.UserUser.HideBackInStockSubscriptionsTab")]
        public bool HideBackInStockSubscriptionsTab { get; set; }

        [WCoreResourceDisplayName("Admin.Configuration.Settings.UserUser.UserNameFormat")]
        public int UserNameFormat { get; set; }

        [WCoreResourceDisplayName("Admin.Configuration.Settings.UserUser.PasswordMinLength")]
        public int PasswordMinLength { get; set; }

        [WCoreResourceDisplayName("Admin.Configuration.Settings.UserUser.PasswordRequireLowercase")]
        public bool PasswordRequireLowercase { get; set; }

        [WCoreResourceDisplayName("Admin.Configuration.Settings.UserUser.PasswordRequireUppercase")]
        public bool PasswordRequireUppercase { get; set; }

        [WCoreResourceDisplayName("Admin.Configuration.Settings.UserUser.PasswordRequireNonAlphanumeric")]
        public bool PasswordRequireNonAlphanumeric { get; set; }

        [WCoreResourceDisplayName("Admin.Configuration.Settings.UserUser.PasswordRequireDigit")]
        public bool PasswordRequireDigit { get; set; }

        [WCoreResourceDisplayName("Admin.Configuration.Settings.UserUser.UnduplicatedPasswordsNumber")]
        public int UnduplicatedPasswordsNumber { get; set; }

        [WCoreResourceDisplayName("Admin.Configuration.Settings.UserUser.PasswordRecoveryLinkDaysValid")]
        public int PasswordRecoveryLinkDaysValid { get; set; }

        [WCoreResourceDisplayName("Admin.Configuration.Settings.UserUser.DefaultPasswordFormat")]
        public int DefaultPasswordFormat { get; set; }

        [WCoreResourceDisplayName("Admin.Configuration.Settings.UserUser.PasswordLifetime")]
        public int PasswordLifetime { get; set; }

        [WCoreResourceDisplayName("Admin.Configuration.Settings.UserUser.FailedPasswordAllowedAttempts")]
        public int FailedPasswordAllowedAttempts { get; set; }

        [WCoreResourceDisplayName("Admin.Configuration.Settings.UserUser.FailedPasswordLockoutMinutes")]
        public int FailedPasswordLockoutMinutes { get; set; }

        [WCoreResourceDisplayName("Admin.Configuration.Settings.UserUser.NewsletterEnabled")]
        public bool NewsletterEnabled { get; set; }

        [WCoreResourceDisplayName("Admin.Configuration.Settings.UserUser.NewsletterTickedByDefault")]
        public bool NewsletterTickedByDefault { get; set; }

        [WCoreResourceDisplayName("Admin.Configuration.Settings.UserUser.HideNewsletterBlock")]
        public bool HideNewsletterBlock { get; set; }

        [WCoreResourceDisplayName("Admin.Configuration.Settings.UserUser.NewsletterBlockAllowToUnsubscribe")]
        public bool NewsletterBlockAllowToUnsubscribe { get; set; }

        [WCoreResourceDisplayName("Admin.Configuration.Settings.UserUser.StoreLastVisitedPage")]
        public bool StoreLastVisitedPage { get; set; }

        [WCoreResourceDisplayName("Admin.Configuration.Settings.UserUser.StoreIpAddresses")]
        public bool StoreIpAddresses { get; set; }

        [WCoreResourceDisplayName("Admin.Configuration.Settings.UserUser.EnteringEmailTwice")]
        public bool EnteringEmailTwice { get; set; }

        [WCoreResourceDisplayName("Admin.Configuration.Settings.UserUser.GenderEnabled")]
        public bool GenderEnabled { get; set; }

        [WCoreResourceDisplayName("Admin.Configuration.Settings.UserUser.FirstNameEnabled")]
        public bool FirstNameEnabled { get; set; }

        [WCoreResourceDisplayName("Admin.Configuration.Settings.UserUser.FirstNameRequired")]
        public bool FirstNameRequired { get; set; }

        [WCoreResourceDisplayName("Admin.Configuration.Settings.UserUser.LastNameEnabled")]
        public bool LastNameEnabled { get; set; }

        [WCoreResourceDisplayName("Admin.Configuration.Settings.UserUser.LastNameRequired")]
        public bool LastNameRequired { get; set; }

        [WCoreResourceDisplayName("Admin.Configuration.Settings.UserUser.DateOfBirthEnabled")]
        public bool DateOfBirthEnabled { get; set; }

        [WCoreResourceDisplayName("Admin.Configuration.Settings.UserUser.DateOfBirthRequired")]
        public bool DateOfBirthRequired { get; set; }

        [WCoreResourceDisplayName("Admin.Configuration.Settings.UserUser.DateOfBirthMinimumAge")]
        [UIHint("Int32Nullable")]
        public int? DateOfBirthMinimumAge { get; set; }

        [WCoreResourceDisplayName("Admin.Configuration.Settings.UserUser.CompanyEnabled")]
        public bool CompanyEnabled { get; set; }

        [WCoreResourceDisplayName("Admin.Configuration.Settings.UserUser.CompanyRequired")]
        public bool CompanyRequired { get; set; }

        [WCoreResourceDisplayName("Admin.Configuration.Settings.UserUser.StreetAddressEnabled")]
        public bool StreetAddressEnabled { get; set; }

        [WCoreResourceDisplayName("Admin.Configuration.Settings.UserUser.StreetAddressRequired")]
        public bool StreetAddressRequired { get; set; }

        [WCoreResourceDisplayName("Admin.Configuration.Settings.UserUser.StreetAddress2Enabled")]
        public bool StreetAddress2Enabled { get; set; }

        [WCoreResourceDisplayName("Admin.Configuration.Settings.UserUser.StreetAddress2Required")]
        public bool StreetAddress2Required { get; set; }

        [WCoreResourceDisplayName("Admin.Configuration.Settings.UserUser.ZipPostalCodeEnabled")]
        public bool ZipPostalCodeEnabled { get; set; }

        [WCoreResourceDisplayName("Admin.Configuration.Settings.UserUser.ZipPostalCodeRequired")]
        public bool ZipPostalCodeRequired { get; set; }

        [WCoreResourceDisplayName("Admin.Configuration.Settings.UserUser.CityEnabled")]
        public bool CityEnabled { get; set; }

        [WCoreResourceDisplayName("Admin.Configuration.Settings.UserUser.CityRequired")]
        public bool CityRequired { get; set; }

        [WCoreResourceDisplayName("Admin.Configuration.Settings.UserUser.CountyEnabled")]
        public bool CountyEnabled { get; set; }

        [WCoreResourceDisplayName("Admin.Configuration.Settings.UserUser.CountyRequired")]
        public bool CountyRequired { get; set; }

        [WCoreResourceDisplayName("Admin.Configuration.Settings.UserUser.CountryEnabled")]
        public bool CountryEnabled { get; set; }

        [WCoreResourceDisplayName("Admin.Configuration.Settings.UserUser.CountryRequired")]
        public bool CountryRequired { get; set; }

        [WCoreResourceDisplayName("Admin.Configuration.Settings.UserUser.StateProvinceEnabled")]
        public bool StateProvinceEnabled { get; set; }

        [WCoreResourceDisplayName("Admin.Configuration.Settings.UserUser.StateProvinceRequired")]
        public bool StateProvinceRequired { get; set; }

        [WCoreResourceDisplayName("Admin.Configuration.Settings.UserUser.PhoneEnabled")]
        public bool PhoneEnabled { get; set; }

        [WCoreResourceDisplayName("Admin.Configuration.Settings.UserUser.PhoneRequired")]
        public bool PhoneRequired { get; set; }

        [WCoreResourceDisplayName("Admin.Configuration.Settings.UserUser.PhoneNumberValidationEnabled")]
        public bool PhoneNumberValidationEnabled { get; set; }

        [WCoreResourceDisplayName("Admin.Configuration.Settings.UserUser.PhoneNumberValidationUseRegex")]
        public bool PhoneNumberValidationUseRegex { get; set; }

        [WCoreResourceDisplayName("Admin.Configuration.Settings.UserUser.PhoneNumberValidationRule")]
        public string PhoneNumberValidationRule { get; set; }

        [WCoreResourceDisplayName("Admin.Configuration.Settings.UserUser.FaxEnabled")]
        public bool FaxEnabled { get; set; }

        [WCoreResourceDisplayName("Admin.Configuration.Settings.UserUser.FaxRequired")]
        public bool FaxRequired { get; set; }

        [WCoreResourceDisplayName("Admin.Configuration.Settings.UserUser.AcceptPrivacyPolicyEnabled")]
        public bool AcceptPrivacyPolicyEnabled { get; set; }        

        #endregion
    }
}