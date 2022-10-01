using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using WCore.Core.Domain.Catalog;
using WCore.Framework.Mvc.ModelBinding;
using WCore.Framework.Models;
using WCore.Web.Areas.Admin.Models.Roles;
using WCore.Web.Areas.Admin.Models.UserAgencies;
using WCore.Core.Domain.Users;

namespace WCore.Web.Areas.Admin.Models.Users
{
    /// <summary>
    /// Represents a user model
    /// </summary>
    public partial class UserModel : BaseWCoreEntityModel
    {
        #region Ctor

        public UserModel()
        {
            AvailableTimeZones = new List<SelectListItem>();
            SendEmail = new SendEmailModel() { SendImmediately = true };
            SendPm = new SendPmModel();

            SelectedUserRoleIds = new List<int>();
            AvailableUserRoles = new List<SelectListItem>();

            AvailableCountries = new List<SelectListItem>();
            AvailableStates = new List<SelectListItem>();
            AvailableVendors = new List<SelectListItem>();
            UserAttributes = new List<UserAttributeModel>();
            AvailableNewsletterSubscriptionStores = new List<SelectListItem>();
            SelectedNewsletterSubscriptionStoreIds = new List<int>();
            AddRewardPoints = new AddRewardPointsToUserModel();
            UserRewardPointsSearchModel = new UserRewardPointsSearchModel();
            UserAddressSearchModel = new UserAddressSearchModel();
            UserOrderSearchModel = new UserOrderSearchModel();
            UserShoppingCartSearchModel = new UserShoppingCartSearchModel();
            UserActivityLogSearchModel = new UserActivityLogSearchModel();
            UserBackInStockSubscriptionSearchModel = new UserBackInStockSubscriptionSearchModel();
            UserAssociatedExternalAuthRecordsSearchModel = new UserAssociatedExternalAuthRecordsSearchModel();

            UserTypes = new List<SelectListItem>();
            UserAgencies = new List<SelectListItem>();
            RoleGroups = new List<SelectListItem>();
        }

        #endregion

        #region Properties

        public bool UsernamesEnabled { get; set; }

        [WCoreResourceDisplayName("Admin.Users.Users.Fields.Username")]
        public string Username { get; set; }

        [DataType(DataType.EmailAddress)]
        [WCoreResourceDisplayName("Admin.Users.Users.Fields.Email")]
        public string Email { get; set; }

        [WCoreResourceDisplayName("Admin.Users.Users.Fields.Password")]
        [DataType(DataType.Password)]
        [NoTrim]
        public string Password { get; set; }

        [WCoreResourceDisplayName("Admin.Users.Users.Fields.Vendor")]
        public int VendorId { get; set; }

        public IList<SelectListItem> AvailableVendors { get; set; }

        //form fields & properties
        public bool GenderEnabled { get; set; }

        [WCoreResourceDisplayName("Admin.Users.Users.Fields.Gender")]
        public string Gender { get; set; }

        public bool FirstNameEnabled { get; set; }
        [WCoreResourceDisplayName("Admin.Users.Users.Fields.FirstName")]
        public string FirstName { get; set; }

        public bool LastNameEnabled { get; set; }
        [WCoreResourceDisplayName("Admin.Users.Users.Fields.LastName")]
        public string LastName { get; set; }

        [WCoreResourceDisplayName("Admin.Users.Users.Fields.FullName")]
        public string FullName { get; set; }

        public bool DateOfBirthEnabled { get; set; }

        [UIHint("DateNullable")]
        [WCoreResourceDisplayName("Admin.Users.Users.Fields.DateOfBirth")]
        public DateTime? DateOfBirth { get; set; }

        public bool CompanyEnabled { get; set; }

        [WCoreResourceDisplayName("Admin.Users.Users.Fields.Company")]
        public string Company { get; set; }

        public bool StreetAddressEnabled { get; set; }

        [WCoreResourceDisplayName("Admin.Users.Users.Fields.StreetAddress")]
        public string StreetAddress { get; set; }

        public bool StreetAddress2Enabled { get; set; }

        [WCoreResourceDisplayName("Admin.Users.Users.Fields.StreetAddress2")]
        public string StreetAddress2 { get; set; }

        public bool ZipPostalCodeEnabled { get; set; }

        [WCoreResourceDisplayName("Admin.Users.Users.Fields.ZipPostalCode")]
        public string ZipPostalCode { get; set; }

        public bool CityEnabled { get; set; }

        [WCoreResourceDisplayName("Admin.Users.Users.Fields.City")]
        public string City { get; set; }

        public bool CountyEnabled { get; set; }

        [WCoreResourceDisplayName("Admin.Users.Users.Fields.County")]
        public string County { get; set; }

        public bool CountryEnabled { get; set; }

        [WCoreResourceDisplayName("Admin.Users.Users.Fields.Country")]
        public int CountryId { get; set; }

