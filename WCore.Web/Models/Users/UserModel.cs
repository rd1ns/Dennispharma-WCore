using System;
using WCore.Core.Domain.Users;
using WCore.Framework.Models;

namespace WCore.Web.Models.Users
{
    public class UserModel : BaseWCoreEntityModel
    {
        #region Ctor
        public UserModel()
        {
            UserGuid = Guid.NewGuid();
        }
        #endregion

        #region Properties
        public Guid UserGuid { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Passwrd { get; set; }
        public string EmailToRevalidate { get; set; }
        public string AdminComment { get; set; }
        public bool IsTaxExempt { get; set; }
        public int AffiliateId { get; set; }
        public int VendorId { get; set; }
        public bool HasShoppingCartItems { get; set; }
        public bool RequireReLogin { get; set; }
        public int FailedLoginAttempts { get; set; }
        public DateTime? CannotLoginUntilDate { get; set; }
        public bool Active { get; set; }
        public bool Deleted { get; set; }
        public bool IsSystemAccount { get; set; }
        public string SystemName { get; set; }
        public string LastIpAddress { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? LastLoginDate { get; set; }
        public DateTime LastActivityDate { get; set; }
        public UserType UserType { get; set; }

        public int RegisteredInStoreId { get; set; }
        public int? BillingAddressId { get; set; }
        public int? ShippingAddressId { get; set; }
        public int RoleGroupId { get; set; }
        #endregion
    }
}
