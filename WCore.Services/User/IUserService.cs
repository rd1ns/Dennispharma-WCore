using WCore.Core;
using WCore.Core.Domain.Users;
using System.Collections.Generic;
using WCore.Core.Domain.Tax;
using System;
using WCore.Core.Domain.Common;
using WCore.Core.Domain.Orders;

namespace WCore.Services.Users
{
    public interface IUserService : IRepository<User>
    {
        IPagedList<User> GetAllByFilters(string searchValue = "", UserType? userType = null, int? userAgencyId = null, bool? IsAgencyManager = null, bool? IsAgencyStaff = null, bool? IsActive = null, bool? Deleted = null, int skip = 0, int take = int.MaxValue);
        User GetOrCreateSearchEngineUser();
        User GetOrCreateBackgroundTaskUser();
        User GetUserByEmail(string Email);
        User GetUserByGuid(Guid guid);
        User GetUserByUsername(string Username);
        User GetUserById(int Id);
        User CheckUser(string Email, string Password);
        User InsertGuestUser();
        User InsertNewsletterUser(string Email);

        /// <summary>
        /// Gets User for shopping cart
        /// </summary>
        /// <param name="shoppingCart">Shopping cart</param>
        /// <returns>Result</returns>
        User GetShoppingCartUser(IList<ShoppingCartItem> shoppingCart);

        /// <summary>
        /// Get full name
        /// </summary>
        /// <param name="user">User</param>
        /// <returns>User full name</returns>
        string GetUserFullName(User user);
        /// <summary>
        /// Gets a value indicating whether user is registered
        /// </summary>
        /// <param name="user">User</param>
        /// <param name="onlyActiveUserRoles">A value indicating whether we should look only in active user roles</param>
        /// <returns>Result</returns>
        bool IsRegistered(User user, bool onlyActiveUserRoles = true);

        /// <summary>
        /// Gets a value indicating whether user is guest
        /// </summary>
        /// <param name="user">User</param>
        /// <param name="onlyActiveUserRoles">A value indicating whether we should look only in active user roles</param>
        /// <returns>Result</returns>
        bool IsGuest(User user, bool onlyActiveUserRoles = true);

        /// <summary>
        /// Gets a default tax display type (if configured)
        /// </summary>
        /// <param name="user">User</param>
        /// <returns>Result</returns>
        TaxDisplayType? GetUserDefaultTaxDisplayType(User user);


        /// <summary>
        /// Gets coupon codes
        /// </summary>
        /// <param name="user">User</param>
        /// <returns>Coupon codes</returns>
        string[] ParseAppliedDiscountCouponCodes(User user);

        /// <summary>
        /// Adds a coupon code
        /// </summary>
        /// <param name="user">User</param>
        /// <param name="couponCode">Coupon code</param>
        /// <returns>New coupon codes document</returns>
        void ApplyDiscountCouponCode(User user, string couponCode);

        /// <summary>
        /// Removes a coupon code
        /// </summary>
        /// <param name="user">User</param>
        /// <param name="couponCode">Coupon code to remove</param>
        /// <returns>New coupon codes document</returns>
        void RemoveDiscountCouponCode(User user, string couponCode);

        #region Gift Cards
        /// <summary>
        /// Gets coupon codes
        /// </summary>
        /// <param name="user">User</param>
        /// <returns>Coupon codes</returns>
        string[] ParseAppliedGiftCardCouponCodes(User user);

        /// <summary>
        /// Adds a coupon code
        /// </summary>
        /// <param name="user">User</param>
        /// <param name="couponCode">Coupon code</param>
        /// <returns>New coupon codes document</returns>
        void ApplyGiftCardCouponCode(User user, string couponCode);

        /// <summary>
        /// Removes a coupon code
        /// </summary>
        /// <param name="user">User</param>
        /// <param name="couponCode">Coupon code to remove</param>
        /// <returns>New coupon codes document</returns>
        void RemoveGiftCardCouponCode(User user, string couponCode);
        #endregion

        #region User passwords

        /// <summary>
        /// Gets user passwords
        /// </summary>
        /// <param name="userId">User identifier; pass null to load all records</param>
        /// <param name="passwordFormat">Password format; pass null to load all records</param>
        /// <param name="passwordsToReturn">Number of returning passwords; pass null to load all records</param>
        /// <returns>List of user passwords</returns>
        IList<UserPassword> GetUserPasswords(int? userId = null,
            PasswordFormat? passwordFormat = null, int? passwordsToReturn = null);

        /// <summary>
        /// Get current user password
        /// </summary>
        /// <param name="userId">User identifier</param>
        /// <returns>User password</returns>
        UserPassword GetCurrentPassword(int userId);

        /// <summary>
        /// Insert a user password
        /// </summary>
        /// <param name="userPassword">User password</param>
        void InsertUserPassword(UserPassword userPassword);

        /// <summary>
        /// Update a user password
        /// </summary>
        /// <param name="userPassword">User password</param>
        void UpdateUserPassword(UserPassword userPassword);

        /// <summary>
        /// Check whether password recovery token is valid
        /// </summary>
        /// <param name="user">User</param>
        /// <param name="token">Token to validate</param>
        /// <returns>Result</returns>
        bool IsPasswordRecoveryTokenValid(User user, string token);

        /// <summary>
        /// Check whether password recovery link is expired
        /// </summary>
        /// <param name="user">User</param>
        /// <returns>Result</returns>
        bool IsPasswordRecoveryLinkExpired(User user);

        /// <summary>
        /// Check whether user password is expired 
        /// </summary>
        /// <param name="user">User</param>
        /// <returns>True if password is expired; otherwise false</returns>
        bool PasswordIsExpired(User user);

        #endregion

        #region User address mapping

        /// <summary>
        /// Gets a list of addresses mapped to user
        /// </summary>
        /// <param name="userId">User identifier</param>
        /// <returns></returns>
        IList<Address> GetAddressesByUserId(int userId);

        /// <summary>
        /// Gets a address mapped to user
        /// </summary>
        /// <param name="userId">User identifier</param>
        /// <param name="addressId">Address identifier</param>
        /// <returns>Result</returns>
        Address GetUserAddress(int userId, int addressId);

        /// <summary>
        /// Gets a user billing address
        /// </summary>
        /// <param name="user">User identifier</param>
        /// <returns>Result</returns>
        Address GetUserBillingAddress(User user);

        /// <summary>
        /// Gets a user shipping address
        /// </summary>
        /// <param name="user">User</param>
        /// <returns>Result</returns>
        Address GetUserShippingAddress(User user);

        /// <summary>
        /// Remove a user-address mapping record
        /// </summary>
        /// <param name="user">User</param>
        /// <param name="address">Address</param>
        void RemoveUserAddress(User user, Address address);

        /// <summary>
        /// Inserts a user-address mapping record
        /// </summary>
        /// <param name="user">User</param>
        /// <param name="address">Address</param>
        void InsertUserAddress(User user, Address address);

        #endregion


    }
}