        public IList<SelectListItem> AvailableCountries { get; set; }

        public bool StateProvinceEnabled { get; set; }

        [WCoreResourceDisplayName("Admin.Users.Users.Fields.StateProvince")]
        public int StateProvinceId { get; set; }

        public IList<SelectListItem> AvailableStates { get; set; }

        public bool PhoneEnabled { get; set; }

        [DataType(DataType.PhoneNumber)]
        [WCoreResourceDisplayName("Admin.Users.Users.Fields.Phone")]
        public string Phone { get; set; }

        public bool FaxEnabled { get; set; }

        [DataType(DataType.PhoneNumber)]
        [WCoreResourceDisplayName("Admin.Users.Users.Fields.Fax")]
        public string Fax { get; set; }

        public List<UserAttributeModel> UserAttributes { get; set; }

        [WCoreResourceDisplayName("Admin.Users.Users.Fields.RegisteredInStore")]
        public string RegisteredInStore { get; set; }

        [WCoreResourceDisplayName("Admin.Users.Users.Fields.AdminComment")]
        public string AdminComment { get; set; }

        [WCoreResourceDisplayName("Admin.Users.Users.Fields.IsTaxExempt")]
        public bool IsTaxExempt { get; set; }

        [WCoreResourceDisplayName("Admin.Users.Users.Fields.Active")]
        public bool Active { get; set; }

        [WCoreResourceDisplayName("Admin.Users.Users.Fields.Affiliate")]
        public int AffiliateId { get; set; }

        [WCoreResourceDisplayName("Admin.Users.Users.Fields.Affiliate")]
        public string AffiliateName { get; set; }

        //time zone
        [WCoreResourceDisplayName("Admin.Users.Users.Fields.TimeZoneId")]
        public string TimeZoneId { get; set; }

        public bool AllowUsersToSetTimeZone { get; set; }

        public IList<SelectListItem> AvailableTimeZones { get; set; }

        //EU VAT
        [WCoreResourceDisplayName("Admin.Users.Users.Fields.VatNumber")]
        public string VatNumber { get; set; }

        public string VatNumberStatusNote { get; set; }

        public bool DisplayVatNumber { get; set; }

        public bool DisplayRegisteredInStore { get; set; }

        //registration date
        [WCoreResourceDisplayName("Admin.Users.Users.Fields.CreatedOn")]
        public DateTime CreatedOn { get; set; }

        [WCoreResourceDisplayName("Admin.Users.Users.Fields.LastActivityDate")]
        public DateTime LastActivityDate { get; set; }

        //IP address
        [WCoreResourceDisplayName("Admin.Users.Users.Fields.IPAddress")]
        public string LastIpAddress { get; set; }

        [WCoreResourceDisplayName("Admin.Users.Users.Fields.LastVisitedPage")]
        public string LastVisitedPage { get; set; }

        //user roles
        [WCoreResourceDisplayName("Admin.Users.Users.Fields.UserRoles")]
        public string UserRoleNames { get; set; }

        public IList<SelectListItem> AvailableUserRoles { get; set; }

        [WCoreResourceDisplayName("Admin.Users.Users.Fields.UserRoles")]
        public IList<int> SelectedUserRoleIds { get; set; }

        //newsletter subscriptions (per store)
        [WCoreResourceDisplayName("Admin.Users.Users.Fields.Newsletter")]
        public IList<SelectListItem> AvailableNewsletterSubscriptionStores { get; set; }

        [WCoreResourceDisplayName("Admin.Users.Users.Fields.Newsletter")]
        public IList<int> SelectedNewsletterSubscriptionStoreIds { get; set; }

        //reward points history
        public bool DisplayRewardPointsHistory { get; set; }

        public AddRewardPointsToUserModel AddRewardPoints { get; set; }

        public UserRewardPointsSearchModel UserRewardPointsSearchModel { get; set; }

        //send email model
        public SendEmailModel SendEmail { get; set; }

        //send PM model
        public SendPmModel SendPm { get; set; }

        //send a private message
        public bool AllowSendingOfPrivateMessage { get; set; }

        //send the welcome message
        public bool AllowSendingOfWelcomeMessage { get; set; }

        //re-send the activation message
        public bool AllowReSendingOfActivationMessage { get; set; }

        //GDPR enabled
        public bool GdprEnabled { get; set; }
        
        public string AvatarUrl { get; internal set; }

        public UserAddressSearchModel UserAddressSearchModel { get; set; }

        public UserOrderSearchModel UserOrderSearchModel { get; set; }

        public UserShoppingCartSearchModel UserShoppingCartSearchModel { get; set; }

        public UserActivityLogSearchModel UserActivityLogSearchModel { get; set; }

        public UserBackInStockSubscriptionSearchModel UserBackInStockSubscriptionSearchModel { get; set; }

        public UserAssociatedExternalAuthRecordsSearchModel UserAssociatedExternalAuthRecordsSearchModel { get; set; }

        #endregion

        [WCoreResourceDisplayName("Admin.Configuration.MiddleName")]
        public string MiddleName { get; set; }
        [WCoreResourceDisplayName("Admin.Configuration.Avatar")]
        public string Avatar { get; set; }
        [WCoreResourceDisplayName("Admin.Configuration.EMail")]
        public string EMail { get; set; }
        [WCoreResourceDisplayName("Admin.Configuration.UserType")]
        public UserType UserType { get; set; }
        public string UserTypeName { get; set; }

        [WCoreResourceDisplayName("Admin.Configuration.UpdatedOn")]
        public DateTime UpdatedOn { get; set; }
        [WCoreResourceDisplayName("Admin.Configuration.Deleted")]
        public bool Deleted { get; set; }
        [WCoreResourceDisplayName("Admin.Configuration.IsActive")]
        public bool IsActive { get; set; }
        [WCoreResourceDisplayName("Admin.Configuration.IsAgencyManager")]
        public bool IsAgencyManager { get; set; }
        [WCoreResourceDisplayName("Admin.Configuration.IsAgencyStaff")]
        public bool IsAgencyStaff { get; set; }

        [WCoreResourceDisplayName("Admin.Configuration.RoleGroup")]
        public int RoleGroupId { get; set; }
        public virtual RoleGroupModel RoleGroup { get; set; }

        [WCoreResourceDisplayName("Admin.Configuration.CreatedUser")]
        public int CreatedUserId { get; set; }
        public virtual UserModel CreatedUser { get; set; }

        [WCoreResourceDisplayName("Admin.Configuration.User")]
        public int UserId { get; set; }
        public virtual UserModel User { get; set; }
        [WCoreResourceDisplayName("Admin.Configuration.UserAgency")]
        public int UserAgencyId { get; set; }
        public virtual UserAgencyModel UserAgency { get; set; }
        public string UserAgencyName { get; set; }

        public List<SelectListItem> UserTypes { get; set; }
        public List<SelectListItem> RoleGroups { get; set; }
        public List<SelectListItem> UserAgencies { get; set; }

        #region Nested classes

        public partial class SendEmailModel : BaseWCoreModel
        {
            [WCoreResourceDisplayName("Admin.Users.Users.SendEmail.Subject")]
            public string Subject { get; set; }

            [WCoreResourceDisplayName("Admin.Users.Users.SendEmail.Body")]
            public string Body { get; set; }

            [WCoreResourceDisplayName("Admin.Users.Users.SendEmail.SendImmediately")]
            public bool SendImmediately { get; set; }

            [WCoreResourceDisplayName("Admin.Users.Users.SendEmail.DontSendBeforeDate")]
            [UIHint("DateTimeNullable")]
            public DateTime? DontSendBeforeDate { get; set; }
        }

        public partial class SendPmModel : BaseWCoreModel
        {
            [WCoreResourceDisplayName("Admin.Users.Users.SendPM.Subject")]
            public string Subject { get; set; }

            [WCoreResourceDisplayName("Admin.Users.Users.SendPM.Message")]
            public string Message { get; set; }
        }

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

        #endregion
    }
    /// <summary>
    /// Represents a User search model
    /// </summary>
    public partial class UserSearchModel : BaseSearchModel
    {
        #region Ctor

        public UserSearchModel()
        {
            UserTypes = new List<SelectListItem>();
            RoleGroups = new List<SelectListItem>();
            UserAgencies = new List<SelectListItem>();
        }

        #endregion

        #region Properties
        [WCoreResourceDisplayName("Admin.Configuration.Query")]
        public string Query { get; set; }
        [WCoreResourceDisplayName("Admin.Configuration.Deleted")]
        public bool? Deleted { get; set; }
        [WCoreResourceDisplayName("Admin.Configuration.IsActive")]
        public bool? IsActive { get; set; }
        [WCoreResourceDisplayName("Admin.Configuration.IsAgencyManager")]
        public bool? IsAgencyManager { get; set; }
        [WCoreResourceDisplayName("Admin.Configuration.IsAgencyStaff")]
        public bool? IsAgencyStaff { get; set; }
        [WCoreResourceDisplayName("Admin.Configuration.UserType")]
        public UserType? UserType { get; set; }
        [WCoreResourceDisplayName("Admin.Configuration.UserAgency")]
        public int? UserAgencyId { get; set; }
        [WCoreResourceDisplayName("Admin.Configuration.RoleGroup")]
        public int? RoleGroupId { get; set; }

        public List<SelectListItem> UserTypes { get; set; }
        public List<SelectListItem> RoleGroups { get; set; }
        public List<SelectListItem> UserAgencies { get; set; }

        #endregion
    }
}